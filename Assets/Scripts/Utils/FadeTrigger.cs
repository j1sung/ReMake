using UnityEngine;

public class FadeTrigger : MonoBehaviour
{
    [SerializeField]private FadeState fadeState;
    private void OnEnable()
    {
        gameObject.GetComponent<FadeEffect>().OnFade(fadeState);
    }
}
