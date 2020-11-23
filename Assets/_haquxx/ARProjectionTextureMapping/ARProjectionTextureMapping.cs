﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARVJ
{
    public class ARProjectionTextureMapping : MonoBehaviour
    {
        [SerializeField]
        private ARBackgroundImage _arBackgroundImage;

        private bool _initialized = false;

        private Texture2D _texture;
        public Texture2D Texture
        {
            get { return _texture; }
            private set { _texture = value; }
        }

        private float _scaleFacXa;
        public float ScaleFacXa
        {
            get { return _scaleFacXa; }
            private set { _scaleFacXa = value; }
        }

        private float _scaleFacYa;
        public float ScaleFacYa
        {
            get { return _scaleFacYa; }
            private set { _scaleFacYa = value; }
        }

        private float _scaleFacXb;
        public float ScaleFacXb
        {
            get { return _scaleFacXb; }
            private set { _scaleFacXb = value; }
        }

        private float _scaleFacYb;
        public float ScaleFacYb
        {
            get { return _scaleFacYb; }
            private set { _scaleFacYb = value; }
        }

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

            CalculateShaderUVMapping(textureSize, Screen.width, Screen.height);
        }

        private void UpdateCameraImage(Texture2D tex)
        {
            if (!_initialized)
            {
                Init(tex);
            }
            _texture = tex;
        }

        public void CalculateShaderUVMapping(Vector2 textureSize, float screenWidth, float screenHeight)
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
    }
}