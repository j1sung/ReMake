using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Vector2 defaultHotspot = Vector2.zero;

    private Texture2D _current;
    private Vector2 _hot;


    void Awake()
    {
        // 씬 바뀔 때마다 기본 커서 적용
        SceneManager.activeSceneChanged += (_, __) => SetDefault();
    }

    void OnDestroy()
    {
        // 이벤트 해제 (메모리 누수 방지)
        SceneManager.activeSceneChanged -= (_, __) => SetDefault();
    }
    void Start() => SetDefault();

    public void SetCursor(Texture2D tex, Vector2 hotspot)
    {
        if (!tex) return;
        if (_current == tex && _hot == hotspot) return;

        Cursor.SetCursor(tex, hotspot, CursorMode.Auto);
        
        _current = tex;
        _hot = hotspot;
    }

    public void SetDefault() => SetCursor(defaultCursor, defaultHotspot);
}