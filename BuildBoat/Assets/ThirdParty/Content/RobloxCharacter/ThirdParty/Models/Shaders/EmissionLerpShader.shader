Shader "Custom/EmissionLerpShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _EmissionColor ("Emission Color", Color) = (1,1,1,1)
        _Lerp ("Lerp", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;
        fixed4 _EmissionColor;
        float _Lerp;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = _Color;
            o.Albedo = c.rgb;
            o.Emission = _EmissionColor.rgb * _Lerp;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}