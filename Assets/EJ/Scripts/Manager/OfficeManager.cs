using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OfficeManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject handoutPanel;
    public GameObject idCardPanel;
    public GameObject requestPanel;

    [Header("Scene Image")]
    public GameObject beforeInteractionOffice;
    public GameObject afterInteractionOffice;
    [SerializeField] private GameObject handout;
    [SerializeField] private GameObject idcard;

    [Header("Audios")]
    [SerializeField] private AudioClip requestArrive; // 유인물 도착 소리
    [SerializeField] private AudioClip paper;  // 종이 소리

    bool interactedHandout = false;
    bool interactedIdcard = false;

    void Start()
    {
        handoutPanel.SetActive(false);
        idCardPanel.SetActive(false);
        requestPanel.SetActive(false);

        beforeInteractionOffice.SetActive(true);
        afterInteractionOffice.SetActive(false);
    }

    // 유인물 클릭
    public void OnClickHandout()
    {   
        SFXPlayer.Instance.PlaySFX(paper);
        handoutPanel.SetActive(true);
        interactedHandout = true;
        CheckRequestUnlock();
    }

    // ID 카드 클릭
    public void OnClickIdcard()
    {
        idCardPanel.SetActive(true);
        interactedIdcard = true;
        CheckRequestUnlock();
    }

    public void CheckRequestUnlock()
    {
        if (interactedHandout && interactedIdcard)
        {
            SFXPlayer.Instance.PlaySFX(requestArrive);
            beforeInteractionOffice.SetActive(false);
            afterInteractionOffice.SetActive(true);
        }
    }

    // 소포 클릭
    public void OnClickRequest()
    {   
        afterInteractionOffice.SetActive(false);
        handout.SetActive(false);
        idcard.SetActive(false);
        requestPanel.SetActive(true);
    }

    // 패널 닫기 버튼
    public void ClosePanel(BaseEventData data)
    {
        // 클릭된 오브젝트 찾기
        PointerEventData ped = data as PointerEventData;
        if (ped != null)
        {
            GameObject clicked = ped.pointerClick;
            clicked.SetActive(false);
        }
    }
}
