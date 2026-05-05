using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace P_EscapeNexus
{
    /// <summary>
    /// Classe principale du jeu.
    /// Gère la boucle du jeu, les différents états (menu, intro, jeu, victoire, défaite),
    /// le chronomètre de 10 minutes, ainsi que les transitions entre les écrans.
    /// S'occupe également du chargement des ressources, de l'affichage global
    /// et des interactions utilisateur liées à l'interface (boutons, clics).
    /// </summary>
    public class Game1 : Game
    {
        private enum State
        {
            Menu,
            Reglage,
            Intro,
            Playing,
            Victoire,
            Defaite
        }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private MouseState previousMouseState;
        private KeyboardState previousKeyboardState;

        private State currentState = State.Victoire;

        private HitboxDebug hitboxDebug;
        private AudioManager audioManager;

        private MenuScreen menuScreen;
        private ReglageScreen reglageScreen;
        private IntroScreen introScreen;
        private PlayingScreen playingScreen;

        private Texture2D victoireTexture;
        private Texture2D defaiteTexture;

        private SpriteFont timerFont;

        private double tempsRestant = 600;
        private bool chronoLance = false;

        private Rectangle btnRejouer;
        private Rectangle btnQuitter;
        private Rectangle btnMenuVictoire;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1080;
            _graphics.PreferredBackBufferHeight = 720;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            hitboxDebug = new HitboxDebug(GraphicsDevice);

            Texture2D menuTexture = Content.Load<Texture2D>("Menu");
            Texture2D textureBtnMute = Content.Load<Texture2D>("mute");
            Texture2D textureBtnPlay = Content.Load<Texture2D>("unmute");

            victoireTexture = Content.Load<Texture2D>("victoire");
            defaiteTexture = Content.Load<Texture2D>("defaite");

            timerFont = Content.Load<SpriteFont>("DefaultFont");

            audioManager = new AudioManager(Content.Load<Song>("SpaceSong"));

            menuScreen = new MenuScreen(
                menuTexture,
                new GameButton(new Rectangle(80, 248, 340, 100)),
                new GameButton(new Rectangle(80, 368, 340, 100)),
                new GameButton(new Rectangle(80, 490, 340, 100)),
                hitboxDebug
            );

            reglageScreen = new ReglageScreen(
                textureBtnMute,
                textureBtnPlay,
                new GameButton(new Rectangle(190, 335, 295, 95)),
                new GameButton(new Rectangle(68, 610, 165, 60)),
                audioManager,
                hitboxDebug
            );

            btnRejouer = new Rectangle(92, 570, 240, 50);
            btnQuitter = new Rectangle(376, 570, 220, 50);
            btnMenuVictoire = new Rectangle(120, 490, 310, 50);

            CreerNouvellePartie();
        }

        private void CreerNouvellePartie()
        {
            introScreen = new IntroScreen(
                new Texture2D[]
                {
                    Content.Load<Texture2D>("H1"),
                    Content.Load<Texture2D>("H2"),
                    Content.Load<Texture2D>("H3"),
                    Content.Load<Texture2D>("H4"),
                    Content.Load<Texture2D>("H5")
                }
            );

            TableauPuzzle tableauPuzzle = new TableauPuzzle(
                Content.Load<Texture2D>("mur8Tableau"),
                Content.Load<Texture2D>("mur8postitGauche"),
                Content.Load<Texture2D>("mur8postitDroite")
            );

            RoomManager roomManager = new RoomManager(
                new Texture2D[]
                {
                    Content.Load<Texture2D>("mur1"),
                    Content.Load<Texture2D>("mur2"),
                    Content.Load<Texture2D>("mur3"),
                    Content.Load<Texture2D>("mur4"),
                    Content.Load<Texture2D>("mur5"),
                    Content.Load<Texture2D>("mur6"),
                    Content.Load<Texture2D>("mur7"),
                    Content.Load<Texture2D>("mur8"),
                    Content.Load<Texture2D>("mur9"),
                    Content.Load<Texture2D>("mur10"),
                    Content.Load<Texture2D>("mur11"),
                    Content.Load<Texture2D>("mur12")
                },
                Content.Load<Texture2D>("porteOuverte"),
                Content.Load<Texture2D>("murPanneauOuvert"),
                Content.Load<Texture2D>("mur6SansOutils"),
                Content.Load<Texture2D>("mur7PorteOuverte"),
                Content.Load<Texture2D>("mur10SansCombinaison"),
                Content.Load<Texture2D>("mur12PorteOuverte")
            );

            InventoryManager inventoryManager = new InventoryManager(
                Content.Load<Texture2D>("inventaire"),
                Content.Load<Texture2D>("badge"),
                Content.Load<Texture2D>("tournevis"),
                1080,
                720
            );

            MessageConsole messageConsole = new MessageConsole(
                Content.Load<SpriteFont>("DefaultFont"),
                new Vector2(775, 565)
            );

            ElectricPanelPuzzle electricPanelPuzzle = new ElectricPanelPuzzle(
                GraphicsDevice,
                Content.Load<Texture2D>("panneauOuvert")
            );

            DigicodePuzzle digicodePuzzle = new DigicodePuzzle(
                GraphicsDevice,
                Content.Load<Texture2D>("digicode"),
                Content.Load<SpriteFont>("DefaultFont")
            );

            playingScreen = new PlayingScreen(
                roomManager,
                inventoryManager,
                messageConsole,
                hitboxDebug,
                Content.Load<Texture2D>("flecheGauche"),
                Content.Load<Texture2D>("flecheDroite"),
                Content.Load<Texture2D>("commande"),
                Content.Load<Texture2D>("outilsAvecObj"),
                Content.Load<Texture2D>("outilsSansObj"),
                electricPanelPuzzle,
                digicodePuzzle,
                tableauPuzzle
            );

            tempsRestant = 600;
            chronoLance = false;
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            bool click = mouseState.LeftButton == ButtonState.Pressed &&
                         previousMouseState.LeftButton == ButtonState.Released;

            if (keyboardState.IsKeyDown(Keys.F1) &&
                previousKeyboardState.IsKeyUp(Keys.F1))
            {
                hitboxDebug.Enabled = !hitboxDebug.Enabled;
            }

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                currentState = State.Menu;
                chronoLance = false;
            }

            audioManager.Update();

            if (currentState == State.Menu)
            {
                menuScreen.Update(mouseState, previousMouseState);

                if (menuScreen.StartClicked)
                {
                    CreerNouvellePartie();
                    introScreen.Reset();
                    currentState = State.Intro;
                }

                if (menuScreen.ReglageClicked)
                {
                    currentState = State.Reglage;
                }

                if (menuScreen.ExitClicked)
                {
                    Exit();
                }
            }
            else if (currentState == State.Reglage)
            {
                reglageScreen.Update(mouseState, previousMouseState);

                if (reglageScreen.BackClicked)
                {
                    currentState = State.Menu;
                }
            }
            else if (currentState == State.Intro)
            {
                introScreen.Update(mouseState, previousMouseState);

                if (introScreen.Finished)
                {
                    tempsRestant = 600;
                    chronoLance = true;
                    currentState = State.Playing;
                }
            }
            else if (currentState == State.Playing)
            {
                if (chronoLance)
                {
                    tempsRestant -= gameTime.ElapsedGameTime.TotalSeconds;
                }

                if (tempsRestant <= 0)
                {
                    tempsRestant = 0;
                    chronoLance = false;
                    currentState = State.Defaite;
                }
                else
                {
                    playingScreen.Update(mouseState, previousMouseState, () =>
                    {
                        chronoLance = false;
                        currentState = State.Victoire;
                    });
                }
            }
            else if (currentState == State.Defaite)
            {
                if (click)
                {
                    if (btnRejouer.Contains(mouseState.Position))
                    {
                        CreerNouvellePartie();
                        introScreen.Reset();
                        currentState = State.Intro;
                    }
                    else if (btnQuitter.Contains(mouseState.Position))
                    {
                        Exit();
                    }
                }
            }
            else if (currentState == State.Victoire)
            {
                if (click)
                {
                    if (btnMenuVictoire.Contains(mouseState.Position))
                    {
                        currentState = State.Menu;
                    }
                }
            }

            previousMouseState = mouseState;
            previousKeyboardState = keyboardState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (currentState == State.Menu)
            {
                menuScreen.Draw(_spriteBatch);
            }
            else if (currentState == State.Reglage)
            {
                reglageScreen.Draw(_spriteBatch);
            }
            else if (currentState == State.Intro)
            {
                introScreen.Draw(_spriteBatch);
            }
            else if (currentState == State.Playing)
            {
                playingScreen.Draw(_spriteBatch);
                DrawTimer();
            }
            else if (currentState == State.Victoire)
            {
                _spriteBatch.Draw(
                    victoireTexture,
                    new Rectangle(0, 0, 1080, 720),
                    Color.White
                );

                hitboxDebug.Draw(_spriteBatch, btnMenuVictoire, Color.Blue);
            }
            else if (currentState == State.Defaite)
            {
                _spriteBatch.Draw(
                    defaiteTexture,
                    new Rectangle(0, 0, 1080, 720),
                    Color.White
                );

                hitboxDebug.Draw(_spriteBatch, btnRejouer, Color.Green);
                hitboxDebug.Draw(_spriteBatch, btnQuitter, Color.Red);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawTimer()
        {
            int minutes = (int)tempsRestant / 60;
            int secondes = (int)tempsRestant % 60;

            string timerTexte = minutes.ToString("00") + ":" + secondes.ToString("00");

            _spriteBatch.DrawString(
                timerFont,
                timerTexte,
                new Vector2(30, 30),
                Color.White
            );
        }
    }
}