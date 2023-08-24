Shader "Custom/GlowEffect"
{
    Properties {
        _BaseMap ("Base Texture", 2D) = "red" {}
        _ScreenRenderTexture("Render Texture", 2D) = "white"{}
        _MaskTexture("Mask Texture", 2D) = "white" {}
    }

    SubShader{

        Tags{"RenderPipeline" = "UniversalPipeline"}

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
                float4 screenPos : TEXCOORD1;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            half4 _BaseMap_ST;

            TEXTURE2D(_ScreenRenderTexture);
            SAMPLER(sampler_ScreenRenderTexture);
            half4 _ScreenRenderTexture_ST;

            TEXTURE2D(_MaskTexture);
            SAMPLER(sampler_MaskTexture);
            half4 _MaskTexture_ST;


            Varyings vert(Attributes input) {
                Varyings o = (Varyings)0;

                VertexPositionInputs posInputs = GetVertexPositionInputs(input.positionOS);
                o.positionCS = posInputs.positionCS;
                o.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                o.screenPos = posInputs.positionNDC;

                return o;
            }

            float4 frag(Varyings i) : SV_TARGET {
                float4 screenColor = SAMPLE_TEXTURE2D(_ScreenRenderTexture, sampler_ScreenRenderTexture, i.screenPos.xy / i.screenPos.w);
                float4 textureColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv);

                screenColor *= (textureColor * 3);

                return float4(screenColor.xyz,1);
            }

            ENDHLSL
        }
    }
}
