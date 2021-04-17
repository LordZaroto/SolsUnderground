using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    class wStick : Item, Weapon
    {
        //Fields
        //-----------------------------
        private int attack;
        private double basicCooldown;
        private double specialCooldown;
        private int knockback;
        private Vector2 hitboxScale;
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
        //------------------------------

        //----------------------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        public wStick(Texture2D texture, Rectangle positionRect)
            : base(ItemType.Weapon, 3, texture, positionRect)
        {
            basicCooldown = 0.3;
            specialCooldown = 5;
            attack = 3;
            knockback = (int)(1 * 32);
            hitboxScale = new Vector2(2, 1);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, positionRect, Color.White);
        }

        public Attack Special(Player player)
        {
            return null;
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
