using UnityEngine;

public class SwapOnVisible : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;
    
    private void OnEnable()
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }
    }
    private void OnDisable()
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(true);
        }
    }
}
