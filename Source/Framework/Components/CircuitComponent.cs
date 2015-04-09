using CircuitDesign.Framework.Generics;
using CircuitDesign.Framework.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitDesign.Framework.Components
{
    public class CircuitComponent : BaseComponent
    {
        public CircuitComponent() :
            base(new Size(1, 1))
        {

        }

        public override void Render(ResourcePool resource, Graphics g)
        {
        }
    }
}
