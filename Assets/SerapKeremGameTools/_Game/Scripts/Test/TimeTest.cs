using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SerapKeremGameTools._Game._TimeSystem
{
    public class TestTimeManager : MonoBehaviour
    {
        [Header("TimeManager Reference")]
        public TimeManager timeManager;

        [Header("UI Elements")]
        public TextMeshProUGUI timerText;
        public TextMeshProUGUI statusText;
        public int addTime=5;
        public int freezeTime=5;

        private void Start()
        {
            // Bağlantı sağlamak için TimeManager eventlerini dinle
            timeManager.OnTimeUpdated.AddListener(UpdateTimerUI);
            timeManager.OnTimerStarted.AddListener(() => UpdateStatusUI("Timer Started"));
            timeManager.OnTimerPaused.AddListener(() => UpdateStatusUI("Timer Paused"));
            timeManager.OnTimerResumed.AddListener(() => UpdateStatusUI("Timer Resumed"));
            timeManager.OnTimerStopped.AddListener(() => UpdateStatusUI("Timer Stopped"));
            timeManager.OnTimeCritical.AddListener(() => UpdateStatusUI("Critical Time!"));
            timeManager.OnTimeFinished.AddListener(() => UpdateStatusUI("Time Finished"));
        }

        private void UpdateTimerUI(float currentTime)
        {
            // Kalan süreyi güncelle
            timerText.text = $"Time Remaining: {currentTime:F2} seconds";
        }

        private void UpdateStatusUI(string status)
        {
            // Durum mesajını güncelle
            statusText.text = $"Status: {status}";
        }

        // Timer başlatma
        public void StartTimer()
        {
            timeManager.StartTimer(60f); // 60 saniyelik bir zamanlayıcı başlat
        }

        // Timer duraklatma
        public void PauseTimer()
        {
            timeManager.PauseTimer();
        }

        // Timer devam ettirme
        public void ResumeTimer()
        {
            timeManager.ResumeTimer();
        }

        // Timer durdurma
        public void StopTimer()
        {
            timeManager.StopTimer();
        }

        // Timer sıfırlama
        public void ResetTimer()
        {
            timeManager.ResetTimer();
        }

        // Zaman ekleme
        public void AddTime()
        {
            if (float.TryParse(addTime.ToString(), out float extraTime))
            {
                timeManager.AddExtraTime(extraTime);
            }
            else
            {
                UpdateStatusUI("Invalid time input!");
            }
        }

        // Timer dondurma
        public void FreezeTime()
        {
            if (float.TryParse(freezeTime.ToString(), out float freezeDuration))
            {
                timeManager.FreezeTimer(freezeDuration);
            }
            else
            {
                UpdateStatusUI("Invalid freeze duration input!");
            }
        }
    }
}
