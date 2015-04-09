using CircuitDesign.Framework.Components;
using CircuitDesign.Framework.Forms;
using CircuitDesign.Framework.Generics;
using System;
using System.Collections.Generic;
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

#if DEBUG
            var componentDirectory = @"C:\Users\Nicco\Desktop\CircuitDesign\Components";
#else // DEBUG
            var componentDirectory = "Components";
#endif // DEBUG

            try
            {
                componentManager.LoadDirectory(componentDirectory);
            }
            catch (Exception ex)
            {
                BaseForm.MsgError(string.Format("Could not load directory '{0}'", componentDirectory));
            }

            using (var form = new MainForm(componentManager))
            {
                Application.Run(form);
            }
        }
    }
}
