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

    private Dictionary<string, QuestAlbum> albumById;

    private void Awake()
    {
        albumById = new Dictionary<string, QuestAlbum>(System.StringComparer.OrdinalIgnoreCase);

        for(int i = 0; i < questAlbums.Count; i++)
        {
            var slot = questAlbums[i];
            if (slot == null) continue;
            if (string.IsNullOrWhiteSpace(slot.questID)) continue;

            // 중복 questID 방지(있으면 덮어쓰거나 경고)
            if (albumById.ContainsKey(slot.questID))
            {
                Debug.LogWarning($"[ResultQuestUI] questID 중복: {slot.questID} (뒤에 것이 덮어씁니다)");
            }

            albumById[slot.questID] = slot;
        }
    }

    private void OnEnable()
    {
        List<QuestData> questResults = ResultManager.instance.unlockedQuestList;

        // unlockedQuestList 갯수 체크
        if (questResults == null || questResults.Count == 0)
            return;

        for(int i = 0; i<questResults.Count; i++)
        {
            var quest = questResults[i];
            if(quest == null) continue;

            if (!albumById.TryGetValue(quest.qID, out var slot))
                continue;

            slot.unlockImage.sprite = unlockSprite;
            slot.textName.text = quest.qName;
            slot.textDescription.text = quest.qDescription;
        }

        /*
        // 업적 붙여넣기 세팅 <- ResultManager 결과 리스트로 가져오기
        for (int i = 0; i<questResults.Count; i++) 
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
        */

        // 이미 앨범에 퀘스트는 채웠으니 원본 삭제... 이거 지우면 원본 데이터 날아가서 save 못함!
        // 대신에 이러면 전체를 앨범 열때마다 계속 덮어씌워야함 -> 갯수 적으면 괜찮음
        //questResults.Clear(); 
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
