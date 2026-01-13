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

    public StageAlbum stageAlbum; // 채우는 스테이지 앨범 리스트
    public GameObject IconPrefab; // 제출 아이콘 프리펩

    public AudioClip pageNext;
    public AudioClip closeBook;
    public AudioClip photoShot;

    private int index; // 현재 스테이지 정보 인덱스

    [SerializeField] private int[] stageEndPages = { 2, 5 }; // 나중에 스테이지 추가되면 더 큰 수로 항목 추가
    private const int maxPage = 10;
    private const int minPage = 1;
    private int currentPage = 1; // 앨범 현재 페이지
    private int previousPage = 1; // 이전 페이지(비교)

    void OnEnable()
    {
        index = ResultManager.instance.CurrentStageInfo - 1;
        Debug.Log("index: " + index);
        
        if (index < 0) return;

        // endingOutcomes 갯수 체크
        if (ResultManager.instance.endingResult.Count == 0) return;

        // === 여기서 부터 채우기 체크 ===
        // 조건 통과하면 결과씬 앨범, 불통은 메인메뉴 & 설정 앨범
        Debug.Log("endingOutcomes.Count: " + ResultManager.instance.endingResult.Count);
        Debug.Log(SceneManager.GetActiveScene().name);

        if (SceneManager.GetActiveScene().name == SceneData.Result.ToString())
        {
            if (stageAlbum.albumImage[0].sprite == null)
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
        List<ObjeData> resultIcons = result.Objes;

        for (int i=0; i < resultIcons.Count; i++)
        {   
            // 4개 초과할 때 마다
            if(i>0 && i%4 == 0)
            {
                for(int j=0; j < stageAlbum.albumImage.Length; j++)
                {
                    // 앨범 사진 초기화
                    stageAlbum.albumImage[j].color = new Color32(239, 229, 219, 0);
                    stageAlbum.albumImage[j].sprite = null;

                    // 아이콘 프리펩 비활성화
                    if (stageAlbum.iconRoot[j].childCount > 0)
                        stageAlbum.iconRoot[j].GetChild(0).gameObject.SetActive(false);
                }
            }
            SFXPlayer.Instance.PlaySFX(photoShot);

            // 앨범 사진 페이드 인
            var img = stageAlbum.albumImage[i&3];
            img.sprite = resultIcons[i].endingImage;
            img.color = Color.white;
            img.DOFade(1f, 1.2f);

            yield return new WaitForSeconds(0.2f);

            // 아이콘 재활용 or 생성
            Transform iconRoot = stageAlbum.iconRoot[i & 3];
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
        int stageIndex = GetStageIndexByPage(currentPage);
        int stageIdx0 = stageIndex - 1;
        Debug.Log($"currentPage: {currentPage}");
        Debug.Log($"stageIdx0: {stageIdx0}");

        // 페이지 변경 시 기존 내용 초기화
        if (currentPage != previousPage)
        {
            for (int i = 0; i < stageAlbum.albumImage.Length; i++)
            {
                // 앨범 사진 초기화
                stageAlbum.albumImage[i].color = new Color32(239, 229, 219, 0);
                stageAlbum.albumImage[i].sprite = null;


                // 아이콘 프리펩 비활성화
                if (stageAlbum.iconRoot[i].childCount > 0)
                    stageAlbum.iconRoot[i].GetChild(0).gameObject.SetActive(false);
            }
        }

        if (index > result.endingResult.Count) return;
        if (stageIdx0 < 0 || stageIdx0 >= result.endingObjes.Count) return;

        List<ObjeData> resultIcons = result.endingObjes[stageIdx0].objs;
        GameObject icon;

        int startPage = (stageIdx0 == 0) ? 1 : stageEndPages[stageIdx0 - 1] + 1;
        int baseGlobalI = (startPage - 1) * 4;

        // 우선 결과가 총 몇개인지 파악하고, 앨범 최대갯수인 4개가 넘는다면,
        // currentPage 1은 0,1,2,3 / 2는 4,5,6,7 이런식으로 인덱스를 시작해서 앨범이 가지고 있는 갯수만큼 채우도록 설정
        for (int i = (currentPage - 1) * 4; i < stageAlbum.albumImage.Length * currentPage; i++)
        {
            int localI = i - baseGlobalI;

            if (localI < 0) continue;
            if (localI >= resultIcons.Count) break; // 결과 오브제 갯수를 넘지않게 확인

            // 앨범 사진 채우기
            var img = stageAlbum.albumImage[localI & 3];
            img.sprite = resultIcons[localI].endingImage;
            img.color = Color.white;

            Transform iconRoot = stageAlbum.iconRoot[localI & 3];
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
            icon.GetComponent<Image>().sprite = resultIcons[localI].iconImage;
            icon.GetComponent<Image>().preserveAspect = true;
        }
        previousPage = currentPage;
    }

    private int GetStageIndexByPage(int page)
    {
        for (int i = 0; i < stageEndPages.Length; i++)
        {
            if (page <= stageEndPages[i])
                return i + 1;
        } 
        return stageEndPages.Length; // 넘어가면 마지막 스테이지로 처리(혹은 -1)
    }


    public void ResetAlbumUI()
    {

        // 각 스테이지 앨범 이미지 초기화
        foreach (var img in stageAlbum.albumImage)
        {
            img.sprite = null;
            img.color = new Color32(239, 229, 219, 0);
        }

        // 아이콘 프리팹 비활성화
        foreach (var root in stageAlbum.iconRoot)
        {
            if (root.childCount > 0)
                root.GetChild(0).gameObject.SetActive(false);
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
        if(currentPage >= maxPage)
            nextBtn.SetActive(false);
        else
            nextBtn.SetActive(true);
    }

    public void OnPrevPageButton()
    {
        currentPage = Mathf.Max(1, currentPage - 1);
        Debug.Log(currentPage);
        FillAlbumPage();
        SFXPlayer.Instance.PlaySFX(pageNext);
        nextBtn.SetActive(true);
        if(currentPage <= minPage)
            prevBtn.SetActive(false);
        else
            prevBtn.SetActive(true);
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
