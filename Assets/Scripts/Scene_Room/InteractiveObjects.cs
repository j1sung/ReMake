using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveObjects : MonoBehaviour
{   
    [SerializeField] GameObject Closed;    // 닫힌 오브젝트
    [SerializeField] GameObject Opened;    // 열린 오브젝트

    [Header("Audios")]
    [SerializeField] private AudioClip openSound; 
    [SerializeField] private AudioClip closeSound; 
    public void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        string objName = gameObject.name;

        if (objName.Contains("Closed", StringComparison.OrdinalIgnoreCase))
        {
            SFXPlayer.Instance.PlaySFX(openSound);
            Closed.SetActive(false);
            Opened.SetActive(true);
        }
        else if (objName.Contains("Opened", StringComparison.OrdinalIgnoreCase))
        {   
            SFXPlayer.Instance.PlaySFX(closeSound);
            Opened.SetActive(false);
            Closed.SetActive(true);
        }
    }
}
