using UnityEngine;

public class InteractiveOjbects : MonoBehaviour
{   
    [SerializeField] GameObject Closed;    // 닫힌 오브젝트
    [SerializeField] GameObject Opened;    // 열린 오브젝트
    public void OnMouseDown()
    {
        Closed.SetActive(false);
        Opened.SetActive(true);
    }
}
