using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    [SerializeField] private Sprite cutImage1;
    [SerializeField] private Sprite cutImage2;
    [SerializeField] private Sprite cutImage3;
    private void Start()
    {
        StartCoroutine("NextCut");
    }

    private IEnumerator NextCut()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<Image>().sprite = cutImage1;
        yield return new WaitForSeconds(0.9f);
        gameObject.GetComponent<Image>().sprite = cutImage2;
        yield return new WaitForSeconds(0.9f);
        gameObject.GetComponent<Image>().sprite = cutImage3;
        yield return new WaitForSeconds(0.9f);
        gameObject.GetComponent<NextAuto>().enabled = true;
    }
}
