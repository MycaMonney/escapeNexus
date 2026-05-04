using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace P_EscapeNexus
{
    /// <summary>
    /// Gère l'affichage des messages du jeu.
    /// Permet d'ajouter des messages et affiche une liste limitée
    /// des derniers messages à l'écran.
    /// </summary>
    public class MessageConsole
    {
        private List<string> messages = new List<string>();
        private SpriteFont font;
        private Vector2 position;

        public MessageConsole(SpriteFont font, Vector2 position)
        {
            this.font = font;
            this.position = position;
        }

        public void Add(string message)
        {
            messages.Add(message);

            if (messages.Count > 5)
                messages.RemoveAt(0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < messages.Count; i++)
            {
                spriteBatch.DrawString(
                    font,
                    messages[i],
                    position + new Vector2(0, i * 25),
                    Color.White
                );
            }
        }
    }
}