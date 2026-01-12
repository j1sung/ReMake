using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ResultCommentUI : MonoBehaviour
{
    public Text comment;
    [SerializeField] private float textSpeed = 1f;

    private NextClick nextClick;

    void OnEnable()
    {
        var result = ResultManager.instance.endingResult;

        if(nextClick == null)
            nextClick = GetComponent<NextClick>();
        if(nextClick != null)
            nextClick.enabled = false;

        comment.text = null;
        comment.DOText(result[ResultManager.instance.CurrentStageInfo - 1].comment, textSpeed)
            .OnComplete(() =>
            {
                if (nextClick != null)
                    nextClick.enabled = true;
            });
    }
}
