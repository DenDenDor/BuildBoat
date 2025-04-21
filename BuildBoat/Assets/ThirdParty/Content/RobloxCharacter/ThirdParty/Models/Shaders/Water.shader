Shader "Custom/WaterShader"
{
    Properties
    {
        _WaterColor ("Water color", Color) = (1, 1, 1, 1)
        _WaterTex ("Water texture", 2D) = "white" {}
        _Tiling ("Water tiling", Vector) = (1, 1, 1, 1)
        _TextureVisibility("Texture visibility", Range(0, 1)) = 1

        [Space(20)]
        _DistTex ("Distortion", 2D) = "white" {}
        _DistTiling ("Distortion tiling", Vector) = (1, 1, 1, 1)

        [Space(20)]
        _WaterHeight ("Water height", Float) = 0

        [Space(20)]
        _MoveDirection ("Direction", Vector) = (0, 0, 0, 0)

        // Новые параметры для волн
        [Space(20)]
        _WaveSpeed ("Wave Speed", Float) = 1.0
        _WaveAmplitude ("Wave Amplitude", Float) = 0.1
        _WaveFrequency ("Wave Frequency", Float) = 1.0

        // Параметры для пены
        [Space(20)]
        _FoamColor ("Foam Color", Color) = (1, 1, 1, 1) // Цвет пены
        _FoamThreshold ("Foam Threshold", Range(0, 1)) = 0.8 // Порог для пены
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #pragma multi_compile_fog
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL; // Добавляем нормаль
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                fixed4 worldPos: TEXCOORD1;
                fixed camHeightOverWater : TEXCOORD2;
                float3 worldNormal : TEXCOORD3; // Передаем нормаль в фрагментный шейдер
                UNITY_FOG_COORDS(4)
                float4 vertex : SV_POSITION;
            };

            sampler2D _WaterTex;
            float4 _WaterTex_ST;
            fixed2 _Tiling;
            fixed4 _WaterColor;

            sampler2D _DistTex;
            fixed2 _DistTiling;

            fixed _WaterHeight;
            fixed _TextureVisibility;

            fixed3 _MoveDirection;

            // Новые параметры для волн
            fixed _WaveSpeed;
            fixed _WaveAmplitude;
            fixed _WaveFrequency;

            // Параметры для пены
            fixed4 _FoamColor;
            fixed _FoamThreshold;

            fixed2 WaterPlaneUV(fixed3 worldPos, fixed camHeightOverWater)
            {
                fixed3 camToWorldRay = worldPos - _WorldSpaceCameraPos;
                fixed3 rayToWaterPlane = (camHeightOverWater / camToWorldRay.y * camToWorldRay);
                return rayToWaterPlane.xz - _WorldSpaceCameraPos.xz;
            }

            v2f vert (appdata v)
            {
                v2f o;

                // Добавляем движение воды через вершинный шейдер
                float wave = sin(v.vertex.x * _WaveFrequency + _Time.y * _WaveSpeed) * _WaveAmplitude;
                v.vertex.y += wave; // Изменяем высоту вершины

                o.worldPos = mul(UNITY_MATRIX_M, v.vertex);
                o.vertex = mul(UNITY_MATRIX_VP, o.worldPos);
                
                o.uv = TRANSFORM_TEX(v.uv, _WaterTex);
                o.camHeightOverWater = _WorldSpaceCameraPos.y - _WaterHeight;

                // Передаем нормаль в мировом пространстве
                o.worldNormal = UnityObjectToWorldNormal(v.normal);

#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)
                fixed3 camToWorldRay = o.worldPos - _WorldSpaceCameraPos;
                fixed3 rayToWaterPlane = (o.camHeightOverWater / camToWorldRay.y * camToWorldRay);

                fixed3 worldPosOnPlane = _WorldSpaceCameraPos - rayToWaterPlane;
                fixed3 positionForFog = lerp(worldPosOnPlane, o.worldPos.xyz, o.worldPos.y > _WaterHeight);
                fixed4 waterVertex = mul(UNITY_MATRIX_VP, fixed4(positionForFog, 1));
                UNITY_TRANSFER_FOG(o, waterVertex);
#endif

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed2 water_uv = WaterPlaneUV(i.worldPos, i.camHeightOverWater);
                fixed4 distortion = tex2D(_DistTex, water_uv * _DistTiling) * 2 - 1;
                fixed2 distorted_uv = ((water_uv + distortion.rg) - _Time.y * _MoveDirection.xz) * _Tiling;

                fixed4 waterCol = tex2D(_WaterTex, distorted_uv);
                waterCol = lerp(_WaterColor, fixed4(1, 1, 1, 1), waterCol.r * _TextureVisibility);

                // Вычисляем пену
                float foamFactor = saturate(dot(i.worldNormal, float3(0, 1, 0))); // Проверяем, насколько нормаль близка к вертикальной
                foamFactor = step(_FoamThreshold, foamFactor); // Если нормаль близка к вертикальной, добавляем пену

                // Смешиваем цвет воды с цветом пены
                waterCol = lerp(waterCol, _FoamColor, foamFactor);

                UNITY_APPLY_FOG(i.fogCoord, waterCol);

                return waterCol;
            }
            ENDCG
        }
    }
}