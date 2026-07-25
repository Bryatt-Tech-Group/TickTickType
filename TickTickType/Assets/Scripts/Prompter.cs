using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Prompter : MonoBehaviour
{
    public string[] prompts;
    public Transform[] spawnTransforms;
    public GameObject promptLabelPrefab;

    private List<GameObject> promptLabels = new List<GameObject>();
    private int currentPromptIndex = -1;
    private int currentCharIndex = -1;
    private bool bPlaying = false;
    
    void Start()
    {
        bPlaying = true;

        // Initialize all prompt labels.
        for (int i = 0; i < prompts.Length; i++)
        {
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
    }
    
    void Update()
    {
        if (!bPlaying) return;

        // Try to select a prompt if one is not already selected.
        if (currentPromptIndex == -1)
        {
            for (int i = 0; i < prompts.Length; i++)
            {
                Key expectedKey = GetExpectedKey(i, 0);

                if (IsKeyPressed(expectedKey))
                {
                    Debug.Log(prompts[i]);
                    
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

                if (currentCharIndex >= prompts[currentPromptIndex].Length)
                {
                    OnPromptComplete();
                }
            }
        }
    }

    Key GetExpectedKey(int promptIndex, int charIndex)
    {
        return CharToKey(prompts[promptIndex][charIndex]);
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
        string prompt = prompts[promptIndex];
        promptLabels[promptIndex].GetComponent<TextMeshProUGUI>().text = "<color=#FFFFFF>" + prompt.Substring(0, charIndex) + "</color><color=#000000>" + prompt.Substring(charIndex) + "</color>";
    }
    
    void OnPromptComplete()
    {
        Debug.Log("Prompt fully typed!");

        currentPromptIndex = -1;
        currentCharIndex = -1;
    }
}
