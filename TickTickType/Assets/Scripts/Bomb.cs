using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SparkTime
{
    public GameObject spark;
    public float spawnTime;

    public SparkTime(GameObject spark, float spawnTime)
    {
        this.spark = spark;
        this.spawnTime = spawnTime;
    }
}

public class Bomb : MonoBehaviour
{
    public float baseTime = 30.0f;
    public float recoveryTime = 1.0f;
    private float currentTime = 0.0f;
    private bool bPlaying = false;

    public TextMeshProUGUI timerSLabel;
    public TextMeshProUGUI timerMSLabel;

    public Prompter prompter;

    public GameObject sparksPrefab;
    private Stack<SparkTime> activeSparks = new Stack<SparkTime>();

    public float minSpawnInterval = 0.5f;
    public float maxSpawnInterval = 1.5f;

    private float timer;
    private float nextSpawnTime;

    public event Action OnEndPlay;

    void Start()
    {
        SetTime(baseTime);
        ScheduleNextSpawn();
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

        timer += Time.deltaTime;

        if (timer > nextSpawnTime)
        {
            SpawnSpark();
            ScheduleNextSpawn();
        }
    }

    public void BeginPlay()
    {
        bPlaying = true;
        prompter.BeginPlay();
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
        
        timerSLabel.text = currentTimeS.ToString("D3");
        timerMSLabel.text = currentTimeMS.ToString("D2");
    }

    public void RecoverTime()
    {
        SetTime(currentTime + recoveryTime);
    }

    void ScheduleNextSpawn()
    {
        timer = 0f;
        nextSpawnTime = UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void SpawnSpark()
    {
        Vector3[] corners = new Vector3[4];
        transform.GetComponent<RectTransform>().GetWorldCorners(corners);

        Vector3 spawnPosition = new Vector3(
            UnityEngine.Random.Range(corners[0].x, corners[2].x),
            UnityEngine.Random.Range(corners[0].y, corners[2].y),
            -1
        );

        activeSparks.Push(new SparkTime(Instantiate(sparksPrefab, spawnPosition, Quaternion.identity, transform), currentTime));
    }

    public void RemoveSpark()
    {
        if (activeSparks.Count <= 0) return;

        SparkTime sparkTime = activeSparks.Pop();
        SetTime(sparkTime.spawnTime);

        Destroy(sparkTime.spark);
    }
}
