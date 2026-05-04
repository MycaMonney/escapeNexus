using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace P_EscapeNexus
{
    /// <summary>
    /// Gère l'écran d'introduction du jeu.
    /// Affiche une série d'images que le joueur peut faire défiler en cliquant
    /// </summary>
    public class IntroScreen
    {
        private Texture2D[] textures;
        private int index = 0;

        public bool Finished { get; private set; }

        public IntroScreen(Texture2D[] textures)
        {
            this.textures = textures;
        }

        public void Reset()
        {
            index = 0;
            Finished = false;
        }

        public void Update(MouseState mouse, MouseState prev)
        {
            if (mouse.LeftButton == ButtonState.Pressed &&
                prev.LeftButton == ButtonState.Released)
            {
                index++;

                if (index >= textures.Length)
                {
                    index = 0;
                    Finished = true;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(textures[index], new Rectangle(0, 0, 1080, 720), Color.White);
        }
    }
}