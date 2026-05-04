using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace P_EscapeNexus
{
    public class ElectricPanelPuzzle
    {
        private Texture2D background;
        private Texture2D pixel;

        private Rectangle[] leftConnectors;
        private Rectangle[] rightConnectors;

        private int[] cables;
        private int[] solution;

        private int draggingCable = -1;

        public bool IsOpen { get; private set; } = false;
        public bool Electricite { get; private set; } = false;

        public ElectricPanelPuzzle(GraphicsDevice graphicsDevice, Texture2D background)
        {
            this.background = background;

            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            // Coordonnées adaptées directement à ta fenêtre 1080x720
            leftConnectors = new Rectangle[]
{
                new Rectangle(195, 140, 70, 60), // rouge
                new Rectangle(195, 260, 70, 60), // jaune
                new Rectangle(195, 380, 70, 60), // bleu
                new Rectangle(195, 500, 70, 60)  // violet
};

            rightConnectors = new Rectangle[]
            {
                new Rectangle(800, 140, 70, 60), // rouge
                new Rectangle(800, 260, 70, 60), // jaune
                new Rectangle(800, 380, 70, 60), // bleu
                new Rectangle(800, 500, 70, 60)  // violet
            };

            cables = new int[] { -1, -1, -1, -1 };

            // Solution : rouge->rouge, jaune->jaune, bleu->bleu, violet->violet
            solution = new int[] { 0, 1, 2, 3 };
        }

        public void Open()
        {
            IsOpen = true;
        }

        public void Close()
        {
            IsOpen = false;
            draggingCable = -1;
        }

        public void Update(MouseState mouse, MouseState previousMouse)
        {
            if (!IsOpen)
                return;

            bool click = mouse.LeftButton == ButtonState.Pressed &&
                         previousMouse.LeftButton == ButtonState.Released;

            bool release = mouse.LeftButton == ButtonState.Released &&
                           previousMouse.LeftButton == ButtonState.Pressed;

            Rectangle closeButton = new Rectangle(975, 30, 95, 90);

            if (click)
            {
                if (closeButton.Contains(mouse.Position))
                {
                    Close();
                    return;
                }

                if (Electricite)
                    return;

                for (int i = 0; i < leftConnectors.Length; i++)
                {
                    if (leftConnectors[i].Contains(mouse.Position))
                    {
                        draggingCable = i;
                        cables[i] = -1;
                        return;
                    }
                }
            }

            if (release && draggingCable != -1)
            {
                for (int i = 0; i < rightConnectors.Length; i++)
                {
                    if (rightConnectors[i].Contains(mouse.Position))
                    {
                        // Une arrivée ne peut avoir qu'un seul câble
                        bool arriveeDejaUtilisee = false;

                        for (int j = 0; j < cables.Length; j++)
                        {
                            if (j != draggingCable && cables[j] == i)
                            {
                                arriveeDejaUtilisee = true;
                                break;
                            }
                        }

                        if (!arriveeDejaUtilisee)
                        {
                            cables[draggingCable] = i;
                        }

                        break;
                    }
                }

                draggingCable = -1;
                CheckSolution();
            }
            CheckSolution();
            
        }

        private void CheckSolution()
        {
            for (int i = 0; i < solution.Length; i++)
            {
                if (cables[i] != solution[i])
                    return;
            }

            Electricite = true;
        }

        public void Draw(SpriteBatch spriteBatch, MouseState mouse, HitboxDebug debug)
        {
            if (!IsOpen)
                return;

            spriteBatch.Draw(background, new Rectangle(0, 0, 1080, 720), Color.White);

            Rectangle closeButton = new Rectangle(970, 25, 95, 90);
            DrawTransparent(spriteBatch, closeButton, Color.White);

            for (int i = 0; i < leftConnectors.Length; i++)
            {
                debug.Draw(spriteBatch, leftConnectors[i], Color.Red);
                debug.Draw(spriteBatch, rightConnectors[i], Color.Blue);
            }

            for (int i = 0; i < cables.Length; i++)
            {
                if (cables[i] != -1)
                {
                    DrawLine(
                        spriteBatch,
                        GetCenter(leftConnectors[i]),
                        GetCenter(rightConnectors[cables[i]]),
                        Color.Yellow,
                        6
                    );
                }
            }

            if (draggingCable != -1)
            {
                DrawLine(
                    spriteBatch,
                    GetCenter(leftConnectors[draggingCable]),
                    mouse.Position.ToVector2(),
                    Color.Yellow,
                    6
                );
            }
        }

        private void DrawTransparent(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(pixel, rectangle, color * 0.45f);
        }

        private Vector2 GetCenter(Rectangle rectangle)
        {
            return new Vector2(
                rectangle.X + rectangle.Width / 2,
                rectangle.Y + rectangle.Height / 2
            );
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, int thickness)
        {
            Vector2 edge = end - start;
            float angle = (float)System.Math.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(
                pixel,
                new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), thickness),
                null,
                color,
                angle,
                Vector2.Zero,
                SpriteEffects.None,
                0
            );
        }
    }
}