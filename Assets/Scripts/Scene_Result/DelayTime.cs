using System.Collections;
using UnityEngine;

public class DelayTime : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(4f);
        GetComponent<ClickNextScene>().enabled = true;
    }
}
