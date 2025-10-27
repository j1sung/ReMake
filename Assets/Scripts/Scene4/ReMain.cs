using UnityEngine;
using UnityEngine.EventSystems;

public class ReMain : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // ���� �ʱ�ȭ ����
        // ...
        // ...

        GameManager.Instance.MoveScene(SceneData.MainMenu);
    }
}
