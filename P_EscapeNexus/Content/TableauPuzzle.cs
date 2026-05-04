using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace P_EscapeNexus
{
    /// <summary>
    /// Gère le puzzle du tableau sur le mur 8.
    /// Permet d'ouvrir un zoom du tableau, de cliquer sur les post-it
    /// et d'afficher l'image correspondante selon le post-it sélectionné.
    /// </summary>
    public class TableauPuzzle
    {
        private Texture2D tableau;
        private Texture2D postitGauche;
        private Texture2D postitDroite;

        public bool IsOpen { get; private set; } = false;

        private bool gaucheOuvert = false;
        private bool droiteOuvert = false;

        // Hitbox post-it (ajuste si besoin)
        private Rectangle hitboxGauche = new Rectangle(640, 170, 100, 100);
        private Rectangle hitboxDroite = new Rectangle(760, 180, 100, 100);

        public TableauPuzzle(Texture2D tableau, Texture2D gauche, Texture2D droite)
        {
            this.tableau = tableau;
            this.postitGauche = gauche;
            this.postitDroite = droite;
        }

        public void Open()
        {
            IsOpen = true;
            gaucheOuvert = false;
            droiteOuvert = false;
        }

        public void Close()
        {
            IsOpen = false;
            gaucheOuvert = false;
            droiteOuvert = false;
        }

        public void Update(MouseState mouse, MouseState prev)
        {
            if (!IsOpen)
                return;

            bool click = mouse.LeftButton == ButtonState.Pressed &&
                         prev.LeftButton == ButtonState.Released;

            if (!click)
                return;

            if (hitboxGauche.Contains(mouse.Position))
            {
                gaucheOuvert = true;
                droiteOuvert = false;
            }
            else if (hitboxDroite.Contains(mouse.Position))
            {
                droiteOuvert = true;
                gaucheOuvert = false;
            }
            else
            {
                Close();
            }
        }

        public void Draw(SpriteBatch sb, HitboxDebug debug)
        {
            if (!IsOpen)
                return;

            Texture2D current = tableau;

            if (gaucheOuvert)
                current = postitGauche;

            if (droiteOuvert)
                current = postitDroite;

            sb.Draw(current, new Rectangle(0, 0, 1080, 720), Color.White);

            // Debug hitbox
            debug.Draw(sb, hitboxGauche, Color.Green);
            debug.Draw(sb, hitboxDroite, Color.Blue);
        }
    }
}