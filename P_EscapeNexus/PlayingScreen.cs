using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace P_EscapeNexus
{
    /// <summary>
    /// Gère l'écran de jeu principal.
    /// Contrôle les interactions du joueur avec l'environnement : murs, objets,
    /// inventaire, puzzles et portes.
    /// Gère la progression dans les salles, les conditions d'accès (badge, électricité, objets)
    /// ainsi que le déclenchement de la victoire.
    /// </summary>
    public class PlayingScreen
    {
        private RoomManager room;
        private InventoryManager inventory;
        private MessageConsole console;
        private HitboxDebug debug;

        private Texture2D flecheG;
        private Texture2D flecheD;
        private Texture2D commande;

        private Texture2D outilsAvecObj;
        private Texture2D outilsSansObj;

        private ElectricPanelPuzzle electricPanel;
        private DigicodePuzzle digicodePuzzle;
        private TableauPuzzle tableauPuzzle;

        private bool afficheOutilsAvecObj = false;

        public PlayingScreen(
            RoomManager room,
            InventoryManager inventory,
            MessageConsole console,
            HitboxDebug debug,
            Texture2D flecheG,
            Texture2D flecheD,
            Texture2D commande,
            Texture2D outilsAvecObj,
            Texture2D outilsSansObj,
            ElectricPanelPuzzle electricPanel,
            DigicodePuzzle digicodePuzzle,
            TableauPuzzle tableauPuzzle)
        {
            this.room = room;
            this.inventory = inventory;
            this.console = console;
            this.debug = debug;

            this.flecheG = flecheG;
            this.flecheD = flecheD;
            this.commande = commande;

            this.outilsAvecObj = outilsAvecObj;
            this.outilsSansObj = outilsSansObj;

            this.electricPanel = electricPanel;
            this.digicodePuzzle = digicodePuzzle;
            this.tableauPuzzle = tableauPuzzle;
        }

        public void Update(MouseState mouse, MouseState prev, Action afficherVictoire)
        {
            Rectangle flecheGauche = new Rectangle(10, 300, 100, 100);
            Rectangle flecheDroite = new Rectangle(970, 300, 100, 100);

            Rectangle porteHitbox = new Rectangle(365, 130, 285, 480);
            Rectangle scanHitbox = new Rectangle(666, 328, 50, 80);
            Rectangle casierHitbox = new Rectangle(777, 186, 150, 215);
            Rectangle outilsHitbox = new Rectangle(573, 182, 230, 182);
            Rectangle objetZoomHitbox = new Rectangle(285, 150, 95, 430);

            Rectangle digicodeHitbox = new Rectangle(666, 328, 50, 80);
            Rectangle tableauHitbox = new Rectangle(380, 220, 330, 190);

            Rectangle combinaisonHitbox = new Rectangle(450, 160, 180, 400);
            Rectangle badgeHitbox = new Rectangle(310, 320, 50, 80);

            bool click = mouse.LeftButton == ButtonState.Pressed &&
                         prev.LeftButton == ButtonState.Released;

            electricPanel.Update(mouse, prev);

            if (electricPanel.IsOpen)
            {
                if (electricPanel.Electricite && !room.Electricite)
                {
                    room.Electricite = true;
                    console.Add("Electricite retablie.");
                }

                return;
            }

            digicodePuzzle.Update(mouse, prev);

            if (digicodePuzzle.CodeValide)
            {
                if (room.Electricite && !room.PorteDigicodeOuverte)
                {
                    room.PorteDigicodeOuverte = true;
                    console.Add("Code correct. Porte ouverte.");
                }
                else if (!room.Electricite)
                {
                    console.Add("Code correct, mais il n'y a pas d'electricite.");
                }

                digicodePuzzle.ResetCodeValide();
            }

            if (digicodePuzzle.IsOpen)
            {
                return;
            }

            tableauPuzzle.Update(mouse, prev);

            if (tableauPuzzle.IsOpen)
                return;

            if (afficheOutilsAvecObj)
            {
                if (click)
                {
                    if (objetZoomHitbox.Contains(mouse.Position) && !inventory.TournevisPris)
                    {
                        inventory.TournevisPris = true;
                        console.Add("Tournevis recupere.");
                    }
                    else
                    {
                        afficheOutilsAvecObj = false;
                    }
                }

                return;
            }

            if (click)
            {
                if (flecheGauche.Contains(mouse.Position))
                {
                    room.AllerGauche();
                }

                if (flecheDroite.Contains(mouse.Position))
                {
                    room.AllerDroite();
                }

                if (room.CurrentMurIndex == 0 &&
                    scanHitbox.Contains(mouse.Position) &&
                    inventory.BadgeSelectionne)
                {
                    room.PorteOuverte = true;
                    console.Add("Badge accepte. Porte ouverte.");
                }
                else if (room.CurrentMurIndex == 0 &&
                         scanHitbox.Contains(mouse.Position) &&
                         !inventory.BadgeSelectionne)
                {
                    console.Add("Un badge est requis.");
                }
                else if (room.CurrentMurIndex == 5 &&
                         outilsHitbox.Contains(mouse.Position))
                {
                    afficheOutilsAvecObj = true;
                    console.Add("Vous inspectez les outils.");
                }
                else if (room.CurrentMurIndex == 2 &&
                         casierHitbox.Contains(mouse.Position) &&
                         room.PanneauOuvert)
                {
                    electricPanel.Open();
                    console.Add("Vous inspectez le panneau electrique.");
                }
                else if (room.CurrentMurIndex == 2 &&
                         casierHitbox.Contains(mouse.Position) &&
                         inventory.TournevisSelectionne &&
                         inventory.TournevisPris)
                {
                    room.PanneauOuvert = true;
                    console.Add("Panneau ouvert avec le tournevis.");
                }
                else if (room.CurrentMurIndex == 2 &&
                         casierHitbox.Contains(mouse.Position) &&
                         !inventory.TournevisSelectionne)
                {
                    console.Add("Il faut utiliser un outil.");
                }
                else if (room.CurrentMurIndex == 2 &&
                         casierHitbox.Contains(mouse.Position) &&
                         inventory.TournevisSelectionne &&
                         !inventory.TournevisPris)
                {
                    console.Add("Vous n'avez pas encore le tournevis.");
                }
                else if (room.CurrentMurIndex == 6 &&
                         porteHitbox.Contains(mouse.Position) &&
                         room.PorteDigicodeOuverte)
                {
                    room.EntrerTroisiemePiece();
                    console.Add("Vous entrez dans la troisieme piece.");
                }
                else if (room.CurrentMurIndex == 8 &&
                         porteHitbox.Contains(mouse.Position))
                {
                    room.RevenirDeuxiemePiece();
                    console.Add("Vous revenez dans la deuxieme piece.");
                }
                else if (room.CurrentMurIndex == 6 &&
                         digicodeHitbox.Contains(mouse.Position))
                {
                    digicodePuzzle.Open();
                    console.Add("Vous inspectez le digicode.");
                }
                else if (room.CurrentMurIndex == 7 &&
                         tableauHitbox.Contains(mouse.Position))
                {
                    tableauPuzzle.Open();
                    console.Add("Vous regardez le tableau.");
                }
                else if (room.CurrentMurIndex == 0 &&
                         porteHitbox.Contains(mouse.Position) &&
                         room.PorteOuverte)
                {
                    room.EntrerDeuxiemePiece();
                    console.Add("Vous entrez dans la deuxieme piece.");
                }
                else if (room.CurrentMurIndex == 4 &&
                         porteHitbox.Contains(mouse.Position) &&
                         room.PorteOuverte)
                {
                    room.RevenirPremierePiece();
                    console.Add("Vous revenez dans la premiere piece.");
                }
                else if (room.CurrentMurIndex == 0 &&
                         porteHitbox.Contains(mouse.Position) &&
                         !room.PorteOuverte)
                {
                    console.Add("La porte est verrouillee.");
                }
                else if (room.CurrentMurIndex == 9 &&
                         combinaisonHitbox.Contains(mouse.Position))
                {
                    if (!room.CombinaisonPrise)
                    {
                        room.CombinaisonPrise = true;
                        console.Add("Combinaison recuperee.");
                    }
                    else
                    {
                        console.Add("Il n'y a plus rien ici.");
                    }
                }
                else if (room.CurrentMurIndex == 11 &&
                         badgeHitbox.Contains(mouse.Position))
                {
                    if (room.CombinaisonPrise && inventory.BadgeSelectionne)
                    {
                        room.PorteFinaleOuverte = true;
                        console.Add("Acces autorise. Porte ouverte.");
                    }
                    else if (!inventory.BadgeSelectionne)
                    {
                        console.Add("Vous devez selectionner le badge.");
                    }
                    else
                    {
                        console.Add("Il vous manque la combinaison.");
                    }
                }
                else if (room.CurrentMurIndex == 11 &&
                         porteHitbox.Contains(mouse.Position) &&
                         room.PorteFinaleOuverte)
                {
                    afficherVictoire();
                }
            }

            inventory.Update(mouse, prev);
        }

        public void Draw(SpriteBatch sb)
        {
            if (electricPanel.IsOpen)
            {
                electricPanel.Draw(sb, Mouse.GetState(), debug);
                return;
            }

            if (digicodePuzzle.IsOpen)
            {
                digicodePuzzle.Draw(sb, debug);
                return;
            }

            if (tableauPuzzle.IsOpen)
            {
                tableauPuzzle.Draw(sb, debug);
                return;
            }

            Rectangle flecheGauche = new Rectangle(10, 300, 100, 100);
            Rectangle flecheDroite = new Rectangle(970, 300, 100, 100);

            Rectangle scanHitbox = new Rectangle(666, 328, 50, 80);
            Rectangle porteHitbox = new Rectangle(365, 130, 285, 480);
            Rectangle casierHitbox = new Rectangle(777, 186, 150, 215);
            Rectangle outilsHitbox = new Rectangle(573, 182, 230, 182);

            Rectangle digicodeHitbox = new Rectangle(666, 328, 50, 80);
            Rectangle tableauHitbox = new Rectangle(380, 220, 330, 190);

            Rectangle combinaisonHitbox = new Rectangle(450, 160, 180, 400);
            Rectangle badgeHitbox = new Rectangle(310, 320, 50, 80);

            Texture2D mur = room.GetMurActuel(inventory.TournevisPris);

            sb.Draw(mur, new Rectangle(0, 0, 1080, 720), Color.White);

            if (room.CurrentMurIndex == 0 || room.CurrentMurIndex == 4)
            {
                if (!room.PorteOuverte)
                    debug.Draw(sb, scanHitbox, Color.Red);

                if (room.PorteOuverte)
                    debug.Draw(sb, porteHitbox, Color.Red);
            }

            if (room.CurrentMurIndex == 2)
                debug.Draw(sb, casierHitbox, Color.Red);

            if (room.CurrentMurIndex == 5)
                debug.Draw(sb, outilsHitbox, Color.Red);

            if (room.CurrentMurIndex == 6)
                debug.Draw(sb, digicodeHitbox, Color.Green);

            if (room.CurrentMurIndex == 7)
                debug.Draw(sb, tableauHitbox, Color.Green);

            if (room.CurrentMurIndex == 9)
                debug.Draw(sb, combinaisonHitbox, Color.Yellow);

            if (room.CurrentMurIndex == 11)
            {
                debug.Draw(sb, badgeHitbox, Color.Blue);

                if (room.PorteFinaleOuverte)
                    debug.Draw(sb, porteHitbox, Color.Red);
            }

            debug.Draw(sb, flecheGauche, Color.Red);
            debug.Draw(sb, flecheDroite, Color.Red);

            sb.Draw(flecheG, flecheGauche, Color.White * 0.4f);
            sb.Draw(flecheD, flecheDroite, Color.White * 0.4f);

            if (afficheOutilsAvecObj)
            {
                Texture2D imageZoom = inventory.TournevisPris ? outilsSansObj : outilsAvecObj;

                sb.Draw(imageZoom, new Rectangle(0, 0, 1080, 720), Color.White);

                if (!inventory.TournevisPris)
                {
                    Rectangle objetZoomHitbox = new Rectangle(285, 150, 95, 430);
                    debug.Draw(sb, objetZoomHitbox, Color.Green);
                }

                return;
            }

            inventory.Draw(sb, debug);

            sb.Draw(
                commande,
                new Rectangle(730, 450, 1080 / 3, 720 / 2),
                Color.White
            );

            console.Draw(sb);
        }
    }
}