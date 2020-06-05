Shader "haquxx/RealDistortion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap("Bump Map", 2D) = "bump" {}
        _BumpAmt("BumpAmt", Range(0,10)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 grabPos : TEXCOORD1;
                half3 worldNormal : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _BGTex;
            sampler2D _BumpMap;
            float _BumpAmt;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Distortion
                float2 bump = UnpackNormal(tex2D(_BumpMap, i.uv + _Time.x / 2)).rg;
                float2 offset = bump * _BumpAmt;

                half2 uv = i.grabPos.xy / i.grabPos.w;
                uv += offset;
                fixed4 col = tex2D(_BGTex, uv);

              // fixed4 col2;
              // col2.rgb = i.worldNormal * 0.5 + 0.5;
              // col2.a = 1.0;

              // float y = col2.b;
              // float test = dot(i.worldNormal.xyz, fixed3(0.0,0.0,1.0));

              // col.a *= test;
                return col;
            }
            ENDCG
        }
    }
}
