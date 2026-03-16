Shader "Hidden/ScrollTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScrollSpeedX ("Speed X", Float) = 0.5
        _ScrollSpeedY ("Speed Y", Float) = 0.5
        _TextureScale ("Texture Scale", Float) = 0.01
        _Color ("Tint", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // 1. Входные данные от меша (позиция и стандартные UV)
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            // 2. "Мостик" между вершинами и пикселями
            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float _ScrollSpeedX;
            float _ScrollSpeedY;
            float _TextureScale;
            fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                // Рассчитываем координаты на основе позиции вершин (чтобы не растягивалось)
                float2 worldUV = v.vertex.xy * _TextureScale;
                
                // Добавляем движение по времени
                float2 offset = float2(_ScrollSpeedX, _ScrollSpeedY) * _Time.y;
                
                // Записываем результат в customUV, которую мы объявили в структуре v2f
                o.uv = worldUV + offset;
                
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Используем i.customUV для отрисовки текстуры
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                return col;
            }
            ENDCG
        }
    }
}