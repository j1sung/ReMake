using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

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

    public AudioClip pageNext;
    public AudioClip closeBook;

    int index; // 현재 스테이지 정보 인덱스

    void OnEnable()
    {
        index = ResultManager.instance.CurrentStageInfo - 1;
        Debug.Log("index: " + index);
        
        if (index < 0) return;

        // endingOutcomes 갯수 체크
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

            /* 
             나중에 스테이지 늘릴거면 결과는 존재하는데 안채워진 부분 있으면 한번에 다 채우는 식으로 바꿔야함
             지금은 앨범을 엔딩보고 무조건 한번씩 들어가서 저장하는거로 생각했는데 만약 2번 엔딩을 연속으로 보고 
             앨범으로 돌아오면 중간꺼는 비어있을거임 
            */
            for (int i = 0; i < result.endingOutcomes[index-1].objeDatas.Count; i++)
            {
                stageAlbums[index-1].albumImage[i].sprite = result.endingOutcomes[index - 1].objeDatas[i].endingImage;
                stageAlbums[index-1].albumImage[i].color = Color.white;

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
            stageAlbums[index].albumImage[i].color = new Color(1,1,1,0);
            stageAlbums[index].albumImage[i].DOFade(1f, 1.2f);

            yield return new WaitForSeconds(0.2f);

            icon = Instantiate(IconPrefab); // icon 생성
            icon.transform.SetParent(stageAlbums[index].iconRoot[i], false); // icon 위치 설정
            icon.GetComponent<Image>().sprite = resultIcons[i].iconImage;
            icon.GetComponent<Image>().preserveAspect = true;

            yield return new WaitForSeconds(0.5f);
        }

        gameObject.GetComponent<NextClick>().enabled = true;
    }

    public void CloseButton()
    {   
        SFXPlayer.Instance.PlaySFX(closeBook);
        album.SetActive(false);
        gameObject.SetActive(false);
    }

    public void QuestButton()
    {
        SFXPlayer.Instance.PlaySFX(pageNext);
        questUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
