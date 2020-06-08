using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class Reflection : MonoBehaviour
{
    [SerializeField]
    private Camera _reflectionCamera;

    private Renderer _renderer;
    private Material _mat;

    private void Start()
    {
        _reflectionCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        _renderer = GetComponent<Renderer>();
        _mat = _renderer.sharedMaterial;
        _mat.SetTexture("_RefTex", _reflectionCamera.targetTexture);

        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    }

    private void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
    }

    private void OnBeginCameraRendering(ScriptableRenderContext context, Camera cam)
    {
        //var cam = Camera.current;
        if(cam == _reflectionCamera)
        {
            var viewMat = cam.worldToCameraMatrix;
            var projMat = GL.GetGPUProjectionMatrix(cam.projectionMatrix, false);
            var modelMat = _renderer.localToWorldMatrix;
            var VP = projMat * viewMat;

            _mat.SetMatrix("_RefM", modelMat);
            _mat.SetMatrix("_RefV", viewMat);
            _mat.SetMatrix("_RefP", projMat);
            _mat.SetMatrix("_RefVP", VP);
        }
    }
}
