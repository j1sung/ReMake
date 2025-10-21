using UnityEngine;
using UnityEngine.EventSystems;

public class ReMain : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // 정보 초기화 세팅
        // ...
        // ...

        GameManager.instance.GoMain();
    }
}
