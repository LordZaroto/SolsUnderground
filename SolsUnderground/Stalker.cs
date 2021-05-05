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
    /// You feel an unsettling presence
    /// </summary>
    class Stalker : Boss
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
        
        //Stalker specific mechanics
        private bool invisible;
        private int stunTime;
        private int stunMultiplyer;
        private double invisTimer;
        private double invisCD;
        private Random rng;
        private int teleChance;

        Texture2D[] textures;
        Texture2D atkTexture;
        //consructor: initializes the fields
        public Stalker(Texture2D[] textures, Rectangle positionRect, int health, int attack, Texture2D atkTexture)
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
            sp1CD = 10;
            sp1Counter = 2;
            sp2CD = 10;
            sp2Counter = 2;
            sp1HitTimer = 0.1;
            sp2HitTimer = 0.1;
            invisible = false;
            stunTime = 1;
            invisTimer = 0;
            invisCD = 0.7;
            stunMultiplyer = 1;
            rng = new Random();
            teleChance = 2;
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

                currentHP -= Math.Max(damage - DefenseMod, 0);

                //Increases knockback recieved by x2
                if (enemyState == EnemyState.faceForward || enemyState == EnemyState.moveForward)
                {
                    Y += knockback * 2;
                }
                if (enemyState == EnemyState.faceLeft || enemyState == EnemyState.moveLeft)
                {
                    X += knockback * 2;
                }
                if (enemyState == EnemyState.faceBack || enemyState == EnemyState.moveBack)
                {
                    Y -= knockback * 2;
                }
                if (enemyState == EnemyState.faceRight || enemyState == EnemyState.moveRight)
                {
                    X -= knockback * 2;
                }

                if (currentHP <= 0)
                {
                    enemyState = EnemyState.dead;
                }

                //Increase stun time at lower health
                if(currentHP < maxHP * 0.7)
                {
                    stunTime = 2;
                    teleChance = 3;
                }
                else if(currentHP < maxHP * 0.3)
                {
                    stunTime = 3;
                    teleChance = 4;
                }
            }
        }

        /// <summary>
        /// movement AI that will chase the player
        /// </summary>
        public override void EnemyMove(Player player, GameTime gameTime)
        {
            //Cannot be invisible while stunned
            invisible = false;
            
            if (!IsStunned)
            {
                //Update the cooldowns
                sp1Counter += gameTime.ElapsedGameTime.TotalSeconds;
                sp2Counter += gameTime.ElapsedGameTime.TotalSeconds;
                moveCounter += gameTime.ElapsedGameTime.TotalSeconds;
                invisTimer += gameTime.ElapsedGameTime.TotalSeconds;

                //Stalker can be invisible
                if (invisTimer > invisCD)
                {
                    invisible = true;
                }

                //Stalker will not be invisible while behind the player or while player is stunned
                if(((player.State == PlayerState.faceForward || player.State == PlayerState.moveForward || player.State == PlayerState.attackForward)
                    && State == EnemyState.moveForward) ||
                    ((player.State == PlayerState.faceBack || player.State == PlayerState.moveBack || player.State == PlayerState.attackBack)
                    && State == EnemyState.moveBack) ||
                    ((player.State == PlayerState.faceLeft || player.State == PlayerState.moveLeft || player.State == PlayerState.attackLeft)
                    && State == EnemyState.moveLeft) ||
                    ((player.State == PlayerState.faceRight || player.State == PlayerState.moveRight || player.State == PlayerState.attackRight)
                    && State == EnemyState.moveRight) ||
                    player.IsStunned == true)
                {
                    invisible = false;
                }

                if (moveCounter >= moveCD)
                {
                    if (!(enemyState == EnemyState.dead))
                    {
                        if (Math.Abs(positionRect.X - player.X) >= Math.Abs(positionRect.Y - player.Y))
                        {
                            if (positionRect.X >= player.X)
                            {
                                texture = textures[2];
                                positionRect.X -= Math.Max(1 + SpeedMod, 0);
                                enemyState = EnemyState.moveLeft;
                            }
                            else
                            {
                                texture = textures[3];
                                positionRect.X += Math.Max(1 + SpeedMod, 0);
                                enemyState = EnemyState.moveRight;
                            }
                        }
                        else if (Math.Abs(positionRect.X - player.X) < Math.Abs(positionRect.Y - player.Y))
                        {
                            if (positionRect.Y >= player.Y)
                            {
                                texture = textures[1];
                                positionRect.Y -= Math.Max(1 + SpeedMod, 0);
                                enemyState = EnemyState.moveForward;
                            }
                            else
                            {
                                texture = textures[0];
                                positionRect.Y += Math.Max(1 + SpeedMod, 0);
                                enemyState = EnemyState.moveBack;
                            }
                        }

                        if(player.IsStunned == false)
                        {
                            if(rng.Next(0, 1000) < teleChance)
                            {
                                X = rng.Next(100, 1000);
                                Y = rng.Next(100, 1000);
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
                    if ((Math.Abs(X - player.X) < 40 && (State == EnemyState.moveRight || State == EnemyState.moveLeft))
                        || (Math.Abs(Y - player.Y) < 40 && (State == EnemyState.moveForward || State == EnemyState.moveBack)))
                    {
                        attacks.Add(BackStab(player));
                    }
                    else if((Math.Abs(X - player.X) > 100 && (State == EnemyState.moveRight || State == EnemyState.moveLeft))
                        || (Math.Abs(Y - player.Y) > 100 && (State == EnemyState.moveForward || State == EnemyState.moveBack)))
                    {
                        attacks.Add(Pressure(player));
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
        /// The stalker delivers a poison dagger to the back
        /// </summary>
        /// <returns></returns>
        public Attack BackStab(Player player)
        {
            //If off cooldown
            if (sp1Counter >= sp1CD)
            {
                //Reset the cooldowns
                //Will deal a heavier blow if player is stunned
                if(player.IsStunned == true)
                {
                    sp1Counter = 0;
                    stunMultiplyer = 3;
                }
                else
                {
                    sp1Counter = 8;
                    stunMultiplyer = 1;
                }

                moveCounter = 0;
                invisTimer = 0;

                //Create the attack hitbox in the direction the player is facing
                if (State == EnemyState.faceForward || State == EnemyState.moveForward)
                {
                    State = EnemyState.attackForward;

                    Attack special = new Attack(
                        new Rectangle(
                            X - (Width * (3 / 4)),
                            Y - (Height + (Height / 2)),
                            Width + (Width * (3 / 4)),
                            Height + (Height / 2)),
                        Attack * stunMultiplyer,
                        knockback * stunMultiplyer,
                        atkTexture,
                        AttackDirection.up,
                        sp1HitTimer,
                        false,
                        new StatusEffect(StatusType.Sick, stunMultiplyer, 5));

                    return special;
                }
                else if (State == EnemyState.faceLeft || State == EnemyState.moveLeft)
                {
                    State = EnemyState.attackLeft;

                    Attack special = new Attack(
                        new Rectangle(
                            X - (Width + (Width / 2)),
                            Y - (Height * (3 / 4)),
                            Width + (Width / 2),
                            Height + (Height * (3 / 4))),
                        Attack * stunMultiplyer,
                        knockback * stunMultiplyer,
                        atkTexture,
                        AttackDirection.left,
                        sp1HitTimer,
                        false,
                        new StatusEffect(StatusType.Sick, stunMultiplyer, 5));

                    return special;
                }
                else if (State == EnemyState.faceBack || State == EnemyState.moveBack)
                {
                    State = EnemyState.attackBack;

                    Attack special = new Attack(
                        new Rectangle(
                            X - (Width * (3 / 4)),
                            Y - (Height / 2),
                            Width + (Width * (3 / 4)),
                            Height + (Height / 2)),
                        Attack * stunMultiplyer,
                        knockback * stunMultiplyer,
                        atkTexture,
                        AttackDirection.down,
                        sp1HitTimer,
                        false,
                        new StatusEffect(StatusType.Sick, stunMultiplyer, 5));

                    return special;
                }
                else if (State == EnemyState.faceRight || State == EnemyState.moveRight)
                {
                    State = EnemyState.attackRight;

                    Attack special = new Attack(
                        new Rectangle(
                            X - (Width / 2),
                            Y - (Height * (3 / 4)),
                            Width + (Width / 2),
                            Height + (Height * (3 / 4))),
                        Attack * stunMultiplyer,
                        knockback * stunMultiplyer,
                        atkTexture,
                        AttackDirection.right,
                        sp1HitTimer,
                        false,
                        new StatusEffect(StatusType.Sick, stunMultiplyer, 5));

                    return special;
                }
            }

            return null;
        }

        /// <summary>
        /// Dashes aggressively towards the player. Does damage if it connects.
        /// </summary>
        /// <returns></returns>
        public Attack Pressure(Player player)
        {
            //If off cooldown
            if (sp2Counter >= sp2CD)
            {
                //Reset the cooldown
                sp2Counter = 0;
                moveCounter = 0;
                invisTimer = 0;

                //Create the attack hitbox in the direction the player is facing
                if (player.State == PlayerState.faceForward || player.State == PlayerState.moveForward)
                {
                    Attack special = new Attack(
                        player.PositionRect,
                        0,
                        0,
                        atkTexture,
                        AttackDirection.up,
                        sp1HitTimer,
                        false,
                        new StatusEffect(StatusType.Stun, 1, stunTime));

                    Y = player.Y + 100;
                    X = player.X;

                    return special;
                }
                else if (player.State == PlayerState.faceLeft || player.State == PlayerState.moveLeft)
                {
                    Attack special = new Attack(
                        player.PositionRect,
                        0,
                        0,
                        atkTexture,
                        AttackDirection.left,
                        sp1HitTimer,
                        false,
                        new StatusEffect(StatusType.Stun, 1, stunTime));

                    Y = player.Y;
                    X = player.X + 100;

                    return special;
                }
                else if (player.State == PlayerState.faceBack || player.State == PlayerState.moveBack)
                {
                    Attack special = new Attack(
                        player.PositionRect,
                        0,
                        0,
                        atkTexture,
                        AttackDirection.down,
                        sp1HitTimer,
                        false,
                        new StatusEffect(StatusType.Stun, 1, stunTime));

                    Y = player.Y - 100;
                    X = player.X;

                    return special;
                }
                else if (player.State == PlayerState.faceRight || player.State == PlayerState.moveRight)
                {
                    Attack special = new Attack(
                        player.PositionRect,
                        0,
                        0,
                        atkTexture,
                        AttackDirection.right,
                        sp1HitTimer,
                        false,
                        new StatusEffect(StatusType.Stun, 1, stunTime));

                    Y = player.Y;
                    X = player.X - 100;

                    return special;
                }
            }

            return null;
        }

        public override void Draw(SpriteBatch sb)
        {
            //Do not draw when invisible
            if(invisible == false)
            {
                //Draw the stalker
                sb.Draw(texture, positionRect, Color.White);

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
}
