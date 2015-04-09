using CircuitDesign.Framework.Components;
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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            var random = new Random(Environment.TickCount);

            var list = new List<TextComponent>();
            for (int i = 0; i < 10; ++i)
            {
                var pos = new Point(
                    random.Next(renderSurface.ClientSize.Width),
                    random.Next(renderSurface.ClientSize.Height));

                list.Add(new TextComponent(pos, "hello world"));
            }
            renderSurface.CircuitComponents = list;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check for changes made

            Close();
        }
    }
}
