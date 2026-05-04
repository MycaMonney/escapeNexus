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
        private Texture2D texture;

        public GameButton(Texture2D texture, Rectangle rectangle)
        {
            this.texture = texture;
            Rectangle = rectangle;
        }

        public bool IsClicked(MouseState mouseState, MouseState previousMouseState)
        {
            return mouseState.LeftButton == ButtonState.Pressed &&
                   previousMouseState.LeftButton == ButtonState.Released &&
                   Rectangle.Contains(mouseState.Position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Rectangle, Color.White);
        }
    }
}