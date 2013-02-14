using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame
{
    class TheHead
    {
        public Rectangle SourceBox;
        public Rectangle DrawBox;
        public Rectangle CollisionBox
        {
            get { return new Rectangle(DrawBox.X + 10, DrawBox.Y + 10, DrawBox.Width - 20, DrawBox.Height - 20); }
        }
        public Texture2D Head;

        public bool Hit = false;
    }
}
