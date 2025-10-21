using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class ResultAlbumUI : MonoBehaviour
{
    // 이 두개는 메인 씬에서만 씀
    public GameObject album; // 상위 앨범 오브젝트
    public GameObject questUI; // 업적 앨범 UI

    public Image[] albumImage; // 앨범 이미지 목록
    public GameObject IconPrefab; // 제출 아이콘 프리펩
    [SerializeField] private Transform[] IconRoot; // 아이콘 생성 부모 위치

    void OnEnable()
    {
        int index = ResultManager.instance.CurrentStageInfo - 1;
        Debug.Log("index: " + index);
        if (index < 0) return;

        Debug.Log("albumImage.Length: " + albumImage.Length);
        // albumImage 범위 체크
        if (index >= albumImage.Length) return;

        Debug.Log("endingOutcomes.Count: " + ResultManager.instance.endingOutcomes.Count);
        // endingOutcomes 범위 체크
        if (index >= ResultManager.instance.endingOutcomes.Count) return;

        Debug.Log("endingOutcomes[index]: " + ResultManager.instance.endingOutcomes[index].endingId);
        // endingOutcomes 값 null 체크
        if (ResultManager.instance.endingOutcomes[index] == null) return;

        // 해당 스테이지 사진창은 비어있는데 결과 리스트에는 항목을 가지고 있다면 => 결과 제출 직후
        if (albumImage[index].sprite == null)
        {
            gameObject.GetComponent<NextClick>().enabled = false;
            StartCoroutine(ResultAlbum());
        }
    }

    private IEnumerator ResultAlbum()
    {
        yield return new WaitForSeconds(1.5f);

        // 해당 스테이지 앨범 이미지 채우기
        var result = ResultManager.instance;
        albumImage[result.CurrentStageInfo - 1].sprite = result.endingOutcomes[result.CurrentStageInfo - 1].albumImage;

        yield return new WaitForSeconds(1f);

        // 해당 스테이지 앨범 제출 아이콘 생성
        List<ObjeData> resultIcons = result.endingOutcomes[result.CurrentStageInfo - 1].objeDatas;
        GameObject icon;
        foreach (ObjeData resultIcon in resultIcons)
        {
            icon = Instantiate(IconPrefab); // icon 생성
            icon.transform.SetParent(IconRoot[ResultManager.instance.CurrentStageInfo-1], false); // icon 위치 설정
            icon.GetComponent<Image>().sprite = resultIcon.iconImage;
            icon.GetComponent<Image>().preserveAspect = true;
        }

        gameObject.GetComponent<NextClick>().enabled = true;
    }

    public void CloseButton()
    {
        album.SetActive(false);
        gameObject.SetActive(false);
    }

    public void QuestButton()
    {
        questUI.SetActive(true);
        gameObject.SetActive(false);
    }

    
   
}
