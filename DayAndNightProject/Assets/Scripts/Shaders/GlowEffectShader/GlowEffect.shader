Shader "Custom/GlowEffect"
{
    Properties {
        _BaseMap ("Base Texture", 2D) = "white" {}
        _ScreenRenderTexture("Render Texture", 2D) = "white"{}
        _OutlineColor("OutlineColor", Color) = (1,1,1,1)
        _TemporaryColor("Temporary Color", Color) = (1,1,1,1)
    }

    SubShader{

        Tags{"RenderPipeline" = "UniversalPipeline"}

        ZTest Always

        Pass {
            Name "2DLit"
            Tags{"LightMode" = "Universal2D" "Queue" = "Transparent"}

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes {
                float3 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 polarUV : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            half4 _BaseMap_ST;

            TEXTURE2D(_ScreenRenderTexture);
            SAMPLER(sampler_ScreenRenderTexture);
            half4 _ScreenRenderTexture_ST;

            float _MaskSubtractionValue;
            float _MaskMultiplicationValue;

            float _MaskLerpMin;
            float _MaskLerpMax;

            float _ThresholdValue;
            float _WaveSpeed;
            int _WaveAmount;

            float4 _OutlineColor;
            float _OutlineWidth;

            float4 _TemporaryColor;

            float2 ConvertUVToPolar(float2 uv, float2 center, float radialScale, float lenghtScale) {
                float2 delta = uv - center;
                float radius = length(delta) * 2 * radialScale;
                float angle = atan2(delta.y, delta.x) * 1.0/6.28 * lenghtScale;
                return float2(radius, angle);
            }

            float InverseLerp(float a, float b, float t) {
                return (t-a)/(b-a);
            }

            Varyings vert(Attributes input) {
                Varyings o = (Varyings)0;

                VertexPositionInputs posInputs = GetVertexPositionInputs(input.positionOS);
                o.positionCS = posInputs.positionCS;
                o.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                o.polarUV = o.uv;
                o.screenPos = posInputs.positionNDC;

                return o;
            }

            float4 frag(Varyings i) : SV_TARGET {
                i.polarUV = ConvertUVToPolar(i.polarUV, float2(0.5,0.5), 1, 20);
                float squiggle = saturate((abs(frac((i.polarUV.y/_WaveAmount) + _Time * _WaveSpeed) - _MaskSubtractionValue)) * _MaskMultiplicationValue);
                float wave = InverseLerp(_MaskLerpMin, _MaskLerpMax, i.polarUV.x) - squiggle;
                float invertedPolar = (1 - wave);
                float clipValue = invertedPolar + _ThresholdValue;
                clip(clipValue);


                float4 screenColor = SAMPLE_TEXTURE2D(_ScreenRenderTexture, sampler_ScreenRenderTexture, i.screenPos.xy / i.screenPos.w);
                float4 textureColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv);
                textureColor *= _TemporaryColor;
                screenColor *= (textureColor);

                if(invertedPolar.x < 1 - _OutlineWidth) {
                    return _OutlineColor;
                }
                return float4(screenColor.xyz,1);
            }

            ENDHLSL
        }
    }
}
