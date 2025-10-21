using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResultFileUI : MonoBehaviour, IPointerClickHandler
{
    public Sprite[] fileImages;
    private Image img;
    public TMP_Text text;

    public GameObject stampObj;
    public GameObject scoreObj;

    private int count = 0;
    private bool isProcessing = false; // 코루틴 실행 여부
   
    void Start()
    {
       // 결과 이미지, 점수 세팅
       img = GetComponent<Image>();
       img.sprite = fileImages[ResultManager.instance.CurrentStageInfo - 1];
    }

    public void OnPointerClick(PointerEventData eventData) 
    {
        if (isProcessing)
            return; // 코루틴 도중이면 클릭 무시
        
        count++;

        if (count == 1) // 첫번째 클릭
        {
            StartCoroutine(ResultFile()); // 결과 표시
        }
        else if(count == 2) // 두번째 클릭 -> 다음 스테이지 암시 이미지
        {
            stampObj.SetActive(false);
            scoreObj.SetActive(false);
            ResultManager.instance.SetNextStage(); // 스테이지 값 증가
            img.sprite = fileImages[ResultManager.instance.CurrentStageInfo - 1]; // 다음 스테이지 암시 표시
        }
        else if(count == 3) // 세번째 클릭 -> 다음 오브젝트 활성화
        {
            gameObject.GetComponent<NextClick>().enabled = true;
            this.enabled = false;
            gameObject.GetComponent<NextClick>().OnPointerClick(eventData);
        }
    }

    private IEnumerator ResultFile()
    {
        isProcessing = true;

        stampObj.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        scoreObj.SetActive(true);
        text.text = ResultManager.instance.endingOutcomes[ResultManager.instance.CurrentStageInfo - 1].score.ToString();

        isProcessing = false;
    }
}
