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
        private Texture2D background;
        private Texture2D mute;
        private Texture2D unmute;
        private GameButton btn;
        private AudioManager audio;
        private HitboxDebug debug;

        public ReglageScreen(Texture2D bg, Texture2D mute, Texture2D unmute, GameButton btn, AudioManager audio, HitboxDebug debug)
        {
            background = bg;
            this.mute = mute;
            this.unmute = unmute;
            this.btn = btn;
            this.audio = audio;
            this.debug = debug;
        }

        public void Update(MouseState mouse, MouseState prev)
        {
            if (btn.IsClicked(mouse, prev))
                audio.ToggleMute();
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(background, new Rectangle(0, 0, 1080, 720), Color.White);

            if (audio.IsMuted)
                sb.Draw(mute, btn.Rectangle, Color.White);
            else
                sb.Draw(unmute, btn.Rectangle, Color.White);

            debug.Draw(sb, btn.Rectangle, Color.Red);
        }
    }
}