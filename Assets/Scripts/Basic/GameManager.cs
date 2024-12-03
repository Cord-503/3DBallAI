using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private CheckPoint currentCheckpoint;

    [Header("Timer Settings")]
    public TextMeshProUGUI timerText;
    private float currentTime = 0f;
    private bool isTimerRunning = true;

    [Header("Win Screen")]
    public GameObject winScreen;
    public TextMeshProUGUI winText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            currentTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(currentTime);
            timerText.text = string.Format("{0:00}:{1:00}.{2:00}",
                timeSpan.Minutes,
                timeSpan.Seconds,
                Mathf.FloorToInt((currentTime % 1f) * 100));
        }
    }

    public void SetCurrentCheckpoint(CheckPoint checkpoint)
    {
        currentCheckpoint = checkpoint;
    }

    public void RespawnPlayer(GameObject player)
    {
        if (currentCheckpoint != null)
        {
            player.transform.position = currentCheckpoint.GetCheckpointPosition();
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void PauseTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = 0f;
        UpdateTimerDisplay();
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public void WinGame()
    {
        PauseTimer();

        if (winScreen != null)
        {
            winScreen.SetActive(true);
        }

        if (winText != null)
        {
            winText.text = "You reached the Sky!";
        }

        Time.timeScale = 0f;
    }
}
