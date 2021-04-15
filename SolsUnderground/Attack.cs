using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    /// <summary>
    /// Holds a hitbox, damage, and knockback value.
    /// </summary>
    class Attack
    {
        private Rectangle hitbox;
        private int knockback;
        private int damage;

        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        public int Knockback
        {
            get { return knockback; }
        }

        public int Damage
        {
            get { return damage; }
        }

        public Attack(Rectangle hitbox, int damage, int knockback)
        {
            this.hitbox = hitbox;
            this.damage = damage;
            this.knockback = knockback;
        }
    }
}
