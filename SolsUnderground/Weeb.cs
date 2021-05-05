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
            activeEffects = new List<StatusEffect>();
            effectCounter = 0;
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
            get { return Math.Max(attack + AttackMod, 0); }
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
        /// overridden method
        /// changes health when hit by the player
        /// </summary>
        /// <param name="damage"></param>
        public override void TakeDamage(int damage, int knockback)
        {
            if (!(enemyState == EnemyState.dead))
            {
                //moveCounter = 0; Testing no knockback on bosses

                currentHP -= Math.Max(damage - DefenseMod, 0);

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
            if (!IsStunned)
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
                                positionRect.X -= Math.Max(2 + SpeedMod, 0);
                                enemyState = EnemyState.moveLeft;
                            }
                            else
                            {
                                texture = textures[3];
                                positionRect.X += Math.Max(2 + SpeedMod, 0);
                                enemyState = EnemyState.moveRight;
                            }
                        }
                        else if (Math.Abs(positionRect.X - player.X) < Math.Abs(positionRect.Y - player.Y))
                        {
                            if (positionRect.Y >= player.Y)
                            {
                                texture = textures[1];
                                positionRect.Y -= Math.Max(2 + SpeedMod, 0);
                                enemyState = EnemyState.moveForward;
                            }
                            else
                            {
                                texture = textures[0];
                                positionRect.Y += Math.Max(2 + SpeedMod, 0);
                                enemyState = EnemyState.moveBack;
                            }
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

            if (!IsStunned)
            {
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
                Rectangle temp = player.PositionRect;
                if (Rectangle.Intersect(temp, PositionRect).Width <= Rectangle.Intersect(temp, PositionRect).Height)
                {
                    if (PositionRect.X > temp.X)
                    {
                        direction = AttackDirection.left;
                    }
                    else
                    {
                        direction = AttackDirection.right;
                    }
                }
                else
                {
                    if (PositionRect.Y > temp.Y)
                    {
                        direction = AttackDirection.up;
                    }
                    else
                    {
                        direction = AttackDirection.down;
                    }
                }
                attacks.Add(new Attack(PositionRect, Attack, knockback, null, direction, 0.15, false, null));
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
            for (int i = 0; i < activeEffects.Count;)
            {
                activeEffects[i].Counter += gameTime.ElapsedGameTime.TotalSeconds;
                activeEffects[i].EffectInterval += gameTime.ElapsedGameTime.TotalSeconds;

                // Check interval for regen/sickness
                if (activeEffects[i].EffectInterval > 0.5)
                {
                    activeEffects[i].EffectInterval -= 0.5;

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
                }

                // Remove if effect reaches end of duration
                if (activeEffects[i].Counter > activeEffects[i].Duration)
                {
                    activeEffects.RemoveAt(i);
                    continue;
                }

                i++;
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

                    Attack special = new Attack(
                        new Rectangle(
                            X - (Width * (3 / 4)),
                            Y - (Height + (Height / 2)),
                            Width + (Width * 2 * (3 / 4)),
                            Height * 2 + (Height / 2)),
                        Attack * 3,
                        knockback * 3,
                        atkTexture,
                        AttackDirection.up,
                        sp1HitTimer,
                        false,
                        null);

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
                        Attack * 3,
                        knockback * 3,
                        atkTexture,
                        AttackDirection.left,
                        sp1HitTimer,
                        false,
                        null);

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
                        Attack * 3,
                        knockback * 3,
                        atkTexture,
                        AttackDirection.down,
                        sp1HitTimer,
                        false,
                        null);

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
                        Attack * 3,
                        knockback * 3,
                        atkTexture,
                        AttackDirection.right,
                        sp1HitTimer,
                        false,
                        null);

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
                        Attack / 2,
                        knockback / 2,
                        atkTexture,
                        AttackDirection.up,
                        sp1HitTimer,
                        false,
                        null);

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
                        Attack / 2,
                        knockback / 2,
                        atkTexture,
                        AttackDirection.left,
                        sp1HitTimer,
                        false,
                        null);

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
                        Attack / 2,
                        knockback / 2,
                        atkTexture,
                        AttackDirection.down,
                        sp1HitTimer,
                        false,
                        null);

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
                        Attack / 2,
                        knockback / 2,
                        atkTexture,
                        AttackDirection.right,
                        sp1HitTimer,
                        false,
                        null);

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

            // Draw status effects
            for (int i = 0; i < activeEffects.Count; i++)
            {
                activeEffects[i].PositionRect = new Rectangle(X + 6 * i, Y - 17, 5, 6);
                activeEffects[i].Draw(sb);
            }
        }
    }
}
