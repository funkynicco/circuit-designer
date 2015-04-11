using CircuitDesign.Framework.Components;
using CircuitDesign.Framework.Forms;
using CircuitDesign.Framework.Generics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CircuitDesign
{
    static class Program
    {
        public const string Caption = "Circuit Designer";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var componentManager = new ComponentManager();

            try
            {
                componentManager.LoadDirectory(Configuration.ComponentsDirectory);
            }
            catch
            {
                var fullPath = Path.IsPathRooted(Configuration.ComponentsDirectory) ?
                    Configuration.ComponentsDirectory :
                    Path.GetFullPath(Configuration.ComponentsDirectory);

                BaseForm.MsgError(string.Format("Could not load directory '{0}'", fullPath));
                return;
            }

            using (var form = new MainForm(componentManager))
            {
                Application.Run(form);
            }
        }
    }
}
