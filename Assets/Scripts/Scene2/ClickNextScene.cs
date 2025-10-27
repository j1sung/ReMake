using UnityEngine;
using UnityEngine.EventSystems;

public class ClickNextScene : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string sceneName;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (sceneName == "MainTitle_f")
            GameManager.instance.GoMain();
        else if (sceneName == "Office_f")
            GameManager.instance.GoOffice();
        else if (sceneName == "Room1_f")
            GameManager.instance.GoRoom();
        else if (sceneName == "JS_Test")
            GameManager.instance.GoResult();
    }
}
