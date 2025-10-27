using UnityEngine;
using UnityEngine.EventSystems;

public class ClickNextScene : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string sceneName;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (sceneName == "MainTitle_f")
            GameManager.Instance.MoveScene(SceneData.MainMenu);
        else if (sceneName == "Office_f")
            GameManager.Instance.MoveScene(SceneData.Office);
        else if (sceneName == "Room1_f")
            GameManager.Instance.MoveScene(SceneData.Room);
        else if (sceneName == "JS_Test")
            GameManager.Instance.MoveScene(SceneData.Result);
    }
}
