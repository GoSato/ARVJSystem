// Digital Camera and Real Camera

Shader "haquxx/ReflectionAR"
{
    Properties
    {
        _MainTex ("BackgroundImage Texture", 2D) = "white" {}
        _RefTex ("Reflection Texture", 2D) = "white" {}
        _BumpMap("Bump Map", 2D) = "bump" {}
        _BumpAmt("BumpAmt", Range(0,10)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector" = "True" }
        Blend SrcAlpha OneMinusSrcAlpha

        //GrabPass
        //{
        //    "_BGTex"
        //}

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
            };

            sampler2D _MainTex;
            //sampler2D _BGTex;
            sampler2D _RefTex;
            float4 _RefTex_ST;
            float4 _RefTex_TexelSize;
            sampler2D _BumpMap;
            float _BumpAmt;
            float4x4 _RefM;
            float4x4 _RefV;
            float4x4 _RefP;
            float4x4 _RefVP;

            float _ScaleFacXa;
            float _ScaleFacYa;
            float _ScaleFacXb;
            float _ScaleFacYb;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float2 screenPos = o.vertex.xy / o.vertex.w;
                o.ref = mul(_RefVP, mul(_RefM, v.vertex));
                o.uv.x = (_ScaleFacXa * screenPos.x) + _ScaleFacXb; // clipした左端~右端(0~1に収まる)
                o.uv.y = -(_ScaleFacYa * screenPos.y) + _ScaleFacYb; // clipした下端~上端(0~1に収まる)
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Distortion
                float2 bump = UnpackNormal(tex2D(_BumpMap, i.uv + _Time.x / 2)).rg;
                float2 offset = bump * _BumpAmt;

                // Real Camera
                // for AR(ARKit,ARCore)
                i.uv.y = 1 - i.uv.y;

                // for ARFoundation
                i.uv.x = 1 - i.uv.x;
                i.uv += offset;
                //fixed4 col = tex2Dproj(_BGTex, i.grabPos);

                // for Legacy
                //fixed4 col = tex2D(_BGTex, uv);
                // for URP
                fixed4 col = tex2D(_MainTex, i.uv);
                
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
