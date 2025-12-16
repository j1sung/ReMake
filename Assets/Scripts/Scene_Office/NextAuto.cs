using System.Collections;
using UnityEngine;

public class NextAuto : MonoBehaviour
{
    [SerializeField] private GameObject[] obje;
    [SerializeField] private float waitTime;
    [SerializeField] private bool isDisableNext = true;

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
        if(isDisableNext)
            gameObject.SetActive(false);
    }
}
