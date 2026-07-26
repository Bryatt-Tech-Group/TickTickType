using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject endScreen;
    public Bomb bomb;
    public Button playButton;

    public DialogueBox dialogueBox;
    public DialogueScript dialogueScript;
    
    public MusicManager musicManager;
    public AudioClip OpeningSong;
    public AudioClip GameplaySong;
    public AudioClip EndingSong;
    
    void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        dialogueBox.OnDialogueEnd += StartBombSequence;
        bomb.OnEndPlay += OnEndPlay;
        
        startScreen.SetActive(true);
        endScreen.SetActive(false);
        bomb.gameObject.SetActive(true);
        dialogueBox.gameObject.SetActive(false);
        
        musicManager.PlaySong(OpeningSong);
    }
    
    void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnPlayButtonClick);
    }

    void OnPlayButtonClick()
    {
        startScreen.SetActive(false);
        //bomb.BeginPlay();
        
        dialogueBox.StartDialogue(dialogueScript);
    }

    void StartBombSequence()
    {
        bomb.BeginPlay();
        musicManager.PlaySong(GameplaySong);
    }

    void OnEndPlay()
    {
        bomb.gameObject.SetActive(false);
        endScreen.SetActive(true);
        musicManager.PlaySong(EndingSong);
    }
}
