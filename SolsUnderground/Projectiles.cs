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
        EnemyState enemy;
        PlayerState player;

        public Projectiles(Rectangle hitbox, int damage, int knockback, EnemyState enemy, PlayerState player)
            :base(hitbox, damage, knockback)
        {
            this.enemy = enemy;
            this.player = player;
            this.hitbox = hitbox;
            this.damage = damage;
            this.knockback = knockback;

        }
        public void MovingHitbox()
        {
            if(enemy != null)
            {
                if (enemy == EnemyState.attackForward)
                    hitbox.X += 1;
                if (enemy == EnemyState.attackBack)
                    hitbox.X -= 1;
                if (enemy == EnemyState.attackLeft)
                    hitbox.X -= 1;
                if (enemy == EnemyState.attackRight)
                    hitbox.X += 1;
            }
            if(player != null)
            {
                if (player == PlayerState.attackForward)
                    hitbox.X += 1;
                if (player == PlayerState.attackBack)
                    hitbox.X -= 1;
                if (player == PlayerState.attackLeft)
                    hitbox.X -= 1;
                if (player == PlayerState.attackRight)
                    hitbox.X += 1;
            }

        }



    }
}
