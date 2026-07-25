using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Prompter : MonoBehaviour
{
    public TextMeshProUGUI promptLabel;
    public string[] prompts;

    private string prompt = "";
    private int promptIndex = 0;
    private int charIndex = 0;

    private bool bPlaying = false;
    
    void Start()
    {
        bPlaying = true;
        charIndex = 0;
        prompt = prompts[promptIndex];
        SetCharIndex(0);
    }

    void SetCharIndex(int index)
    {
        promptLabel.text = "<color=#FFFFFF>" + prompt.Substring(0, index) + "</color><color=#000000>" + prompt.Substring(index) + "</color>";
    }
    
    void Update()
    {
        if (!bPlaying) return;
        
        if (charIndex >= prompt.Length)
            return;

        if (Keyboard.current == null)
            return; // no keyboard connected

        char expected = prompt[charIndex];
        Key expectedKey = CharToKey(expected);

        if (expectedKey != Key.None && Keyboard.current[expectedKey].wasPressedThisFrame)
        {
            SetCharIndex(++charIndex);
            Debug.Log($"Correct! ({charIndex}/{prompt.Length})");

            if (charIndex >= prompt.Length)
            {
                OnPromptComplete();
            }
        }
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
        return Key.None; // extend this for punctuation/space if your prompt needs it
    }
    
    void OnPromptComplete()
    {
        Debug.Log("Prompt fully typed!");
        
        if (++promptIndex >= prompts.Length)
        {
            prompt = "Congrats you did it!";
            bPlaying = false;
        }
        else
        {
            prompt = prompts[promptIndex];
            charIndex = 0;
            SetCharIndex(charIndex);
        }
    }
}
