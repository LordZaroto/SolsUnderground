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
        private string name;
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
        //------------------------------

        //----------------------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        public wStick(string name, Texture2D texture, Rectangle positionRect)
            : base(ItemType.Weapon, 3, texture, positionRect)
        {
            this.name = name;
            this.texture = texture;
            this.positionRect = positionRect;
            basicCooldown = 0.3;
            specialCooldown = 5;
            attack = 3;
            knockback = (int)(1 * 32);
        }

        public void Special()
        {

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
            if(state == PlayerState.faceForward)
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
