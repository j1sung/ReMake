using UnityEngine;

public class FadeTrigger : MonoBehaviour
{
    [SerializeField]private FadeState fadeState;
    private void Start()
    {
        gameObject.GetComponent<FadeEffect>().OnFade(fadeState);
    }
}
