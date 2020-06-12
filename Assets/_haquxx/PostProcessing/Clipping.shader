Shader "haquxx/PostEffect/Clipping"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _ScaleFacXa("ScaleFacXa", Float) = 0
        _ScaleFacYa("ScaleFacYa", Float) = 0
        _ScaleFacXb("ScaleFacXb", Float) = 0
        _ScaleFacYb("ScaleFacYb", Float) = 0
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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _ScaleFacXa;
            float _ScaleFacYa;
            float _ScaleFacXb;
            float _ScaleFacYb;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // -w~w
                o.uv = v.uv;
                o.uv = o.uv * 2.0 - 1.0; // -1~1
                o.uv.x = (_ScaleFacXa * o.uv.x) + _ScaleFacXb; // clipした左端~右端(0~1に収まる)
                o.uv.y = (_ScaleFacYa * o.uv.y) + _ScaleFacYb; // clipした下端~上端(0~1に収まる)
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}