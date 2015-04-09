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
        private readonly ComponentResource _resource;

        public ComponentResource Resource { get { return _resource; } }

        public LinkedListNode<BaseComponent> MyNode { get; set; }
        public string Name { get; set; }
        public Point Position { get; set; }
        public Size Size { get; set; }
        public ComponentRotation Rotation { get; private set; }

        protected BaseComponent(ComponentResource resource, string name, Size size)
        {
            _resource = resource;
            Name = name;
            Size = size;
            Position = new Point(0, 0);
            Rotation = ComponentRotation.Normal;
        }

        public override string ToString()
        {
            return Name;
        }

        public void Rotate()
        {
            byte by = (byte)Rotation;
            if (++by > (byte)ComponentRotation.R270)
                by = 0;
            
            Rotate((ComponentRotation)by);
        }

        public void Rotate(ComponentRotation rotation)
        {
            Rotation = rotation;

            switch (Rotation)
            {
                case ComponentRotation.Normal:
                case ComponentRotation.R180:
                    Size = new Size(Resource.Width, Resource.Height);
                    break;
                case ComponentRotation.R90:
                case ComponentRotation.R270:
                    Size = new Size(Resource.Height, Resource.Width);
                    break;
            }
        }

        public virtual void Render(ResourcePool resource, Graphics g, Rectangle bounds)
        {
        }
    }

    public enum ComponentRotation : byte
    {
        Normal = 0,
        R90,
        R180,
        R270
    }
}
