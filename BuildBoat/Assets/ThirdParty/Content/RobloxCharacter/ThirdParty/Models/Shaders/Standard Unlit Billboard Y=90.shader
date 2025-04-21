Shader "Custom/Particles/Standard Unlit Billboard Y=90"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            v2f vert(appdata_t v)
            {
                v2f o;

                // Получаем позицию вершины в мировом пространстве
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);

                // Получаем направление на камеру
                float3 camDir = normalize(_WorldSpaceCameraPos - worldPos.xyz);

                // Фиксируем ось Y на 90 градусов
                float3 up = float3(0, 1, 0);
                float3 right = normalize(cross(up, camDir));
                float3 newForward = normalize(cross(right, up));

                // Создаем матрицу поворота
                float3x3 rotationMatrix = float3x3(
                    right.x, up.x, newForward.x,
                    right.y, up.y, newForward.y,
                    right.z, up.z, newForward.z
                );

                // Применяем поворот к вершине
                worldPos.xyz = mul(rotationMatrix, worldPos.xyz);

                // Преобразуем обратно в пространство клипа
                o.vertex = mul(UNITY_MATRIX_VP, worldPos);

                // Передаем текстурные координаты и цвет
                o.texcoord = v.texcoord;
                o.color = v.color * _Color;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Отрисовываем текстуру с учетом цвета
                fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
                return col;
            }
            ENDCG
        }
    }
}