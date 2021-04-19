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
        GameTime gameTime;
        float _timer = 0f;
        float aoeTimer;
        int maxHealth;
        int health;
        bool movingUp;

        private double sp1Counter;
        private double sp1CD;

        public VendingMachineBoss(Texture2D[] textures, Rectangle positionRect, int health, int attack)
        {
            this.textures = textures;
            this.texture = textures[0];
            this.positionRect = positionRect;
            maxHealth = health;
            this.currentHP = maxHealth;
            this.attack = attack;
            this.knockback = 64;
            moveCD = 2;
            moveCounter = moveCD;
            kbCD = 2;
            kbCounter = kbCD;
            movingUp = true;
            _timer = 0f;
            aoeTimer = 0f;

            sp1CD = 7;
            sp1Counter = 6;
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
            get;
            set;
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

        public override void TakeDamage(int damage, int knockback)
        {
            if (!(enemyState == EnemyState.dead))
            {
                moveCounter = 0;

                currentHP -= damage;

                if (enemyState == EnemyState.faceForward || enemyState == EnemyState.moveForward)
                {
                    Y += (int)(knockback);
                }
                if (enemyState == EnemyState.faceLeft || enemyState == EnemyState.moveLeft)
                {
                    X += (int)(knockback);
                }
                if (enemyState == EnemyState.faceBack || enemyState == EnemyState.moveBack)
                {
                    Y -= (int)(knockback);
                }
                if (enemyState == EnemyState.faceRight || enemyState == EnemyState.moveRight)
                {
                    X -= (int)(knockback);
                }

                if (currentHP <= 0)
                {
                    enemyState = EnemyState.dead;
                }
            }
        }

        public override void EnemyMove(Player player, GameTime gameTime)
        {
            //Update the cooldowns
            moveCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (moveCounter >= moveCD)
            {
                if (!(enemyState == EnemyState.dead))
                {
                    if (currentHP > maxHealth / 3 * 2)
                    {
                        if (!(Math.Abs((positionRect.X +(positionRect.Width/2)) - (player.X + player.Width/2)) < 150 && Math.Abs((positionRect.Y + (positionRect.Height / 2)) - (player.Y + player.Height / 2)) < 150))
                        {

                            if (movingUp)
                            {
                                positionRect.Y += 1;
                            }
                            else
                            {
                                positionRect.Y -= 1;
                            }
                            if (_timer > 3)
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
                    }
                    else if (currentHP <= maxHealth / 3 * 2 && currentHP > maxHealth / 3)
                    {
                        if (!(Math.Abs((positionRect.X + (positionRect.Width / 2)) - (player.X + player.Width / 2)) < 150 && Math.Abs((positionRect.Y + (positionRect.Height / 2)) - (player.Y + player.Height / 2)) < 150))
                        {
                            if (movingUp)
                            {
                                positionRect.Y += 2;
                            }
                            else
                            {
                                positionRect.Y -= 2;
                            }
                            if (_timer > 2.5)
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
                    }
                    else
                    {
                        
                            if (Math.Abs(positionRect.X - player.X) >= Math.Abs(positionRect.Y - player.Y))
                            {
                                if (positionRect.X >= player.X)
                                {
                                    positionRect.X -= 3;
                                    enemyState = EnemyState.moveLeft;
                                }
                                else
                                {
                                    positionRect.X += 3;
                                    enemyState = EnemyState.moveRight;
                                }
                            }
                            else if (Math.Abs(positionRect.X - player.X) < Math.Abs(positionRect.Y - player.Y))
                            {
                                if (positionRect.Y >= player.Y)
                                {
                                    positionRect.Y -= 3;
                                    enemyState = EnemyState.moveBack;
                                }
                                else
                                {
                                    positionRect.Y += 3;
                                    enemyState = EnemyState.moveForward;
                                }
                            }
                        
                    }
                }
            }
        }

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
                new Rectangle(X, Y - 10, (int)(Width * ((double)health / (double)maxHealth)), 3),
                Color.Red);
        }

        public void UpdateTimer(GameTime gameTime)
        {
                _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
        }

        public Attack Shoot()
        {
            return null;
        }

        public Attack AOE()
        {
            if (sp1Counter >= sp1CD)
            {
                //Reset the cooldown
                sp1Counter = 0;
                moveCounter = 0;

                Attack special = new Attack(
                        new Rectangle(
                            X - 50,
                            Y - 50,
                            Width + 100,
                            Height + 100),
                        attack * 3,
                        knockback * 3);

                return special;
            }
            return null;
        }

        public override Attack BossAttack(Player player)
        {
            if (moveCounter >= moveCD)
            {
                //If close to player
                if ((Math.Abs(X - player.X) < 120 && (Math.Abs(Y - player.Y) < 120)))
                {
                    _timer = 0;
                    if(_timer > 1.5)
                    {
                        _timer = 0;
                        return AOE();
                    }
                    
                }
                else
                {
                    if(health > maxHealth / 3 * 2)
                    {
                        if (_timer % 2 == 0)
                        {
                            return Shoot();
                        }
                    }
                    else
                    {
                        if (_timer % 1 == 0)
                        {
                            return Shoot();
                        }
                    }
                    
                }
            }

            return null;
        }
    }
}
