using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResultFileUI : MonoBehaviour, IPointerClickHandler
{
    public Sprite[] files;
    public Sprite[] filesSigned;
    private Image img;
    public TMP_Text text;

    public GameObject stampObj;

    public AudioClip nextPage; // 넘기는 소리

    private int count = 0;
    private bool isProcessing = false; // 코루틴 실행 여부
   
    void Start()
    {
       // 결과 이미지, 점수 세팅
       img = GetComponent<Image>();
       img.sprite = filesSigned[ResultManager.instance.CurrentStageInfo - 1];
        StartCoroutine(ResultFile()); // 결과 표시
    }

    public void OnPointerClick(PointerEventData eventData) 
    {
        if (isProcessing)
            return; // 코루틴 도중이면 클릭 무시
        
        count++;

        if(count == 1) // 첫번째 클릭 -> 다음 스테이지 암시 이미지
        {
            stampObj.SetActive(false);
            SFXPlayer.Instance.PlaySFX(nextPage);
            img.color = new Color(1, 1, 1, 0);
            img.sprite = files[ResultManager.instance.CurrentStageInfo]; // 다음 스테이지 암시 표시
            img.DOFade(1f, 2f);
            ResultManager.instance.SetNextStage(); // 스테이지 값 증가
        }
        else if(count == 2) // 두번째 클릭 -> 다음 오브젝트 활성화
        {
            gameObject.GetComponent<NextClick>().enabled = true;
            this.enabled = false;
            gameObject.GetComponent<NextClick>().OnPointerClick(eventData);
        }
    }

    private IEnumerator ResultFile()
    {
        isProcessing = true;

        yield return new WaitForSeconds(1f);

        stampObj.SetActive(true);
        
        isProcessing = false;
    }
}
