using UnityEngine;
using System.Collections.Generic;
using SerapKeremGameTools._Game._objectPool;

namespace SerapKeremGameTools._Game._AudioSystem
{
    /// <summary>
    /// Manages audio playback and object pooling for AudioPlayers.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [Header("Audio Clips List")]
        [Tooltip("A list of all available audio clips that can be played.")]
        [SerializeField]
        private List<Audio> audioClips; // List to store audio clips

        [Header("AudioPlayer Prefab")]
        [Tooltip("The AudioPlayer prefab used to play audio.")]
        [SerializeField]
        private AudioPlayer audioPlayerPrefab; // Reference to the AudioPlayer prefab

        private ObjectPool<AudioPlayer> audioPlayerPool; // Object pool for AudioPlayers
        [SerializeField]
        [Tooltip("The maximum number of AudioPlayers that can be in the pool.")]
        private int poolSize = 10;

        /// <summary>
        /// Initializes the AudioManager instance and sets up the audio pool.
        /// Ensures only one instance of AudioManager exists and loads audio clips.
        /// </summary>
        void Awake()
        {
            // Ensure only one instance of AudioManager exists
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            // Create the audio player pool with a capacity of poolSize
            InitializeAudioPlayerPool();

            // Load the audio clips from the Resources folder
            LoadAudioClips();
        }

        /// <summary>
        /// Initializes the ObjectPool for AudioPlayers.
        /// </summary>
        private void InitializeAudioPlayerPool()
        {
            // Create the audio player pool with a capacity of poolSize
            audioPlayerPool = new ObjectPool<AudioPlayer>(audioPlayerPrefab, poolSize, transform);
        }

        /// <summary>
        /// Loads all audio clips from the Resources/Audio folder.
        /// </summary>
        private void LoadAudioClips()
        {
            // Load all AudioClips from Resources/Audio
            AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");
            foreach (var clip in clips)
            {
                Audio newAudio = new Audio()
                {
                    name = clip.name,
                    clip = clip,
                    volume = 1f,
                    pitch = 1f,
                    loop = false
                };
                audioClips.Add(newAudio);
            }
        }

        /// <summary>
        /// Plays an audio clip by its name from the audioClips list.
        /// </summary>
        /// <param name="audioName">The name of the audio clip to play.</param>
        public void PlayAudio(string audioName)
        {
            // Find the audio clip by name
            Audio audio = audioClips.Find(a => a.name == audioName);
            if (audio != null)
            {
                Debug.Log("Audio played");

                // Get an AudioPlayer from the pool and play the audio
                AudioPlayer audioPlayer = audioPlayerPool.GetObject();
                audioPlayer.PlayAudio(audio);
            }
            else
            {
                Debug.LogWarning($"Audio not found: {audioName}");
            }
        }

        /// <summary>
        /// Returns the AudioPlayer to the pool after it has finished playing.
        /// </summary>
        /// <param name="audioPlayer">The AudioPlayer to return to the pool.</param>
        public void ReturnAudioPlayerToPool(AudioPlayer audioPlayer)
        {
            audioPlayerPool.ReturnObject(audioPlayer);
        }
    }
}
