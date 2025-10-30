using UnityEngine;

[ExecuteAlways]
public class OverlayPulsing : MonoBehaviour
{
    public ColorOverlayEffect effect; // 카메라에 붙은 스크립트 Drag & Drop
    public float multiplyMax = 1f;    // 곱하기 최댓값
    public float overlayMax = 1.71f;  // 오버레이 최댓값
    public float minFactor = 0.5f;    // 최소 비율 (0.5 = 절반까지 감소)
    public float speed = 1.0f;        // 속도 조절 (1이면 1초에 한 주기)

    private void Update()
    {
        if (effect == null) return; 

        // 0~1 사이에서 반복
        float t = Mathf.PingPong(Time.time * speed, 1f);

        // 부드럽게 0.5 ~ 1.0 사이로 변환
        float factor = Mathf.Lerp(minFactor, 1f, t);

        // 각각 강도에 적용
        effect.overlayIntensity = overlayMax * factor;
    }
}
