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
    abstract class GameObject
    {
        protected Texture2D texture;
    }
}
