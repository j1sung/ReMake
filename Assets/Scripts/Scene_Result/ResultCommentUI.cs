using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ResultCommentUI : MonoBehaviour
{
    public Text comment;
    [SerializeField] private float textSpeed = 1f;

    void OnEnable()
    {
        var result = ResultManager.instance.endingResult;
        comment.text = null;
        comment.DOText(result[ResultManager.instance.CurrentStageInfo - 1].comment, textSpeed);
    }
}
