using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    class wRITchieClaw : Item, Weapon
    {
        //Fields
        //-----------------------------
        private int attack;
        private double basicCooldown;
        private double specialCooldown;
        private int knockback;
        private Rectangle hitboxF;
        private Rectangle hitboxL;
        private Rectangle hitboxB;
        private Rectangle hitboxR;
        //-----------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        //Properties
        //----------------------------------------

        //Weapon Position
        //------------------------------

        public Rectangle Position
        {
            get { return positionRect; }
            set { positionRect = value; }
        }

        public Rectangle HitboxF
        {
            get { return hitboxF; }
            set { hitboxF = value; }
        }

        public Rectangle HitboxL
        {
            get { return hitboxL; }
            set { hitboxL = value; }
        }

        public Rectangle HitboxB
        {
            get { return hitboxB; }
            set { hitboxB = value; }
        }

        public Rectangle HitboxR
        {
            get { return hitboxR; }
            set { hitboxR = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }
        //------------------------------

        //Weapon Stats
        //------------------------------

        public int Attack
        {
            get { return attack; }
            set { attack = value; }
        }

        public double BasicCooldown
        {
            get { return basicCooldown; }
        }

        public double SpecialCooldown
        {
            get { return specialCooldown; }
        }

        public int Knockback
        {
            get { return knockback; }
        }
        //------------------------------

        //----------------------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        //previous version
        //public RITchieClaw()
        //{
        //    basicCooldown = 0.1;
        //    specialCooldown = 3;
        //    attack = 7;
        //    knockback = (int)(0.8 * 32); 
        //}

        //public RITchieClaw(Texture2D texture, Rectangle positionRect)
        public wRITchieClaw(Texture2D texture, Rectangle positionRect)
            : base(ItemType.Weapon, 7, texture, positionRect)
        {
            basicCooldown = 0.1;
            specialCooldown = 3;
            attack = 7;
            knockback = (int)(0.8 * 32);
        }

        /// <summary>
        /// The player will dash a long distance, striking enemies along the way.
        /// </summary>
        public Attack Special(Player player)
        {
            System.Diagnostics.Debug.WriteLine(player.Y);
            //Create the attack hitbox in the direction the player is facing
            if (player.State == PlayerState.faceForward || player.State == PlayerState.moveForward)
            {
                player.State = PlayerState.attackForward;

                positionRect = new Rectangle(
                        player.X - (player.Width * (3 / 4)),
                        player.Y - (player.Height * 5 + (player.Height / 2)),
                        player.Width + (player.Width * 2 * (3 / 4)),
                        player.Height * 6 + (player.Height / 2));

                Attack special = new Attack(
                    positionRect,
                    attack / 2,
                    knockback / 2);

                System.Diagnostics.Debug.WriteLine(player.Y);
                player.Y -= player.Height * 5;
                System.Diagnostics.Debug.WriteLine(player.Y);
                return special;
            }
            else if (player.State == PlayerState.faceLeft || player.State == PlayerState.moveLeft)
            {
                player.State = PlayerState.attackLeft;

                positionRect = new Rectangle(
                        player.X - (player.Width * 5 + (player.Width / 2)),
                        player.Y - (player.Height * (3 / 4)),
                        player.Width * 6 + (player.Width / 2),
                        player.Height + (player.Height * 2 * (3 / 4)));

                Attack special = new Attack(
                    positionRect,
                    attack / 2,
                    knockback / 2);

                player.X -= player.Width * 5;

                return special;
            }
            else if (player.State == PlayerState.faceBack || player.State == PlayerState.moveBack)
            {
                player.State = PlayerState.attackBack;

                positionRect = new Rectangle(
                        player.X - (player.Width * (3 / 4)),
                        player.Y - (player.Height / 2),
                        player.Width + (player.Width * 2 * (3 / 4)),
                        player.Height * 6 + (player.Height / 2));

                Attack special = new Attack(
                    positionRect,
                    attack / 2,
                    knockback / 2);

                player.Y += player.Height * 5;

                return special;
            }
            else if (player.State == PlayerState.faceRight || player.State == PlayerState.moveRight)
            {
                player.State = PlayerState.attackRight;

                positionRect = new Rectangle(
                        player.X - (player.Width / 2),
                        player.Y - (player.Height * (3 / 4)),
                        player.Width * 6 + (player.Width / 2),
                        player.Height + (player.Height * 2 * (3 / 4)));

                Attack special = new Attack(
                    positionRect,
                    attack / 2,
                    knockback / 2);

                player.X += player.Width * 5;

                return special;
            }

            return null;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, positionRect, Color.Green);
        }

        public Rectangle GetHitbox(int x, int y, int width, int height, PlayerState state)
        {
            if (state == PlayerState.faceForward)
            {
                positionRect = new Rectangle(x - width / 2, y - height / 2, width * 2, height);
                return positionRect;
            }
            else if (state == PlayerState.faceLeft)
            {
                positionRect = new Rectangle(x - width / 2, y - height / 2, width, height * 2);
                return positionRect;
            }
            else if (state == PlayerState.faceBack)
            {
                positionRect = new Rectangle(x - width / 2, y + height / 2, width * 2, height);
                return positionRect;
            }
            else // if faceRight
            {
                positionRect = new Rectangle(x + width / 2, y - height / 2, width, height * 2);
                return positionRect;
            }
        }
    }
}
