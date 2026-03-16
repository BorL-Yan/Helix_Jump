Shader "Custom/JellyURP"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _BaseMap("Base Map", 2D) = "white" {}
        
        [Header(Jelly Settings)]
        _ImpactPos("Impact Position", Vector) = (0,0,0,0)
        _ImpactTime("Impact Time", Float) = -100
        _WaveSpeed("Wave Speed", Float) = 10
        _WaveAmplitude("Wave Amplitude", Float) = 0.5
        _Stiffness("Stiffness", Float) = 2.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
            };

            // Для поддержки SRP Batcher все свойства должны быть в CBUFFER
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _ImpactPos;
                float _ImpactTime;
                float _WaveSpeed;
                float _WaveAmplitude;
                float _Stiffness;
            CBUFFER_END

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            Varyings vert(Attributes input)
            {
                Varyings output;

                // 1. Получаем мировую позицию вершины
                float3 worldPos = TransformObjectToWorld(input.positionOS.xyz);
                
                // 2. Логика деформации
                float dist = distance(worldPos, _ImpactPos.xyz);
                float timePassed = _Time.y - _ImpactTime;

                // Эффект активен короткое время после удара
                if (timePassed < 2.0 && timePassed > 0)
                {
                    float wave = sin(dist * _Stiffness - timePassed * _WaveSpeed);
                    // Затухание по расстоянию и времени
                    float attenuation = exp(-dist * 1.5) * exp(-timePassed * 4.0);
                    
                    // Смещаем мировую позицию по Y
                    worldPos.y += wave * attenuation * _WaveAmplitude;
                }

                // 3. Переводим измененную мировую позицию в пространство отсечения (Clip Space)
                output.positionCS = TransformWorldToHClip(worldPos);
                output.uv = input.uv;
                
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half4 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
                return texColor * _BaseColor;
            }
            ENDHLSL
        }
    }
}