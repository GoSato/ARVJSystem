using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ClippingRenderFeature : ScriptableRendererFeature
{
    private ClippingPass clippingPass;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        clippingPass.Setup(renderer.cameraColorTarget);
        renderer.EnqueuePass(clippingPass);
    }

    public override void Create()
    {
        clippingPass = new ClippingPass(RenderPassEvent.BeforeRenderingPostProcessing);
    }
}
