using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CircuitDesign.Framework.Generics
{
    public sealed class ComponentResource : IDisposable
    {
        public string Name { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Image Image { get; private set; }

        private ComponentResource()
        {
            Image = null;
        }

        public void Dispose()
        {
            if (Image != null)
                Image.Dispose();
        }

        public static ComponentResource Create(string imageFile, string xmlFile)
        {
            var resource = new ComponentResource();

            resource.Image = Image.FromFile(imageFile);

            var doc = new XmlDocument();
            doc.Load(xmlFile);

            var name = doc.SelectSingleNode("Component/Name");
            var width = doc.SelectSingleNode("Component/Width");
            var height = doc.SelectSingleNode("Component/Height");

            if (name != null &&
                width != null &&
                height != null)
            {
                resource.Name = name.InnerText;
                resource.Width = int.Parse(width.InnerText);
                resource.Height = int.Parse(height.InnerText);
            }

            return resource;
        }
    }
}
