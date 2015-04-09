using CircuitDesign.Framework.Components;
using CircuitDesign.Framework.Forms;
using CircuitDesign.Framework.Generics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CircuitDesign
{
    public partial class MainForm : BaseForm
    {
        private readonly LinkedList<BaseComponent> _components;

        public MainForm(ComponentManager componentManager)
        {
            InitializeComponent();
            Text = Program.Caption;

            var random = new Random(Environment.TickCount);

            _components = new LinkedList<BaseComponent>();

            var com = new CircuitComponent(componentManager.Components.Skip(1).FirstOrDefault(), "test");
            com.Name += " - " + com.Resource.Name;
            com.Position = new Point(schematicSurface.ClientSize.Width / 2, schematicSurface.ClientSize.Height / 2);
            com.MyNode = _components.AddLast(com);

            /*int j = 1;
            for (int i = 0; i < 10; ++i)
            {
                foreach (var res in componentManager.Components)
                {
                    var pos = new Point(
                    random.Next(schematicSurface.ClientSize.Width),
                    random.Next(schematicSurface.ClientSize.Height));

                    var com = new CircuitComponent(string.Format("#{0} - ", j++), res);
                    com.Name += com.Resource.Name;
                    com.Position = pos;
                    com.MyNode = _components.AddLast(com);
                }
            }*/

            schematicSurface.CircuitComponents = _components;

            schematicSurface.ComponentSelectionChanged += schematicSurface_ComponentSelectionChanged;
            schematicSurface.RequestDeleteComponent += schematicSurface_RequestDeleteComponent;
        }

        void schematicSurface_RequestDeleteComponent(BaseComponent component)
        {
            if (component != null)
            {
                if (_components.Remove(component))
                    schematicSurface.Invalidate();
            }
        }

        void schematicSurface_ComponentSelectionChanged(BaseComponent component)
        {
            if (component != null &&
                component != _components.Last.Value &&
                _components.Count > 1)
            {
                // swap the node with the last in order to bring the component to the front (the last is the front in linked list)
                var lastNode = _components.Last;
                _components.Remove(lastNode);
                _components.AddBefore(component.MyNode, lastNode);
                _components.Remove(component.MyNode);
                component.MyNode = _components.AddLast(component);

                // update interface, if having a listbox ..
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Delete &&
                schematicSurface.SelectedComponent != null &&
                MsgWarn(string.Format(
                "Are you sure you wish to remove component '{0}'?\nThis action cannot be undone.",
                schematicSurface.SelectedComponent.Name), MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var item = schematicSurface.SelectedComponent;
                schematicSurface.SelectedComponent = null;

                // update interface

                _components.Remove(item.MyNode);
                schematicSurface.Invalidate();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check for changes made

            Close();
        }
    }
}
