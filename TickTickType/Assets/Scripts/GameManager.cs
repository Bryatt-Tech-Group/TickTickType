using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject endScreen;
    public Bomb bomb;
    public Button playButton;
    
    void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        bomb.OnEndPlay += OnEndPlay;
        
        startScreen.SetActive(true);
        endScreen.SetActive(false);
        bomb.gameObject.SetActive(true);
    }
    
    void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnPlayButtonClick);
    }

    void OnPlayButtonClick()
    {
        startScreen.SetActive(false);
        bomb.BeginPlay();
    }

    void OnEndPlay()
    {
        bomb.gameObject.SetActive(false);
        endScreen.SetActive(true);
    }
}
