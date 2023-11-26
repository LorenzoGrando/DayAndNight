Shader "Custom/CutsceneMat"
{
    Properties {
        _BaseMap ("Base Texture", 2D) = "white" {}
        _LeftTexture("LeftTex", 2D) = "white"{}
        _RightTexture("RightTex", 2D) = "white"{}
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
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            half4 _BaseMap_ST;

            TEXTURE2D(_LeftTexture);
            SAMPLER(sampler_LeftTexture);
            half4 _LeftTexture_ST;

            TEXTURE2D(_RightTexture);
            SAMPLER(sampler_RightTexture);
            half4 _RightTexture_ST;


            Varyings vert(Attributes input) {
                Varyings o = (Varyings)0;

                VertexPositionInputs posInputs = GetVertexPositionInputs(input.positionOS);
                o.positionCS = posInputs.positionCS;
                o.uv = TRANSFORM_TEX(input.uv, _BaseMap);

                return o;
            }

            float4 frag(Varyings i) : SV_TARGET {


                float4 leftTextureColor = SAMPLE_TEXTURE2D(_LeftTexture, sampler_LeftTexture, i.uv);
                float4 rightTextureColor = SAMPLE_TEXTURE2D(_RightTexture, sampler_RightTexture, i.uv);

                float4 finalColor = lerp(leftTextureColor, rightTextureColor, i.uv.x/3);

                /*
                if(i.uv.x < .33) {
                    finalColor = leftTextureColor;
                }

                if(i.uv.x > .66) {
                    finalColor = rightTextureColor;
                }
                */

                return float4(finalColor);
            }

            ENDHLSL
        }
    }
}