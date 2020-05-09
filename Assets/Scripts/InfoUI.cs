using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoUI : MonoBehaviour
{

    [Header("PLAY")]
    [SerializeField] private GameObject playUI;
    [SerializeField] private Text timeText;
    [SerializeField] private Color secondsDangerColor;
    [SerializeField] private Text counterText;
    [SerializeField] private Text actionText;

    [Header("FINISH")]
    [SerializeField] private GameObject finishUI;
    [SerializeField] private Text endCounterText;

    [Header("PAUSE")]
    [SerializeField] private GameObject pauseUI;

    private static int SECONDS_DANGER = 10;

    private int score = 0;

    void Start()
    {
        UpdateCounter();
        ShowPlay();
    }

    public void HideAll()
    {
        playUI.SetActive(false);
        finishUI.SetActive(false);
        pauseUI.SetActive(false);
    }

    public void ShowPlay()
    {
        HideAll();
        playUI.SetActive(true);
    }

    public void ShowFinish()
    {
        HideAll();
        finishUI.SetActive(true);

        endCounterText.text = counterText.text;
    }

    public void ShowPause()
    {
        HideAll();
        pauseUI.SetActive(true);
    }

    public void AddScore()
    {
        score++;
        UpdateCounter();
    }

    void UpdateCounter()
    {
        counterText.text = score.ToString();
    }

    public void SetActionText(string text)
    {
        actionText.enabled = true;
        actionText.text = text;
    }

    public void ClearActionText()
    {
        actionText.enabled = false;
        actionText.text = "";
    }

    public void UpdateTimer(float secondsRemaining)
    {
        System.TimeSpan seconds = System.TimeSpan.FromSeconds(secondsRemaining);
        timeText.text = seconds.ToString(@"m\:ss");

        if (secondsRemaining <= SECONDS_DANGER)
        {
            timeText.color = secondsDangerColor;
        }
        else
        {
            timeText.color = Color.white;
        }
    }
}
