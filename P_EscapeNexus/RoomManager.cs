using Microsoft.Xna.Framework.Graphics;

namespace P_EscapeNexus
{
    /// <summary>
    /// Gère les salles, les murs affichés et les changements d'image
    /// selon l'état du jeu, comme les portes ouvertes, les objets pris
    /// ou les puzzles résolus.
    /// </summary>
    public class RoomManager
    {
        private Texture2D[] murTextures;
        private Texture2D texturePorteOuverte;
        private Texture2D textureMurPanneauOuvert;
        private Texture2D textureMur6SansOutils;
        private Texture2D mur7PorteOuverte;
        private Texture2D mur10SansCombinaison;
        private Texture2D mur12PorteOuverte;

        public int CurrentMurIndex { get; set; } = 0;

        public int SalleActuelle { get; set; } = 1;

        public bool PorteOuverte { get; set; } = false;
        public bool PanneauOuvert { get; set; } = false;
        public bool Electricite { get; set; } = false;
        public bool PorteDigicodeOuverte { get; set; } = false;
        public bool CombinaisonPrise { get; set; } = false;
        public bool PorteFinaleOuverte { get; set; } = false;

        public RoomManager(
        Texture2D[] murs,
        Texture2D porteOuverte,
        Texture2D murPanneauOuvert,
        Texture2D mur6SansOutils,
        Texture2D mur7PorteOuverte,
        Texture2D mur10SansCombinaison,
        Texture2D mur12PorteOuverte)
        {
            this.murTextures = murs;
            this.texturePorteOuverte = porteOuverte;
            this.textureMurPanneauOuvert = murPanneauOuvert;
            this.textureMur6SansOutils = mur6SansOutils;
            this.mur7PorteOuverte = mur7PorteOuverte;
            this.mur10SansCombinaison = mur10SansCombinaison;
            this.mur12PorteOuverte = mur12PorteOuverte;
        }

        public void AllerGauche()
        {
            CurrentMurIndex++;

            if (SalleActuelle == 1)
            {
                if (CurrentMurIndex > 3)
                    CurrentMurIndex = 0;
            }
            else if (SalleActuelle == 2)
            {
                if (CurrentMurIndex > 7)
                    CurrentMurIndex = 4;
            }
            else if (SalleActuelle == 3)
            {
                if (CurrentMurIndex > 11)
                    CurrentMurIndex = 8;
            }
        }

        public void AllerDroite()
        {
            CurrentMurIndex--;

            if (SalleActuelle == 1)
            {
                if (CurrentMurIndex < 0)
                    CurrentMurIndex = 3;
            }
            else if (SalleActuelle == 2)
            {
                if (CurrentMurIndex < 4)
                    CurrentMurIndex = 7;
            }
            else if (SalleActuelle == 3)
            {
                if (CurrentMurIndex < 8)
                    CurrentMurIndex = 11;
            }
        }

        public void EntrerDeuxiemePiece()
        {
            SalleActuelle = 2;
            CurrentMurIndex = 4;
        }

        public void RevenirPremierePiece()
        {
            SalleActuelle = 1;
            CurrentMurIndex = 0;
        }

        public void EntrerTroisiemePiece()
        {
            SalleActuelle = 3;
            CurrentMurIndex = 8;
        }

        public void RevenirDeuxiemePiece()
        {
            SalleActuelle = 2;
            CurrentMurIndex = 6;
        }

        public Texture2D GetMurActuel(bool tournevisPris)
        {
            Texture2D murActuel = murTextures[CurrentMurIndex];

            if (CurrentMurIndex == 0 && PorteOuverte)
                murActuel = texturePorteOuverte;

            if (CurrentMurIndex == 2 && PanneauOuvert)
                murActuel = textureMurPanneauOuvert;

            if (CurrentMurIndex == 5 && tournevisPris)
                murActuel = textureMur6SansOutils;

            if (CurrentMurIndex == 6 && PorteDigicodeOuverte)
                murActuel = mur7PorteOuverte;
            if (CurrentMurIndex == 9 && CombinaisonPrise)
                murActuel = mur10SansCombinaison;
            if (CurrentMurIndex == 11 && PorteFinaleOuverte)
                murActuel = mur12PorteOuverte;
            return murActuel;
        }
    }
}