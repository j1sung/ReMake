using TMPro;
using UnityEngine;

public class ResultCommentUI : MonoBehaviour
{
    public TMP_Text comment;

    void OnEnable()
    {
        var result = ResultManager.instance.endingOutcomes;
        comment.text = result[ResultManager.instance.CurrentStageInfo - 1].comment;
    }

    
}
