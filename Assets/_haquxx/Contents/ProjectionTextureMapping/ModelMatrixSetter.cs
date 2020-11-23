using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARVJ
{
    public class ModelMatrixSetter : MonoBehaviour
    {
        [SerializeField] private Material _material;

        private void Start()
        {
            var mats = GetComponent<Renderer>().materials;
            var _arProjectionTextureMapping = FindObjectOfType<ARProjectionTextureMapping>();
            var tex = _arProjectionTextureMapping.Texture;

            foreach (var mat in mats)
            {
                if (mat.name == string.Format("{0} (Instance)", _material.name))
                {
                    mat.SetMatrix("_MatrixM", transform.localToWorldMatrix);
                    mat.SetTexture("_MainTex", tex);
                    mat.SetFloat("_ScaleFacXa", _arProjectionTextureMapping.ScaleFacXa);
                    mat.SetFloat("_ScaleFacYa", _arProjectionTextureMapping.ScaleFacYa);
                    mat.SetFloat("_ScaleFacXb", _arProjectionTextureMapping.ScaleFacXb);
                    mat.SetFloat("_ScaleFacYb", _arProjectionTextureMapping.ScaleFacYb);
                    break;
                }
            }
        }
    }
}