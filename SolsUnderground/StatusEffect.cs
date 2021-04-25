using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Alex Dale
/// 
/// Defines an effect that an entity can have, such as a stat modifier
/// or a health effect.
/// </summary>

namespace SolsUnderground
{
    public enum StatusType
    {
        AtkUp,
        AtkDown,
        DefUp,
        DefDown,
        SpdUp,
        SpdDown,
        Regen,
        Sick,
        Stun
    }

    class StatusEffect : StaticObject
    {
        // Fields
        // Inherits texture and positionRect
        private static List<Texture2D> effectSprites = new List<Texture2D>();
        private StatusType effect;
        private Color color;
        private int power;
        private double duration;
        private double counter;
        private double effectInterval;

        // Properties

        public StatusType Effect
        {
            get { return effect; }
        }

        public int Power
        {
            get { return power; }
        }

        public double Duration
        {
            get { return duration; }
        }

        public double Counter
        {
            get { return counter; }
            set { counter = value; }
        }

        public double EffectInterval
        {
            get { return effectInterval; }
            set { effectInterval = value; }
        }

        public override int X
        {
            get { return positionRect.X; }
            set { positionRect.X = value; }
        }

        public override int Y
        {
            get { return positionRect.Y; }
            set { positionRect.Y = value; }
        }
        public override int Width
        {
            get { return positionRect.Width; }
            set { positionRect.Width = value; }
        }

        public override int Height
        {
            get { return positionRect.Height; }
            set { positionRect.Height = value; }
        }

        public override Rectangle PositionRect
        {
            get { return positionRect; }
            set { positionRect = value; }
        }

        // Constructor
        public StatusEffect(StatusType effect, int power, double duration)
        {
            positionRect = new Rectangle(0, 0, 5, 6);
            this.effect = effect;
            this.power = power;
            this.duration = duration;
            counter = 0;
            effectInterval = 0;

            switch (effect)
            {
                case StatusType.AtkUp:
                case StatusType.AtkDown:
                    texture = effectSprites[0];
                    color = Color.Red;
                    break;

                case StatusType.DefUp:
                case StatusType.DefDown:
                    texture = effectSprites[0];
                    color = Color.Blue;
                    break;

                case StatusType.SpdUp:
                case StatusType.SpdDown:
                    texture = effectSprites[0];
                    color = Color.Yellow;
                    break;

                case StatusType.Regen:
                    texture = effectSprites[1];
                    color = Color.White;
                    break;

                case StatusType.Sick:
                    texture = effectSprites[2];
                    color = Color.White;
                    break;

                case StatusType.Stun:
                    texture = effectSprites[3];
                    color = Color.White;
                    break;
            }
        }

        // Methods

        public static void LoadEffectSprite(Texture2D effectSprite)
        {
            effectSprites.Add(effectSprite);
        }

        public override void Draw(SpriteBatch sb)
        {
            // Draw negative modifiers flipped downwards
            switch (effect)
            {
                case StatusType.AtkDown:
                case StatusType.DefDown:
                case StatusType.SpdDown:
                    sb.Draw(texture, positionRect, null, color, 0, Vector2.Zero, SpriteEffects.FlipVertically, 0);
                    break;

                default:
                    sb.Draw(texture, positionRect, color);
                    break;
            }
        }
    }
}
