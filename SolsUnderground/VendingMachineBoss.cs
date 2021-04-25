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

        AttackDirection attackDirection;

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
            activeEffects = new List<StatusEffect>();
            effectCounter = 0;
            moveCD = 2;
            moveCounter = moveCD;
            kbCD = 2;
            kbCounter = kbCD;
            movingUp = true;
            _timer = 0f;
            _attackTimer = 0f;
            float _AOETimer = 0f;

            attackDirection = AttackDirection.left;

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
            get { return Math.Max(attack + AttackMod, 0); }
            set { attack = value; }
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

        protected override int AttackMod
        {
            get
            {
                int attackMod = 0;

                foreach (StatusEffect fx in activeEffects)
                {
                    if (fx.Effect == StatusType.AtkUp)
                        attackMod += fx.Power;

                    if (fx.Effect == StatusType.AtkDown)
                        attackMod -= fx.Power;
                }

                return attackMod;
            }
        }
        protected override int DefenseMod
        {
            get
            {
                int defenseMod = 0;

                foreach (StatusEffect fx in activeEffects)
                {
                    if (fx.Effect == StatusType.DefUp)
                        defenseMod += fx.Power;

                    if (fx.Effect == StatusType.DefDown)
                        defenseMod -= fx.Power;
                }

                return defenseMod;
            }
        }
        protected override int SpeedMod
        {
            get
            {
                int speedMod = 0;

                foreach (StatusEffect fx in activeEffects)
                {
                    if (fx.Effect == StatusType.SpdUp)
                        speedMod += fx.Power;

                    if (fx.Effect == StatusType.SpdDown)
                        speedMod -= fx.Power;
                }

                return speedMod;
            }
        }
        protected override bool IsStunned
        {
            get
            {
                foreach (StatusEffect fx in activeEffects)
                {
                    if (fx.Effect == StatusType.Stun)
                        return true;
                }

                return false;
            }
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

                currentHP -= Math.Max(damage - DefenseMod, 0);

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
                if (!(enemyState == EnemyState.dead) && !IsStunned)
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
                            positionRect.Y -= Math.Max(1 + SpeedMod, 0);
                        }
                        else
                        {
                            positionRect.Y += Math.Max(1 + SpeedMod, 0);
                        }
                        
                     
                    }
                    else if (currentHP <= maxHP / 3 * 2 && currentHP > maxHP / 3)
                    {
                     
                        if (movingUp)
                        {
                            positionRect.Y -= Math.Max(2 + SpeedMod, 0);
                        }
                        else
                        {
                            positionRect.Y += Math.Max(2 + SpeedMod, 0);
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
                                positionRect.X -= Math.Max(3 + SpeedMod, 0);
                                enemyState = EnemyState.moveLeft;
                            }
                            else
                            {
                                texture = textures[3];
                                positionRect.X += Math.Max(3 + SpeedMod, 0);
                                enemyState = EnemyState.moveRight;
                            }
                        }
                        else if (Math.Abs(positionRect.X - player.X) < Math.Abs(positionRect.Y - player.Y))
                        {
                            if (positionRect.Y >= player.Y)
                            {
                                texture = textures[1];
                                positionRect.Y -= Math.Max(3 + SpeedMod, 0);
                                enemyState = EnemyState.moveBack;
                            }
                            else
                            {
                                texture = textures[0];
                                positionRect.Y += Math.Max(3 + SpeedMod, 0);
                                enemyState = EnemyState.moveForward;
                            }
                        }
                        
                    }
                    
                    if(player.X < X && Math.Abs((player.X + player.Width/2) - (X + Width/2)) > Math.Abs((player.Y + player.Height / 2) - (Y + Height / 2)))
                    {
                        attackDirection = AttackDirection.left;
                    }
                    else if(player.X >= X && Math.Abs((player.X + player.Width / 2) - (X + Width / 2)) >= Math.Abs((player.Y + player.Height / 2) - (Y + Height / 2)))
                    {
                        attackDirection = AttackDirection.right;
                    }
                    else if (player.Y >= Y && Math.Abs((player.X + player.Width / 2) - (X + Width / 2)) <= Math.Abs((player.Y + player.Height / 2) - (Y + Height / 2)))
                    {
                        attackDirection = AttackDirection.down;
                    }
                    else
                    {
                        attackDirection = AttackDirection.up;
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

            // Draw status effects
            for (int i = 0; i < activeEffects.Count; i++)
            {
                activeEffects[i].PositionRect = new Rectangle(X + 6 * i, Y - 17, 5, 6);
                activeEffects[i].Draw(sb);
            }
        }

        public void UpdateTimer(GameTime gameTime)
        {
            if (!IsStunned)
            {
                _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                _attackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                _AOETimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
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
                //moveCounter = 0;



                Projectile can = new Projectile(
                    new Rectangle(X + Width / 2, Y + Height / 2, 
                    textures[4].Width, textures[4].Height),
                    Math.Max(3 + AttackMod, 0), 5, 32, textures[4], attackDirection, false, null);
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
                        Attack * 3,
                        knockback * 3,
                        atkTexture,
                        attackDirection, //This is temporary - Should probably change based off player position
                        sp1HitTimer,
                        false,
                        null);

                return special;
            }
            return null;
        }

        /// <summary>
        /// uses AOE when player is close, shoots faster when there is less health
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override List<Attack> EnemyAttack(Player player)
        {
            List<Attack> attacks = new List<Attack>();

            if (!IsStunned)
            {
                if (moveCounter >= moveCD)
                {
                    //If close to player


                    if (_AOETimer > 1)
                    {
                        //_timer = 0; 
                        attacks.Add(AOE());
                    }



                    if (currentHP > maxHP / 3 * 2)
                    {
                        if (_attackTimer > 2)
                        {
                            _attackTimer = 0;
                            attacks.Add(Shoot());
                        }
                    }
                    else
                    {
                        if (_attackTimer > 1)
                        {
                            _attackTimer = 0;
                            attacks.Add(Shoot());
                        }
                    }
                }

                attacks.Add(new Attack(PositionRect, Attack, knockback, null, attackDirection, 0.15, false, null));
            }
            

            return attacks;
        }

        /// <summary>
        /// Adds a status effect to the enemy.
        /// </summary>
        /// <param name="effect"></param>
        public override void AddEffect(StatusEffect effect)
        {
            activeEffects.Add(effect);
        }

        /// <summary>
        /// Updates the timer for each effect and activates the
        /// non stat-modifying effects, and removes any finished effects
        /// </summary>
        /// <param name="gameTime"></param>
        public override void UpdateEffects(GameTime gameTime)
        {
            effectCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // Update effects every half-second
            if (effectCounter > 0.5)
            {
                for (int i = 0; i < activeEffects.Count;)
                {
                    activeEffects[i].Duration -= effectCounter;

                    // Apply any active effects
                    switch (activeEffects[i].Effect)
                    {
                        case StatusType.Regen:
                            Health += activeEffects[i].Power;

                            if (Health > maxHP)
                                Health = maxHP;
                            break;

                        case StatusType.Sick:
                            Health -= activeEffects[i].Power;

                            if (currentHP <= 0)
                                enemyState = EnemyState.dead;
                            break;
                    }

                    // Remove if effect reaches end of duration
                    if (activeEffects[i].Duration < 0)
                    {
                        activeEffects.RemoveAt(i);
                        continue;
                    }

                    i++;
                }

                effectCounter -= 0.5;
            }
        }
    }
}
