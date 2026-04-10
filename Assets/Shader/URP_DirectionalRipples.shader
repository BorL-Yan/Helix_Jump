Shader "Custom/URP_DirectionalRipples"
{
    Properties
    {
        // Базовые цвета воды
        _Color ("Water Color", Color) = (0, 0.5, 1, 0.8) // Полупрозрачный синий
        _DeepColor ("Deep Water Color", Color) = (0, 0.2, 0.5, 1) // Темный синий
        _FoamColor ("Foam/Ripple Color", Color) = (1, 1, 1, 1) // Непрозрачный белый

        // Настройка глубины для цвета воды
        _DepthFadeDistance ("Water Color Depth Fade", Float) = 3.0

        // Настройка пульсирующих кругов (Ripples)
        _RipplesFrequency ("Ripples Frequency", Float) = 20.0
        _RipplesSpeed ("Ripples Speed", Float) = 3.0
        // Чем больше это значение, тем тоньше будут кольца
        _RipplesThickness ("Ripples Thickness", Float) = 50.0 
    }

    SubShader
    {
        // Теги для прозрачной воды в URP
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            // Подключаем библиотеки URP
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float4 screenPos    : TEXCOORD0;
            };

            // Объявление переменных из Properties
            half4 _Color;
            half4 _DeepColor;
            half4 _FoamColor;
            float _DepthFadeDistance;
            float _RipplesFrequency;
            float _RipplesSpeed;
            float _RipplesThickness;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                // Преобразуем позицию из локальных координат в клип-координаты
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                // Вычисляем экранные координаты для сэмплирования текстуры глубины
                OUT.screenPos = ComputeScreenPos(OUT.positionCS);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                // Вычисляем экранные UV координаты
                float2 uv = IN.screenPos.xy / IN.screenPos.w;
                
                // === ЭТАП 1: ГЛУБИНА ===
                // Сэмплируем глубину сцены (расстояние от камеры до твердых объектов ЗА водой)
                float rawDepth = SampleSceneDepth(uv);
                float sceneDepth = LinearEyeDepth(rawDepth, _ZBufferParams);

                // Вычисляем линейную глубину самой поверхности воды
                float surfaceDepth = IN.screenPos.w;
                
                // Вычисляем разницу глубин (глубина воды под этой точкой)
                // depthDiff = 0 у самого берега/объекта, увеличивается с глубиной
                float depthDiff = sceneDepth - surfaceDepth;


                // === ЭТАП 2: ЦВЕТ ВОДЫ ===
                // Плавный градиент для цвета воды на основе глубины
                float waterDepthMask = saturate(depthDiff / _DepthFadeDistance);
                half4 waterColor = lerp(_Color, _DeepColor, waterDepthMask);


                // === ЭТАП 3: ЧЕТКИЕ, ПУЛЬСИРУЮЩИЕ КОНТУРНЫЕ КРУГИ (RIPPLES) ===
                // Вычисляем фазу волны. Глубина * Частота - Время * Скорость.
                // Вычитание времени заставляет волны бесконечно расходиться НАРУЖУ от объектов.
                float wavePhase = (depthDiff * _RipplesFrequency) - (_Time.y * _RipplesSpeed);
                
                // Функция косинуса создает бесконечный повторяющийся волновой паттерн.
                // Преобразуем cos(X) из [-1..1] в [0..1]
                float ripples = saturate(cos(wavePhase) * 0.5 + 0.5);
                
                // Критический шаг: возводим косинус в большую степень.
                // Это "сужает" паттерн, превращая мягкие волны в очень тонкие, четкие кольца.
                ripples = pow(ripples, _RipplesThickness);
                
                // Накладываем белую пену по маске колец
                half4 finalColor = lerp(waterColor, _FoamColor, ripples * _FoamColor.a);
                
                // Устанавливаем прозрачность: пена непрозрачна, вода полупрозрачна
                finalColor.a = max(waterColor.a, ripples * _FoamColor.a);

                return finalColor;
            }
            ENDHLSL
        }
    }
    FallBack "Transparent/Diffuse"
}