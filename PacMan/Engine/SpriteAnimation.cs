using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ViewportAdapters;

namespace PacMan.Engine
{
    public class SpriteAnimation
    {
        private readonly ViewportAdapter _viewportAdapter;
        private readonly Texture2D _spriteSheet;
        private readonly int _x;
        private readonly int _y;
        private readonly int _frameHeight;
        private readonly int _frameWidth;
        private readonly int _totalFrames;
        private float _time;
        private int _frameIndex;
        private readonly float _frameTime;

        public SpriteAnimation(ViewportAdapter viewportAdapter, Texture2D spriteSheet, int x, int y, int frameHeight, int frameWidth, int totalFrames)
        {
            _viewportAdapter = viewportAdapter;
            _spriteSheet = spriteSheet;
            _x = x;
            _y = y;
            _frameHeight = frameHeight;
            _frameWidth = frameWidth;
            _totalFrames = totalFrames;
            _frameIndex = 0;
            _frameTime = 1f / _totalFrames;
            Idle = true;
        }

        public bool Idle { get; set; }

        public void Update(GameTime gameTime)
        {
            if (Idle)
            {
                _frameIndex = 0;
            }
            else
            {
                _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                while (_time > _frameTime)
                {
                    // Play the next frame in the SpriteSheet
                    _frameIndex++;

                    // reset elapsed time
                    _time = 0f;
                }
                if (_frameIndex >= _totalFrames) _frameIndex = 0;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Calculate the source rectangle of the current frame.
            var source = new Rectangle(_frameIndex * _frameWidth, _y, _frameWidth, _frameHeight);

            // Calculate position and origin to draw in the center of the screen
            Vector2 position = new Vector2(_viewportAdapter.Center.X, _viewportAdapter.Center.Y);
            Vector2 origin = new Vector2(_frameWidth / 2.0f, _frameHeight);

            spriteBatch.Draw(_spriteSheet, position, source, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}