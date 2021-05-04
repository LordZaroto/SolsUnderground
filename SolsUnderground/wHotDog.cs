using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    class wHotDog : Item, Weapon
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
        //---------------
        //------------------------------

        //----------------------------------------

        //---------------------------------------------------------------------
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //---------------------------------------------------------------------

        public wHotDog(Texture2D texture, Rectangle positionRect)
            : base(ItemType.Weapon, 30, texture, positionRect)
        {
            name = "Hot Dog";
            basicCooldown = 0.4;
            specialCooldown = 20;
            attack = 5;
            knockback = (int)(1 * 32);
            hitboxScale = new Vector2(2, 1.5f);
            timer = 0.1;
        }

        /// <summary>
        /// The player eats the hotdog and recovers health.
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

            // Heal player for 20 HP
            player.Hp += 20;
            if (player.Hp > player.MaxHp)
                player.Hp = player.MaxHp;

            // Grant regen effect
            player.AddEffect(new StatusEffect(StatusType.Regen, 3, 5));

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
