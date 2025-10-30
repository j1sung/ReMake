Shader "Custom/ColorLayers"
{
    Properties
    {
        _MainTex ("Source", 2D) = "white" {}          // 카메라 최종 화면
        _MultiplyTex ("Multiply Tex", 2D) = "white" {}// 곱하기 텍스처
        _OverlayTex  ("Overlay Tex", 2D)  = "white" {}// 오버레이 텍스처
        _MultiplyIntensity ("Multiply Intensity", Range(0,1)) = 1
        _OverlayIntensity  ("Overlay Intensity",  Range(0,5)) = 1
        // Tiling/Offset (Unity 규약: _ST suffix 자동 지원)
        _OverlayGamma ("Overlay Gamma", Range(0.1,3.0)) = 0.2
        _MultiplyTex_ST ("", Vector) = (1,1,0,0)
        _OverlayTex_ST  ("", Vector) = (1,1,0,0)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off ZWrite Off ZTest Always
        // 알파 블렌딩은 여기서 안 써도 됨. 최종 프레임에 바로 기록.
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _MultiplyTex;
            sampler2D _OverlayTex;
            float4 _MainTex_TexelSize;

            float4 _MultiplyTex_ST;
            float4 _OverlayTex_ST;
            float _MultiplyIntensity;
            float _OverlayIntensity;
             float _OverlayGamma;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = v.uv; // 풀스크린 쿼드
                return o;
            }

            float3 overlayOp(float3 baseCol, float3 overCol)
            {
                float3 low  = 2.0 * baseCol * overCol;
                float3 high = 1.0 - 2.0 * (1.0 - baseCol) * (1.0 - overCol);
                return lerp(low, high, step(0.5, baseCol));
            }

            float2 TransformUV(float2 uv, float4 st) {
                // Unity의 Tiling/Offset 규약
                return uv * st.xy + st.zw;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 src = tex2D(_MainTex, i.uv);

                // 각 보정 텍스처 샘플 (없으면 흰색/투명으로 임의 처리)
                float2 uvMul = TransformUV(i.uv, _MultiplyTex_ST);
                float2 uvOvr = TransformUV(i.uv, _OverlayTex_ST);

                fixed4 mulTex = tex2D(_MultiplyTex, uvMul);
                fixed4 ovrTex = tex2D(_OverlayTex,  uvOvr);

                // 알파로 마스크 가능 (PNG 투명영역만 적용)
                float3 afterMul = lerp(src.rgb, src.rgb * mulTex.rgb,
                                       _MultiplyIntensity * mulTex.a);

                 float3 ov = overlayOp(afterMul, pow(ovrTex.rgb, _OverlayGamma));
                float3 diff = ov - afterMul;
                float3 outRGB = afterMul + diff * (_OverlayIntensity * ovrTex.a);

                return fixed4(outRGB, src.a);
            }
            ENDCG
        }
    }
    Fallback Off
}
