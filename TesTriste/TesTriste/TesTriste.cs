using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TesTriste
{
    public class TesTriste : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Previous and current keyboardstates
        private KeyboardState _pKs;
        private KeyboardState _cKs;

        private int _timer;

        private Plate _plate;

        private Piece _currentPiece,
                      _holdedPiece;

        private int _score;

        enum GameState { Running, GameOver, Pause }
        private GameState _gameState;

        public TesTriste()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _plate = new Plate { Cells = new byte[10, 22] };

            _currentPiece = new Piece(_plate);

            _gameState = GameState.Running;

            _graphics.PreferredBackBufferWidth = 400;
            _graphics.PreferredBackBufferHeight = 580;
            _graphics.ApplyChanges();

            _pKs = Keyboard.GetState();
            _cKs = Keyboard.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            IsMouseVisible = true;
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            TexturesManager.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive)
                return;

            _cKs = Keyboard.GetState();

            //

            if (_pKs.IsKeyDown(Keys.P) && _cKs.IsKeyUp(Keys.P))
                if (_gameState == GameState.Pause)
                    _gameState = GameState.Running;
                else if (_gameState == GameState.Running)
                    _gameState = GameState.Pause;

            if (_gameState == GameState.Pause)
            {
                _pKs = _cKs;
                return;
            }

            if (_gameState == GameState.GameOver)
                return;

            if (_pKs.IsKeyDown(Keys.Down) && _cKs.IsKeyUp(Keys.Down))
                _currentPiece.Drop();

            if (_pKs.IsKeyDown(Keys.Left) && _cKs.IsKeyUp(Keys.Left))
                _currentPiece.GoLeft();

            if (_pKs.IsKeyDown(Keys.Right) && _cKs.IsKeyUp(Keys.Right))
                _currentPiece.GoRight();

            if (_pKs.IsKeyDown(Keys.H) && _cKs.IsKeyUp(Keys.H))
            {
                if (_holdedPiece == null)
                {
                    _holdedPiece = _currentPiece;
                    _currentPiece.Clear();
                    _currentPiece = new Piece(_plate);
                }
                else
                {
                    Piece aux = _holdedPiece;
                    _holdedPiece = _currentPiece;
                    _currentPiece.Clear();
                    _currentPiece = aux;
                    _currentPiece.PutUp();
                    _currentPiece.Print();
                }
            }

            _timer++;
            if (_timer == 20)
            {
                if (_currentPiece.CanGoDown())
                    _currentPiece.GoDown();
                else
                {
                    _currentPiece.Burn();
                    _score += 10;

                    int count = 0;

                    for (int y = 0; y < 22; y++)
                        if (_plate.IsLineFull(y))
                        {
                            count++;
                            _plate.EmptyLine(y);
                            y--;
                        }

                    _score += (int)(Math.Pow(Math.Exp(count), 3) * 30);

                    if (IsGameOver())
                    {
                        _gameState = GameState.GameOver;
                        return;
                    }
                    _currentPiece = new Piece(_plate);
                }
                _timer = 0;
            }
       

            base.Update(gameTime);

            _pKs = _cKs;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(TexturesManager.GameBackground, new Vector2(0, 0), Color.White);

            _plate.Draw(_spriteBatch);

            _spriteBatch.DrawString(
                TexturesManager.Trebuchet18, _score.ToString(CultureInfo.InvariantCulture),
                new Vector2(312 -
                    TexturesManager.Trebuchet18.MeasureString(_score.ToString(CultureInfo.InvariantCulture)).X/2, 82),
                new Color(255, 255, 255, 180));

            if (_holdedPiece != null)
                _holdedPiece.Draw(312, 168, _spriteBatch);

            if (_gameState == GameState.GameOver)
                _spriteBatch.Draw(TexturesManager.GameOver, new Vector2(0, 0),
                    new Color(255, 255, 255, 255));

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool IsGameOver()
        {
            return _plate.Cells[4, 0] == 1 || _plate.Cells[5, 0] == 1;
        }
    }
}
