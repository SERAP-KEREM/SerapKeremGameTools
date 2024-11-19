﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SerapKeremGameTools._Game._TimeSystem
{
    /// <summary>
    /// Provides a test interface for TimeManager functionality using UI elements.
    /// </summary>
    public class TestTimeManager : MonoBehaviour
    {
        [Tooltip("Displays the elapsed game time.")]
        public TextMeshProUGUI timeText;

        [Tooltip("Displays the remaining countdown time.")]
        public TextMeshProUGUI countdownText;

        [Tooltip("Button to start the game time.")]
        public Button startButton;

        [Tooltip("Button to pause the game time.")]
        public Button pauseButton;

        [Tooltip("Button to resume the game time.")]
        public Button resumeButton;

        [Tooltip("Button to start a countdown from 3 seconds.")]
        public Button countdownButton;

        private void Start()
        {
            startButton.onClick.AddListener(StartGame);
            pauseButton.onClick.AddListener(PauseGame);
            resumeButton.onClick.AddListener(ResumeGame);
            countdownButton.onClick.AddListener(StartCountdown);

            TimeManager.Instance.OnTimeStart.AddListener(OnTimeStart);
            TimeManager.Instance.OnTimeEnd.AddListener(OnTimeEnd);
            TimeManager.Instance.OnCountDownStart.AddListener(OnCountDownStart);
            TimeManager.Instance.OnCountDownEnd.AddListener(OnCountDownEnd);
        }

        /// <summary>
        /// Starts the game time using TimeManager.
        /// </summary>
        private void StartGame()
        {
            TimeManager.Instance.StartTime();
        }

        /// <summary>
        /// Pauses the game time using TimeManager.
        /// </summary>
        private void PauseGame()
        {
            TimeManager.Instance.PauseTime();
        }

        /// <summary>
        /// Resumes the game time using TimeManager.
        /// </summary>
        private void ResumeGame()
        {
            TimeManager.Instance.ResumeTime();
        }

        /// <summary>
        /// Starts a countdown from 3 seconds using TimeManager.
        /// </summary>
        private void StartCountdown()
        {
            TimeManager.Instance.CountdownFromThree();
        }

        private void OnTimeStart()
        {
            Debug.Log("Game time started.");
        }

        private void OnTimeEnd()
        {
            Debug.Log("Game time ended.");
        }

        private void OnCountDownStart()
        {
            Debug.Log("Countdown started.");
        }

        private void OnCountDownEnd()
        {
            Debug.Log("Countdown ended.");
        }

        private void Update()
        {
            // Update the game time display
            timeText.text = $"Time: {TimeManager.Instance.GetGameTimeElapsed():F2}";

            // Update the countdown display
            if (TimeManager.Instance.IsCountdownActive())
            {
                countdownText.text = $"Countdown: {TimeManager.Instance.GetCountdownTimeLeft():F2}";
            }
            else
            {
                countdownText.text = "Countdown: --";
            }
        }
    }
}
