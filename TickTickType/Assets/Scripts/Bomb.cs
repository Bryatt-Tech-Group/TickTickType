using System;
using TMPro;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float baseTime = 30.0f;
    private float currentTime = 0.0f;
    
    public TextMeshProUGUI timerSLabel;
    public TextMeshProUGUI timerMSLabel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = baseTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        
        int currentTimeS = (int)Math.Floor(currentTime);
        int currentTimeMS = (int)((currentTime - currentTimeS) * 100);
        
        timerSLabel.text = currentTimeS.ToString();
        timerMSLabel.text = currentTimeMS.ToString();
    }
}
