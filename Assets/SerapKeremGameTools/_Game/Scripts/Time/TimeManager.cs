using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SerapKeremGameTools._Game._TimeSystem
{
    public class TimeManager : MonoBehaviour
    {
        [Header("TimeManager Parameters")]
        [SerializeField, Tooltip("Initial time for the level in seconds.")]
        private float _initialTime = 300f; // Ba?lang?� s�resi (saniye)

        [SerializeField, Tooltip("The time interval for the timer updates (in seconds).")]
        private float _updateInterval = 1f; // Zaman g�ncelleme aral??? (saniye)

        [SerializeField, Tooltip("The critical time threshold (in seconds).")]
        private float _criticalTimeThreshold = 15f; // Kritik zaman e?i?i (saniye)

        private float _currentTime; // ?u anki zaman
        private bool _isTimerRunning; // Timer'?n �al???p �al??mad???n? kontrol eder
        private Coroutine _freezeCoroutine; // Timer'? dondurmak i�in kullan?lan coroutine

        // Events
        public UnityEvent<float> OnTimeUpdated; // Zaman g�ncellendi?inde tetiklenen event
        public UnityEvent OnTimerStarted; // Timer ba?lad???nda tetiklenen event
        public UnityEvent OnTimerPaused; // Timer duraklat?ld???nda tetiklenen event
        public UnityEvent OnTimerResumed; // Timer devam ettirildi?inde tetiklenen event
        public UnityEvent OnTimerStopped; // Timer durduruldu?unda tetiklenen event
        public UnityEvent OnTimeCritical; // Zaman kritik e?i?e d�?t�?�nde tetiklenen event
        public UnityEvent OnTimeFinished; // Zaman s?f?rland???nda tetiklenen event

        private void Start()
        {
            // Oyunun ba?lang?c?nda timer'? ba?lat
            StartTimer(_initialTime);
        }

        /// <summary>
        /// Timer'? belirtilen s�re ile ba?lat?r.
        /// </summary>
        /// <param name="timeInSeconds">Ba?lang?� s�resi (saniye)</param>
        public void StartTimer(float timeInSeconds)
        {
            StopTimer(); // Mevcut timer varsa durdur
            _currentTime = timeInSeconds; // Yeni s�reyi ayarla
            _isTimerRunning = true; // Timer'? �al??t?r
            OnTimerStarted?.Invoke(); // Timer ba?lad???nda event tetiklenir
            StartCoroutine(TimerCoroutine()); // Timer coroutine'ini ba?lat
        }

        /// <summary>
        /// Timer'?n countdown (geri say?m) i?lemi.
        /// </summary>
        private IEnumerator TimerCoroutine()
        {
            while (_isTimerRunning)
            {
                yield return new WaitForSeconds(_updateInterval); // Update intervali kadar bekle

                _currentTime -= _updateInterval; // Zaman? azalt
                OnTimeUpdated?.Invoke(_currentTime); // Zaman g�ncellendi?inde event tetiklenir

                if (_currentTime <= _criticalTimeThreshold && _currentTime > 0)
                {
                    OnTimeCritical?.Invoke(); // Kritik e?i?e d�?t�?�nde event tetiklenir
                }

                if (_currentTime <= 0)
                {
                    HandleTimeExpired(); // Zaman s?f?rland???nda i?lemi ba?lat
                    break;
                }
            }
        }

        /// <summary>
        /// Zaman s?f?rland???nda yap?lacak i?lemler.
        /// </summary>
        private void HandleTimeExpired()
        {
            _currentTime = 0; // Zaman? s?f?rla
            _isTimerRunning = false; // Timer'? durdur
            OnTimeFinished?.Invoke(); // Zaman bitince event tetiklenir
        }

        /// <summary>
        /// Timer'? duraklat?r.
        /// </summary>
        public void PauseTimer()
        {
            if (!_isTimerRunning) return; // Timer �al??m?yorsa �?k

            _isTimerRunning = false; // Timer'? durdur
            OnTimerPaused?.Invoke(); // Timer duraklat?ld???nda event tetiklenir
        }

        /// <summary>
        /// Timer'? devam ettirir.
        /// </summary>
        public void ResumeTimer()
        {
            if (_isTimerRunning) return; // Timer zaten �al???yorsa �?k

            _isTimerRunning = true; // Timer'? tekrar ba?lat
            OnTimerResumed?.Invoke(); // Timer devam ettirildi?inde event tetiklenir
            StartCoroutine(TimerCoroutine()); // Timer coroutine'ini ba?lat
        }

        /// <summary>
        /// Timer'? tamamen durdurur.
        /// </summary>
        public void StopTimer()
        {
            _isTimerRunning = false; // Timer'? durdur
            StopAllCoroutines(); // T�m coroutine'leri durdur
            OnTimerStopped?.Invoke(); // Timer durduruldu?unda event tetiklenir
        }

        /// <summary>
        /// Timer'a ekstra s�re ekler.
        /// </summary>
        /// <param name="extraTime">Ekstra s�re (saniye)</param>
        public void AddExtraTime(float extraTime)
        {
            _currentTime += extraTime; // Ekstra s�reyi ekle
            OnTimeUpdated?.Invoke(_currentTime); // Zaman g�ncellendi?inde event tetiklenir
        }

        /// <summary>
        /// Timer'? belirli bir s�re dondurur.
        /// </summary>
        /// <param name="duration">Dondurma s�resi (saniye)</param>
        public void FreezeTimer(float duration)
        {
            if (_freezeCoroutine != null) return; // E?er zaten bir freeze coroutine'i varsa, �?k

            PauseTimer(); // Timer'? duraklat
            _freezeCoroutine = StartCoroutine(ResumeTimerAfterFreeze(duration)); // Dondurmay? ba?lat
        }

        /// <summary>
        /// Dondurma s�resi tamamland?ktan sonra timer'? devam ettirir.
        /// </summary>
        /// <param name="duration">Dondurma s�resi (saniye)</param>
        private IEnumerator ResumeTimerAfterFreeze(float duration)
        {
            yield return new WaitForSeconds(duration); // Dondurma s�resi kadar bekle
            ResumeTimer(); // Timer'? devam ettir
            _freezeCoroutine = null; // Coroutine bitince null yap
        }

        /// <summary>
        /// Global zaman efekti i�in Time.timeScale'? ayarlar.
        /// </summary>
        /// <param name="scale">Yeni zaman h?z? (1 normal h?zd?r).</param>
        public void SetTimeScale(float scale)
        {
            Time.timeScale = scale; // Time.timeScale'? ayarla
        }

        /// <summary>
        /// Timer'? ba?taki s�re ile s?f?rlar.
        /// </summary>
        public void ResetTimer()
        {
            StopTimer(); // Timer'? durdur
            StartTimer(_initialTime); // Timer'? ba?lat
        }

        /// <summary>
        /// ?u anki kalan s�reyi d�nd�r�r.
        /// </summary>
        public float GetCurrentTime()
        {
            return _currentTime; // Kalan s�reyi d�nd�r
        }

        /// <summary>
        /// Timer'?n �al???p �al??mad???n? kontrol eder.
        /// </summary>
        public bool IsTimerRunning()
        {
            return _isTimerRunning; // Timer �al???yorsa true d�nd�r
        }
    }
}
