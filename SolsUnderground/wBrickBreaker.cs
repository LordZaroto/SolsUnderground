using System;
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
        private Vector2 hitboxScale;
        private Rectangle hitboxF;
        private Rectangle hitboxL;
        private Rectangle hitboxB;
        private Rectangle hitboxR;
        private string name;
        private double timer;
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

        public wBrickBreaker(Texture2D texture, Rectangle positionRect)
            : base(ItemType.Weapon, 8, texture, positionRect)
        {
            name = "Brick Breaker";
            basicCooldown = 0.7;
            specialCooldown = 5;
            attack = 8;
            knockback = (int)(1.2 * 32);
            hitboxScale = new Vector2(2, 2);
            timer = 0.1;
        }

        /// <summary>
        /// The player slams the ground and creates a shockwave.
        /// </summary>
        public Attack Special(Player player)
        {
            AttackDirection atkdir = AttackDirection.up;
            
            switch (player.State)
            {
                case PlayerState.faceForward:
                case PlayerState.moveForward:
                    player.State = PlayerState.attackForward;
                    atkdir = AttackDirection.up;
                    break;

                case PlayerState.faceLeft:
                case PlayerState.moveLeft:
                    player.State = PlayerState.attackLeft;
                    atkdir = AttackDirection.left;
                    break;

                case PlayerState.faceBack:
                case PlayerState.moveBack:
                    player.State = PlayerState.attackBack;
                    atkdir = AttackDirection.down;
                    break;

                case PlayerState.faceRight:
                case PlayerState.moveRight:
                    player.State = PlayerState.attackRight;
                    atkdir = AttackDirection.right;
                    break;
            }

            // w.X = p.X + (p.W - w.W) / 2
            positionRect = new Rectangle(
                player.X - player.Width * 2,
                player.Y - player.Height * 2,
                player.Width * 5,
                player.Height * 5);

            return new Attack(positionRect, (int)(attack * 1.2), (int)(knockback * 1.5), texture, atkdir, timer);
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
