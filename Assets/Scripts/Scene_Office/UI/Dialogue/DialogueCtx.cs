using UnityEngine;

public enum Speaker { Senior, K }

[System.Serializable]
public class DialogueCtx
{
    public Speaker speaker;
    [TextArea] public string text;
}
