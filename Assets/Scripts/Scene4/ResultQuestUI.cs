using UnityEngine;

public class ResultQuestUI : MonoBehaviour
{
    public GameObject album; // 상위 앨범 오브젝트
    public GameObject resultUI; // 결과 앨범 UI
    private void OnEnable()
    {
        // 업적 붙여넣기 세팅 <- ResultManager 결과 리스트로 가져오기
    }
    public void CloseButton()
    {
        album.SetActive(false);
        gameObject.SetActive(false);
    }
    public void ResultButton()
    {
        resultUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
