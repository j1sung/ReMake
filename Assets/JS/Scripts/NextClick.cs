using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NextClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject nextObject;
    public bool isDisableNext = true;
    public GameObject[] extraDisableTargets; // 같이 비활성화 할 오브젝트(선택)

    // IPointerClickHandler 인터페이스의 필수 함수
    public void OnPointerClick(PointerEventData eventData)
    {
        // 다음 오브젝트 활성화
        if(nextObject != null)
            nextObject.SetActive(true);
            
        // 자신 오브젝트 비활성화
        if(isDisableNext)
            gameObject.SetActive(false);
        
        // 추가 타겟 비활성화
        foreach(var target in extraDisableTargets)
        {
            if(target != null)
                target.SetActive(false);
        }
    }
}
