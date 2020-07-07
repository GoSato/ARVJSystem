Shader "haquxx/BGImageProjection/Displacement"
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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD1;
                float2  uv : TEXCOORD0;
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
                //o.screenPos = ComputeScreenPos(o.vertex); // プラットフォームの違いを吸収. 0~w
                o.screenPos.xy = o.vertex.xy / o.vertex.w; // -1~1

                o.uv.x = (_ScaleFacXa * o.screenPos.x) + _ScaleFacXb; // clipした左端~右端(0~1に収まる)
                o.uv.y = -(_ScaleFacYa * o.screenPos.y) + _ScaleFacYb; // clipした下端~上端(0~1に収まる)

                float dist = length(v.vertex.xz);
                float wave = (sin(_Time.z) + 1.0) / 2.0;
                v.vertex += fixed4(0, wave * 0.3, 0, 0) * dist;
                o.vertex = UnityObjectToClipPos(v.vertex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //half2 uv = i.screenPos.xy / i.screenPos.w; // 0~1

                // for AR(ARKit,ARCore)
                i.uv.y = 1 - i.uv.y;

                // for ARFoundation
                i.uv.x = 1 - i.uv.x;

                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}