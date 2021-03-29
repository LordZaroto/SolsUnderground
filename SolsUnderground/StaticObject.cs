using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    abstract class StaticObject : GameObject
    {
        protected Rectangle positionRect;

        //properties
        public abstract int X
        {
            get;
            set;
        }

        public abstract int Y
        {
            get;
            set;
        }
        public abstract int Width
        {
            get;
            set;
        }

        public abstract int Height
        {
            get;
            set;
        }
    }
}
