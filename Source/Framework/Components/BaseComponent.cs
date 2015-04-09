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
    public abstract class BaseComponent : ICircuitComponent, IRenderable
    {
        public Point Position { get; set; }
        public Size Size { get; protected set; }

        protected BaseComponent(Size size)
        {
            Size = size;
            Position = new Point(0, 0);
        }

        public virtual void Render(ResourcePool resource, Graphics g)
        {
        }
    }
}
