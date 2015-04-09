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
    public class TextComponent : BaseComponent
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public TextComponent(Point pos) :
            this(pos, string.Empty)
        {
        }

        public TextComponent(Point pos, string text) :
            base(new Size(0, 0))
        {
            Position = pos;
            _text = text;
        }

        public override void Render(ResourcePool resource, Graphics g)
        {
            g.DrawString(_text, resource.TextFont, Brushes.White, base.Position);
        }
    }
}
