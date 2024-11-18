using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SerapKeremGameTools._Game._AudioSystem
{
    /// <summary>
    /// Contains all the properties related to an audio clip.
    /// </summary>
    [System.Serializable]
    public class Audio
    {
        [Tooltip("Unique name for the audio. This name will be used in PlayAudio.")]
        public string name;

        [Tooltip("The audio clip to be played.")]
        public AudioClip clip;

        [Tooltip("The volume of the audio. Ranges from 0 (silent) to 1 (maximum).")]
        [Range(0f, 1f)]
        public float volume = 1f;

        [Tooltip("The pitch of the audio. Ranges from 0.1 to 3.")]
        [Range(0.1f, 3f)]
        public float pitch = 1f;

        [Tooltip("Whether the audio should loop.")]
        public bool loop = false;
    }
}
