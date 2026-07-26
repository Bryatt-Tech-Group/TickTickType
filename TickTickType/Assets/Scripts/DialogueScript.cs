using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(2, 5)]
    public string text;
    public Mood mood;
}

public enum Mood
{
    Neutral,
    Happy,
    Angry,
    Excited,
    Sleepy,
    Sad
}

[CreateAssetMenu(fileName = "New Dialogue Script", menuName = "Dialogue/Dialogue Script")]
public class DialogueScript : ScriptableObject
{
    public List<DialogueLine> lines = new List<DialogueLine>();
}