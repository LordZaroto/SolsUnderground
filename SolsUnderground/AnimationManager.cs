using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolsUnderground
{
    class AnimationManager
    {
        //fields
        private Animation _animation;

        private float _timer;

        private Player player;
        
        public Vector2 Position { get; set; }

        //creates new animation manager
        public AnimationManager(Animation animation, Player player)
        {
            _animation = animation;
            this.player = player;
        }

        /// <summary>
        /// starts an animation
        /// </summary>
        /// <param name="animation"></param>
        public void Play(Animation animation)
        {
            if(_animation == animation)
            {
                return;
            }
            _animation = animation;
            _animation.CurrentFrame = 0;
            _timer = 0;
        }

        /// <summary>
        /// stops an animation
        /// </summary>
        public void Stop()
        {
            _timer = 0f;
            _animation.CurrentFrame = 0;
        }

        /// <summary>
        /// increments the current frame to play through the animation
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(_timer > _animation.FrameSpeed)
            {
                _timer = 0f;
                _animation.CurrentFrame++;

                if(_animation.CurrentFrame >= _animation.FrameCount)
                {
                    _animation.CurrentFrame = 0;
                }
            }
        }

        /// <summary>
        /// draws the current frame
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_animation.Texture, player.PositionRect, new Rectangle(_animation.CurrentFrame*_animation.FrameWidth, 0, _animation.FrameWidth, _animation.FrameHeight), Color.White);
        }
    }
}
