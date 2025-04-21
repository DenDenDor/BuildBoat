Shader "Custom/BayerDitheringShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorDepth ("Color Depth", Float) = 8.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
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
                float4 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float _ColorDepth;

            // Матрица Байера 4x4
            static const float bayerMatrix[16] = {
                0.0, 8.0, 2.0, 10.0,
                12.0, 4.0, 14.0, 6.0,
                3.0, 11.0, 1.0, 9.0,
                15.0, 7.0, 13.0, 5.0
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Получаем цвет из текстуры
                fixed4 col = tex2D(_MainTex, i.uv);

                // Вычисляем UV для дизеринга
                float2 screenUV = i.screenPos.xy / i.screenPos.w * _ScreenParams.xy;
                float2 ditherCoord = fmod(screenUV, 4.0); // 4x4 матрица Байера
                int index = int(ditherCoord.x) + int(ditherCoord.y) * 4;

                // Получаем значение из матрицы Байера
                float ditherValue = bayerMatrix[index] / 16.0;

                // Уменьшаем глубину цвета
                float colorDepth = pow(2, _ColorDepth);
                col.rgb = floor(col.rgb * colorDepth) / colorDepth;

                // Применяем дизеринг
                col.rgb += (ditherValue - 0.5) / colorDepth;

                return col;
            }
            ENDCG
        }
    }
}