using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.InputListeners;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.ViewportAdapters;

namespace PacMan.Engine
{
    public class Player : IActorTarget
    {
        private readonly ViewportAdapter _viewportAdapter;
        private int _moveX;
        private int _moveY;
        private SpriteSheetAnimationGroup _animationGroup;
        private SpriteSheetAnimator _animator;
        private int _keysPressed;

        public Player(ViewportAdapter viewportAdapter, InputListenerManager inputListenerManager)
        {
            _viewportAdapter = viewportAdapter;
            var keyboardListener = inputListenerManager.AddListener<KeyboardListener>();
            keyboardListener.KeyPressed += _keyboardListener_KeyPressed;
            keyboardListener.KeyReleased += KeyboardListenerOnKeyReleased;
            keyboardListener.KeyTyped += KeyboardListener_KeyTyped;
            _moveX = _moveY = 0;
            _keysPressed = 0;
        }

        private void KeyboardListener_KeyTyped(object sender, KeyboardEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KeyboardListenerOnKeyReleased(object sender, KeyboardEventArgs keyboardEventArgs)
        {
            _keysPressed--;
            if (_keysPressed == 0)
            {
                switch (keyboardEventArgs.Key)
                {
                    case Keys.Left:
                        _animator.PlayAnimation("leftIdle");
                        _moveX = 0;
                        break;
                    case Keys.Right:
                        _animator.PlayAnimation("rightIdle");
                        _moveX = 0;
                        break;
                    case Keys.Up:
                        _animator.PlayAnimation("backIdle");
                        _moveY = 0;
                        break;
                    case Keys.Down:
                        _animator.PlayAnimation("frontIdle");
                        _moveY = 0;
                        break;
                }
            }
        }

        private void UpdatePlayerPosition(int x, int y)
        {
            var actualPosition = _animator.Sprite.Position;
            var newPosition = new Vector2(actualPosition.X + x, actualPosition.Y + y);

            if (newPosition.X < 0)
            {
                newPosition.X = _viewportAdapter.VirtualWidth;
            }
            else if (newPosition.X > _viewportAdapter.VirtualWidth)
            {
                newPosition.X = 0;
            }

            if (newPosition.Y < 0)
            {
                newPosition.Y = _viewportAdapter.VirtualHeight;
            }
            else if (newPosition.Y > _viewportAdapter.VirtualHeight)
            {
                newPosition.Y = 0;
            }

            Velocity = new Vector2(100f * _moveX, 100f * _moveY);
            Position = newPosition;
        }

        private void _keyboardListener_KeyPressed(object sender, KeyboardEventArgs keyboardEventArgs)
        {
            _moveX = _moveY = 0;
            _keysPressed++;
            switch (keyboardEventArgs.Key)
            {
                case Keys.Left:
                    _animator.PlayAnimation("left");
                    _moveX = -1;
                    break;
                case Keys.Right:
                    _animator.PlayAnimation("right");
                    _moveX = 1;
                    break;
                case Keys.Up:
                    _animator.PlayAnimation("back");
                    _moveY = -1;
                    break;
                case Keys.Down:
                    _animator.PlayAnimation("front");
                    _moveY = 1;
                    break;
            }
        }

        public void LoadContent(ContentManager content)
        {
            _animationGroup = content.Load<SpriteSheetAnimationGroup>("player-animation");
            _animator = new SpriteSheetAnimator(_animationGroup);
            Position = new Vector2(300f, 300f);
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            UpdatePlayerPosition(_moveX, _moveY);
            _animator.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _animator.Sprite.Draw(spriteBatch);
        }

        public Vector2 Position {
            get { return _animator.Sprite.Position; }
            set { _animator.Sprite.Position = value; }
        }

        public RectangleF BoundingBox => _animator.Sprite.GetBoundingRectangle();

        public void OnCollision(CollisionInfo collisionInfo)
        {
            Position -= collisionInfo.PenetrationVector;
            Velocity = Vector2.Zero;
        }

        public Vector2 Velocity { get; set; }
    }
}