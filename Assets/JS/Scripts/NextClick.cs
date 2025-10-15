using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NextClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject go;

    // IPointerClickHandler 인터페이스의 필수 함수
    public void OnPointerClick(PointerEventData eventData)
    {
        go.SetActive(true);
        gameObject.SetActive(false);
        
    }
}
