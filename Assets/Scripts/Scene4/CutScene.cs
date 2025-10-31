using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CutScene : MonoBehaviour
{
    [SerializeField] private Sprite cutImage1;
    [SerializeField] private Sprite cutImage2;
    [SerializeField] private Sprite cutImage3;

    Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    private void Start()
    {
        StartCoroutine("NextCut");
    }

    private IEnumerator NextCut()
    {
        yield return new WaitForSeconds(0.3f);
        //image.color = new Color(1, 1, 1, 0);
        image.sprite = cutImage1;
        //image.DOFade(1f, 0.9f);

        yield return new WaitForSeconds(0.9f);
        //image.color = new Color(1, 1, 1, 0);
        image.sprite = cutImage2;
        //image.DOFade(1f, 0.9f);

        yield return new WaitForSeconds(0.9f);
        //image.color = new Color(1, 1, 1, 0);
        image.sprite = cutImage3;
        //image.DOFade(1f, 0.9f);

        yield return new WaitForSeconds(0.9f);
        gameObject.GetComponent<NextAuto>().enabled = true;
    }
}
