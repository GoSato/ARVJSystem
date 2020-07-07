using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARVJ
{
    public class ModelMatrixSetter : MonoBehaviour
    {
        private Material _mat;

        private void Start()
        {
            _mat = GetComponent<Renderer>().material;
            _mat.SetMatrix("_MatrixM", transform.localToWorldMatrix);

            var _arProjectionTextureMapping = FindObjectOfType<ARProjectionTextureMapping>();
            var tex = _arProjectionTextureMapping.Texture;
            _mat.SetTexture("_MainTex", tex);
            _mat.SetFloat("_ScaleFacXa", _arProjectionTextureMapping.ScaleFacXa);
            _mat.SetFloat("_ScaleFacYa", _arProjectionTextureMapping.ScaleFacYa);
            _mat.SetFloat("_ScaleFacXb", _arProjectionTextureMapping.ScaleFacXb);
            _mat.SetFloat("_ScaleFacYb", _arProjectionTextureMapping.ScaleFacYb);
        }
    }
}