using UnityEngine;
using System.Collections;

namespace SerapKeremGameTools._Game._AudioSystem
{
    /// <summary>
    /// Manages the audio playback for each AudioPlayer instance.
    /// </summary>
    public class AudioPlayer : MonoBehaviour
    {
        private AudioSource audioSource;

        void Awake()
        {
            // Initialize the AudioSource component
            audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Plays the given audio clip with the specified settings.
        /// </summary>
        /// <param name="audio">Audio settings to play</param>
        public void PlayAudio(Audio audio)
        {
            if (!audioSource.isPlaying)  // Eğer ses zaten çalmıyorsa
            {
                audioSource.clip = audio.clip;
                audioSource.volume = audio.volume;
                audioSource.pitch = audio.pitch;
                audioSource.loop = audio.loop;
                audioSource.Play();

                // Eğer ses döngü yapmıyorsa, bitince havuza geri dönsün
                if (!audio.loop)
                {
                    StartCoroutine(ReturnToPoolAfterPlaying(audio));
                }
            }
        }


        /// <summary>
        /// Waits until the audio clip finishes and returns the AudioPlayer to the pool.
        /// </summary>
        private IEnumerator ReturnToPoolAfterPlaying(Audio audio)
        {
            yield return new WaitForSeconds(audio.clip.length);
            AudioManager.Instance.ReturnAudioPlayerToPool(this);
        }
    }
}
