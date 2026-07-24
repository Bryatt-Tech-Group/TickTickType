using System;
using TMPro;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float baseTime = 30.0f;
    private float currentTime = 0.0f;
    private bool bPlaying = false;
    
    public TextMeshProUGUI timerSLabel;
    public TextMeshProUGUI timerMSLabel;
    
    public event Action OnEndPlay;
    
    void Start()
    {
        SetTime(baseTime);
    }

    void Update()
    {
        if (!bPlaying)
        {
            return;
        }
        
        SetTime(currentTime - Time.deltaTime);

        if (currentTime <= 0.0f)
        {
            EndPlay();
        }
    }

    public void BeginPlay()
    {
        bPlaying = true;
    }

    void EndPlay()
    {
        bPlaying = false;
        
        OnEndPlay?.Invoke();
    }

    void SetTime(float newTime)
    {
        currentTime = Math.Clamp(newTime, 0.0f, baseTime);
        
        int currentTimeS = (int)Math.Floor(currentTime);
        int currentTimeMS = (int)((currentTime - currentTimeS) * 100);
        
        timerSLabel.text = currentTimeS.ToString();
        timerMSLabel.text = currentTimeMS.ToString("D2");
    }
}
