using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace P_EscapeNexus
{
    /// <summary>
    /// Classe principale du jeu.
    /// Gère les états (menu, jeu, intro) ainsi que le chargement et l'affichage.
    /// </summary>
    public class Game1 : Game
    {
        // Les états du jeu
        private enum State
        {
            Menu,
            Reglage,
            Intro,
            Playing
        }

        // Gestion de la fenêtre et de l'affichage
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Sauvegarde de l'état précédent de la souris et du clavier
        private MouseState previousMouseState;
        private KeyboardState previousKeyboardState;

        // État actuel du jeu
        private State currentState = State.Playing;

        // Gestion des hitbox en mode debug
        private HitboxDebug hitboxDebug;

        // Gestion de la musique
        private AudioManager audioManager;

        // Les différents écrans du jeu
        private MenuScreen menuScreen;
        private ReglageScreen reglageScreen;
        private IntroScreen introScreen;
        private PlayingScreen playingScreen;

        public Game1()
        {
            // Initialisation de la fenêtre du jeu
            _graphics = new GraphicsDeviceManager(this);

            // Dossier où se trouvent les images, sons, polices, etc.
            Content.RootDirectory = "Content";

            // Affiche le curseur de la souris
            IsMouseVisible = true;

            // Taille de la fenêtre
            _graphics.PreferredBackBufferWidth = 1080;
            _graphics.PreferredBackBufferHeight = 720;
        }

        protected override void LoadContent()
        {
            // Permet de dessiner les textures à l'écran
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Initialise le système de debug des hitbox
            hitboxDebug = new HitboxDebug(GraphicsDevice);

            // Chargement des textures du menu
            Texture2D menuTexture = Content.Load<Texture2D>("Menu");
            Texture2D textureBtnStart = Content.Load<Texture2D>("startBtn");
            Texture2D textureBtnExit = Content.Load<Texture2D>("exit");
            Texture2D textureBtnReglage = Content.Load<Texture2D>("reglage");
            Texture2D textureBtnMute = Content.Load<Texture2D>("mute");
            Texture2D textureBtnPlay = Content.Load<Texture2D>("unmute");

            // Chargement et création du gestionnaire audio
            audioManager = new AudioManager(Content.Load<Song>("SpaceSong"));

            // Création de l'écran du menu principal
            menuScreen = new MenuScreen(
                menuTexture,
                new GameButton(textureBtnStart, new Rectangle(450, 220, 1080 / 5, 720 / 6)),
                new GameButton(textureBtnReglage, new Rectangle(450, 350, 1080 / 5, 720 / 6)),
                new GameButton(textureBtnExit, new Rectangle(450, 480, 1080 / 5, 720 / 6)),
                hitboxDebug
            );

            // Création de l'écran des réglages
            reglageScreen = new ReglageScreen(
                menuTexture,
                textureBtnMute,
                textureBtnPlay,
                new GameButton(textureBtnPlay, new Rectangle(450, 220, 1080 / 5, 720 / 6)),
                audioManager,
                hitboxDebug
            );

            // Création de l'introduction avec plusieurs images
            introScreen = new IntroScreen(
                new Texture2D[]
                {
                    Content.Load<Texture2D>("H1"),
                    Content.Load<Texture2D>("H2"),
                    Content.Load<Texture2D>("H3"),
                    Content.Load<Texture2D>("H4")
                }
            );

            // Puzzle du tableau sur le mur 8
            TableauPuzzle tableauPuzzle = new TableauPuzzle(
                Content.Load<Texture2D>("mur8Tableau"),
                Content.Load<Texture2D>("mur8postitGauche"),
                Content.Load<Texture2D>("mur8postitDroite")
            );

            // Gestion des murs et des salles
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

            // Gestion de l'inventaire
            InventoryManager inventoryManager = new InventoryManager(
                Content.Load<Texture2D>("inventaire"),
                Content.Load<Texture2D>("badge"),
                Content.Load<Texture2D>("tournevis"),
                1080,
                720
            );

            // Console de messages affichée en jeu
            MessageConsole messageConsole = new MessageConsole(
                Content.Load<SpriteFont>("DefaultFont"),
                new Vector2(775, 565)
            );

            // Puzzle du panneau électrique
            ElectricPanelPuzzle electricPanelPuzzle = new ElectricPanelPuzzle(
                GraphicsDevice,
                Content.Load<Texture2D>("panneauOuvert")
            );

            // Puzzle du digicode
            DigicodePuzzle digicodePuzzle = new DigicodePuzzle(
                GraphicsDevice,
                Content.Load<Texture2D>("digicode"),
                Content.Load<SpriteFont>("DefaultFont")
            );

            // Création de l'écran principal de jeu
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
        }

        protected override void Update(GameTime gameTime)
        {
            // Récupère l'état actuel de la souris et du clavier
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            // Active ou désactive l'affichage des hitbox avec F1
            if (keyboardState.IsKeyDown(Keys.F1) &&
                previousKeyboardState.IsKeyUp(Keys.F1))
            {
                hitboxDebug.Enabled = !hitboxDebug.Enabled;
            }

            // Retour au menu avec Échap
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                currentState = State.Menu;
            }

            // Met à jour la musique
            audioManager.Update();

            // Met à jour l'écran selon l'état actuel du jeu
            if (currentState == State.Menu)
            {
                menuScreen.Update(mouseState, previousMouseState);

                if (menuScreen.StartClicked)
                {
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
            }
            else if (currentState == State.Intro)
            {
                introScreen.Update(mouseState, previousMouseState);

                if (introScreen.Finished)
                {
                    currentState = State.Playing;
                }
            }
            else if (currentState == State.Playing)
            {
                playingScreen.Update(mouseState, previousMouseState);
            }

            // Sauvegarde les états actuels souris/clavier pour les comparer à la prochaine frame
            previousMouseState = mouseState;
            previousKeyboardState = keyboardState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Nettoie l'écran avant de redessiner
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Dessine l'écran correspondant à l'état actuel
            if (currentState == State.Menu)
                menuScreen.Draw(_spriteBatch);
            else if (currentState == State.Reglage)
                reglageScreen.Draw(_spriteBatch);
            else if (currentState == State.Intro)
                introScreen.Draw(_spriteBatch);
            else if (currentState == State.Playing)
                playingScreen.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}