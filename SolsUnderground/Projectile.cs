using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    class Projectile: Attack
    {
        private int speed;

        public Projectile(Rectangle hitbox, int damage, int speed, int knockback, 
            Texture2D texture, AttackDirection attack, bool isPlayerAttack, StatusEffect effect)
            :base(hitbox, damage, knockback, texture, attack, float.MaxValue, isPlayerAttack, effect)
        {
            this.speed = speed;
        }

        public void Move()
        {
            if (attackDirection == AttackDirection.down)
                positionRect.Y += speed;
            if (attackDirection == AttackDirection.up)
                positionRect.Y -= speed;
            if (attackDirection == AttackDirection.left)
                positionRect.X -= speed;
            if (attackDirection == AttackDirection.right)
                positionRect.X += speed;
        }
    }
}
