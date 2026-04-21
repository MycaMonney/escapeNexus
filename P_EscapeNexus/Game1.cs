using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace P_EscapeNexus
{
    public class Game1 : Game
    {
        public enum GameStateManager
        {
            Menu,
            Reglage,
            Intro,
            Playing,
            Pause,
            End
        }
        int currentTextureIndex = 0;
        MouseState previousMouseState;

        Texture2D[] introTextures;
        private Song spaceSongMusic;
        private bool isMusicStarted;
        private bool isMuted = false;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameStateManager currentState;
        private Texture2D TextureIntro1;
        private Texture2D TextureIntro2;
        private Texture2D TextureIntro3;
        private Texture2D TextureIntro4;
        private Texture2D menuTexture;
        private Texture2D _debugTexture;
        private Texture2D textureBtnStart;
        private Texture2D textureBtnExit;
        private Texture2D textureBtnReglage;
        private Texture2D textureBtnMute;
        private Texture2D textureBtnPlay;
        
        // IA Probleme collision voir doc
        bool inputLocked = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1080;
            _graphics.PreferredBackBufferHeight = 720;
        }

        protected override void Initialize()
        {
            currentState = GameStateManager.Menu;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            introTextures = new Texture2D[4];
            introTextures[0] = Content.Load<Texture2D>("H1");
            introTextures[1] = Content.Load<Texture2D>("H2");
            introTextures[2] = Content.Load<Texture2D>("H3");
            introTextures[3] = Content.Load<Texture2D>("H4");

            

            menuTexture = Content.Load<Texture2D>("Menu");
            textureBtnStart = Content.Load<Texture2D>("startBtn");
            textureBtnExit = Content.Load<Texture2D>("exit");
            textureBtnReglage = Content.Load<Texture2D>("reglage");
            textureBtnMute = Content.Load<Texture2D>("mute");
            textureBtnPlay = Content.Load<Texture2D>("unmute");

            _debugTexture = new Texture2D(GraphicsDevice, 1, 1);
            _debugTexture.SetData(new[] { Color.White });
            spaceSongMusic = Content.Load<Song>("SpaceSong");
        }

        protected override void Update(GameTime gameTime)
        {
            if (inputLocked)
            {
                previousMouseState = Mouse.GetState();
                inputLocked = false;
                return;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                currentState = GameStateManager.Menu;
            if (!isMusicStarted)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(spaceSongMusic);
                isMusicStarted = true;
            }

            // si on mute → couper le son
            if (isMuted)
            {
                MediaPlayer.Pause();
            }
            else
            {
                if (MediaPlayer.State != MediaState.Playing)
                    MediaPlayer.Resume();
            }

            MouseState mouseState = Mouse.GetState();
            if (currentState == GameStateManager.Intro)
            {
                if (mouseState.LeftButton == ButtonState.Pressed &&
                    previousMouseState.LeftButton == ButtonState.Released)
                {
                    currentTextureIndex++;

                    if (currentTextureIndex >= introTextures.Length)
                    {
                        currentTextureIndex = 0;
                        inputLocked = true;
                        currentState = GameStateManager.Menu;

                        previousMouseState = Mouse.GetState();
                    }

                }
            }
            if (currentState == GameStateManager.Menu)
            {
                Rectangle startBtn = new Rectangle(450, 220, _graphics.PreferredBackBufferWidth / 5, _graphics.PreferredBackBufferHeight / 6);
                Rectangle reglageBtn = new Rectangle(450, 350, _graphics.PreferredBackBufferWidth / 5, _graphics.PreferredBackBufferHeight / 6);
                Rectangle exitBtn = new Rectangle(450, 480, _graphics.PreferredBackBufferWidth / 5, _graphics.PreferredBackBufferHeight / 6);
                if (mouseState.LeftButton == ButtonState.Pressed &&
    previousMouseState.LeftButton == ButtonState.Released &&
    startBtn.Contains(mouseState.Position))
                {
                    currentTextureIndex = 0;
                    currentState = GameStateManager.Intro;
                }
                if (mouseState.LeftButton == ButtonState.Pressed && exitBtn.Contains(mouseState.Position))
                {
                    Exit();
                }
                if (mouseState.LeftButton == ButtonState.Pressed && reglageBtn.Contains(mouseState.Position))
                {
                    currentTextureIndex = 0;
                    currentState = GameStateManager.Reglage;
                }
            }
            if (currentState == GameStateManager.Reglage)
            {
                Rectangle muteBtn = new Rectangle(450, 220, _graphics.PreferredBackBufferWidth / 5, _graphics.PreferredBackBufferHeight / 6);

                if (mouseState.LeftButton == ButtonState.Pressed &&
                    previousMouseState.LeftButton == ButtonState.Released &&
                    muteBtn.Contains(mouseState.Position))
                {
                    isMuted = !isMuted;
                }
            }

            previousMouseState = mouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            if (currentState == GameStateManager.Intro)
            { _spriteBatch.Draw(introTextures[currentTextureIndex], new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White); }

            else if (currentState == GameStateManager.Menu)
            {
                _spriteBatch.Draw(menuTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
                Rectangle startBtn = new Rectangle(450, 220, _graphics.PreferredBackBufferWidth / 5, _graphics.PreferredBackBufferHeight / 6);
                Rectangle reglageBtn = new Rectangle(450, 350, _graphics.PreferredBackBufferWidth / 5, _graphics.PreferredBackBufferHeight / 6);
                Rectangle exitBtn = new Rectangle(450, 480, _graphics.PreferredBackBufferWidth / 5, _graphics.PreferredBackBufferHeight / 6);
                _spriteBatch.Draw(textureBtnStart, startBtn, Color.White);
                _spriteBatch.Draw(textureBtnExit, exitBtn, Color.White);
                _spriteBatch.Draw(textureBtnReglage, reglageBtn, Color.White);
                // Hitbox visible
                _spriteBatch.Draw(_debugTexture, startBtn, Color.Red * 0.4f);
                _spriteBatch.Draw(_debugTexture, exitBtn, Color.Blue* 0.4f);
                _spriteBatch.Draw(_debugTexture, reglageBtn, Color.Green* 0.4f);
            }
            else if (currentState == GameStateManager.Reglage)
            {
                _spriteBatch.Draw(menuTexture,
                    new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
                    Color.White);

                Rectangle muteBtn = new Rectangle(450, 220, _graphics.PreferredBackBufferWidth / 5, _graphics.PreferredBackBufferHeight / 6);

                if (isMuted)
                    _spriteBatch.Draw(textureBtnMute, muteBtn, Color.White); // unmute button
                else
                    _spriteBatch.Draw(textureBtnPlay, muteBtn, Color.White); // mute button

                _spriteBatch.Draw(_debugTexture, muteBtn, Color.Red * 0.4f);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
