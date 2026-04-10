Shader "Custom/Jelle"
{
    Properties
    {
        [Header(Base Material Settings)]
        _smoothness("smoothness",Range(0.0, 1)) = 0.5
        _Metalic("Metalic",Range(0.0, 1)) = 0.5
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _BaseMap("Base Map", 2D) = "white" {}

        [Header(Jelly Settings)]
        _DegreeAttenuation("Degree Attenuation",Float) = 1.5
        _WaveSpeed("Wave Speed", Float) = 10
        _WaveAmplitude("Wave Amplitude", Float) = 0.5
        _Stiffness("Stiffness", Float) = 2.0

        _ImpactPos("Impact Pos", Vector) = (0,0,0,0)
        _ImpactTime("Impact Time", Float) = -999
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            Name "ForwardLit"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile_fragment _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                float2 texcoord1  : TEXCOORD1;
                float3 normal     : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 viewDir    : TEXCOORD3;
                float3 normalWS   : TEXCOORD2;
                float3 positionWS : TEXCOORD1;
                float2 uv         : TEXCOORD0;

                DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 4);
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _ImpactPos;
                float  _Metalic;
                float  _smoothness;
                float  _ImpactTime;
                float  _WaveSpeed;
                float  _WaveAmplitude;
                float  _Stiffness;
                float  _DegreeAttenuation;
            CBUFFER_END

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            Varyings vert(Attributes input)
            {
                Varyings output;

                // 1. world position
                float3 worldPos = TransformObjectToWorld(input.positionOS.xyz);

                // normal
                float3 normalWS = TransformObjectToWorldNormal(input.normal);

                // 2. impact logic
                float dist = distance(worldPos, _ImpactPos.xyz);
                float timePassed = _Time.y - _ImpactTime;

                if (timePassed < 2.0 && timePassed > 0)
                {
                    float wave = sin(dist * _Stiffness - timePassed * _WaveSpeed);

                    float attenuation =
                        exp(-dist * _DegreeAttenuation) *
                        exp(-timePassed * 4.0);

                    worldPos.y += wave * attenuation * _WaveAmplitude;
                }

                output.positionCS = TransformWorldToHClip(worldPos);
                output.positionWS = worldPos;
                output.viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                output.normalWS = normalize(normalWS);

                OUTPUT_LIGHTMAP_UV(input.texcoord1, unity_LightmapST, output.lightmapUV);
                OUTPUT_SH(output.normalWS, output.vertexSH);

                output.uv = input.uv;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half4 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);

                InputData inputData = (InputData)0;
                inputData.positionWS = input.positionWS;
                inputData.normalWS = normalize(input.normalWS);
                inputData.viewDirectionWS = input.viewDir;
                inputData.bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, inputData.normalWS);

                SurfaceData surfaceData = (SurfaceData)0;
                surfaceData.albedo = texColor.rgb * _BaseColor.rgb;
                surfaceData.metallic = _Metalic;
                surfaceData.smoothness = _smoothness;
                surfaceData.occlusion = 1;

                return UniversalFragmentPBR(inputData, surfaceData);
            }

            ENDHLSL
        }
    }
}