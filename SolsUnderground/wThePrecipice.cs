using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Preston Gilmore

namespace SolsUnderground
{
    /// <summary>
    /// The Weeb's favorite LARP weapon. You feel
    /// a bizzare force surging within the blade.
    /// </summary>
    class wThePrecipice : Item, Weapon
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
        //---------------
        //------------------------------

        //----------------------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        public wThePrecipice(Texture2D texture, Rectangle positionRect)
            : base(ItemType.Weapon, 5, texture, positionRect)
        {
            name = "The Precipice";
            basicCooldown = 0.5;
            specialCooldown = 6;
            attack = 9;
            knockback = (int)(1.5 * 32);
            hitboxScale = new Vector2(2, 3);
            timer = 0.1;
        }

        /// <summary>
        /// The player will dash a long distance, striking enemies along the way.
        /// </summary>
        public Attack Special(Player player)
        {
            //Create the attack hitbox in the direction the player is facing
            if (player.State == PlayerState.faceForward || player.State == PlayerState.moveForward)
            {
                player.State = PlayerState.attackForward;

                player.Y -= player.Height * 2;

                positionRect = new Rectangle(
                        player.X - (player.Width * (3 / 4)),
                        player.Y - (player.Height * 2 + (player.Height / 2)),
                        player.Width + (player.Width * 2 * (3 / 4)),
                        player.Height * 3 + (player.Height / 2));

                Attack special = new Attack(
                    positionRect,
                    attack * 2,
                    knockback * 2,
                    texture,
                    AttackDirection.up,
                    timer,
                    true,
                    null);

                return special;
            }
            else if (player.State == PlayerState.faceLeft || player.State == PlayerState.moveLeft)
            {
                player.State = PlayerState.attackLeft;

                player.X -= player.Width * 2;

                positionRect = new Rectangle(
                        player.X - (player.Width * 2 + (player.Width / 2)),
                        player.Y - (player.Height * (3 / 4)),
                        player.Width * 3 + (player.Width / 2),
                        player.Height + (player.Height * 2 * (3 / 4)));

                Attack special = new Attack(
                    positionRect,
                    attack * 2,
                    knockback * 2,
                    texture,
                    AttackDirection.left,
                    timer,
                    true,
                    null);

                return special;
            }
            else if (player.State == PlayerState.faceBack || player.State == PlayerState.moveBack)
            {
                player.State = PlayerState.attackBack;

                player.Y += player.Height * 2;

                positionRect = new Rectangle(
                        player.X - (player.Width * (3 / 4)),
                        player.Y - (player.Height / 2),
                        player.Width + (player.Width * 2 * (3 / 4)),
                        player.Height * 3 + (player.Height / 2));

                Attack special = new Attack(
                    positionRect,
                    attack * 2,
                    knockback * 2,
                    texture,
                    AttackDirection.down,
                    timer,
                    true,
                    null);

                return special;
            }
            else if (player.State == PlayerState.faceRight || player.State == PlayerState.moveRight)
            {
                player.State = PlayerState.attackRight;

                player.X += player.Width * 2;

                positionRect = new Rectangle(
                        player.X - (player.Width / 2),
                        player.Y - (player.Height * (3 / 4)),
                        player.Width * 3 + (player.Width / 2),
                        player.Height + (player.Height * 2 * (3 / 4)));

                Attack special = new Attack(
                    positionRect,
                    attack * 2,
                    knockback * 2,
                    texture,
                    AttackDirection.right,
                    timer,
                    true,
                    null);

                return special;
            }

            return null;
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
