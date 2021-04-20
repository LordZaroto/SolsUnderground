using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    //Preston Gilmore
    //Braden Flanders

    /// <summary>
    /// Weapons have different attack stats and unique abilities.
    /// Basic Attacks and ablities will have cooldowns.
    /// </summary>
    interface  Weapon
    {
        //Properties
        //----------------------------------------
        int X { get; set; }

        int Y { get; set; }

        int Width { get; set; }

        int Height { get; set; }

        Texture2D Sprite { get; }

        string Name { get; }

        Rectangle Position { get; set; }

        Rectangle HitboxF { get; set; }

        Rectangle HitboxL { get; set; }

        Rectangle HitboxB { get; set; }

        Rectangle HitboxR { get; set; }

        /// <summary>
        /// Base damage dealt to enemies with this weapon
        /// </summary>
        int Attack { get; set; }
        
        /// <summary>
        /// Minimum time that must pass between each Basic Attack
        /// </summary>
        double BasicCooldown { get; }

        /// <summary>
        /// Minimum time that must pass between each Special Ability use
        /// </summary>
        double SpecialCooldown { get; }

        /// <summary>
        /// A multiplier for how far enemies will be knocked back
        /// </summary>
        int Knockback { get; }

        /// <summary>
        /// The amount of time that the Hitbox lingers for
        /// </summary>
        double Timer { get; set; }
        //----------------------------------------

        //Methods
        //----------------------------------------------------------

        void Draw(SpriteBatch sb);

        /// <summary>
        /// Will be overridden by each weapon type.
        /// </summary>
        Attack Special(Player player);

        /// <summary>
        /// Returns the basic attack hit box
        /// </summary>
        Rectangle GetHitbox(int x, int y, int width, int height, PlayerState state);
    }
}
