using System.Collections;
using UnityEngine;

public class NextAuto : MonoBehaviour
{
    [SerializeField] private GameObject[] obje;
    [SerializeField] private float waitTime;

    private void OnEnable()
    {
        Invoke("Next", waitTime);
    }

    void Next()
    {
        foreach (var obj in obje) 
        {
            obj.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
