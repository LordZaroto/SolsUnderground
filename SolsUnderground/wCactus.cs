﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    class wCactus : Item, Weapon
    {
        //Fields
        //-----------------------------
        private int attack;
        private double basicCooldown;
        private double specialCooldown;
        private int knockback;
        private Vector2 hitboxScale;
        private string name;
        private double timer;
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

        public string Name
        {
            get { return name; }
        }
        public double Timer
        {
            get { return timer; }
            set { timer = value; }
        }
        //------------------------------

        //----------------------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        public wCactus(Texture2D texture, Rectangle positionRect)
            : base(ItemType.Weapon, 20, texture, positionRect)
        {
            name = "Cactus";
            this.texture = texture;
            this.positionRect = positionRect;
            basicCooldown = 0.6;
            specialCooldown = 8;
            attack = 7;
            knockback = (int)(0.5 * 32);
            hitboxScale = new Vector2(2, 3);
            timer = 0.1;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, positionRect, Color.White);
        }

        /// <summary>
        /// Plants a cactus that sits and damages enemies for several seconds.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public Attack Special(Player player)
        {
            // w.X = p.X + (p.W - w.W) / 2
            positionRect = new Rectangle(
                player.X + (player.Width - (int)(player.Width * hitboxScale.X) / 2),
                player.Y + (player.Height - (int)(player.Height * hitboxScale.Y) / 2),
                (int)(player.Width * hitboxScale.X),
                (int)(player.Height * hitboxScale.Y));

            return new Attack(positionRect, 1, knockback * 3, texture, AttackDirection.allAround, 5, true, null);
        }

        public Rectangle GetHitbox(int x, int y, int width, int height, PlayerState state)
        {
            // Vector2 hitboxScale represents scaling factors of hitbox
            // using foward hitbox as default

            // To place weapon's center X on player's center X
            // w.X = p.X + (p.W - w.W) / 2

            // To place weapon's edge X + Width on player's center X
            // w.X = (p.X - w.W) + p.W / 2

            if (state == PlayerState.faceForward)
            {
                positionRect = new Rectangle(
                    x + (int)(width * (1 - hitboxScale.X) / 2),
                    y - (int)(height * hitboxScale.Y) + width / 2,
                    (int)(width * hitboxScale.X),
                    (int)(height * hitboxScale.Y));
                return positionRect;
            }
            else if (state == PlayerState.faceLeft)
            {
                positionRect = new Rectangle(
                    x - (int)(width * hitboxScale.Y) + width / 2,
                    y + (int)(height * (1 - hitboxScale.X) / 2),
                    (int)(width * hitboxScale.Y),
                    (int)(height * hitboxScale.X));
                return positionRect;
            }
            else if (state == PlayerState.faceBack)
            {
                positionRect = new Rectangle(
                    x + (int)(width * (1 - hitboxScale.X) / 2),
                    y + height / 2,
                    (int)(width * hitboxScale.X),
                    (int)(height * hitboxScale.Y));
                return positionRect;
            }
            else // if faceRight
            {
                positionRect = new Rectangle(
                    x + width / 2,
                    y + (int)(height * (1 - hitboxScale.X) / 2),
                    (int)(width * hitboxScale.Y),
                    (int)(height * hitboxScale.X));
                return positionRect;
            }
        }
    }
}
