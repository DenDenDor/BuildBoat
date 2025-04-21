Shader "Mobile/ColorReplaceDiffuse"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // Основная текстура
        _Color ("Color", Color) = (1,1,1,1) // Основной цвет материала
        _ReplaceColor ("Replace Color", Color) = (0,0,0,1) // Цвет, который нужно заменить
        _Tolerance ("Tolerance", Range(0, 1)) = 0.1 // Допуск для замены
        _Lerp ("Lerp", Range(0, 1)) = 0.5 // Параметр для плавного перехода
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" "LightMode"="ForwardBase" "IgnoreProjector"="True" }
        LOD 150

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _ReplaceColor;
            float _Tolerance;
            float _Lerp;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Получаем цвет текстуры
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // Проверяем, находится ли цвет текселя в пределах допуска
                float diffR = abs(texColor.r - _ReplaceColor.r);
                float diffG = abs(texColor.g - _ReplaceColor.g);
                float diffB = abs(texColor.b - _ReplaceColor.b);

                if (diffR < _Tolerance && diffG < _Tolerance && diffB < _Tolerance)
                {
                    // Плавный переход между ReplaceColor и Color
                    texColor.rgb = lerp(_ReplaceColor.rgb, _Color.rgb, _Lerp);

                    // Добавляем эмиссию
                    texColor.rgb += _Lerp;
                }

                // Вычисляем диффузную засветку
                float3 worldNormal = normalize(i.worldNormal);
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float diff = max(0, dot(worldNormal, lightDir)); // Диффузное освещение

                // Ограничиваем эффект удаления темных оттенков
                float minBrightness = lerp(0.0, 0.576, _Lerp); // Максимальное значение 0.576
                diff = max(diff, minBrightness); // Подтягиваем темные значения

                // Применяем освещение к цвету
                fixed4 finalColor = texColor * _LightColor0 * diff;

                // Уменьшаем видимость теней
                finalColor.rgb *= 0.5;

                // Применяем туман
                UNITY_APPLY_FOG(i.fogCoord, finalColor);

                return finalColor;
            }
            ENDCG
        }
    }
}