using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace P_EscapeNexus
{
    /// <summary>
    /// Permet d'afficher les hitbox en mode debug.
    /// Dessine des rectangles ou des contours pour visualiser les zones interactives du jeu.
    /// Peut être activé ou désactivé pendant le jeu.
    /// </summary>
    public class HitboxDebug
    {
        private Texture2D pixel;

        public bool Enabled { get; set; } = false;

        public HitboxDebug(GraphicsDevice graphicsDevice)
        {
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            if (!Enabled)
                return;

            spriteBatch.Draw(pixel, rectangle, color * 0.4f);
        }

        public void DrawContour(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int thickness)
        {
            spriteBatch.Draw(pixel, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);
            spriteBatch.Draw(pixel, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - thickness, rectangle.Width, thickness), color);
            spriteBatch.Draw(pixel, new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);
            spriteBatch.Draw(pixel, new Rectangle(rectangle.X + rectangle.Width - thickness, rectangle.Y, thickness, rectangle.Height), color);
        }
    }
}