using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARVJ
{
    public class ARFakeIBL : MonoBehaviour
    {
        [SerializeField]
        private Material _skyboxMat;
        [SerializeField]
        private ARBackgroundImage _arBackgroundImage;

        private void Start()
        {
            _arBackgroundImage.OnCameraUpdate += UpdateCameraImage;
        }

        private void OnDestroy()
        {
            _arBackgroundImage.OnCameraUpdate -= UpdateCameraImage;
        }

        public void UpdateCameraImage(Texture2D tex)
        {
            // Update Skybox
            _skyboxMat.SetTexture("_MainTex", tex);
            // Update Environment Lighting
            DynamicGI.UpdateEnvironment();
        }
    }
}