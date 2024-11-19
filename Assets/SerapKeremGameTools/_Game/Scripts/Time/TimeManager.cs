using System;
using UnityEngine;
using UnityEngine.Events;

namespace SerapKeremGameTools._Game._TimeSystem
{
    /// <summary>
    /// Manages game time and countdown functionality, providing events for key time states.
    /// </summary>
    public class TimeManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of TimeManager.
        /// </summary>
        public static TimeManager Instance;

        [Tooltip("Total game time elapsed in seconds.")]
        private float gameTimeElapsed;

        [Tooltip("Indicates whether the game time is running.")]
        private bool isGameTimeRunning;

        [Tooltip("Countdown time left in seconds.")]
        private float countdownTimeLeft;

        [Tooltip("Indicates whether the countdown is active.")]
        private bool countdownActive;

        [Tooltip("Event invoked when game time starts.")]
        public UnityEvent OnTimeStart = new UnityEvent();

        [Tooltip("Event invoked when game time ends.")]
        public UnityEvent OnTimeEnd = new UnityEvent();

        [Tooltip("Event invoked when a countdown starts.")]
        public UnityEvent OnCountDownStart = new UnityEvent();

        [Tooltip("Event invoked when a countdown ends.")]
        public UnityEvent OnCountDownEnd = new UnityEvent();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Update()
        {
            if (isGameTimeRunning)
                gameTimeElapsed += Time.deltaTime;

            if (countdownActive)
            {
                countdownTimeLeft -= Time.deltaTime;

                if (countdownTimeLeft <= 0)
                {
                    countdownActive = false;
                    countdownTimeLeft = 0;
                    OnCountDownEnd.Invoke();
                }
            }
        }

        /// <summary>
        /// Starts tracking the game time from zero.
        /// </summary>
        public void StartTime()
        {
            isGameTimeRunning = true;
            gameTimeElapsed = 0;
            OnTimeStart.Invoke();
        }

        /// <summary>
        /// Pauses the game time.
        /// </summary>
        public void PauseTime()
        {
            isGameTimeRunning = false;
        }

        /// <summary>
        /// Resumes the game time.
        /// </summary>
        public void ResumeTime()
        {
            isGameTimeRunning = true;
        }

        /// <summary>
        /// Starts a countdown from three seconds.
        /// </summary>
        public void CountdownFromThree()
        {
            countdownTimeLeft = 3f;
            countdownActive = true;
            OnCountDownStart.Invoke();
        }

        /// <summary>
        /// Gets the total elapsed game time.
        /// </summary>
        /// <returns>Elapsed game time in seconds.</returns>
        public float GetGameTimeElapsed()
        {
            return gameTimeElapsed;
        }

        /// <summary>
        /// Checks if a countdown is currently active.
        /// </summary>
        /// <returns>True if a countdown is active, otherwise false.</returns>
        public bool IsCountdownActive()
        {
            return countdownActive;
        }

        /// <summary>
        /// Gets the remaining time of the countdown.
        /// </summary>
        /// <returns>Countdown time left in seconds.</returns>
        public float GetCountdownTimeLeft()
        {
            return countdownTimeLeft;
        }
    }
}
