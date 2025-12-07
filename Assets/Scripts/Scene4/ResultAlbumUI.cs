using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;
using System.Linq;
using System.Data;

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

    public GameObject nextBtn; // 다음페이지 버튼
    public GameObject prevBtn; // 이전 페이지 버튼

    public List<StageAlbum> stageAlbums; // 채우는 스테이지 앨범 리스트
    public GameObject IconPrefab; // 제출 아이콘 프리펩

    public AudioClip pageNext;
    public AudioClip closeBook;
    public AudioClip photoShot;

    private int index; // 현재 스테이지 정보 인덱스

    private int currentPage = 1; // 앨범 현재 페이지
    private int previousPage = 1; // 이전 페이지(비교)

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
            // 처음 앨범 열 때도 첫 페이지를 표시하도록
            FillAlbumPage();
            
        }
    }

    private IEnumerator ResultAlbum()
    {
        yield return new WaitForSeconds(1f);

        // 해당 스테이지 앨범 사진 & 아이콘 채우기
        var result = ResultManager.instance;
        List<ObjeData> resultIcons = result.endingOutcomes[index].objeDatas;
        
        for (int i=0; i < result.endingOutcomes[index].objeDatas.Count; i++)
        {   
            // 4개 초과할 때 마다
            if(i>0 && i%4 == 0)
            {
                for(int j=0; j < stageAlbums[index].albumImage.Length; j++)
                {
                    // 앨범 사진 초기화
                    stageAlbums[index].albumImage[j].color = new Color32(239, 229, 219, 255);
                    stageAlbums[index].albumImage[j].sprite = null;

                    // 아이콘 프리펩 비활성화
                    if (stageAlbums[index].iconRoot[j].childCount > 0)
                        stageAlbums[index].iconRoot[j].GetChild(0).gameObject.SetActive(false);
                }
            }
            SFXPlayer.Instance.PlaySFX(photoShot);

            // 앨범 사진 페이드 인
            var img = stageAlbums[index].albumImage[i&3];
            img.sprite = result.endingOutcomes[index].objeDatas[i].endingImage;
            img.color = new Color(1,1,1,0);
            img.DOFade(1f, 1.2f);

            yield return new WaitForSeconds(0.2f);

            // 아이콘 재활용 or 생성
            Transform iconRoot = stageAlbums[index].iconRoot[i & 3];
            GameObject icon;

            if (iconRoot.childCount > 0)
            {
                // 이미 프리펩이 존재하면 활성화
                icon = iconRoot.GetChild(0).gameObject;
                icon.SetActive(true);
            }
            else
            {
                // 없으면 새로 생성
                icon = Instantiate(IconPrefab, iconRoot, false); // icon 생성
            }
            
            icon.GetComponent<Image>().sprite = resultIcons[i].iconImage;
            icon.GetComponent<Image>().preserveAspect = true;

            yield return new WaitForSeconds(0.5f);
        }

        gameObject.GetComponent<NextClick>().enabled = true;
    }

    private void FillAlbumPage()
    {
        var result = ResultManager.instance;
        if (index <= 0 || index > result.endingOutcomes.Count) return;

        List<ObjeData> resultIcons = result.endingOutcomes[index - 1].objeDatas;
        GameObject icon;

        // 페이지 변경 시 기존 내용 초기화
        if (currentPage != previousPage)
        {
            for (int i = 0; i < stageAlbums[index - 1].albumImage.Length; i++)
            {
                // 앨범 사진 초기화
                stageAlbums[index - 1].albumImage[i].color = new Color32(239, 229, 219, 255);
                stageAlbums[index - 1].albumImage[i].sprite = null;
                

                // 아이콘 프리펩 비활성화
                if (stageAlbums[index - 1].iconRoot[i].childCount > 0)
                    stageAlbums[index - 1].iconRoot[i].GetChild(0).gameObject.SetActive(false);
            }
        }
        /* 
         나중에 스테이지 늘릴거면 결과는 존재하는데 안채워진 부분 있으면 한번에 다 채우는 식으로 바꿔야함
         지금은 앨범을 엔딩보고 무조건 한번씩 들어가서 저장하는거로 생각했는데 만약 2번 엔딩을 연속으로 보고 
         앨범으로 돌아오면 중간꺼는 비어있을거임 
        */

        // 우선 결과가 총 몇개인지 파악하고, 앨범 최대갯수인 4개가 넘는다면,
        // currentPage 1은 0,1,2,3 / 2는 4,5,6,7 이런식으로 인덱스를 시작해서 앨범이 가지고 있는 갯수만큼 채우도록 설정
        for (int i = (currentPage - 1) * 4; i < stageAlbums[index - 1].albumImage.Length * currentPage; i++)
        {
            if (i >= result.endingOutcomes[index - 1].objeDatas.Count) break; // 결과 오브제 갯수를 넘지않게 확인

            // 앨범 사진 채우기
            var img = stageAlbums[index - 1].albumImage[i & 3];
            img.sprite = result.endingOutcomes[index - 1].objeDatas[i].endingImage;
            img.color = Color.white;

            Transform iconRoot = stageAlbums[index - 1].iconRoot[i & 3];
            if (iconRoot.childCount > 0)
            {
                // 이미 프리펩이 존재하면 활성화
                icon = iconRoot.GetChild(0).gameObject;
                icon.SetActive(true);
            }
            else
            {
                // 없으면 새로 생성
                icon = Instantiate(IconPrefab, iconRoot, false); // icon 생성
            }
            icon.GetComponent<Image>().sprite = resultIcons[i].iconImage;
            icon.GetComponent<Image>().preserveAspect = true;
        }
        previousPage = currentPage;
    }

    public void ResetAlbumUI()
    {
        foreach (var stage in stageAlbums)
        {
            // 각 스테이지 앨범 이미지 초기화
            foreach (var img in stage.albumImage)
            {
                img.sprite = null;
                img.color = new Color32(239, 229, 219, 255);
            }

            // 아이콘 프리팹 비활성화
            foreach (var root in stage.iconRoot)
            {
                if (root.childCount > 0)
                    root.GetChild(0).gameObject.SetActive(false);
            }
        }

        // 페이지 리셋
        currentPage = 1;
        previousPage = 1;
    }

    public void OnNextPageButton()
    {
        currentPage++;
        Debug.Log(currentPage);
        FillAlbumPage();
        SFXPlayer.Instance.PlaySFX(pageNext);
        prevBtn.SetActive(true);
        nextBtn.SetActive(false);
    }

    public void OnPrevPageButton()
    {
        currentPage = Mathf.Max(1, currentPage - 1);
        Debug.Log(currentPage);
        FillAlbumPage();
        SFXPlayer.Instance.PlaySFX(pageNext);
        nextBtn.SetActive(true);
        prevBtn.SetActive(false);
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
