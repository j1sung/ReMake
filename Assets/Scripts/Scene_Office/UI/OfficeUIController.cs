using System;
using UnityEngine;

public class OfficeUIController : MonoBehaviour
{
    public static OfficeUIController Instance;
    public enum BlockMode { Transparent, Opaque }

    [SerializeField] private GameObject _transparentBlockPanel;
    [SerializeField] private GameObject _opaqueBlockPanel;

    private GameObject _currentBlockPanel;
    private GameObject _currentText;
    private GameObject _currentImage;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowUI(OfficeUIContext context)
    {
        _currentBlockPanel = context.blockMode == BlockMode.Transparent
            ? _transparentBlockPanel
            : _opaqueBlockPanel;

        _currentBlockPanel.SetActive(true);

        if (context.text != null)
        {
            _currentText = context.text;
            _currentText.SetActive(true);
        }

        if (context.image != null)
        {
            _currentImage = context.image;
            _currentImage.SetActive(true);
        }
    }

    public void HideUI()
    {
        if (_currentBlockPanel != null) _currentBlockPanel.SetActive(false);
        if (_currentText != null) _currentText.SetActive(false);
        if (_currentImage != null) _currentImage.SetActive(false);

        _currentBlockPanel = null;
        _currentText = null;
        _currentImage = null;
    }
}
