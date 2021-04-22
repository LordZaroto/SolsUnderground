using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    class VendingMachineBoss : Boss
    {
        //fields
        private EnemyState enemyState;
        private double moveCounter;
        private double moveCD;
        private double kbCounter;
        private double kbCD;
        Texture2D[] textures;
        Texture2D atkTexture;
        GameTime gameTime;
        float _timer;
        float _AOETimer;
        float _attackTimer;

        bool movingUp;

        private double sp1Counter;
        private double sp1CD;
        private double sp2Counter;
        private double sp2CD;
        private double sp1HitTimer;
        private double sp2HitTimer;

        //constructor
        public VendingMachineBoss(Texture2D[] textures, Rectangle positionRect, int health, int attack, Texture2D atkTexture)
        {
            this.textures = textures;
            this.texture = textures[0];
            this.atkTexture = atkTexture;
            this.positionRect = positionRect;
            this.maxHP = health;
            this.currentHP = maxHP;
            this.attack = attack;
            this.knockback = 64;
            moveCD = 2;
            moveCounter = moveCD;
            kbCD = 2;
            kbCounter = kbCD;
            movingUp = true;
            _timer = 0f;
            _attackTimer = 0f;
            float _AOETimer = 0f;


            sp1CD = 5;
            sp1Counter = 3;
            //sp2Counter stuff
            sp1HitTimer = 0.1;
            sp2HitTimer = 0.1;
        }


        //properties
        public int MaxHealth
        {
            get;
        }

        public override int Health
        {
            get;
            set;
        }

        public override int Attack
        {
            get;
            set;
        }

        public override int Knockback
        {
            get;
            set;
        }

        public override EnemyState State
        {
            get { return enemyState; }
            set { enemyState = value; }
        }

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

        public bool IsAOE
        {
            get;
            set;
        }

        public bool IsShoot
        {
            get;
            set;
        }

        /// <summary>
        /// overriden take damage method - no knockback
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="knockback"></param>
        public override void TakeDamage(int damage, int knockback)
        {
            if (!(enemyState == EnemyState.dead))
            {
                //moveCounter = 0;

                currentHP -= damage;

                /*if (enemyState == EnemyState.faceForward || enemyState == EnemyState.moveForward)
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
                }*/

                if (currentHP <= 0)
                {
                    enemyState = EnemyState.dead;
                }
            }
        }

        /// <summary>
        /// moves in 3 stages
        /// 1 - up and down slowly
        /// 2 - up and down faster
        /// 3 - chases player
        /// </summary>
        /// <param name="player"></param>
        /// <param name="gameTime"></param>
        public override void EnemyMove(Player player, GameTime gameTime)
        {
            //Update the cooldowns
            moveCounter += gameTime.ElapsedGameTime.TotalSeconds;
            sp1Counter += gameTime.ElapsedGameTime.TotalSeconds;
            sp2Counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (moveCounter >= moveCD)
            {
                if (!(enemyState == EnemyState.dead))
                {
                    if (currentHP > maxHP / 3 * 2)
                    {

                        if (_timer > 2)
                        {
                            _timer = 0;
                            if (movingUp)
                            {
                                movingUp = false;
                            }
                            else
                            {
                                movingUp = true;
                            }
                        }
                        if (movingUp)
                        {
                            positionRect.Y -= 1;
                        }
                        else
                        {
                            positionRect.Y += 1;
                        }
                        
                     
                    }
                    else if (currentHP <= maxHP / 3 * 2 && currentHP > maxHP / 3)
                    {
                     
                        if (movingUp)
                        {
                            positionRect.Y -= 2;
                        }
                        else
                        {
                            positionRect.Y += 2;
                        }
                        if (_timer > 2)
                        {
                            _timer = 0;
                            if (movingUp)
                            {
                                movingUp = false;
                            }
                            else
                            {
                                movingUp = true;
                            }
                        }
                     
                    }
                    else
                    {
                        
                            if (Math.Abs(positionRect.X - player.X) >= Math.Abs(positionRect.Y - player.Y))
                            {
                                if (positionRect.X >= player.X)
                                {
                                texture = textures[2];
                                    positionRect.X -= 3;
                                    enemyState = EnemyState.moveLeft;
                                }
                                else
                                {
                                texture = textures[3];
                                positionRect.X += 3;
                                    enemyState = EnemyState.moveRight;
                                }
                            }
                            else if (Math.Abs(positionRect.X - player.X) < Math.Abs(positionRect.Y - player.Y))
                            {
                                if (positionRect.Y >= player.Y)
                                {
                                    texture = textures[1];
                                    positionRect.Y -= 3;
                                    enemyState = EnemyState.moveBack;
                                }
                                else
                                {
                                    texture = textures[0];
                                    positionRect.Y += 3;
                                    enemyState = EnemyState.moveForward;
                                }
                            }
                        
                    }
                    
                }
            }
        }

        //draws a different color when charging for AOE
        public override void Draw(SpriteBatch sb)
        {
            if (sp1Counter > sp1CD - 1)
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

        public void UpdateTimer(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _attackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _AOETimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        }

        /// <summary>
        /// not implemented yet
        /// </summary>
        /// <returns></returns>
        public Attack Shoot()
        {
            if(sp2Counter >= sp2CD)
            {
                sp2Counter = 0;
                moveCounter = 0;
                Projectiles can = new Projectiles(new Rectangle(X + Width / 2, Y + Height / 2, textures[4].Width, textures[4].Height), 3, 32, textures[4], AttackDirection.left, sp2HitTimer);
                return can;
            }
            return null;
        }

        /// <summary>
        /// deals damage all around
        /// </summary>
        /// <returns></returns>
        public Attack AOE()
        {
            if (sp1Counter >= sp1CD)
            {
                //Reset the cooldown
                sp1Counter = 0;
                //moveCounter = 0;

                Attack special = new Attack(
                        new Rectangle(
                            X - 80,
                            Y - 80,
                            Width + 160,
                            Height + 160),
                        attack * 3,
                        knockback * 3,
                        atkTexture,
                        AttackDirection.up, //This is temporary - Should probably change based off player position
                        sp1HitTimer);

                return special;
            }
            return null;
        }

        /// <summary>
        /// uses AOE when player is close, shoots faster when there is less health
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override Attack BossAttack(Player player)
        {
            if (moveCounter >= moveCD)
            {
                //If close to player

                 
                 if (_AOETimer > 1)
                 {
                     //_timer = 0; 
                     return AOE();
                 }
             
             
             
                 if(currentHP > maxHP / 3 * 2)
                 {
                     if (_attackTimer > 2)
                     {
                         return Shoot();
                     }
                 }
                 else
                 {
                     if (_attackTimer > 1)
                     {
                         return Shoot();
                     }
                 }
                 
             
            }

            return null;
        }
    }
}
