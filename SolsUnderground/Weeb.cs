﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    //Preston Gilmore

    /// <summary>
    /// An avid anime fan who is cosplaying as an OP
    /// anime protagonist. He likes to LARP, and also
    /// happens to be very good at Kendo.
    /// </summary>
    class Weeb : Boss
    {
        private EnemyState enemyState;
        private double moveCounter;
        private double moveCD;
        private double kbCounter;
        private double kbCD;
        private double sp1Counter;
        private double sp1CD;
        private double sp2Counter;
        private double sp2CD;
        private double sp1HitTimer;
        private double sp2HitTimer;
        Texture2D[] textures;
        Texture2D atkTexture;
        //consructor: initializes the fields
        public Weeb(Texture2D[] textures, Rectangle positionRect, int health, int attack, Texture2D atkTexture)
        {
            this.textures = textures;
            this.texture = textures[0];
            this.atkTexture = atkTexture;
            this.positionRect = positionRect;
            this.maxHP = health;
            this.currentHP = health;
            this.attack = attack;
            this.knockback = 32;
            moveCD = 0.2;
            moveCounter = moveCD;
            kbCD = 0.1;
            kbCounter = kbCD;
            sp1CD = 7;
            sp1Counter = 6;
            sp2CD = 3.5;
            sp2Counter = 2;
            sp1HitTimer = 0.1;
            sp2HitTimer = 0.1;
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
                //moveCounter = 0; Testing no knockback on bosses

                currentHP -= damage;

                //Reduces knockback recieved by 50%
                if (enemyState == EnemyState.faceForward || enemyState == EnemyState.moveForward)
                {
                    Y += knockback / 2;
                }
                if (enemyState == EnemyState.faceLeft || enemyState == EnemyState.moveLeft)
                {
                    X += knockback / 2;
                }
                if (enemyState == EnemyState.faceBack || enemyState == EnemyState.moveBack)
                {
                    Y -= knockback / 2;
                }
                if (enemyState == EnemyState.faceRight || enemyState == EnemyState.moveRight)
                {
                    X -= knockback / 2;
                }

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
            sp1Counter += gameTime.ElapsedGameTime.TotalSeconds;
            sp2Counter += gameTime.ElapsedGameTime.TotalSeconds;
            moveCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (moveCounter >= moveCD)
            {
                if (!(enemyState == EnemyState.dead))
                {
                    if (Math.Abs(positionRect.X - player.X) >= Math.Abs(positionRect.Y - player.Y))
                    {
                        if (positionRect.X >= player.X)
                        {
                            texture = textures[2];
                            positionRect.X -= 2;
                            enemyState = EnemyState.moveLeft;
                        }
                        else
                        {
                            texture = textures[3];
                            positionRect.X += 2;
                            enemyState = EnemyState.moveRight;
                        }
                    }
                    else if (Math.Abs(positionRect.X - player.X) < Math.Abs(positionRect.Y - player.Y))
                    {
                        if (positionRect.Y >= player.Y)
                        {
                            texture = textures[1];
                            positionRect.Y -= 2;
                            enemyState = EnemyState.moveForward;
                        }
                        else
                        {
                            texture = textures[0];
                            positionRect.Y += 2;
                            enemyState = EnemyState.moveBack;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The Weeb will unleash different special moves according
        /// to his position in relation to the player.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override List<Attack> EnemyAttack(Player player)
        {
            List<Attack> attacks = new List<Attack>();
            AttackDirection direction = AttackDirection.left;

            if (moveCounter >= moveCD)
            {
                //If close to player
                if ((Math.Abs(X - player.X) < 50 && (State == EnemyState.moveRight || State == EnemyState.moveLeft))
                    || (Math.Abs(Y - player.Y) < 50 && (State == EnemyState.moveForward || State == EnemyState.moveBack)))
                {
                    attacks.Add(DragonWrath());
                }
                else
                {
                    attacks.Add(DragonDash());
                }
            }

            // Determine direction for collision attack
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
            attacks.Add(new Attack(PositionRect, attack, knockback, null, direction, 0.15, false));

            return attacks;
        }

        /// <summary>
        /// The Weeb lunges forward unleahing a devestating attack.
        /// </summary>
        /// <returns></returns>
        public Attack DragonWrath()
        {
            //If off cooldown
            if(sp1Counter >= sp1CD)
            {
                //Reset the cooldown
                sp1Counter = 0;
                moveCounter = 0;

                //Create the attack hitbox in the direction the player is facing
                if (State == EnemyState.faceForward || State == EnemyState.moveForward)
                {
                    State = EnemyState.attackForward;

                    Attack special = new Attack(
                        new Rectangle(
                            X - (Width * (3 / 4)),
                            Y - (Height + (Height / 2)),
                            Width + (Width * 2 * (3 / 4)),
                            Height * 2 + (Height / 2)),
                        attack * 3,
                        knockback * 3,
                        atkTexture,
                        AttackDirection.up,
                        sp1HitTimer,
                        false);

                    Y -= Height;
                    return special;
                }
                else if (State == EnemyState.faceLeft || State == EnemyState.moveLeft)
                {
                    State = EnemyState.attackLeft;

                    Attack special = new Attack(
                        new Rectangle(
                            X - (Width + (Width / 2)),
                            Y - (Height * (3 / 4)),
                            Width * 2 + (Width / 2),
                            Height + (Height * 2 * (3 / 4))),
                        attack * 3,
                        knockback * 3,
                        atkTexture,
                        AttackDirection.left,
                        sp1HitTimer,
                        false);

                    X -= Width;

                    return special;
                }
                else if (State == EnemyState.faceBack || State == EnemyState.moveBack)
                {
                    State = EnemyState.attackBack;

                    Attack special = new Attack(
                        new Rectangle(
                            X - (Width * (3 / 4)),
                            Y - (Height / 2),
                            Width + (Width * 2 * (3 / 4)),
                            Height * 2 + (Height / 2)),
                        attack * 3,
                        knockback * 3,
                        atkTexture,
                        AttackDirection.down,
                        sp1HitTimer,
                        false);

                    Y += Height;

                    return special;
                }
                else if (State == EnemyState.faceRight || State == EnemyState.moveRight)
                {
                    State = EnemyState.attackRight;

                    Attack special = new Attack(
                        new Rectangle(
                            X - (Width / 2),
                            Y - (Height * (3 / 4)),
                            Width * 2 + (Width / 2),
                            Height + (Height * 2 * (3 / 4))),
                        attack * 3,
                        knockback * 3,
                        atkTexture,
                        AttackDirection.right,
                        sp1HitTimer,
                        false);

                    X += Width;

                    return special;
                }
            }

            return null;
        }

        /// <summary>
        /// Dashes aggressively towards the player. Does damage if it connects.
        /// </summary>
        /// <returns></returns>
        public Attack DragonDash()
        {
            //If off cooldown
            if (sp2Counter >= sp2CD)
            {
                //Reset the cooldown
                sp2Counter = 0;
                moveCounter = 0;

                //Create the attack hitbox in the direction the player is facing
                if (State == EnemyState.faceForward || State == EnemyState.moveForward)
                {
                    State = EnemyState.attackForward;

                    Attack special = new Attack(
                        new Rectangle(
                            X - (Width * (3 / 4)),
                            Y - (Height * 3 + (Height / 2)),
                            Width + (Width * 2 * (3 / 4)),
                            Height * 4 + (Height / 2)),
                        attack / 2,
                        knockback / 2,
                        atkTexture,
                        AttackDirection.up,
                        sp1HitTimer,
                        false);

                    Y -= Height * 3;
                    return special;
                }
                else if (State == EnemyState.faceLeft || State == EnemyState.moveLeft)
                {
                    State = EnemyState.attackLeft;

                    Attack special = new Attack(
                        new Rectangle(
                            X - (Width * 3 + (Width / 2)),
                            Y - (Height * (3 / 4)),
                            Width * 4 + (Width / 2),
                            Height + (Height * 2 * (3 / 4))),
                        attack / 2,
                        knockback / 2,
                        atkTexture,
                        AttackDirection.left,
                        sp1HitTimer,
                        false);

                    X -= Width * 3;

                    return special;
                }
                else if (State == EnemyState.faceBack || State == EnemyState.moveBack)
                {
                    State = EnemyState.attackBack;

                    Attack special = new Attack(
                        new Rectangle(
                            X - (Width * (3 / 4)),
                            Y - (Height / 2),
                            Width + (Width * 2 * (3 / 4)),
                            Height * 4 + (Height / 2)),
                        attack / 2,
                        knockback / 2,
                        atkTexture,
                        AttackDirection.down,
                        sp1HitTimer,
                        false);

                    Y += Height * 3;

                    return special;
                }
                else if (State == EnemyState.faceRight || State == EnemyState.moveRight)
                {
                    State = EnemyState.attackRight;

                    Attack special = new Attack(
                        new Rectangle(
                            X - (Width / 2),
                            Y - (Height * (3 / 4)),
                            Width * 4 + (Width / 2),
                            Height + (Height * 2 * (3 / 4))),
                        attack / 2,
                        knockback / 2,
                        atkTexture,
                        AttackDirection.right,
                        sp1HitTimer,
                        false);

                    X += Width * 3;

                    return special;
                }
            }

            return null;
        }

        public override void Draw(SpriteBatch sb)
        {
            // Use draw colors to give player attack tells
            if (sp2Counter > sp2CD - 1)
            {
                sb.Draw(texture, positionRect, Color.Purple);
            }
            else if (sp1Counter > sp1CD - 1)
            {
                sb.Draw(texture, positionRect, Color.Green);
            }
            else
            {
                sb.Draw(texture, positionRect, Color.White);
            }

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
