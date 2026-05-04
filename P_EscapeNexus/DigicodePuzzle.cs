using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace P_EscapeNexus
{
    /// <summary>
    /// Gère le puzzle du digicode.
    /// Permet au joueur de saisir un code avec des hitbox,
    /// vérifie si le code est correct et indique quand le code est validé.
    /// </summary>
    public class DigicodePuzzle
    {
        private Texture2D background;
        private Texture2D pixel;
        private SpriteFont font;

        private string codeSaisi = "";
        private string codeCorrect = "872";

        public bool IsOpen { get; private set; } = false;
        public bool CodeValide { get; private set; } = false;

        private Rectangle closeButton = new Rectangle(950, 35, 100, 90);

        private Rectangle[] touches =
        {
            new Rectangle(452, 225, 60, 60), // 1
            new Rectangle(533, 225, 60, 60), // 2
            new Rectangle(615, 225, 60, 60), // 3
                                    
            new Rectangle(452, 300, 60, 60), // 4
            new Rectangle(533, 300, 60, 60), // 5
            new Rectangle(614, 300, 60, 60), // 6
                                    
            new Rectangle(452, 382, 60, 60), // 7
            new Rectangle(533, 382, 60, 60), // 8
            new Rectangle(614, 382, 60, 60), // 9
                                    
            new Rectangle(533, 460, 60, 60), // 0
        };
        public void ResetCodeValide()
        {
            CodeValide = false;
        }
        public DigicodePuzzle(GraphicsDevice graphicsDevice, Texture2D background, SpriteFont font)
        {
            this.background = background;
            this.font = font;

            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        public void Open()
        {
            IsOpen = true;
            codeSaisi = "";
            CodeValide = false;
        }

        public void Close()
        {
            IsOpen = false;
            codeSaisi = "";
        }

        public void Update(MouseState mouse, MouseState previousMouse)
        {
            if (!IsOpen)
                return;

            bool click = mouse.LeftButton == ButtonState.Pressed &&
                         previousMouse.LeftButton == ButtonState.Released;

            if (!click)
                return;

            if (closeButton.Contains(mouse.Position))
            {
                Close();
                return;
            }

            for (int i = 0; i < touches.Length; i++)
            {
                if (touches[i].Contains(mouse.Position))
                {
                    int chiffre = i + 1;

                    if (i == 9)
                        chiffre = 0;

                    codeSaisi += chiffre.ToString();

                    if (codeSaisi.Length >= 3)
                    {
                        if (codeSaisi == codeCorrect)
                        {
                            CodeValide = true;
                            Close();
                        }
                        else
                        {
                            codeSaisi = "";
                        }
                    }

                    return;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, HitboxDebug debug)
        {
            if (!IsOpen)
                return;

            // Fond
            spriteBatch.Draw(background, new Rectangle(0, 0, 1080, 720), Color.White);

            // Bouton fermer (croix visible)
            spriteBatch.Draw(pixel, closeButton, Color.Red * 0.4f);
            spriteBatch.DrawString(font, "X", new Vector2(985, 55), Color.White);

            // Affichage du code
            spriteBatch.DrawString(font, "Code : " + codeSaisi, new Vector2(430, 140), Color.White);

            // Debug hitbox (F1)
            debug.Draw(spriteBatch, closeButton, Color.Red);

            for (int i = 0; i < touches.Length; i++)
            {
                debug.Draw(spriteBatch, touches[i], Color.Green);
            }
        }
    }
}