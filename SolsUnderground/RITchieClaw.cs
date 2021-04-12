using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    class RITchieClaw : Weapon
    {
        //Fields
        //-----------------------------
        private Texture2D texture;
        private Rectangle positionRect;
        private int attack;
        private double basicCooldown;
        private double knockback;
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
        public int X
        {
            get { return positionRect.X; }
            set { positionRect.X = value; }
        }
        public int Y
        {
            get { return positionRect.Y; }
            set { positionRect.Y = value; }
        }
        public int Width
        {
            get { return positionRect.Width; }
            set { positionRect.Width = value; }
        }
        public int Height
        {
            get { return positionRect.Height; }
            set { positionRect.Height = value; }
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

        public double Knockback
        {
            get { return knockback; }
        }
        //------------------------------

        //----------------------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        public RITchieClaw(Texture2D texture, Rectangle positionRect)
        {
            this.texture = texture;
            this.positionRect = positionRect;
            basicCooldown = 0.1;
            attack = 7;
            knockback = 0.8;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, positionRect, Color.White);
        }

        /// <summary>
        /// The player will dash a long distance, striking enemies along the way.
        /// </summary>
        public void Special()
        {
            return;
        }

        public Rectangle GetHitbox(int x, int y, int width, int height, PlayerState state)
        {
            if (state == PlayerState.faceForward)
            {
                return new Rectangle(x - width / 2, y - height / 2, width * 2, height);
            }
            else if (state == PlayerState.faceLeft)
            {
                return new Rectangle(x - width / 2, y - height / 2, width, height * 2);
            }
            else if (state == PlayerState.faceBack)
            {
                return new Rectangle(x - width / 2, y + height / 2, width * 2, height);
            }
            else // if faceRight
            {
                return new Rectangle(x + width / 2, y - height / 2, width, height * 2);
            }
        }
    }
}
