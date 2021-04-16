﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    class wBrickBreaker : Item, Weapon
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

        public Texture2D Sprite
        {
            get { return texture; }
        }

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

        public wBrickBreaker(Texture2D texture, Rectangle positionRect)
            : base(ItemType.Weapon, 5, texture, positionRect)
        {
            basicCooldown = 0.7;
            specialCooldown = 5;
            attack = 5;
            knockback = (int)(1.2 * 32);
        }

        /// <summary>
        /// The player slams the ground and creates a shockwave.
        /// </summary>
        public Attack Special(Player player)
        {
            switch (player.State)
            {
                case PlayerState.faceForward:
                case PlayerState.moveForward:
                    player.State = PlayerState.attackForward;
                    break;

                case PlayerState.faceLeft:
                case PlayerState.moveLeft:
                    player.State = PlayerState.attackLeft;
                    break;

                case PlayerState.faceBack:
                case PlayerState.moveBack:
                    player.State = PlayerState.attackBack;
                    break;

                case PlayerState.faceRight:
                case PlayerState.moveRight:
                    player.State = PlayerState.attackRight;
                    break;
            }

            // w.X = p.X + (p.W - w.W) / 2
            positionRect = new Rectangle(
                player.X - player.Width * 2,
                player.Y - player.Height * 2,
                player.Width * 5,
                player.Height * 5);

            return new Attack(positionRect, (int)(attack * 1.2), (int)(knockback * 1.5));
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, positionRect, Color.White);
        }

        public Rectangle GetHitbox(int x, int y, int width, int height, PlayerState state)
        {
            if (state == PlayerState.faceForward)
            {
                positionRect = new Rectangle(x - width, y - height * 3 / 2, width * 3, height * 2);
                return positionRect;
            }
            else if (state == PlayerState.faceLeft)
            {
                positionRect = new Rectangle(x - width * 3 / 2, y - height, width * 2, height * 3);
                return positionRect;
            }
            else if (state == PlayerState.faceBack)
            {
                positionRect = new Rectangle(x - width, y + height / 2, width * 3, height * 2);
                return positionRect;
            }
            else // if faceRight
            {
                positionRect = new Rectangle(x + width / 2, y - height, width * 2, height * 3);
                return positionRect;
            }
        }
    }
}
