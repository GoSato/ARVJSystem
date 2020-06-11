using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARVJ
{
    public class ARProjectionTextureMapping : MonoBehaviour
    {
        [SerializeField]
        private Material _mat;

        [SerializeField]
        private ARBackgroundImage _arBackgroundImage;

        private bool _initialized = false;

        private void Start()
        {
            _arBackgroundImage.OnCameraUpdate += UpdateCameraImage;
        }

        private void OnDestroy()
        {
            _arBackgroundImage.OnCameraUpdate -= UpdateCameraImage;
        }

        private void Init(Texture2D tex)
        {
            // ARLibraryからカメラ画像取得

            Vector2 textureSize = new Vector2(tex.width, tex.height);

            float ScaleFacYa = 0.0f;
            float ScaleFacYb = 0.0f;
            float ScaleFacXa = 0.0f;
            float ScaleFacXb = 0.0f;

            CalculateShaderUVMapping(out ScaleFacYa, out ScaleFacYb, out ScaleFacXa, out ScaleFacXb, textureSize, Screen.width, Screen.height);
            SetShaderUVMapping(ScaleFacYa, ScaleFacYb, ScaleFacXa, ScaleFacXb);
        }

        private void UpdateCameraImage(Texture2D tex)
        {
            if (!_initialized)
            {
                Init(tex);
            }
            _mat.SetTexture("_MainTex", tex);
        }

        public void CalculateShaderUVMapping(out float ScaleFacYa, out float ScaleFacYb, out float ScaleFacXa, out float ScaleFacXb, Vector2 textureSize, float screenWidth, float screenHeight)
        {

            float cameraTextureAspect = textureSize.y / textureSize.x;
            float screenAspect = screenHeight / screenWidth;

            // 画面がカメラより横に長い
            if (cameraTextureAspect > screenAspect)
            {
                float clippedHeightFac = screenAspect / cameraTextureAspect;
                float scaleFactorYbottom = (1.0f - clippedHeightFac) / 2.0f; // clippingした下端
                float scaleFactorYtop = scaleFactorYbottom + clippedHeightFac; // clippingした上端

                ScaleFacYa = (scaleFactorYtop - scaleFactorYbottom) / 2.0f;
                ScaleFacYb = ((scaleFactorYtop - scaleFactorYbottom) / 2.0f) + scaleFactorYbottom;
                ScaleFacXa = 0.5f;
                ScaleFacXb = 0.5f;
            }
            // 画面がカメラより縦に長い
            else if (cameraTextureAspect < screenAspect)
            {
                float clippedWidthFac = cameraTextureAspect / screenAspect;
                float scaleFactorXLeft = (1.0f - clippedWidthFac) / 2.0f; // clippingした左端
                float scaleFactorXRight = scaleFactorXLeft + clippedWidthFac; // clippingした右端

                ScaleFacXa = (scaleFactorXRight - scaleFactorXLeft) / 2.0f;
                ScaleFacXb = ((scaleFactorXRight - scaleFactorXLeft) / 2.0f) + scaleFactorXLeft;
                ScaleFacYa = 0.5f;
                ScaleFacYb = 0.5f;
            }
            else
            {
                ScaleFacXa = 0.5f;
                ScaleFacXb = 0.5f;
                ScaleFacYa = 0.5f;
                ScaleFacYb = 0.5f;
            }
        }


        public void SetShaderUVMapping(float ScaleFacYa, float ScaleFacYb, float ScaleFacXa, float ScaleFacXb)
        {
            _mat.SetFloat("_ScaleFacXa", ScaleFacXa);
            _mat.SetFloat("_ScaleFacYa", ScaleFacYa);
            _mat.SetFloat("_ScaleFacXb", ScaleFacXb);
            _mat.SetFloat("_ScaleFacYb", ScaleFacYb);
        }
    }
}