using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Slide_Collection : MonoBehaviour
{
    private readonly List<ObjeData> _objeList = new List<ObjeData>();
    private int _currentIndex = 0;

    [Header("UI")]
    [SerializeField] private Text ObjeNameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Image ObjeImage;

    void OnEnable()
    {
        if (_objeList.Count > 0)
        {
            _currentIndex = Mathf.Clamp(_currentIndex, 0, _objeList.Count - 1);
            ShowCurrent();
        }
        else
        {
            ClearUI();
        }
    }

    public void AddObje(ClickObje obje)
    {
        if (obje.data == null) return;
        if (_objeList.Contains(obje.data)) return;
        _objeList.Add(obje.data);
    }

    public void Next()
    {
        if (_objeList.Count == 0) return;

        _currentIndex = (_currentIndex + 1) % _objeList.Count;
        ShowCurrent();
    }

    public void Prev()
    {
        if (_objeList.Count == 0) return;

        _currentIndex = (_currentIndex - 1 + _objeList.Count) % _objeList.Count;
        ShowCurrent();
    }

    void ShowCurrent()
    {
        ObjeData data = _objeList[_currentIndex];

        ObjeImage.enabled = true;

        ObjeNameText.text = data.objeName;
        descriptionText.text = data.objeDescription;
        ObjeImage.sprite = data.puzzleImage;
    }

    void ClearUI()
    {
        ObjeNameText.text = string.Empty;
        descriptionText.text = string.Empty;

        ObjeImage.sprite = null;
        ObjeImage.enabled = false;
    }

    public void OnClickCollectionImage()
    {
        var data = _objeList[_currentIndex];

        // case 1: 시크릿 설명 + 이미지가 있는 경우 (2단 순환)
        if (data.secretImage != null && !string.IsNullOrEmpty(data.secretDescription))
        {
            if (ObjeImage.sprite == data.puzzleImage)
            {
                ObjeImage.sprite = data.secretImage;
                descriptionText.text = data.secretDescription;
            }
            else
            {
                ObjeImage.sprite = data.puzzleImage;
                descriptionText.text = data.objeDescription;
            }
            return;
        }

        // case 2: thirdiconImage만 있는 경우 (이미지 3단 순환)
        if (data.secondiconImage != null)
        {
            if (ObjeImage.sprite == data.secondiconImage)
                ObjeImage.sprite = data.thirdiconImage;
            else if (ObjeImage.sprite == data.thirdiconImage)
                ObjeImage.sprite = data.puzzleImage;
            else
                ObjeImage.sprite = data.secondiconImage;
        }
    }
}