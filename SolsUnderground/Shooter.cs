﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    class Shooter : Enemy
    {
        private EnemyState enemyState;
        private double moveCounter;
        private double moveCD;
        private double attackCounter;
        private double attackCD;
        private double kbCounter;
        private double kbCD;
        Texture2D[] textures;
        //consructor: initializes the fields
        public Shooter(Texture2D[] textures, Rectangle positionRect, int health, int attack)
        {
            this.textures = textures;
            this.texture = textures[0];
            this.positionRect = positionRect;
            this.maxHP = health;
            this.currentHP = health;
            this.attack = attack;
            this.knockback = 32;
            moveCD = 0.3;
            moveCounter = moveCD;
            kbCD = 0.1;
            kbCounter = kbCD;
            attackCD = 1.7;
            attackCounter = attackCD;
        }

        //properties
        public override int X
        {
            get { return positionRect.X; }
            set { positionRect.X = value; }
        }

        public override int Y
        {
            get { return positionRect.Y; }
            set { positionRect.Y = value; }
        }

        public override int Health
        {
            get { return currentHP; }
            set { currentHP = value; }
        }

        public override int Attack
        {
            get { return attack; }
            set { attack = value; }
        }

        public override int Knockback
        {
            get { return knockback; }
            set { knockback = value; }
        }

        public override int Width
        {
            get { return positionRect.Width; }
            set { positionRect.Width = value; }
        }

        public override int Height
        {
            get { return positionRect.Height; }
            set { positionRect.Height = value; }
        }

        public override Rectangle PositionRect
        {
            get { return positionRect; }
            set { positionRect = value; }
        }

        public override EnemyState State
        {
            get { return enemyState; }
            set { enemyState = value; }
        }

        /// <summary>
        /// overridden method
        /// changes health when hit by the player
        /// </summary>
        /// <param name="damage"></param>
        public override void TakeDamage(int damage, int knockback)
        {
            if (!(enemyState == EnemyState.dead))
            {
                moveCounter = 0;

                currentHP -= damage;

                //Shooters should be stationary and do not get knocked back

                if (currentHP <= 0)
                {
                    enemyState = EnemyState.dead;
                }
            }
        }

        /// <summary>
        /// movement AI that will chase the player
        /// </summary>
        public override void EnemyMove(Player player, GameTime gameTime)
        {
            //Update the cooldowns
            moveCounter += gameTime.ElapsedGameTime.TotalSeconds;
            attackCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (moveCounter >= moveCD)
            {
                if (!(enemyState == EnemyState.dead))
                {
                    if (Math.Abs(positionRect.X - player.X) >= Math.Abs(positionRect.Y - player.Y))
                    {
                        if (positionRect.X >= player.X)
                        {
                            texture = textures[2];
                            enemyState = EnemyState.moveLeft;
                        }
                        else
                        {
                            texture = textures[3];
                            enemyState = EnemyState.moveRight;
                        }
                    }
                    else if (Math.Abs(positionRect.X - player.X) < Math.Abs(positionRect.Y - player.Y))
                    {
                        if (positionRect.Y >= player.Y)
                        {
                            texture = textures[1];
                            enemyState = EnemyState.moveForward;
                        }
                        else
                        {
                        texture = textures[0];
                            enemyState = EnemyState.moveBack;
                        }
                    }
                }
            }
        }

        public override List<Attack> EnemyAttack(Player player)
        {
            List<Attack> attacks = new List<Attack>();
            AttackDirection direction = AttackDirection.left;

            // Check direction for collision hitbox
            switch (enemyState)
            {
                case EnemyState.faceForward:
                case EnemyState.moveForward:
                    direction = AttackDirection.up;
                    break;

                case EnemyState.faceLeft:
                case EnemyState.moveLeft:
                    direction = AttackDirection.left;
                    break;

                case EnemyState.faceBack:
                case EnemyState.moveBack:
                    direction = AttackDirection.down;
                    break;

                case EnemyState.faceRight:
                case EnemyState.moveRight:
                    direction = AttackDirection.right;
                    break;
            }

            // Shoot projectile when not on cooldown
            if (attackCounter >= attackCD)
            {
                attackCounter -= attackCD;

                attacks.Add(new Projectile(positionRect, attack, 3, knockback, texture, direction, false));
            }

            attacks.Add(new Attack(PositionRect, attack, knockback, null, direction, 0.15, false));

            return attacks;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, positionRect, Color.White);

            // Draw HP bar
            sb.Draw(Program.drawSquare,
                new Rectangle(X, Y - 10, Width, 3),
                Color.Black);
            sb.Draw(Program.drawSquare,
                new Rectangle(X, Y - 10, (int)(Width * ((double)currentHP / (double)maxHP)), 3),
                Color.Red);
        }
    }
}
