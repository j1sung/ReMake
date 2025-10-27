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

    int index;

    void OnEnable()
    {
        index = ResultManager.instance.CurrentStageInfo - 1;
        Debug.Log("index: " + index);
        
        if (index < 0) return;

        // endingOutcomes 값 null 체크
        if (ResultManager.instance.endingOutcomes.Count == 0) return;

        // === 여기서 부터 채우기 체크 ===
        // 조건 통과하면 씬 4 앨범, 불통은 메인메뉴 & 설정 앨범
        Debug.Log("endingOutcomes.Count: " + ResultManager.instance.endingOutcomes.Count);
        if (index < ResultManager.instance.endingOutcomes.Count)
        {
            if (stageAlbums[index].albumImage[0].sprite == null)
            {
                gameObject.GetComponent<NextClick>().enabled = false;
                StartCoroutine(ResultAlbum());
            }
        }
        else // 메인메뉴 & 설정에서 앨범 열때(instance 유지) -> 해당 스테이지 부분만 새로 채워주면 됨
        {
            // 해당 스테이지 앨범 이미지 & 아이콘 채우기
            var result = ResultManager.instance;
            List<ObjeData> resultIcons = result.endingOutcomes[index-1].objeDatas;
            GameObject icon;

            for (int i = 0; i < result.endingOutcomes[index-1].objeDatas.Count; i++)
            {
                stageAlbums[index-1].albumImage[i].sprite = result.endingOutcomes[index - 1].objeDatas[i].endingImage;

                icon = Instantiate(IconPrefab); // icon 생성
                icon.transform.SetParent(stageAlbums[index - 1].iconRoot[i], false); // icon 위치 설정
                icon.GetComponent<Image>().sprite = resultIcons[i].iconImage;
                icon.GetComponent<Image>().preserveAspect = true;
            }
        }
    }

    private IEnumerator ResultAlbum()
    {
        yield return new WaitForSeconds(1f);

        // 해당 스테이지 앨범 이미지 & 아이콘 채우기
        var result = ResultManager.instance;
        List<ObjeData> resultIcons = result.endingOutcomes[index].objeDatas;
        GameObject icon;

        for (int i=0; i < result.endingOutcomes[index].objeDatas.Count; i++)
        {
            stageAlbums[index].albumImage[i].sprite = result.endingOutcomes[index].objeDatas[i].endingImage;
            
            yield return new WaitForSeconds(0.5f);

            icon = Instantiate(IconPrefab); // icon 생성
            icon.transform.SetParent(stageAlbums[index].iconRoot[i], false); // icon 위치 설정
            icon.GetComponent<Image>().sprite = resultIcons[i].iconImage;
            icon.GetComponent<Image>().preserveAspect = true;

            yield return new WaitForSeconds(0.3f);
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
