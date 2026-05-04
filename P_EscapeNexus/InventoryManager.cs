using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace P_EscapeNexus
{
    /// <summary>
    /// Gère l'inventaire du joueur.
    /// Permet de sélectionner des objets, d'afficher leur état
    /// et de gérer les interactions avec les cases de l'inventaire.
    /// </summary>
    public class InventoryManager
    {
        private Texture2D textureInventaire;
        private Texture2D textureBadge;
        private Texture2D textureTournevis;

        private int screenWidth;
        private int screenHeight;

        public int SelectedCase { get; private set; } = -1;
        public bool TournevisPris { get; set; } = false;

        public bool BadgeSelectionne => SelectedCase == 1;
        public bool TournevisSelectionne => SelectedCase == 2;

        public Rectangle Case1 => new Rectangle(30, 626, 110, 75);
        public Rectangle Case2 => new Rectangle(154, 626, 110, 75);
        public Rectangle Case3 => new Rectangle(278, 626, 110, 75);
        public Rectangle Case4 => new Rectangle(402, 626, 110, 75);

        public InventoryManager(Texture2D textureInventaire, Texture2D textureBadge, Texture2D textureTournevis, int screenWidth, int screenHeight)
        {
            this.textureInventaire = textureInventaire;
            this.textureBadge = textureBadge;
            this.textureTournevis = textureTournevis;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }

        public void Update(MouseState mouseState, MouseState previousMouseState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed &&
                previousMouseState.LeftButton == ButtonState.Released)
            {
                if (Case1.Contains(mouseState.Position))
                    SelectedCase = 1;
                else if (Case2.Contains(mouseState.Position))
                    SelectedCase = 2;
                else if (Case3.Contains(mouseState.Position))
                    SelectedCase = 3;
                else if (Case4.Contains(mouseState.Position))
                    SelectedCase = 4;
            }
        }

        public void Draw(SpriteBatch spriteBatch, HitboxDebug hitboxDebug)
        {
            spriteBatch.Draw(
                textureInventaire,
                new Rectangle(0, 550, screenWidth / 2, screenHeight / 3),
                Color.White
            );

            if (SelectedCase == 1)
                hitboxDebug.DrawContour(spriteBatch, Case1, Color.Yellow, 4);

            if (SelectedCase == 2)
                hitboxDebug.DrawContour(spriteBatch, Case2, Color.Yellow, 4);

            if (SelectedCase == 3)
                hitboxDebug.DrawContour(spriteBatch, Case3, Color.Yellow, 4);

            if (SelectedCase == 4)
                hitboxDebug.DrawContour(spriteBatch, Case4, Color.Yellow, 4);

            spriteBatch.Draw(
                textureBadge,
                new Rectangle(37, 630, screenWidth / 12, screenHeight / 11),
                Color.White
            );

            if (TournevisPris)
            {
                spriteBatch.Draw(
                    textureTournevis,
                    new Rectangle(160, 630, 80, 65),
                    Color.White
                );
            }
        }
    }
}