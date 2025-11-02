using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class CursorHoverDetector : MonoBehaviour
{
    [SerializeField] private CursorManager cursorManager;   // 부모(GameManager)에서 자동 탐색
    [SerializeField] private Texture2D interactiveCursor;
    [SerializeField] private Vector2 interactiveHotspot = Vector2.zero;

    private Camera targetCamera;
    private Collider2D _lastCollider;
    private Button _lastUIButton;
    private GameObject _lastUIInteractive;

    void Awake()
    {
        if (!cursorManager) cursorManager = GetComponentInParent<CursorManager>();
        SceneManager.activeSceneChanged += (_, __) => { targetCamera = null; _lastCollider = null; _lastUIButton = null; _lastUIInteractive = null; };
    }
    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= (_, __) => { targetCamera = null; _lastCollider = null; _lastUIButton = null; _lastUIInteractive = null; };
    }

    void Update()
    {
        if (!targetCamera) targetCamera = Camera.main;
        if (!targetCamera) return;

        // 1) 월드(콜라이더) 감지: 태그 Interactive
        Vector2 mouseWorld = targetCamera.ScreenToWorldPoint(Input.mousePosition);
        var hitCol = Physics2D.OverlapPoint(mouseWorld);

        // 2) UI 단 한 번의 Raycast로 버튼/태그 둘 다 판별
        var uiHit = GetUIHitUnderMouse();

        // 변경 없으면 스킵
        if (hitCol == _lastCollider && uiHit.button == _lastUIButton && uiHit.interactive == _lastUIInteractive) return;
        _lastCollider = hitCol; _lastUIButton = uiHit.button; _lastUIInteractive = uiHit.interactive;

        bool isInteractive =
            (hitCol && hitCol.CompareTag("Interactive"))   // 월드: 태그
            || (uiHit.button != null)                      // UI: 버튼
            || (uiHit.interactive != null);                // UI: 태그

        if (isInteractive && interactiveCursor)
            cursorManager.SetCursor(interactiveCursor, interactiveHotspot);
        else
            cursorManager.SetDefault();
    }

    // 결과를 하나로 묶어 반환
    struct UIHit { public Button button; public GameObject interactive; }

    UIHit GetUIHitUnderMouse()
    {
        var res = new UIHit();
        if (!EventSystem.current) return res;

        var data = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        // 최상단부터 스캔: 버튼(활성) / Interactive 태그를 각각 최초 1회만 채집
        foreach (var r in results)
        {
            if (!res.button)
            {
                var btn = r.gameObject.GetComponentInParent<Button>();
                if (btn && btn.interactable) res.button = btn;
            }
            if (!res.interactive && r.gameObject.CompareTag("Interactive"))
                res.interactive = r.gameObject;

            if (res.button && res.interactive) break; // 둘 다 찾았으면 조기 종료
        }
        return res;
    }
}