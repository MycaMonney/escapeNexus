using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace P_EscapeNexus
{
    /// <summary>
    /// Gère l'écran du menu principal.
    /// Affiche les boutons (jouer, réglages, quitter) et détecte les clics
    /// pour naviguer entre les différentes parties du jeu.
    /// </summary>
    public class MenuScreen
    {
        private Texture2D background;
        private GameButton startBtn;
        private GameButton reglageBtn;
        private GameButton exitBtn;
        private HitboxDebug debug;

        public bool StartClicked { get; private set; }
        public bool ReglageClicked { get; private set; }
        public bool ExitClicked { get; private set; }

        public MenuScreen(Texture2D bg, GameButton start, GameButton reglage, GameButton exit, HitboxDebug debug)
        {
            background = bg;
            startBtn = start;
            reglageBtn = reglage;
            exitBtn = exit;
            this.debug = debug;
        }

        public void Update(MouseState mouse, MouseState prev)
        {
            StartClicked = startBtn.IsClicked(mouse, prev);
            ReglageClicked = reglageBtn.IsClicked(mouse, prev);
            ExitClicked = exitBtn.IsClicked(mouse, prev);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(background, new Rectangle(0, 0, 1080, 720), Color.White);

            // Hitbox visibles seulement si F1 est activé
            debug.Draw(sb, startBtn.Rectangle, Color.Red);
            debug.Draw(sb, reglageBtn.Rectangle, Color.Green);
            debug.Draw(sb, exitBtn.Rectangle, Color.Blue);
        }
    }
}