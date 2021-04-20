using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    public enum AttackDirection
    {
        up,
        down,
        right,
        left
    }
    
    /// <summary>
    /// Holds a hitbox, damage, and knockback value.
    /// </summary>
    class Attack
    {
        private Rectangle hitbox;
        private int knockback;
        private int damage;
        private Texture2D texture;
        private AttackDirection attackDirection;
        private double timer;

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

        public Attack(Rectangle hitbox, int damage, int knockback, Texture2D texture, AttackDirection atkDir, double timer)
        {
            this.hitbox = hitbox;
            this.damage = damage;
            this.knockback = knockback;
            this.texture = texture;
            this.timer = timer;
            attackDirection = atkDir;
        }
    }
}
