// Digital Camera and Real Camera

Shader "haquxx/ReflectionAR"
{
    Properties
    {
        _RefTex ("Reflection Texture", 2D) = "white" {}
        _BumpMap("Bump Map", 2D) = "bump" {}
        _BumpAmt("BumpAmt", Range(0,10)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector" = "True" }
        Blend SrcAlpha OneMinusSrcAlpha

        GrabPass
        {
            "_BGTex"
        }

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
                float4 ref : TEXCOORD1;
                float4 grabPos : TEXCOORD2;
            };

            sampler2D _BGTex;
            sampler2D _RefTex;
            float4 _RefTex_ST;
            float4 _RefTex_TexelSize;
            sampler2D _BumpMap;
            float _BumpAmt;
            float4x4 _RefM;
            float4x4 _RefV;
            float4x4 _RefP;
            float4x4 _RefVP;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.vertex);
                o.ref = mul(_RefVP, mul(_RefM, v.vertex));
                o.uv = TRANSFORM_TEX(v.uv, _RefTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Distortion
                float2 bump = UnpackNormal(tex2D(_BumpMap, i.uv + _Time.x / 2)).rg;
                float2 offset = bump * _BumpAmt;

                // Real Camera
                half2 uv = i.grabPos.xy / i.grabPos.w;
                uv += offset;
                //fixed4 col = tex2Dproj(_BGTex, i.grabPos);
                fixed4 col = tex2D(_BGTex, uv);

                
                // Reflection
                i.ref.xy = offset + i.ref.xy;
                half2 uv2 = i.ref.xy / i.ref.w * 0.5 + 0.5;
                fixed4 col2 = tex2D(_RefTex, uv2);

                fixed4 finalCol = col * (1.0 - col2.a) + col2 * col2.a;

                return finalCol;
            }
            ENDCG
        }
    }
}
