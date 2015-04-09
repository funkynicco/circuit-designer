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

namespace CircuitDesign.Controls
{
    public partial class RenderSurface : UserControl
    {
        private IEnumerable<BaseComponent> _circuitComponents = null;

        public IEnumerable<BaseComponent> CircuitComponents
        {
            get { return _circuitComponents; }
            set
            {
                _circuitComponents = value;
                Invalidate();
            }
        }

        private readonly ResourcePool _resourcePool = new ResourcePool();

        public RenderSurface()
        {
            InitializeComponent();
            DoubleBuffered = true;

            SizeChanged += (sender, e) => Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.BlueViolet);

            var bounds = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);

            foreach (var component in _circuitComponents)
            {
                var rect = new Rectangle(component.Position, component.Size);

                if (bounds.Contains(rect))
                    component.Render(_resourcePool, e.Graphics);
            }
        }
    }
}
