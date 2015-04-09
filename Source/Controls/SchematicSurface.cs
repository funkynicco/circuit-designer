using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CircuitDesign.Framework.Components;
using CircuitDesign.Framework.Generics;
using CircuitDesign.Framework.Forms;

namespace CircuitDesign.Controls
{
    public delegate void ComponentSelectionChangedEvent(BaseComponent component);
    public delegate void RequestDeleteComponentEvent(BaseComponent component);

    public partial class SchematicSurface : UserControl
    {
        private Point _dragOrigin = new Point(0, 0);
        private bool _isDragging = false;

        private ContextMenu _componentMenu = null;

        private Point _surfaceOffset = new Point(0, 0); // the current offset within the surface
        private Size _surfaceSize = new Size(960 + 1, 640 + 1); // the size of the entire surface
        private bool _isPanning = false; // while panning, _dragOrigin is used

        private LinkedList<BaseComponent> _circuitComponents = null;

        public LinkedList<BaseComponent> CircuitComponents
        {
            get { return _circuitComponents; }
            set
            {
                _circuitComponents = value;
                Invalidate();
            }
        }

        private BaseComponent _selectedComponent = null;
        public BaseComponent SelectedComponent
        {
            get
            {
                return _selectedComponent;
            }
            set
            {
                _selectedComponent = value;

                if (ComponentSelectionChanged != null)
                    ComponentSelectionChanged(_selectedComponent);

                Invalidate();
            }
        }

        public event ComponentSelectionChangedEvent ComponentSelectionChanged;
        public event RequestDeleteComponentEvent RequestDeleteComponent;

        private readonly ResourcePool _resourcePool = new ResourcePool();

        public SchematicSurface()
        {
            InitializeComponent();
            DoubleBuffered = true;

            SizeChanged += (sender, e) => Invalidate();
        }

        private Rectangle GetSurfaceBounds()
        {
            return new Rectangle(
                _surfaceOffset.X,
                _surfaceOffset.Y,
                _surfaceSize.Width,
                _surfaceSize.Height);
        }

        private Rectangle GetVisibleSurfaceBounds()
        {
            var left = Math.Max(0, _surfaceOffset.X);
            var top = Math.Max(0, _surfaceOffset.Y);
            var right = Math.Min(ClientSize.Width, _surfaceOffset.X + _surfaceSize.Width);
            var bottom = Math.Min(ClientSize.Height, _surfaceOffset.Y + _surfaceSize.Height);

            return new Rectangle(left, top, right - left, bottom - top);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.BlueViolet);

            var bounds = GetSurfaceBounds();

            e.Graphics.FillRectangle(Brushes.Azure, bounds);

            // draw grid
            if (true)
            {
                for (int w = _surfaceOffset.X; w < _surfaceOffset.X + _surfaceSize.Width; w += 16)
                {
                    e.Graphics.DrawLine(Pens.Lime, w, _surfaceOffset.Y, w, _surfaceOffset.Y + _surfaceSize.Height);
                }

                for (int h = _surfaceOffset.Y; h < _surfaceOffset.Y + _surfaceSize.Height; h += 16)
                {
                    e.Graphics.DrawLine(Pens.Lime, _surfaceOffset.X, h, _surfaceOffset.X + _surfaceSize.Width, h);
                }
            }

            if (_circuitComponents != null)
            {
                foreach (var component in _circuitComponents)
                {
                    var pos = component.Position;
                    pos.Offset(bounds.Left, bounds.Top);

                    var rect = new Rectangle(pos, component.Size);

                    if (!IsOutsideRect(bounds, rect))
                    {
                        component.Render(_resourcePool, e.Graphics, bounds);
                        if (component == SelectedComponent)
                        {
                            rect.Inflate(-1, -1);
                            e.Graphics.DrawRectangle(Pens.Orange, rect);
                        }
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            var bounds = GetSurfaceBounds();

            for (var node = _circuitComponents.Last; node != null; node = node.Previous)
            {
                var rect = new Rectangle(Point.Add(node.Value.Position, new Size(bounds.Left, bounds.Top)), node.Value.Size);
                if (rect.Contains(e.Location))
                {
                    SelectedComponent = node.Value;
                    if (e.Button == MouseButtons.Left)
                    {
                        _isDragging = true;
                        _dragOrigin = new Point(e.Location.X - node.Value.Position.X, e.Location.Y - node.Value.Position.Y);
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        if (_componentMenu != null)
                        {
                            foreach (MenuItem item in _componentMenu.MenuItems)
                            {
                                if (item != null)
                                    item.Dispose();
                            }
                            _componentMenu.Dispose();
                        }

                        _componentMenu = new ContextMenu();
                        _componentMenu.MenuItems.Add(node.Value.Name);
                        _componentMenu.MenuItems.Add("-");
                        _componentMenu.MenuItems.Add(new MenuItem("Rotate", (sender, ev) =>
                            {
                                node.Value.Rotate();
                                Invalidate();
                            }));
                        _componentMenu.MenuItems.Add(new MenuItem("Delete", (sender, ev) =>
                            {
                                if (BaseForm.MsgWarn(
                                    "Are you sure you wish to remove this component?\nThis action cannot be undone.",
                                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    if (RequestDeleteComponent != null)
                                        RequestDeleteComponent(node.Value);
                                }
                            }));
                        _componentMenu.Show(this, new Point(e.Location.X, e.Location.Y));
                    }

                    return;
                }
            }

            SelectedComponent = null;

            if (e.Button == MouseButtons.Middle)
            {
                _isPanning = true;
                _dragOrigin = new Point(e.Location.Y - _surfaceOffset.X, e.Location.Y - _surfaceOffset.Y);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _isDragging = false;
            _isPanning = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var bounds = GetSurfaceBounds();

            if (_isDragging &&
                SelectedComponent != null)
            {
                var newPt = new Point(e.Location.X - _dragOrigin.X, e.Location.Y - _dragOrigin.Y);

                SelectedComponent.Position = newPt;
                Invalidate();
            }
            else if (_isPanning)
            {
                _surfaceOffset = new Point(e.Location.X - _dragOrigin.X, e.Location.Y - _dragOrigin.Y);
                Invalidate();
            }
        }

        private bool IsOutsideRect(Rectangle boundary, Rectangle rect)
        {
            if (rect.Right < boundary.Left ||
                rect.Bottom < boundary.Top ||
                rect.Left > boundary.Right ||
                rect.Top > boundary.Bottom)
                return true;

            return false;
        }
    }
}
