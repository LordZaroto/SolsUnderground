﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    abstract class StaticObject : GameObject
    {
        abstract public void Draw(SpriteBatch sb);
    }
}
