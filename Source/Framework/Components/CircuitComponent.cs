using CircuitDesign.Framework.Generics;
using CircuitDesign.Framework.Render;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitDesign.Framework.Components
{
    public class CircuitComponent : BaseComponent
    {
        public CircuitComponent(ComponentResource resource, string name) :
            base(resource, name, new Size(resource.Width, resource.Height))
        {
        }

        public override void Render(ResourcePool resource, Graphics g, Rectangle bounds)
        {
            var destinationPoints = new Point[3];
            int j = 0;

            switch (Rotation)
            {
                case ComponentRotation.Normal:
                    g.DrawImage(Resource.Image, new Rectangle(Point.Add(Position, new Size(bounds.Left, bounds.Top)), Size));
                    break;
                case ComponentRotation.R90:
                    destinationPoints[j++] = new Point(bounds.Left + Position.X + Size.Width, bounds.Top + Position.Y);
                    destinationPoints[j++] = new Point(bounds.Left + Position.X + Size.Width, bounds.Top + Position.Y + Size.Height);
                    destinationPoints[j++] = new Point(bounds.Left + Position.X, bounds.Top + Position.Y);
                    g.DrawImage(Resource.Image, destinationPoints);
                    break;
                case ComponentRotation.R180:
                    destinationPoints[j++] = new Point(bounds.Left + Position.X + Size.Width, bounds.Top + Position.Y + Size.Height);
                    destinationPoints[j++] = new Point(bounds.Left + Position.X, bounds.Top + Position.Y + Size.Height);
                    destinationPoints[j++] = new Point(bounds.Left + Position.X + Size.Width, bounds.Top + Position.Y);
                    g.DrawImage(Resource.Image, destinationPoints);
                    break;
                case ComponentRotation.R270:
                    destinationPoints[j++] = new Point(bounds.Left + Position.X, bounds.Top + Position.Y + Size.Height);
                    destinationPoints[j++] = new Point(bounds.Left + Position.X, bounds.Top + Position.Y);
                    destinationPoints[j++] = new Point(bounds.Left + Position.X + Size.Width, bounds.Top + Position.Y + Size.Height);
                    g.DrawImage(Resource.Image, destinationPoints);
                    break;
            }

            //if (Rotated)
            //{
            /*var mat = new Matrix();
            mat.Translate(-(float)Size.Width / 2, -(float)Size.Height / 2);
            mat.Rotate(90);
            var pt = new PointF(mat.OffsetX, mat.OffsetY);*/

            /*var destinationPoints = new Point[]
                {
                    new Point(Position.X + Size.Width, Position.Y),
                    new Point(Position.X + Size.Width, Position.Y + Size.Height),
                    new Point(Position.X, Position.Y)
                };

            g.DrawImage(_resource.Image, destinationPoints);*/

            g.ResetTransform();

            if (!string.IsNullOrWhiteSpace(Name))
            {
                var size = g.MeasureString(Name, resource.ComponentNameFont);
                g.DrawString(Name, resource.ComponentNameFont, Brushes.Black, bounds.Left + Position.X + (float)Size.Width / 2 - size.Width / 2, bounds.Top + Position.Y - size.Height);
            }
        }
    }
}
