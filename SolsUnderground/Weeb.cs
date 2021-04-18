using System;
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
        Texture2D[] textures;
        //consructor: initializes the fields
        public Weeb(Texture2D[] textures, Rectangle positionRect, int health, int attack)
        {
            this.textures = textures;
            this.texture = textures[0];
            this.positionRect = positionRect;
            this.health = health;
            this.attack = attack;
            this.knockback = 32;
            moveCD = 0.3;
            moveCounter = moveCD;
            kbCD = 0.1;
            kbCounter = kbCD;
            sp1CD = 7;
            sp1Counter = sp1CD;
            sp2CD = 3.5;
            sp2Counter = sp2CD;
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
            get { return health; }
            set { health = value; }
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

                health -= damage;

                if (enemyState == EnemyState.faceForward || enemyState == EnemyState.moveForward)
                {
                    Y += knockback;
                }
                if (enemyState == EnemyState.faceLeft || enemyState == EnemyState.moveLeft)
                {
                    X += knockback;
                }
                if (enemyState == EnemyState.faceBack || enemyState == EnemyState.moveBack)
                {
                    Y -= knockback;
                }
                if (enemyState == EnemyState.faceRight || enemyState == EnemyState.moveRight)
                {
                    X -= knockback;
                }

                if (health <= 0)
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
                            enemyState = EnemyState.moveBack;
                        }
                        else
                        {
                            texture = textures[0];
                            positionRect.Y += 2;
                            enemyState = EnemyState.moveForward;
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
        public override Attack BossAttack(Player player)
        {
            //If close to player
            if(Math.Abs(X - player.X) < 30 || Math.Abs(Y - player.Y) < 30)
            {
                return DragonWrath();
            }
            else
            {
                return DragonDash();
            }
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

                    positionRect = new Rectangle(
                            X - (Width * (3 / 4)),
                            Y - (Height * 2 + (Height / 2)),
                            Width + (Width * 2 * (3 / 4)),
                            Height * 3 + (Height / 2));

                    Attack special = new Attack(
                        positionRect,
                        attack * 2,
                        knockback * 2);

                    Y -= Height * 2;
                    return special;
                }
                else if (State == EnemyState.faceLeft || State == EnemyState.moveLeft)
                {
                    State = EnemyState.attackLeft;

                    positionRect = new Rectangle(
                            X - (Width * 2 + (Width / 2)),
                            Y - (Height * (3 / 4)),
                            Width * 3 + (Width / 2),
                            Height + (Height * 2 * (3 / 4)));

                    Attack special = new Attack(
                        positionRect,
                        attack * 2,
                        knockback * 2);

                    X -= Width * 2;

                    return special;
                }
                else if (State == EnemyState.faceBack || State == EnemyState.moveBack)
                {
                    State = EnemyState.attackBack;

                    positionRect = new Rectangle(
                            X - (Width * (3 / 4)),
                            Y - (Height / 2),
                            Width + (Width * 2 * (3 / 4)),
                            Height * 3 + (Height / 2));

                    Attack special = new Attack(
                        positionRect,
                        attack * 2,
                        knockback * 2);

                    Y += Height * 2;

                    return special;
                }
                else if (State == EnemyState.faceRight || State == EnemyState.moveRight)
                {
                    State = EnemyState.attackRight;

                    positionRect = new Rectangle(
                            X - (Width / 2),
                            Y - (Height * (3 / 4)),
                            Width * 3 + (Width / 2),
                            Height + (Height * 2 * (3 / 4)));

                    Attack special = new Attack(
                        positionRect,
                        attack * 2,
                        knockback * 2);

                    X += Width * 2;

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

                //Create the attack hitbox in the direction the player is facing
                if (State == EnemyState.faceForward || State == EnemyState.moveForward)
                {
                    State = EnemyState.attackForward;

                    positionRect = new Rectangle(
                            X - (Width * (3 / 4)),
                            Y - (Height * 5 + (Height / 2)),
                            Width + (Width * 2 * (3 / 4)),
                            Height * 6 + (Height / 2));

                    Attack special = new Attack(
                        positionRect,
                        attack / 2,
                        knockback / 2);

                    Y -= Height * 5;
                    return special;
                }
                else if (State == EnemyState.faceLeft || State == EnemyState.moveLeft)
                {
                    State = EnemyState.attackLeft;

                    positionRect = new Rectangle(
                            X - (Width * 5 + (Width / 2)),
                            Y - (Height * (3 / 4)),
                            Width * 6 + (Width / 2),
                            Height + (Height * 2 * (3 / 4)));

                    Attack special = new Attack(
                        positionRect,
                        attack / 2,
                        knockback / 2);

                    X -= Width * 5;

                    return special;
                }
                else if (State == EnemyState.faceBack || State == EnemyState.moveBack)
                {
                    State = EnemyState.attackBack;

                    positionRect = new Rectangle(
                            X - (Width * (3 / 4)),
                            Y - (Height / 2),
                            Width + (Width * 2 * (3 / 4)),
                            Height * 6 + (Height / 2));

                    Attack special = new Attack(
                        positionRect,
                        attack / 2,
                        knockback / 2);

                    Y += Height * 5;

                    return special;
                }
                else if (State == EnemyState.faceRight || State == EnemyState.moveRight)
                {
                    State = EnemyState.attackRight;

                    positionRect = new Rectangle(
                            X - (Width / 2),
                            Y - (Height * (3 / 4)),
                            Width * 6 + (Width / 2),
                            Height + (Height * 2 * (3 / 4)));

                    Attack special = new Attack(
                        positionRect,
                        attack / 2,
                        knockback / 2);

                    X += Width * 5;

                    return special;
                }
            }

            return null;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, positionRect, Color.White);
        }
    }
}
