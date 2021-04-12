using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Abstract class that holds a texture
//Almost all classes will inherit from GameObject
namespace SolsUnderground
{
    //Braden Flanders
    abstract class GameObject
    {
        protected Texture2D texture;
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

        public abstract Rectangle PositionRect
        {
            get;
            set;
        }
    }
}
