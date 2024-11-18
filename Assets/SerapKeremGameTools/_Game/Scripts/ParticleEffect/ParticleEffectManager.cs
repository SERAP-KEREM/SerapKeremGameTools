using SerapKeremGameTools._Game._Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SerapKeremGameTools._Game._ParticleEffectSystem
{
    public class ParticleEffectManager : MonoSingleton<ParticleEffectManager>
    {
        // List of all particle effects data
        [SerializeField]
        private List<ParticleEffectData> particleEffectDataList = new List<ParticleEffectData>();

        // Dictionary to store pools for each particle effect
        private Dictionary<string, Queue<ParticleSystem>> particlePools = new Dictionary<string, Queue<ParticleSystem>>();

        // Dictionary to store the parent GameObject for each particle effect group
        private Dictionary<string, GameObject> particleEffectParents = new Dictionary<string, GameObject>();

        protected override void Awake()
        {
            base.Awake();  // MonoSingleton'dan gelen Awake metodunu ça??r?r
            InitializeParticlePools(); // Particle havuzlar?n? ba?lat?r
        }

        /// <summary>
        /// Initializes the particle pools using the particle effect data.
        /// This method initializes particle pools for each defined particle effect.
        /// </summary>
        private void InitializeParticlePools()
        {
            foreach (var data in particleEffectDataList)
            {
                Queue<ParticleSystem> pool = new Queue<ParticleSystem>();

                // Create a parent GameObject for this particle effect group
                GameObject effectParent = new GameObject(data.ParticleName);
                effectParent.transform.SetParent(transform); // Set ParticleEffectManager as the parent

                // Store this parent in the dictionary
                particleEffectParents[data.ParticleName] = effectParent;

                // Create the pool for each particle effect
                for (int i = 0; i < data.ParticleCount; i++)
                {
                    var particlePrefab = data.ParticleSystem;

                    if (particlePrefab != null)
                    {
                        var instance = Instantiate(particlePrefab, effectParent.transform); // Instantiate under the group parent
                        instance.gameObject.SetActive(false);  // Deactivate initially
                        pool.Enqueue(instance);  // Add to pool
                    }
                    else
                    {
                        Debug.LogError($"Particle prefab not found for: {data.ParticleName}");
                    }
                }

                particlePools[data.ParticleName] = pool;  // Add pool to dictionary
            }
        }

        /// <summary>
        /// Plays a particle effect at the given position and rotation.
        /// </summary>
        /// <param name="particleName">The name of the particle effect to play.</param>
        /// <param name="position">The position where the particle effect will be played.</param>
        /// <param name="rotation">The rotation of the particle effect.</param>
        public void PlayParticle(string particleName, Vector3 position, Quaternion rotation)
        {
            if (particlePools.TryGetValue(particleName, out var pool) && pool.Count > 0)
            {
                var particle = pool.Dequeue();  // Get a particle from the pool
                particle.transform.position = position;
                particle.transform.rotation = rotation;

                particle.gameObject.SetActive(true);  // Activate particle
                particle.Play();

                // Return the particle to the pool after it's done playing
                StartCoroutine(ReturnToPoolAfterDuration(particle, particleName));
            }
            else
            {
                Debug.LogWarning($"No available particle in the pool for: {particleName}");
            }
        }

        /// <summary>
        /// Returns the particle system to the pool after it finishes playing.
        /// </summary>
        /// <param name="particleSystem">The particle system to return.</param>
        /// <param name="effectName">The name of the effect being returned.</param>
        public void ReturnEffectToPool(ParticleSystem particleSystem, string effectName)
        {
            ReturnToPoolAfterDuration(particleSystem, effectName); // Call the restricted method internally
        }

        /// <summary>
        /// Returns the particle system to the pool after it finishes playing.
        /// </summary>
        /// <param name="particle">The particle system to return to the pool.</param>
        /// <param name="particleName">The name of the particle effect.</param>
        private IEnumerator ReturnToPoolAfterDuration(ParticleSystem particle, string particleName)
        {
            yield return new WaitForSeconds(particle.main.duration);

            particle.Stop();
            particle.gameObject.SetActive(false);

            if (particlePools.ContainsKey(particleName))
            {
                particlePools[particleName].Enqueue(particle);  // Return to pool
            }
            else
            {
                Debug.LogError($"No pool found for particle: {particleName}");
            }
        }
    }
}
