using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Alex Dale
/// 
/// This interface defines Armor items which can be equipped by the player
/// to affect their max health, defense, or movement speed.
/// 
/// </summary>

namespace SolsUnderground
{
    interface Armor
    {
        // Properties
        int X { get; set; }

        int Y { get; set; }

        int Width { get; set; }

        int Height { get; set; }

        Texture2D Sprite { get; }

        Rectangle Position { get; set; }

        int Defense { get; }

        int Speed { get; }

        int HP { get; }

        // Methods

        void Draw(SpriteBatch sb);
    }
}
