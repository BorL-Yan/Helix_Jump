Shader "Custom/WaterURP"
{
    Properties
    {
        _DepthFadeDistance ("Depth Fade Distance", Range(0.0, 10.0)) = 1.0
        _Absorbance ("Absorbance", Range(0.0, 10.0)) = 2.0
        _BaseAlpha ("Base Alpha", Range(0.0, 1.0)) = 0.8
        
        _ShallowColor ("Shallow Color", Color) = (0.22, 0.66, 1.0, 1.0)
        _DeepColor ("Deep Color", Color) = (0.0, 0.25, 0.45, 1.0)
        
        _FoamAmount ("Foam Amount", Range(0.0, 2.0)) = 0.2
        _FoamColor ("Foam Color", Color) = (1, 1, 1, 1)
        _FoamNoise ("Foam Noise", 2D) = "white" {}
        _FoamNoiseScale ("Foam Noise Scale", Float) = 3.0
        _FoamNoiseSpeed ("Foam Noise Speed", Float) = 0.05
        
        _Roughness ("Roughness", Range(0.0, 1.0)) = 0.05
        _SpecularStrength ("Specular Strength", Range(0.0, 2.0)) = 1.0
        
        _WaveTexture ("Wave Texture", 2D) = "white" {}
        _WaveScale ("Wave Scale", Float) = 4.0
        _HeightScale ("Height Scale", Float) = 0.15
        
        _Normal1 ("Normal Map 1", 2D) = "bump" {}
        _WaveDir1 ("Wave Direction 1", Vector) = (1.0, 0.0, 0, 0)
        _Normal2 ("Normal Map 2", 2D) = "bump" {}
        _WaveDir2 ("Wave Direction 2", Vector) = (0.0, 1.0, 0, 0)
        _WaveSpeed ("Wave Speed", Range(0.0, 0.2)) = 0.015
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }
        
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };
            
            struct Varyings
            {
                float4 positionCS  : SV_POSITION;
                float3 positionWS  : TEXCOORD0;
                float4 screenPos   : TEXCOORD1;
            };
            
            TEXTURE2D(_WaveTexture); SAMPLER(sampler_WaveTexture);
            TEXTURE2D(_Normal1);     SAMPLER(sampler_Normal1);
            TEXTURE2D(_Normal2);     SAMPLER(sampler_Normal2);
            TEXTURE2D(_FoamNoise);   SAMPLER(sampler_FoamNoise);
            
            CBUFFER_START(UnityPerMaterial)
                float  _DepthFadeDistance;
                float  _Absorbance;
                float  _BaseAlpha;
                float4 _ShallowColor;
                float4 _DeepColor;
                float  _FoamAmount;
                float4 _FoamColor;
                float  _FoamNoiseScale;
                float  _FoamNoiseSpeed;
                float  _Roughness;
                float  _SpecularStrength;
                float  _WaveScale;
                float  _HeightScale;
                float2 _WaveDir1;
                float2 _WaveDir2;
                float  _WaveSpeed;
            CBUFFER_END

            float3 ReconstructWorldPosition(float2 screenUV, float rawDepth)
            {
                float4 ndc = float4(screenUV * 2.0 - 1.0, rawDepth, 1.0);
                #if UNITY_UV_STARTS_AT_TOP
                    ndc.y = -ndc.y;
                #endif
                float4 viewPos = mul(unity_CameraInvProjection, ndc);
                viewPos /= viewPos.w;
                float4 worldPos = mul(unity_CameraToWorld, viewPos);
                return worldPos.xyz;
            }
            
            Varyings vert(Attributes input)
            {
                Varyings o = (Varyings)0;
                
                float3 positionWS = TransformObjectToWorld(input.positionOS);
                float4 positionCS = TransformWorldToHClip(positionWS);
                float4 screenPos = ComputeScreenPos(positionCS);
                
                o.positionWS = positionWS;
                o.positionCS = positionCS;
                o.screenPos = screenPos;
                return o;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                float2 screenUV = input.screenPos.xy / input.screenPos.w;
                float rawDepth = SampleSceneDepth(screenUV);
                float3 sceneWorldPos = ReconstructWorldPosition(screenUV,rawDepth);
                float3 surfaceWorldPos = input.positionWS;
                
                float waterDepth = max(0.0,surfaceWorldPos.y - sceneWorldPos.y);
                float depthFade = saturate(exp(-waterDepth / _DepthFadeDistance));
                float3 colBland = lerp(_DeepColor,_ShallowColor,depthFade);
               
                
                
                
                float4 r = float4(colBland,1);
                return r/2;
            }
            ENDHLSL
        }
    }
}