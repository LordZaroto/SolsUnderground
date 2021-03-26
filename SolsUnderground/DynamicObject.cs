using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


//GameObject that can move
//Abstract class that holds a Vector2 object
//Any object that will be able to move will inherit from this class
namespace SolsUnderground
{
    abstract class DynamicObject : GameObject
    {
        protected Vector2 position; 

        //properties
        public abstract float X
        {
            get;
            set;
        }

        public abstract float Y
        {
            get;
            set;
        }

        /// <summary>
        /// to be overridden with movement AI for enemies or controls for the player
        /// </summary>
        public abstract void Move();
    }
}
