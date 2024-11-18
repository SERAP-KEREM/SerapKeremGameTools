using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SerapKeremGameTools._Game._ParticleEffectSystem
{
    [System.Serializable]
    public class ParticleEffectData
    {
        [Header("Particle Settings")]
        [Tooltip("The name of the particle effect.")]
        [SerializeField]
        private string _particleName;

        [Tooltip("The number of particles to be generated.")]
        [SerializeField]
        private int _particleCount;

        [Tooltip("The list of particle system prefabs.")]
        [SerializeField]
        private List<ParticleSystem> _particleSystemList;

        /// <summary>
        /// Gets the name of the particle effect.
        /// </summary>
        public string ParticleName
        {
            get { return _particleName; }
            private set { _particleName = value ?? throw new System.ArgumentNullException(nameof(value)); }
        }

        /// <summary>
        /// Gets the number of particles to be generated.
        /// </summary>
        public int ParticleCount
        {
            get { return _particleCount; }
            private set { _particleCount = value > 0 ? value : throw new System.ArgumentOutOfRangeException(nameof(value), "Particle count must be greater than zero."); }
        }

        /// <summary>
        /// Gets a random particle system from the list of particle systems.
        /// This property selects a random particle effect from the list.
        /// </summary>
        public ParticleSystem ParticleSystem => _particleSystemList[Random.Range(0, _particleSystemList.Count)];

        /// <summary>
        /// Initializes a new instance of the ParticleEffectData class.
        /// </summary>
        /// <param name="particleSystems">The list of particle system prefabs.</param>
        /// <param name="count">The number of particles to be generated.</param>
        /// <param name="name">The name of the particle effect.</param>
        public ParticleEffectData(List<ParticleSystem> particleSystems, int count, string name)
        {
            ParticleSystemList = particleSystems ?? throw new System.ArgumentNullException(nameof(particleSystems));
            ParticleCount = count;
            ParticleName = name ?? throw new System.ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Gets or sets the list of particle system prefabs.
        /// </summary>
        public List<ParticleSystem> ParticleSystemList
        {
            get { return _particleSystemList; }
            private set { _particleSystemList = value ?? throw new System.ArgumentNullException(nameof(value)); }
        }
    }
}

