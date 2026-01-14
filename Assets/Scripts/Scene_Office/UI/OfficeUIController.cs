using UnityEngine;
using TMPro;

public class OfficeUIController : MonoBehaviour
{
    public static OfficeUIController Instance;
    public enum BlockMode { Transparent, Opaque }

    [SerializeField] private GameObject _transparentBlockPanel;
    [SerializeField] private GameObject _opaqueBlockPanel;

    [Header("Fixed Text UI")]
    [SerializeField] private TMP_Text _textUI;   // 고정 텍스트

    private GameObject _currentBlockPanel;
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
        // Block panel
        _currentBlockPanel = context.blockMode == BlockMode.Transparent
            ? _transparentBlockPanel
            : _opaqueBlockPanel;

        _currentBlockPanel.SetActive(true);

        // Text (고정)
        if (!string.IsNullOrEmpty(context.text))
        {
            _textUI.text = context.text;
            _textUI.gameObject.SetActive(true);
        }

        // Image (교체)
        if (context.image != null)
        {
            _currentImage = context.image;
            _currentImage.SetActive(true);
        }
    }

    public void HideUI()
    {
        if (_currentBlockPanel != null)
            _currentBlockPanel.SetActive(false);

        if (_textUI != null)
            _textUI.gameObject.SetActive(false);

        if (_currentImage != null)
            _currentImage.SetActive(false);

        _currentBlockPanel = null;
        _currentImage = null;
    }
}