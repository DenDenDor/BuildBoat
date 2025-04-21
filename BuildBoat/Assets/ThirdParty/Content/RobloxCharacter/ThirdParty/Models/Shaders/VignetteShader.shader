Shader "Custom/VignetteShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _VignetteColor ("Vignette Color", Color) = (0, 0, 0, 1)
        _VignettePower ("Vignette Power", Float) = 2.0
        _VignetteSmoothness ("Vignette Smoothness", Float) = 0.5
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
            };

            sampler2D _MainTex;
            float4 _VignetteColor;
            float _VignettePower;
            float _VignetteSmoothness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Calculate vignette effect
                float2 uv = i.uv - 0.5; // Center UV
                float dist = length(uv);
                float vignette = smoothstep(_VignetteSmoothness, 1.0, dist);
                vignette = pow(vignette, _VignettePower);

                // Apply vignette
                col.rgb = lerp(col.rgb, _VignetteColor.rgb, vignette);
                return col;
            }
            ENDCG
        }
    }
}