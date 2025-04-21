Shader "Custom/ColorSmoothDissolveShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1, 1, 1, 1) // Основной цвет объекта
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0 // Степень растворения
        _DissolveColor ("Dissolve Color", Color) = (1, 0, 0, 1) // Цвет границы растворения
        _EdgeWidth ("Edge Width", Range(0, 0.1)) = 0.02 // Ширина границы растворения
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color; // Основной цвет объекта
            float _DissolveAmount; // Степень растворения
            float4 _DissolveColor; // Цвет границы растворения
            float _EdgeWidth; // Ширина границы растворения

            // Функция для генерации шума Перлина
            float noise(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Основной цвет объекта
                fixed4 col = _Color;

                // Генерация шума для растворения
                float dissolveNoise = noise(i.uv * 10.0); // Увеличиваем частоту шума

                // Плавное растворение с использованием smoothstep
                float dissolveEffect = smoothstep(_DissolveAmount - _EdgeWidth, _DissolveAmount, dissolveNoise);

                // Применяем эффект растворения
                col.rgb = lerp(col.rgb, _DissolveColor.rgb, dissolveEffect);
                col.a *= (1.0 - dissolveEffect); // Уменьшаем альфа-канал

                return col;
            }
            ENDCG
        }
    }
}