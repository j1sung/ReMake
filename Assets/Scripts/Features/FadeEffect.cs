using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum FadeState { FadeIn = 0, FadeOut, FadeInOut, FadeLoop }
public class FadeEffect : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime;
    [SerializeField]
    private AnimationCurve fadeCurve;
    private Image image;
    private FadeState fadeState;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void OnFade(FadeState state)
    {
        fadeState = state;

        switch (fadeState) 
        {
            case FadeState.FadeIn:
                StartCoroutine(Fade(1, 0));
                break;
            case FadeState.FadeOut:
                StartCoroutine(Fade(0, 1));
                break;
            case FadeState.FadeInOut:
            case FadeState.FadeLoop:
                StartCoroutine(FadeInOut());
                break;
            default:
                break;
        }
    }

    private IEnumerator FadeInOut()
    {
        while (true)
        {
            yield return Fade(1, 0);
            yield return Fade(0, 1);

            if(fadeState == FadeState.FadeInOut)
            {
                break;
            }
        }
    }
    
    private IEnumerator Fade(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while(percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime; // fadeTime만큼 fade 시간 설정함

            Color color = image.color;
            // color.a = Mathf.Lerp(start, end, percent);
            color.a = Mathf.Lerp(start, end, fadeCurve.Evaluate(percent));
            image.color = color;

            yield return null;
        }
    }
}
