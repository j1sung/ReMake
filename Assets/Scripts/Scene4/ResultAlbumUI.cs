using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

[System.Serializable]
public class StageAlbum
{
    public int stageNumber; // 현재 스테이지 단계
    public Image[] albumImage; // 채워지는 앨범 이미지 슬롯
    public Transform[] iconRoot; // 아이콘 생성 부모 위치
}

public class ResultAlbumUI : MonoBehaviour
{
    // 이 두개는 메인 씬에서만 씀
    public GameObject album; // 상위 앨범 오브젝트
    public GameObject questUI; // 업적 앨범 UI

    public List<StageAlbum> stageAlbums; // 채우는 스테이지 앨범 리스트
    public GameObject IconPrefab; // 제출 아이콘 프리펩

    void OnEnable()
    {
        int index = ResultManager.instance.CurrentStageInfo - 1;
        Debug.Log("index: " + index);
        if (index < 0) return;

        Debug.Log("endingOutcomes.Count: " + ResultManager.instance.endingOutcomes.Count);
        // endingOutcomes 범위 체크
        if (index >= ResultManager.instance.endingOutcomes.Count) return;

        Debug.Log("endingOutcomes[index]: " + ResultManager.instance.endingOutcomes[index].endingId);
        // endingOutcomes 값 null 체크
        if (ResultManager.instance.endingOutcomes[index] == null) return;

        // 해당 스테이지 사진창은 비어있는데 결과 리스트에는 항목을 가지고 있다면 => 결과 제출 직후
        if (stageAlbums[index].albumImage[0].sprite == null)
        {
            gameObject.GetComponent<NextClick>().enabled = false;
            StartCoroutine(ResultAlbum());
        }
    }

    private IEnumerator ResultAlbum()
    {
        yield return new WaitForSeconds(1f);

        // 해당 스테이지 앨범 이미지 & 아이콘 채우기
        var result = ResultManager.instance;
        List<ObjeData> resultIcons = result.endingOutcomes[result.CurrentStageInfo - 1].objeDatas;
        GameObject icon;

        for (int i=0; i < result.endingOutcomes[result.CurrentStageInfo-1].objeDatas.Count; i++)
        {
            stageAlbums[result.CurrentStageInfo - 1].albumImage[i].sprite = result.endingOutcomes[result.CurrentStageInfo - 1].objeDatas[i].endingImage;
            yield return new WaitForSeconds(1f);
        }

        
        
        for (int i = 0; i < result.endingOutcomes[result.CurrentStageInfo - 1].objeDatas.Count; i++)
        {
            icon = Instantiate(IconPrefab); // icon 생성
            icon.transform.SetParent(stageAlbums[result.CurrentStageInfo - 1].iconRoot[i], false); // icon 위치 설정
            icon.GetComponent<Image>().sprite = resultIcons[i].iconImage;
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
