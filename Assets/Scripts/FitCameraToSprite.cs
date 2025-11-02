using UnityEngine;

[ExecuteAlways]
public class FitCameraToSprite : MonoBehaviour
{
    void Start()
    {
        Fit();
    }

    void Fit()
    {
        Camera cam = Camera.main;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (cam == null || sr == null) return;

        // 스프라이트 크기 계산
        float spriteHeight = sr.bounds.size.y;
        float spriteWidth = sr.bounds.size.x;

        // 화면 비율
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = spriteWidth / spriteHeight;

        // 카메라 사이즈 계산
        if (screenRatio >= targetRatio)
        {
            cam.orthographicSize = spriteHeight / 2f;
        }
        else
        {
            float diffInSize = targetRatio / screenRatio;
            cam.orthographicSize = spriteHeight / 2f * diffInSize;
        }

        // 배경 중앙 정렬
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);
    }
}
