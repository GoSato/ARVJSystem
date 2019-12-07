Shader "haquxx/AR/PeoppleOcclusion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);

            sampler2D _MainTex;
            sampler2D _BackgroundTex;
            sampler2D _StencilTex;
            sampler2D _DepthTex;

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
                float2 uv = i.uv;
                uv.y = 1.0 - uv.y;

                float stencil = tex2D(_StencilTex, uv).r;
                float realCameraDepth = tex2D(_DepthTex, uv).r;
                float arCameraDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
                arCameraDepth = LinearEyeDepth(arCameraDepth);

                if(stencil < 0.9)
                {
                    return col;
                }

                float delta = realCameraDepth - arCameraDepth;
                if(delta < 0.0)
                {
                    return tex2D(_BackgroundTex, i.uv);
                }
                else
                {
                    return col;
                }
            }
            ENDCG
        }
    }
}
