using Microsoft.Xna.Framework.Media;

namespace P_EscapeNexus
{
    /// <summary>
    /// Gère la musique du jeu.
    /// Permet de lancer la musique en boucle et d'activer ou désactiver le son (mute).
    /// </summary>
    public class AudioManager
    {
        private Song music;
        private bool isMusicStarted = false;

        public bool IsMuted { get; private set; } = false;

        public AudioManager(Song music)
        {
            this.music = music;
        }

        public void ToggleMute()
        {
            IsMuted = !IsMuted;
        }

        public void Update()
        {
            if (!isMusicStarted)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(music);
                isMusicStarted = true;
            }

            if (IsMuted)
            {
                MediaPlayer.Pause();
            }
            else
            {
                if (MediaPlayer.State != MediaState.Playing)
                    MediaPlayer.Resume();
            }
        }
    }
}