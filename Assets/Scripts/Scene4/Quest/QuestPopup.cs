using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPopup : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    //[SerializeField] private Image image;
    [SerializeField] private Text text;

    private CanvasGroup cg;
    private Queue<string> messageQueue = new Queue<string>();
    private bool isShowing = false;

    private void Awake()
    {
        cg = popup.GetComponent<CanvasGroup>();
        if(cg == null) cg = popup.AddComponent<CanvasGroup>();
        cg.alpha = 0;
        popup.SetActive(false);
    }
    public void EnablePopup(QuestData quest)
    {
        // 업적 텍스트 삽입
        string msg = $"{quest.qName} 달성!";
        messageQueue.Enqueue(msg); // 동시 업적 달성 처리를 위해 큐에 저장

        if (!isShowing)
        {
            StartCoroutine(ProcessQueue());
        }

    }

    private IEnumerator ProcessQueue()
    {
        isShowing = true;

        while (messageQueue.Count > 0) 
        {
            string msg = messageQueue.Dequeue();

            popup.SetActive(true);
            text.text = msg;
            cg.alpha = 0;

            Sequence seq = DOTween.Sequence();
            seq.Append(cg.DOFade(1f, 0.4f))
               .AppendInterval(1.5f)
               .Append(cg.DOFade(0f, 0.4f))
               .AppendCallback(() => popup.SetActive(false))
               .Play();

            yield return seq.WaitForCompletion(); // 시퀀스 끝날때까지 대기
        }

        isShowing = false;
    }
}
