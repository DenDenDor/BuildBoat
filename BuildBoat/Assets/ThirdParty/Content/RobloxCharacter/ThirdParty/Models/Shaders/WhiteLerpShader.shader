Shader "Custom/WhiteLerpShader"
{
  Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _WhiteLerp ("White Lerp", Range(0, 1)) = 0 // Параметр для контроля побеления
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
            #pragma multi_compile_fog // Добавляем поддержку тумана (опционально)

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _WhiteLerp;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 albedo = tex2D(_MainTex, i.uv) * _Color;
                fixed4 white = fixed4(1, 1, 1, 1);
                fixed4 finalColor = lerp(albedo, white, _WhiteLerp); // Интерполяция между Albedo и белым
                UNITY_APPLY_FOG(i.fogCoord, finalColor); // Применяем туман (опционально)
                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}