using UnityEngine;
using UnityEngine.UI;

public class Slide_Inspect : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text descText;
    [SerializeField] private Image image;
    [SerializeField] private Button interactButton;

    private ClickObje _currentObje;

    public void Show(ClickObje obje)
    {
        _currentObje = obje;

        nameText.text = obje.data.objeName;
        descText.text = obje.data.objeDescription;
        image.sprite = obje.data.puzzleImage;

        interactButton.gameObject.SetActive(!obje.IsInteracted);
    }

    public void OnClickInspectImage()
    {
        var result = _currentObje.Interact();

        // 항상 재생되는 사운드
        if (_currentObje.data.alwaysSound != null)
            SFXPlayer.Instance.PlaySFX(_currentObje.data.alwaysSound);

        switch (result)
        {
            case ObjeInteractResult.CanInteract:
                ShowSecret();
                break;

            case ObjeInteractResult.CycleImage:
                CycleImage();
                break;
        }
    }

    void ShowSecret()
    {
        interactButton.gameObject.SetActive(true);
        _currentObje.IsInteracted = false;

        var data = _currentObje.data;

        if (data.secretSound != null)
            SFXPlayer.Instance.PlaySFX(data.secretSound);

        descText.text = data.secretDescription;
        image.sprite = data.secretImage;
    }

    void CycleImage()
    {
        var img = image;
        var data = _currentObje.data;

        if (img.sprite == data.secondiconImage)
            img.sprite = data.thirdiconImage;
        else if (img.sprite == data.thirdiconImage)
            img.sprite = data.puzzleImage;
        else
            img.sprite = data.secondiconImage;
    }
}
