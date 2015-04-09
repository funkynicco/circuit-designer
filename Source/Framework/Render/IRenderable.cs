using CircuitDesign.Framework.Generics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitDesign.Framework.Render
{
    public interface IRenderable
    {
        void Render(ResourcePool resource, Graphics g);
    }
}
