using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


//GameObject that can move
//Abstract class that holds a rectangle object
//Any object that will be able to move will inherit from this class
namespace SolsUnderground
{
    abstract class DynamicObject : GameObject
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

        /// <summary>
        /// Draw the object on screen.
        /// </summary>
        /// <param name="sb"></param>
        public abstract void Draw(SpriteBatch sb);
    }
}
