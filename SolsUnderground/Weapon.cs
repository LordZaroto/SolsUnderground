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
        /// A multiplier for how far enemies will be knocked back
        /// </summary>
        int Knockback { get; }
        //----------------------------------------

        //Methods
        //----------------------------------------------------------

        void Draw(SpriteBatch sb);

        /// <summary>
        /// Will be overridden by each weapon type.
        /// </summary>
        void Special();

        /// <summary>
        /// Returns the basic attack hit box
        /// </summary>
        Rectangle GetHitbox(int x, int y, int width, int height, PlayerState state);
    }
}
