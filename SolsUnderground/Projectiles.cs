using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    class Projectiles: Attack
    {
        Rectangle hitbox;
        int damage;
        int knockback;
        AttackDirection attack;

        public Projectiles(Rectangle hitbox, int damage, int knockback, Texture2D texture, AttackDirection attack, double timer)
            :base(hitbox, damage, knockback, texture, attack, timer)
        {
            this.hitbox = hitbox;
            this.attack = attack;

        }

        public void MovingHitbox()
        {
                if (attack == AttackDirection.down)
                    hitbox.Y += 1;
                if (attack == AttackDirection.up)
                    hitbox.Y -= 1;
                if (attack == AttackDirection.left)
                    hitbox.X -= 1;
                if (attack == AttackDirection.right)
                    hitbox.X += 1;

        }
    }
}
