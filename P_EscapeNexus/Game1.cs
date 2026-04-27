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
        int selectedInventoryCase = -1;
        MouseState previousMouseState;

        int currentMurIndex = 0;
        Texture2D[] murTextures;
        Texture2D[] introTextures;
        private Song spaceSongMusic;
        private bool isMusicStarted;
        private bool isMuted = false;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameStateManager currentState;
        private Texture2D menuTexture;
        private Texture2D _debugTexture;
        private Texture2D textureBtnStart;
        private Texture2D textureBtnExit;
        private Texture2D textureBtnReglage;
        private Texture2D textureBtnMute;
        private Texture2D textureBtnPlay;
        private Texture2D textureInventaire;
        private Texture2D textureCommande;

        private Texture2D textureMur1;
        private Texture2D textureMur2;
        private Texture2D textureMur3;
        private Texture2D textureMur4;
        private Texture2D textureFlecheDroite;
        private Texture2D textureFlecheGauche;
        private Texture2D textureBadge;

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
            textureCommande = Content.Load<Texture2D>("commande");
            textureInventaire = Content.Load<Texture2D>("inventaire");
            textureMur1 = Content.Load<Texture2D>("mur1");
            textureMur2 = Content.Load<Texture2D>("mur2");
            textureMur3 = Content.Load<Texture2D>("mur3");
            textureMur4 = Content.Load<Texture2D>("mur4");
            textureFlecheDroite = Content.Load<Texture2D>("flecheDroite");
            textureFlecheGauche= Content.Load<Texture2D>("flecheGauche");
            textureBadge = Content.Load<Texture2D>("badge");

            murTextures = new Texture2D[]
            {
                textureMur1,
                textureMur2,
                textureMur3,
                textureMur4
            };

            _debugTexture = new Texture2D(GraphicsDevice, 1, 1);
            _debugTexture.SetData(new[] { Color.White });
            spaceSongMusic = Content.Load<Song>("SpaceSong");
        }
        private void DrawContour(Rectangle rectangle, Color color, int thickness)
        {
            // Haut
            _spriteBatch.Draw(_debugTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);

            // Bas
            _spriteBatch.Draw(_debugTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - thickness, rectangle.Width, thickness), color);

            // Gauche
            _spriteBatch.Draw(_debugTexture, new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);

            // Droite
            _spriteBatch.Draw(_debugTexture, new Rectangle(rectangle.X + rectangle.Width - thickness, rectangle.Y, thickness, rectangle.Height), color);
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
                        currentState = GameStateManager.Playing;

                        previousMouseState = Mouse.GetState();
                    }

                }
            }
            if (currentState == GameStateManager.Playing)
            {
                Rectangle flecheGauche = new Rectangle(20, 300, 100, 100);
                Rectangle flecheDroite = new Rectangle(960, 300, 100, 100);

                if (mouseState.LeftButton == ButtonState.Pressed &&
                    previousMouseState.LeftButton == ButtonState.Released)
                {
                    if (flecheGauche.Contains(mouseState.Position))
                    {
                        currentMurIndex++;

                        if (currentMurIndex >= murTextures.Length)
                            currentMurIndex = 0;
                    }

                    if (flecheDroite.Contains(mouseState.Position))
                    {
                        currentMurIndex--;

                        if (currentMurIndex < 0)
                            currentMurIndex = murTextures.Length - 1;
                    }
                }
                Rectangle caseHighlight1 = new Rectangle(30, 626, 110, 75);
                Rectangle caseHighlight2 = new Rectangle(154, 626, 110, 75);
                Rectangle caseHighlight3 = new Rectangle(278, 626, 110, 75);
                Rectangle caseHighlight4 = new Rectangle(402, 626, 110, 75);

                if (mouseState.LeftButton == ButtonState.Pressed &&
                    previousMouseState.LeftButton == ButtonState.Released)
                {
                    if (caseHighlight1.Contains(mouseState.Position))
                        selectedInventoryCase = 1;
                    else if (caseHighlight2.Contains(mouseState.Position))
                        selectedInventoryCase = 2;
                    else if (caseHighlight3.Contains(mouseState.Position))
                        selectedInventoryCase = 3;
                    else if (caseHighlight4.Contains(mouseState.Position))
                        selectedInventoryCase = 4;
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
                _spriteBatch.Draw(_debugTexture, exitBtn, Color.Blue * 0.4f);
                _spriteBatch.Draw(_debugTexture, reglageBtn, Color.Green * 0.4f);
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
            else if (currentState == GameStateManager.Playing)
            {
                Rectangle flecheGauche = new Rectangle(10, 300, 100, 100);
                Rectangle flecheDroite = new Rectangle(970, 300, 100, 100);
                _spriteBatch.Draw(
                    murTextures[currentMurIndex],
                     new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
                    Color.White
                 ); 
                // Hitbox visibles
                _spriteBatch.Draw(_debugTexture, flecheGauche, Color.Red * 0.4f);
                _spriteBatch.Draw(_debugTexture, flecheDroite, Color.Red * 0.4f);
                _spriteBatch.Draw(textureFlecheGauche, flecheGauche, Color.White * 0.4f);
                _spriteBatch.Draw(textureFlecheDroite, flecheDroite, Color.White * 0.4f);
                
                //Rectangle commande = new Rectangle()
                //_spriteBatch.Draw(_debugTexture, commande , )

                _spriteBatch.Draw(textureInventaire, new Rectangle(0, 550, _graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 3), Color.White);
                _spriteBatch.Draw(textureCommande, new Rectangle(720, 490, _graphics.PreferredBackBufferWidth / 3, _graphics.PreferredBackBufferHeight / 3), Color.White);
                Rectangle caseHighlight1 = new Rectangle(30, 626, 110, 75);
                Rectangle caseHighlight2 = new Rectangle(154, 626, 110, 75);
                Rectangle caseHighlight3 = new Rectangle(278, 626, 110, 75);
                Rectangle caseHighlight4 = new Rectangle(402, 626, 110, 75);

                Rectangle caseInventaire1 = new Rectangle(37, 632, 95, 65);

                if (selectedInventoryCase == 1)
                    DrawContour(caseHighlight1, Color.Yellow, 4);

                if (selectedInventoryCase == 2)
                    DrawContour(caseHighlight2, Color.Yellow, 4);

                if (selectedInventoryCase == 3)
                    DrawContour(caseHighlight3, Color.Yellow, 4);

                if (selectedInventoryCase == 4)
                    DrawContour(caseHighlight4, Color.Yellow, 4);
                _spriteBatch.Draw(textureBadge, new Rectangle(37, 630, _graphics.PreferredBackBufferWidth / 12, _graphics.PreferredBackBufferHeight / 11), Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
