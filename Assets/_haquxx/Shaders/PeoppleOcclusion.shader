Shader "haquxx/AR/PeoppleOcclusion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EdgeThreshold ("Edge Threshold", Range(0.0, 1.0)) = 0.5
        _MosaicScale ("Mosaic Scale", float) = 25
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
            #pragma shader_feature PEOPPLE BACKGROUND
            #pragma multi_compile _ MONOCHROME NEGAPOSI PASTEL EDGE MOSAIC COLORBALANCE

            #include "UnityCG.cginc"

            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);

            sampler2D _MainTex;         // No Mask
            sampler2D _BackgroundTex;   // Mask(Peopple Occlusuion)
            sampler2D _StencilTex;
            sampler2D _DepthTex;

            float _Width;
            float _Height;
            float _EdgeThreshold;
            float _MosaicScale;

            static float redScale   = 0.298912; 
            static float greenScale = 0.586611; 
            static float blueScale  = 0.114478;
            static fixed3  monochromeScale = fixed3(redScale, greenScale, blueScale);
            
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

            float gray (fixed4 c) {
                return 0.2126 * c.r + 0.7152 * c.g + 0.0722 * c.b;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                #ifdef BACKGROUND // 背景にエフェクトをかける
                    fixed4 col = tex2D(_BackgroundTex, i.uv);
                    fixed4 effectedCol = tex2D(_MainTex, i.uv);
                    sampler2D Tex = _MainTex;
                #elif PEOPPLE
                    fixed4 col = tex2D(_MainTex, i.uv);
                    fixed4 effectedCol = tex2D(_BackgroundTex, i.uv);
                    sampler2D Tex = _BackgroundTex;
                #endif
                
                // Effect *********************************************************
             #ifdef MONOCHROME
                float monochrome = dot(effectedCol.rgb, monochromeScale);
                //fixed monochrome = (effectedCol.r + effectedCol.g + effectedCol.b) / 3;
                effectedCol = fixed4(monochrome, monochrome, monochrome, 1);
             #elif NEGAPOSI
                effectedCol = fixed4(1-effectedCol.r, 1-effectedCol.g, 1-effectedCol.b, 1);
             #elif PASTEL
                if(effectedCol.r < 0.5) effectedCol.r += 1;
                if(effectedCol.g < 0.5) effectedCol.g += 1;
                if(effectedCol.b < 0.5) effectedCol.b += 1;
             #elif EDGE
                float dx = 1.0 / _Width;
                float dy = 1.0 / _Height;
                float c00 = gray(tex2D(Tex, i.uv + float2(-dx, -dy))); 
                float c01 = gray(tex2D(Tex, i.uv + float2(-dx, 0.0))); 
                float c02 = gray(tex2D(Tex, i.uv + float2(-dx, dy))); 
                float c10 = gray(tex2D(Tex, i.uv + float2(0, -dy))); 
                float c12 = gray(tex2D(Tex, i.uv + float2(0, dy))); 
                float c20 = gray(tex2D(Tex, i.uv + float2(dx, -dy))); 
                float c21 = gray(tex2D(Tex, i.uv + float2(dx, 0.0))); 
                float c22 = gray(tex2D(Tex, i.uv + float2(dx, dy))); 

                float sx = -1.0 * c00 + -2.0 * c10 + -1.0 * c20 + 1.0 * c02 + 2.0 * c12 + 1.0 * c22;
                float sy = -1.0 * c00 + -2.0 * c01 + -1.0 * c02 + 1.0 * c20 + 2.0 * c21 + 1.0 * c22;

                float g = sqrt(sx * sx + sy * sy);

                effectedCol = g > _EdgeThreshold ? fixed4(1, 1, 1, 1) : fixed4(0, 0, 0, 1);
             #elif MOSAIC
                float2 uv2 = i.uv;
                uv2.x = floor(uv2.x * _Width / _MosaicScale) / (_Width / _MosaicScale) + (_MosaicScale / 2.0) / _Width;
                uv2.y = floor(uv2.y * _Height / _MosaicScale) / (_Height / _MosaicScale) + (_MosaicScale / 2.0) / _Height;
                effectedCol = tex2D(Tex, uv2);
             #elif COLORBALANCE
                effectedCol = fixed4(effectedCol.r*0.7, effectedCol.g*0.5, effectedCol.b, 1);
             #endif
                 // ***************************************************************

                float2 uv = i.uv;
                uv.y = 1.0 - uv.y;

                float stencil = tex2D(_StencilTex, uv).r;
                float realCameraDepth = tex2D(_DepthTex, uv).r;
                float arCameraDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
                arCameraDepth = LinearEyeDepth(arCameraDepth);

                // Render NoMaskArea
                if(stencil < 0.9)
                {
                #ifdef BACKGROUND
                    return effectedCol;
                #elif PEOPPLE
                    return col;
                #endif
                }

                // Render MaskArea
                float delta = realCameraDepth - arCameraDepth;
                if(delta < 0.0)
                {
                #ifdef BACKGROUND
                    return col;
                #elif PEOPPLE
                    return effectedCol;
                #endif
                }
                // Render NoMaskArea
                else
                {
                #ifdef BACKGROUND
                    return effectedCol;
                #elif PEOPPLE
                    return col;
                #endif
                }
            }
            ENDCG
        }
    }
}
