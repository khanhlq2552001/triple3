Shader"Custom/SpriteWithNewBackground"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _BackgroundTex ("Background Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha   // Hỗ trợ alpha blending
            ZWrite On                       // Không ghi vào Z-buffer, đảm bảo vẽ theo thứ tự
            Cull Off                          // Hiển thị cả hai mặt

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Lấy màu từ nền
                half4 backgroundColor = tex2D(_BackgroundTex, i.uv);

                return backgroundColor;
            }
            ENDCG
        }
    }
    Fallback "Transparent"
}
