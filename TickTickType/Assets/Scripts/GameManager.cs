using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button myButton;
    public TextMeshProUGUI buttonLabel; // optional, if you want to change text

    public Button exitButton;

    private int clickCount = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myButton.onClick.AddListener(OnButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnDestroy()
    {
        // Good practice: remove listener when object is destroyed
        myButton.onClick.RemoveListener(OnButtonClick);
    }
    
    void OnButtonClick()
    {
        clickCount++;
        Debug.Log("Button clicked! Count: " + clickCount);

        if (buttonLabel != null)
        {
            buttonLabel.text = "Clicked " + clickCount + " times";
        }
    }

    void OnExitButtonClick()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
