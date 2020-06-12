using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ClippingPass : ScriptableRenderPass
{
    static readonly string _renderTag = "Render Clipping Effects";
    static readonly int MainTexId = Shader.PropertyToID("_MainTex");
    static readonly int TempTargetId = Shader.PropertyToID("_TempTargetClipping");
    static readonly int ScaleFacXaId = Shader.PropertyToID("_ScaleFacXa");
    static readonly int ScaleFacYaId = Shader.PropertyToID("_ScaleFacYa");
    static readonly int ScaleFacXbId = Shader.PropertyToID("_ScaleFacXb");
    static readonly int ScaleFacYbId = Shader.PropertyToID("_ScaleFacYb");

    private RenderTargetIdentifier _currentTarget;
    private Material _clippingMaterial;
    private Clipping _clipping;

    public ClippingPass(RenderPassEvent evt)
    {
        renderPassEvent = evt;
        var shader = Shader.Find("haquxx/PostEffect/Clipping");
        if(shader == null)
        {
            Debug.LogError("Shader not found.");
            return;
        }
        _clippingMaterial = CoreUtils.CreateEngineMaterial(shader);
    }

    public void Setup(in RenderTargetIdentifier currentTarget)
    {
        _currentTarget = currentTarget;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if(_clippingMaterial == null)
        {
            Debug.LogError("Material not created.");
            return;
        }

        if (!renderingData.cameraData.postProcessEnabled) return;

        var stack = VolumeManager.instance.stack;
        _clipping = stack.GetComponent<Clipping>();
        if (_clipping == null) return;
        if (!_clipping.IsActive()) return;

        var cmd = CommandBufferPool.Get(_renderTag);
        Render(cmd, ref renderingData);
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    private void Render(CommandBuffer cmd, ref RenderingData renderingData)
    {
        ref var cameraData = ref renderingData.cameraData;
        var source = _currentTarget;
        int destination = TempTargetId;

        var w = cameraData.camera.scaledPixelWidth;
        var h = cameraData.camera.scaledPixelHeight;

        // Set property
        float ScaleFacYa = 0.0f;
        float ScaleFacYb = 0.0f;
        float ScaleFacXa = 0.0f;
        float ScaleFacXb = 0.0f;

        Vector2 textureSize = new Vector2(_clipping.Width.value, _clipping.Height.value);

        CalculateShaderUVMapping(out ScaleFacYa, out ScaleFacYb, out ScaleFacXa, out ScaleFacXb, new Vector2(w, h), textureSize.x, textureSize.y);
        SetShaderUVMapping(ScaleFacYa, ScaleFacYb, ScaleFacXa, ScaleFacXb);

        int shaderPass = 0;
        cmd.SetGlobalTexture(MainTexId, source);
        cmd.GetTemporaryRT(destination, w, h, 0, FilterMode.Point, RenderTextureFormat.Default);
        cmd.Blit(source, destination);
        cmd.Blit(destination, source, _clippingMaterial, shaderPass);
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
        _clippingMaterial.SetFloat(ScaleFacXaId, ScaleFacXa);
        _clippingMaterial.SetFloat(ScaleFacYaId, ScaleFacYa);
        _clippingMaterial.SetFloat(ScaleFacXbId, ScaleFacXb);
        _clippingMaterial.SetFloat(ScaleFacYbId, ScaleFacYb);
    }
}
