using System.Collections;
using UnityEngine;

public class NextAuto : MonoBehaviour
{
    [SerializeField] private GameObject obje;
    private void OnEnable()
    {
        Invoke("Next", 1.5f);
    }

    void Next()
    {
        obje.SetActive(true);
        gameObject.SetActive(false);
    }
}
