Shader "Mobile/DiffuseDithered" {
Properties {
    _Color ("Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _WhiteLerp ("White Lerp", Range(0, 1)) = 0  // Новый параметр для Lerp
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 150

CGPROGRAM
#include "Dither Functions.cginc"
#pragma surface surf Lambert noforwardadd
float4 _Color;
sampler2D _MainTex;
float _WhiteLerp;  // Переменная для Lerp

struct Input {
    float2 uv_MainTex;
    float4 screenPos;
}; 

void surf (Input IN, inout SurfaceOutput o) {
    fixed4 c = _Color * tex2D(_MainTex, IN.uv_MainTex);
    
    // Применение Lerp для плавного перехода к белому цвету
    c.rgb = lerp(c.rgb, float3(1, 1, 1), _WhiteLerp);
    
    o.Albedo = c.rgb;
    ditherClip(IN.screenPos.xy / IN.screenPos.w, c.a);
    o.Alpha = c.a;
}


ENDCG
}

Fallback "Mobile/VertexLit"
}