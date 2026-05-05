using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace P_EscapeNexus
{
    /// <summary>
    /// Gère l'écran des réglages du jeu.
    /// Permet au joueur d'activer ou désactiver le son via un bouton
    /// et affiche l'état actuel (mute ou non).
    /// </summary>
    public class ReglageScreen
    {
        private Texture2D imageMute;
        private Texture2D imageUnmute;

        private GameButton soundButton;
        private GameButton backButton;

        private AudioManager audioManager;
        private HitboxDebug debug;

        public bool BackClicked { get; private set; }

        public ReglageScreen(
            Texture2D mute,
            Texture2D unmute,
            GameButton soundBtn,
            GameButton backBtn,
            AudioManager audio,
            HitboxDebug debug)
        {
            imageMute = mute;
            imageUnmute = unmute;
            soundButton = soundBtn;
            backButton = backBtn;
            audioManager = audio;
            this.debug = debug;
        }

        public void Update(MouseState mouse, MouseState prev)
        {
            // reset à chaque frame
            BackClicked = false;

            if (soundButton.IsClicked(mouse, prev))
            {
                audioManager.ToggleMute();
            }

            if (backButton.IsClicked(mouse, prev))
            {
                BackClicked = true;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            Texture2D currentImage = audioManager.IsMuted ? imageMute : imageUnmute;

            sb.Draw(currentImage, new Rectangle(0, 0, 1080, 720), Color.White);

            // Debug
            debug.Draw(sb, soundButton.Rectangle, Color.Yellow);
            debug.Draw(sb, backButton.Rectangle, Color.Red);
        }
    }
}