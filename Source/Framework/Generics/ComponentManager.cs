using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CircuitDesign.Framework.Generics
{
    public sealed class ComponentManager
    {
        private List<ComponentResource> _components = new List<ComponentResource>();

        public IEnumerable<ComponentResource> Components { get { return _components; } }

        public void LoadDirectory(string directory)
        {
            var rootDirectory = new DirectoryInfo(directory);
            foreach (var dir in rootDirectory.GetDirectories())
            {
                var files = dir.GetFiles();
                var imageFile = files.Where((a) => string.Compare(a.Name, "component.png", true) == 0).FirstOrDefault();
                var xmlFile = files.Where((a) => string.Compare(a.Name, "component.xml", true) == 0).FirstOrDefault();

                if (imageFile != null &&
                    xmlFile != null)
                {
                    _components.Add(ComponentResource.Create(imageFile.FullName, xmlFile.FullName));
                }
            }
        }
    }
}
