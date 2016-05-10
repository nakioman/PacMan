using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.InputListeners;
using MonoGame.Extended.ViewportAdapters;

namespace PacMan.Engine
{
    public class Player
    {
        private readonly ViewportAdapter _viewportAdapter;
        private readonly InputListenerManager _inputListenerManager;
        private Texture2D _spriteSheet;
        private readonly IDictionary<string, SpriteAnimation> _animations;
        private string _currentAnimation;
        private KeyboardListener _keyboardListener;

        public Player(ViewportAdapter viewportAdapter, InputListenerManager inputListenerManager)
        {
            _viewportAdapter = viewportAdapter;
            _inputListenerManager = inputListenerManager;
            _animations = new Dictionary<string, SpriteAnimation>();
            _currentAnimation = "front";
            _keyboardListener = _inputListenerManager.AddListener<KeyboardListener>();
            _keyboardListener.KeyPressed += _keyboardListener_KeyPressed;
            _keyboardListener.KeyReleased += KeyboardListenerOnKeyReleased;
        }

        private void KeyboardListenerOnKeyReleased(object sender, KeyboardEventArgs keyboardEventArgs)
        {
            _animations[_currentAnimation].Idle = true;
        }

        private void _keyboardListener_KeyPressed(object sender, KeyboardEventArgs keyboardEventArgs)
        {
            switch (keyboardEventArgs.Key)
            {
                case Keys.Left:
                    _currentAnimation = "left";
                    break;
                case Keys.Right:
                    _currentAnimation = "right";
                    break;
                case Keys.Up:
                    _currentAnimation = "back";
                    break;
                case Keys.Down:
                    _currentAnimation = "front";
                    break;
            }
            _animations[_currentAnimation].Idle = false;
        }

        public void ContentLoad(ContentManager contet)
        {
            _spriteSheet = contet.Load<Texture2D>("Player");
            _animations.Add("front", new SpriteAnimation(_viewportAdapter, _spriteSheet, 0, 0, 104, 64, 4));
            _animations.Add("left", new SpriteAnimation(_viewportAdapter, _spriteSheet, 0, 104, 104, 64, 4));
            _animations.Add("right", new SpriteAnimation(_viewportAdapter, _spriteSheet, 0, 208, 104, 64, 4));
            _animations.Add("back", new SpriteAnimation(_viewportAdapter, _spriteSheet, 0, 312, 104, 64, 4));
        }

        public void UnloadContent()
        {
            _inputListenerManager.RemoveListener(_keyboardListener);
            _animations.Clear();
            _spriteSheet.Dispose();
        }

        public void Update(GameTime gameTime)
        {
            _animations[_currentAnimation].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _animations[_currentAnimation].Draw(spriteBatch);
        }
    }
}