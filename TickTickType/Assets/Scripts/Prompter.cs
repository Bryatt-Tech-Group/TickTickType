using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Prompter : MonoBehaviour
{
    public Transform[] spawnTransforms;
    public GameObject promptLabelPrefab;

    public int maxPrompts = 5;
    int minPromptLength = 4;
    int maxPromptLength = 10;

    private string[] potentialPrompts;
    private List<string> activePrompts = new List<string>();
    private List<GameObject> promptLabels = new List<GameObject>();
    private int currentPromptIndex = -1;
    private int currentCharIndex = -1;
    private bool bPlaying = false;

    public event Action OnPromptSolved;
    
    void Start()
    {
        TextAsset wordFile = Resources.Load<TextAsset>("wordlist");
        potentialPrompts = wordFile.text.Split('\n');

        // Clean up each entry.
        for (int i = 0; i < potentialPrompts.Length; i++)
        {
            potentialPrompts[i] = potentialPrompts[i].Trim();
        }

        activePrompts = new List<string>(maxPrompts);
        promptLabels = new List<GameObject>(maxPrompts);    
    }

    public void BeginPlay()
    {
        // Initialize all prompt labels.
        for (int i = 0; i < maxPrompts; i++)
        {
            activePrompts.Add(GetRandomPrompt());

            GameObject newPrompt = Instantiate(promptLabelPrefab, transform);
            promptLabels.Add(newPrompt);

            RectTransform promptRect = newPrompt.GetComponent<RectTransform>();
            RectTransform spawnRect = spawnTransforms[i].GetComponent<RectTransform>();

            // Match position/rotation/scale to the spawn point
            promptRect.anchoredPosition = spawnRect.anchoredPosition;
            promptRect.rotation = spawnRect.rotation;
            promptRect.localScale = spawnRect.localScale;

            UpdatePromptLabel(i, 0);
        }

        bPlaying = true;
    }

    void Update()
    {
        if (!bPlaying) return;

        // Try to select a prompt if one is not already selected.
        if (currentPromptIndex == -1)
        {
            for (int i = 0; i < maxPrompts; i++)
            {
                Key expectedKey = GetExpectedKey(i, 0);

                if (IsKeyPressed(expectedKey))
                {
                    currentPromptIndex = i;
                    currentCharIndex = 0;
                    break;
                }
            }
        }

        // If a prompt is selected, try to solve it!
        if (currentPromptIndex != -1)
        {
            Key expectedKey = GetExpectedKey(currentPromptIndex, currentCharIndex);

            if (IsKeyPressed(expectedKey))
            {
                UpdatePromptLabel(currentPromptIndex, ++currentCharIndex);

                if (currentCharIndex >= activePrompts[currentPromptIndex].Length)
                {
                    OnPromptComplete();
                }
            }
        }
    }

    Key GetExpectedKey(int promptIndex, int charIndex)
    {
        return CharToKey(activePrompts[promptIndex][charIndex]);
    }

    bool IsKeyPressed(Key key)
    {
        return key != Key.None && Keyboard.current[key].wasPressedThisFrame;
    }

    Key CharToKey(char c)
    {
        char lower = char.ToLower(c);
        
        if (lower >= 'a' && lower <= 'z')
        {
            // Key.A through Key.Z map directly by enum offset
            return Key.A + (lower - 'a');
        }
        
        if (lower >= '0' && lower <= '9')
        {
            return Key.Digit0 + (lower - '0');
        }
        
        return Key.None;
    }

    void UpdatePromptLabel(int promptIndex, int charIndex)
    {
        string prompt = activePrompts[promptIndex];
        promptLabels[promptIndex].GetComponent<TextMeshProUGUI>().text = "<color=#FFFFFF>" + prompt.Substring(0, charIndex) + "</color><color=#000000>" + prompt.Substring(charIndex) + "</color>";
    }
    
    void OnPromptComplete()
    {
        Debug.Log("Prompt fully typed!");
        
        activePrompts[currentPromptIndex] = GetRandomPrompt();
        UpdatePromptLabel(currentPromptIndex, 0);

        currentPromptIndex = -1;
        currentCharIndex = -1;
        
        OnPromptSolved?.Invoke();
    }

    public void IncreaseDifficulty()
    {
        minPromptLength++;
        maxPromptLength++;
    }
    
    string GetRandomPrompt()
    {
        string potentialPrompt = "";
        bool firstLetterConflict = true;

        while (potentialPrompt.Length < minPromptLength || potentialPrompt.Length >= maxPromptLength || firstLetterConflict)
        {
            potentialPrompt = potentialPrompts[UnityEngine.Random.Range(0, potentialPrompts.Length)];

            firstLetterConflict = false;
            foreach (string active in activePrompts)
            {
                if (active.Length > 0 && potentialPrompt[0] == active[0])
                {
                    firstLetterConflict = true;
                    break;
                }
            }
        }

        return potentialPrompt;
    }
}
