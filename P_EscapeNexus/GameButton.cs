using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace P_EscapeNexus
{
    /// <summary>
    /// Représente un bouton interactif dans le jeu.
    /// Permet de détecter les clics de la souris sur une zone donnée
    /// et d'afficher une texture associée.
    /// </summary>

    public class GameButton
    {
        public Rectangle Rectangle { get; private set; }

        public GameButton(Rectangle rectangle)
        {
            Rectangle = rectangle;
        }

        public bool IsClicked(MouseState mouseState, MouseState previousMouseState)
        {
            return mouseState.LeftButton == ButtonState.Pressed &&
                   previousMouseState.LeftButton == ButtonState.Released &&
                   Rectangle.Contains(mouseState.Position);
        }
    }
}