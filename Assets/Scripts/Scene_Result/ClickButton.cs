using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite pressedSprite;

    public void OnPointerDown(PointerEventData eventData)
    {
        targetImage.sprite = pressedSprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetImage.sprite = normalSprite;
    }
}
