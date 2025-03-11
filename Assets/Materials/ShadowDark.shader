Shader "Custom/SpriteWithNewBackgroundDark"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _BackgroundTex ("Background Texture", 2D) = "white" {}
        _DarknessFactor ("Background Darkness", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
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
            sampler2D _BackgroundTex;
            float4 _MainTex_ST;
            float _DarknessFactor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half4 backgroundColor = tex2D(_BackgroundTex, i.uv);
                backgroundColor.rgb *= _DarknessFactor; // Làm tối nền bằng cách nhân với hệ số nhỏ hơn 1
                return backgroundColor;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
