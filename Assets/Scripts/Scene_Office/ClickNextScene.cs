using UnityEngine;
using UnityEngine.EventSystems;

public class ClickNextScene : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string sceneName;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (sceneName == "main")
            GameManager.Instance.MoveScene(SceneData.MainMenu);
        else if (sceneName == "office")
            GameManager.Instance.MoveScene(SceneData.Office);
        else if (sceneName == "room")
            GameManager.Instance.MoveScene(SceneData.Stage1);
        else if (sceneName == "result")
            GameManager.Instance.MoveScene(SceneData.Result);
    }
}
