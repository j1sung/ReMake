using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TypingEffect typing;
    [SerializeField] private string[] lines;
    [SerializeField] GameObject SpeechBubble;

    int index = 0;
    bool canClick = false;

    void Update()
    {
        if (!canClick) return;

        if (Input.GetMouseButtonDown(0))
            Next();
    }

    void EnableClick()
    {
        canClick = true;
    }

    public void PlayCurrent()
    {
        canClick = false;
        typing.Type(lines[index], EnableClick); // 출력 종료시, EnableClick 호출
    }

    void Next()
    {
        index++;

        if (index < lines.Length)
            PlayCurrent();
        else
            EndDialogue();
    }

    // 통화 종료 후 수행
    void EndDialogue()
    {   
        SpeechBubble.SetActive(false);
        OfficeStateMachine.SetState(OfficeState.BeforeInteracts);
    }
}