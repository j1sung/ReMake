using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ColorOverlayEffect : MonoBehaviour
{
    [Header("Textures (optional)")]
    public Texture2D multiplyTex;   // 보정_곱하기 PNG
    public Vector2 multiplyTiling = Vector2.one;
    public Vector2 multiplyOffset = Vector2.zero;
    [Range(0, 1)] public float multiplyIntensity = 1f;

    public Texture2D overlayTex;    // 보정_오버레이 PNG
    public Vector2 overlayTiling = Vector2.one;
    public Vector2 overlayOffset = Vector2.zero;
    [Range(0, 5)] public float overlayIntensity = 1f;

    [Header("Material")]
    public Material material;       // ColorLayers 셰이더로 만든 머티리얼

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material == null)
        {
            Graphics.Blit(src, dest); // 패스
            return;
        }

        // 원본 프레임
        material.SetTexture("_MainTex", src);

        // Multiply
        if (multiplyTex != null)
        {
            material.SetTexture("_MultiplyTex", multiplyTex);
            // _ST 규약: (tiling.x, tiling.y, offset.x, offset.y)
            material.SetVector("_MultiplyTex_ST",
                new Vector4(multiplyTiling.x, multiplyTiling.y, multiplyOffset.x, multiplyOffset.y));
            material.SetFloat("_MultiplyIntensity", multiplyIntensity);
        }
        else
        {
            // 텍스처가 없으면 강도 0
            material.SetFloat("_MultiplyIntensity", 0f);
        }

        // Overlay
        if (overlayTex != null)
        {
            material.SetTexture("_OverlayTex", overlayTex);
            material.SetVector("_OverlayTex_ST",
                new Vector4(overlayTiling.x, overlayTiling.y, overlayOffset.x, overlayOffset.y));
            material.SetFloat("_OverlayIntensity", overlayIntensity);
        }
        else
        {
            material.SetFloat("_OverlayIntensity", 0f);
        }

        Graphics.Blit(src, dest, material);
    }
}
