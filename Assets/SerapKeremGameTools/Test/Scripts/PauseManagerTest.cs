using SerapKeremGameTools._Game._TimeSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SerapKeremGameTools._Game._AudioSystem;
using SerapKeremGameTools._Game._ParticleEffectSystem;

namespace SerapKeremGameTools._Game._PauseSystem
{
    public class PauseManagerTest : MonoBehaviour
    {
        [Header("UI Elements")]
        public TextMeshProUGUI countdownText; // Geri say?m için
        public TextMeshProUGUI timerText; // Oyun zaman?n? göstermek için
        public Button pauseButton; // Pause i?lemi için buton

        [Header("Audio Settings")]
        [SerializeField]
        private string[] audioName;

        [Header("Particle Effect Settings")]
        [SerializeField]
        private string[] particleEffectName;

        private bool gameStarted;
        [SerializeField]
        private int countDown;

        private void Start()
        {
            // Ba?lang?çta UI ve sistemleri ayarla
            countdownText.gameObject.SetActive(true);
            timerText.gameObject.SetActive(false);

            // Pause butonunu ayarla
            pauseButton.onClick.AddListener(OnPauseButtonClicked);
            // Geri say?m? ba?lat
            StartCountdown(countDown);
        }

        private void Update()
        {
            // Oyun ba?lad?ysa timer text'i güncelle
            if (gameStarted)
            {

               
                if (audioName.Length > 0 && !AudioManager.Instance.IsPlaying(audioName[0]))
                {
                    AudioManager.Instance.PlayAudio(audioName[0]);
                    ParticleEffectManager.Instance.PlayParticle(particleEffectName[0], transform.position, transform.rotation);
                }

                float elapsedTime = TimeManager.Instance.GetGameTimeElapsed();
                timerText.text = $"Time: {elapsedTime:F2}s";
            }
        }

        private void StartCountdown(float duration)
        {
            countDown = Mathf.CeilToInt(duration);
            StartCoroutine(CountdownRoutine());
        }

        private IEnumerator CountdownRoutine()
        {
            while (countDown > 0)
            {
                countdownText.text = countDown.ToString();
                yield return new WaitForSeconds(1f);
                countDown--;
            }

            countdownText.gameObject.SetActive(false);
            timerText.gameObject.SetActive(true);
            gameStarted = true;
            TimeManager.Instance.StartTime();
        }

        private void OnPauseButtonClicked()
        {
            PauseManager.Instance.TogglePause();
            if (PauseManager.Instance.IsGamePaused())
            {
                AudioManager.Instance.PauseAllAudio();
                if (particleEffectName.Length > 0)
                {
                    ParticleEffectManager.Instance.StopAllEffects(); 
                }

            }
            else
            {
                AudioManager.Instance.ResumeAllAudio();

                if (particleEffectName.Length > 0)
                {
                    ParticleEffectManager.Instance.PlayParticle(particleEffectName[0], transform.position, transform.rotation);
                }
            }
        }
    }
}
