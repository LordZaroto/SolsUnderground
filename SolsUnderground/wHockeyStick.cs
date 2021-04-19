using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    class wHockeyStick : Item, Weapon
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
        private string name;
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
        public string Name
        {
            get { return name; }
        }
        //---------------
        //------------------------------

        //----------------------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        public wHockeyStick(Texture2D texture, Rectangle positionRect)
            : base(ItemType.Weapon, 5, texture, positionRect)
        {
            name = "Hockey Stick";
            basicCooldown = 0.4;
            specialCooldown = 5;
            attack = 5;
            knockback = (int)(1 * 32);
            hitboxScale = new Vector2(0.5f, 4);
        }

        /// <summary>
        /// The player spins and knocks enemies away.
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
                player.X - player.Width,
                player.Y - player.Height,
                player.Width * 3,
                player.Height * 3);

            return new Attack(positionRect, (int)(attack * 0.5), (int)(knockback * 3));
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, positionRect, Color.White);
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
