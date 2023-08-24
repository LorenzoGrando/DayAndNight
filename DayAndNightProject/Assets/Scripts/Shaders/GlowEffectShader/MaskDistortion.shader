Shader "Custom/MaskDistortion"
{
    Properties {
        _BaseMap ("Base Texture", 2D) = "red" {}
    }

    SubShader{

        Tags{"RenderPipeline" = "UniversalPipeline"}

        //Blend SrcAlpha OneMinusSrcAlpha
        ZTest Always

        Pass {
            Name "DistortMask"
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

            Varyings vert(Attributes input) {
                Varyings o = (Varyings)0;

                VertexPositionInputs posInputs = GetVertexPositionInputs(input.positionOS);
                o.positionCS = posInputs.positionCS;
                o.uv = TRANSFORM_TEX(input.uv, _BaseMap);

                return o;
            }

            float4 frag(Varyings i) : SV_TARGET {
                return float4(1,1,1,1);
            }

            ENDHLSL
        }
    }
}
