using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SerapKeremGameTools._Game._TimeSystem
{
    public class TimeManager : MonoBehaviour
    {
        [Header("TimeManager Parameters")]
        [SerializeField, Tooltip("Initial time for the level in seconds.")]
        private float _initialTime = 300f; // Ba?lang?ç süresi (saniye)

        [SerializeField, Tooltip("The time interval for the timer updates (in seconds).")]
        private float _updateInterval = 1f; // Zaman güncelleme aral??? (saniye)

        [SerializeField, Tooltip("The critical time threshold (in seconds).")]
        private float _criticalTimeThreshold = 15f; // Kritik zaman e?i?i (saniye)

        private float _currentTime; // ?u anki zaman
        private bool _isTimerRunning; // Timer'?n çal???p çal??mad???n? kontrol eder
        private Coroutine _freezeCoroutine; // Timer'? dondurmak için kullan?lan coroutine

        // Events
        public UnityEvent<float> OnTimeUpdated; // Zaman güncellendi?inde tetiklenen event
        public UnityEvent OnTimerStarted; // Timer ba?lad???nda tetiklenen event
        public UnityEvent OnTimerPaused; // Timer duraklat?ld???nda tetiklenen event
        public UnityEvent OnTimerResumed; // Timer devam ettirildi?inde tetiklenen event
        public UnityEvent OnTimerStopped; // Timer durduruldu?unda tetiklenen event
        public UnityEvent OnTimeCritical; // Zaman kritik e?i?e dü?tü?ünde tetiklenen event
        public UnityEvent OnTimeFinished; // Zaman s?f?rland???nda tetiklenen event

        private void Start()
        {
            // Oyunun ba?lang?c?nda timer'? ba?lat
            StartTimer(_initialTime);
        }

        /// <summary>
        /// Timer'? belirtilen süre ile ba?lat?r.
        /// </summary>
        /// <param name="timeInSeconds">Ba?lang?ç süresi (saniye)</param>
        public void StartTimer(float timeInSeconds)
        {
            StopTimer(); // Mevcut timer varsa durdur
            _currentTime = timeInSeconds; // Yeni süreyi ayarla
            _isTimerRunning = true; // Timer'? çal??t?r
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
                OnTimeUpdated?.Invoke(_currentTime); // Zaman güncellendi?inde event tetiklenir

                if (_currentTime <= _criticalTimeThreshold && _currentTime > 0)
                {
                    OnTimeCritical?.Invoke(); // Kritik e?i?e dü?tü?ünde event tetiklenir
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
            if (!_isTimerRunning) return; // Timer çal??m?yorsa ç?k

            _isTimerRunning = false; // Timer'? durdur
            OnTimerPaused?.Invoke(); // Timer duraklat?ld???nda event tetiklenir
        }

        /// <summary>
        /// Timer'? devam ettirir.
        /// </summary>
        public void ResumeTimer()
        {
            if (_isTimerRunning) return; // Timer zaten çal???yorsa ç?k

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
            StopAllCoroutines(); // Tüm coroutine'leri durdur
            OnTimerStopped?.Invoke(); // Timer durduruldu?unda event tetiklenir
        }

        /// <summary>
        /// Timer'a ekstra süre ekler.
        /// </summary>
        /// <param name="extraTime">Ekstra süre (saniye)</param>
        public void AddExtraTime(float extraTime)
        {
            _currentTime += extraTime; // Ekstra süreyi ekle
            OnTimeUpdated?.Invoke(_currentTime); // Zaman güncellendi?inde event tetiklenir
        }

        /// <summary>
        /// Timer'? belirli bir süre dondurur.
        /// </summary>
        /// <param name="duration">Dondurma süresi (saniye)</param>
        public void FreezeTimer(float duration)
        {
            if (_freezeCoroutine != null) return; // E?er zaten bir freeze coroutine'i varsa, ç?k

            PauseTimer(); // Timer'? duraklat
            _freezeCoroutine = StartCoroutine(ResumeTimerAfterFreeze(duration)); // Dondurmay? ba?lat
        }

        /// <summary>
        /// Dondurma süresi tamamland?ktan sonra timer'? devam ettirir.
        /// </summary>
        /// <param name="duration">Dondurma süresi (saniye)</param>
        private IEnumerator ResumeTimerAfterFreeze(float duration)
        {
            yield return new WaitForSeconds(duration); // Dondurma süresi kadar bekle
            ResumeTimer(); // Timer'? devam ettir
            _freezeCoroutine = null; // Coroutine bitince null yap
        }

        /// <summary>
        /// Global zaman efekti için Time.timeScale'? ayarlar.
        /// </summary>
        /// <param name="scale">Yeni zaman h?z? (1 normal h?zd?r).</param>
        public void SetTimeScale(float scale)
        {
            Time.timeScale = scale; // Time.timeScale'? ayarla
        }

        /// <summary>
        /// Timer'? ba?taki süre ile s?f?rlar.
        /// </summary>
        public void ResetTimer()
        {
            StopTimer(); // Timer'? durdur
            StartTimer(_initialTime); // Timer'? ba?lat
        }

        /// <summary>
        /// ?u anki kalan süreyi döndürür.
        /// </summary>
        public float GetCurrentTime()
        {
            return _currentTime; // Kalan süreyi döndür
        }

        /// <summary>
        /// Timer'?n çal???p çal??mad???n? kontrol eder.
        /// </summary>
        public bool IsTimerRunning()
        {
            return _isTimerRunning; // Timer çal???yorsa true döndür
        }
    }
}
