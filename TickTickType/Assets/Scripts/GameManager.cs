using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject endScreen;
    public Bomb bomb;
    public Button playButton;
    public Button restartButton;

    public DialogueBox dialogueBox;
    public DialogueScript dialogueScript;
    
    public MusicManager musicManager;
    public AudioClip OpeningSong;
    public AudioClip GameplaySong;
    public AudioClip EndingSong;
    
    private int promptsSolved = 0;
    
    void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        restartButton.onClick.AddListener(RestartGame);
        dialogueBox.OnDialogueEnd += StartBombSequence;
        bomb.OnEndPlay += OnEndPlay;
        bomb.prompter.OnPromptSolved += OnPromptSolved;
        
        startScreen.SetActive(true);
        endScreen.SetActive(false);
        bomb.gameObject.SetActive(false);
        dialogueBox.gameObject.SetActive(false);
        
        promptsSolved = 0;
        
        musicManager.PlaySong(OpeningSong);
    }
    
    void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnPlayButtonClick);
    }

    void OnPlayButtonClick()
    {
        startScreen.SetActive(false);
        bomb.gameObject.SetActive(true);
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

    void OnPromptSolved()
    {
        //bomb.RecoverTime();
        bomb.RemoveSpark();
        promptsSolved++;
    }
    
    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
