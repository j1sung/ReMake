using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class QuestAlbum
{
    public string questID; // 퀘스트 ID
    public Image unlockImage; // 채워지는 앨범 이미지 슬롯
    public TMP_Text textName; // 이름 텍스트 위치
    public TMP_Text textDescription; // 설명 텍스트 위치
}

public class ResultQuestUI : MonoBehaviour
{
    public GameObject album; // 상위 앨범 오브젝트
    public GameObject resultUI; // 결과 앨범 UI
    public Sprite unlockSprite; // unlock 이미지
    public Sprite lockSprite; // lock 이미지

    public AudioClip pageNext;
    public AudioClip closeBook;

    public List<QuestAlbum> questAlbums;

    private void OnEnable()
    {
        var questResults = ResultManager.instance.unlockedQuestList;

        // unlockedQuestList 갯수 체크
        if (questResults.Count == 0) return;

        // 업적 붙여넣기 세팅 <- ResultManager 결과 리스트로 가져오기
        for(int i = 0; i<questResults.Count; i++) 
        {
            for(int j = 0; j < questAlbums.Count; j++)
            {
                if (questResults[i].qID == questAlbums[j].questID)
                {
                    questAlbums[j].unlockImage.sprite = unlockSprite;
                    questAlbums[j].textName.text = questResults[i].qName;
                    questAlbums[j].textDescription.text = questResults[i].qDescription;
                }
            }
        }

        // 이미 앨범에 채운 퀘스트는 변형됐으므로 저장소 비우기 -> 대신 또 같은걸 채울 수 있으므로 채울때 검증 필요
        questResults.Clear();
    }
    public void ResetQuestUI()
    {
        foreach (var quest in questAlbums)
        {
            quest.unlockImage.sprite = lockSprite;
            quest.textName.text = "???";
            quest.textDescription.text = "미션 내용 잠김";
        }
    }
    public void CloseButton()
    {   
        SFXPlayer.Instance.PlaySFX(closeBook);
        album.SetActive(false);
        gameObject.SetActive(false);
    }
    public void ResultButton()
    {
        SFXPlayer.Instance.PlaySFX(pageNext);
        resultUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
