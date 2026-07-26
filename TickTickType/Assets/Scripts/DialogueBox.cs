using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueBox : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;
    public Image portraitImage;
    public AudioSource audioSource;
    public AudioClip blipSound;

    [Header("Continue Indicator")]
    public RectTransform continueIndicator; // the little arrow/icon GameObject
    public float bobHeight = 8f;
    public float bobSpeed = 2f;

    [Header("Portraits")]
    public MoodPortrait[] portraits;

    [Header("Typing Settings")]
    public float charactersPerSecond = 30f;
    public bool playBlipEveryCharacter = false;
    public int blipEveryNCharacters = 2;

    private List<DialogueLine> currentDialogue;
    private int currentLineIndex;
    private Coroutine typingCoroutine;
    private Coroutine bobCoroutine;
    private bool isTyping;
    private Vector2 indicatorStartPos;
    
    public event Action OnDialogueEnd;

    [System.Serializable]
    public class MoodPortrait
    {
        public Mood mood;
        public Sprite sprite;
    }

    void Awake()
    {
        if (continueIndicator != null)
        {
            indicatorStartPos = continueIndicator.anchoredPosition;
            continueIndicator.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        bool advancePressed =
            (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
            (Keyboard.current != null && (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame));

        if (advancePressed)
        {
            HandleAdvanceInput();
        }
    }

    public void StartDialogue(DialogueScript script)
    {
        currentDialogue = script.lines;
        currentLineIndex = 0;
        gameObject.SetActive(true);
        ShowLine(currentLineIndex);
    }

    void HandleAdvanceInput()
    {
        if (isTyping)
        {
            SkipTyping();
        }
        else
        {
            currentLineIndex++;
            if (currentLineIndex < currentDialogue.Count)
            {
                ShowLine(currentLineIndex);
            }
            else
            {
                EndDialogue();
            }
        }
    }

    void ShowLine(int index)
    {
        DialogueLine line = currentDialogue[index];

        speakerNameText.text = line.speakerName;
        SetPortrait(line.mood);

        HideIndicator();

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(line.text));
    }

    IEnumerator TypeLine(string fullText)
    {
        isTyping = true;
        dialogueText.text = "";
        float delay = 1f / charactersPerSecond;

        for (int i = 0; i < fullText.Length; i++)
        {
            dialogueText.text += fullText[i];

            if (fullText[i] != ' ')
            {
                bool shouldPlay = playBlipEveryCharacter || (i % blipEveryNCharacters == 0);
                if (shouldPlay && blipSound != null)
                {
                    audioSource.PlayOneShot(blipSound);
                }
            }

            yield return new WaitForSeconds(delay);
        }

        isTyping = false;
        ShowIndicator();
    }

    void SkipTyping()
    {
        StopCoroutine(typingCoroutine);
        dialogueText.text = currentDialogue[currentLineIndex].text;
        isTyping = false;
        ShowIndicator();
    }

    void ShowIndicator()
    {
        if (continueIndicator == null) return;

        continueIndicator.gameObject.SetActive(true);
        if (bobCoroutine != null)
            StopCoroutine(bobCoroutine);
        bobCoroutine = StartCoroutine(BobIndicator());
    }

    void HideIndicator()
    {
        if (continueIndicator == null) return;

        if (bobCoroutine != null)
            StopCoroutine(bobCoroutine);
        continueIndicator.gameObject.SetActive(false);
        continueIndicator.anchoredPosition = indicatorStartPos; // reset position
    }

    IEnumerator BobIndicator()
    {
        while (true)
        {
            float yOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            continueIndicator.anchoredPosition = indicatorStartPos + new Vector2(0, yOffset);
            yield return null;
        }
    }

    void SetPortrait(Mood mood)
    {
        foreach (MoodPortrait mp in portraits)
        {
            if (mp.mood == mood)
            {
                portraitImage.sprite = mp.sprite;
                return;
            }
        }

        Debug.LogWarning($"No portrait found for mood: {mood}");
    }

    void EndDialogue()
    {
        HideIndicator();
        gameObject.SetActive(false);
        
        OnDialogueEnd?.Invoke();
    }
}