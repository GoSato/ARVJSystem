using System;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// This component tests getting the latest camera image
/// and converting it to RGBA format. If successful,
/// it displays the image on the screen as a RawImage
/// and also displays information about the image.
///
/// This is useful for computer vision applications where
/// you need to access the raw pixels from camera image
/// on the CPU.
///
/// This is different from the ARCameraBackground component, which
/// efficiently displays the camera image on the screen. If you
/// just want to blit the camera texture to the screen, use
/// the ARCameraBackground, or use Graphics.Blit to create
/// a GPU-friendly RenderTexture.
///
/// In this example, we get the camera image data on the CPU,
/// convert it to an RGBA format, then display it on the screen
/// as a RawImage texture to demonstrate it is working.
/// This is done as an example; do not use this technique simply
/// to render the camera image on screen.
/// </summary>
public class DepthImageComposition : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The AROcclusionManager which will produce frame events.")]
    AROcclusionManager m_OcclusionManager;

     /// <summary>
    /// Get or set the <c>AROcclusionManager</c>.
    /// </summary>
    public AROcclusionManager occlusionManager
    {
        get { return m_OcclusionManager; }
        set { m_OcclusionManager = value; }
    }

    [SerializeField]
    RawImage m_RawImageStencil;

    /// <summary>
    /// The UI RawImage used to display the image on screen.
    /// </summary>
    public RawImage rawImageStencil
    {
        get { return m_RawImageStencil; }
        set { m_RawImageStencil = value; }
    }

    [SerializeField]
    RawImage m_RawImageDepth;

    /// <summary>
    /// The UI RawImage used to display the image on screen.
    /// </summary>
    public RawImage rawImageDepth
    {
        get { return m_RawImageDepth; }
        set { m_RawImageDepth = value; }
    }

    [SerializeField]
    RawImage m_RawImageBackground;

    /// <summary>
    /// The UI RawImage used to display the image on screen.
    /// </summary>
    public RawImage rawImageBackground
    {
        get { return m_RawImageBackground; }
        set { m_RawImageBackground = value; }
    }

    [SerializeField]
    private RenderTexture _backgroundRT;

    private ARCameraBackground _arCameraBackground;

    [SerializeField]
    private Material _peoppleOcclusionMat;

    [SerializeField]
    Text m_ImageInfo;

    /// <summary>
    /// The UI Text used to display information about the image on screen.
    /// </summary>
    public Text imageInfo
    {
        get { return m_ImageInfo; }
        set { m_ImageInfo = value; }
    }

    void LogTextureInfo(StringBuilder stringBuilder, string textureName, Texture2D texture)
    {
        stringBuilder.AppendFormat("texture : {0}\n", textureName);
        if (texture == null)
        {
            stringBuilder.AppendFormat("   <null>\n");
        }
        else
        {
            stringBuilder.AppendFormat("   format : {0}\n", texture.format.ToString());
            stringBuilder.AppendFormat("   width  : {0}\n", texture.width);
            stringBuilder.AppendFormat("   height : {0}\n", texture.height);
            stringBuilder.AppendFormat("   mipmap : {0}\n", texture.mipmapCount);
        }
    }

    private void Start()
    {
        m_RawImageBackground.texture = _backgroundRT;
        _arCameraBackground = FindObjectOfType<ARCameraBackground>();
        Camera.main.depthTextureMode |= DepthTextureMode.Depth;

        _peoppleOcclusionMat.SetFloat("_Width", Screen.width);
        _peoppleOcclusionMat.SetFloat("_Height", Screen.height);
        Debug.LogFormat("Screen Width : Height, {0}:{1}", Screen.width, Screen.height);
    }

    void Update()
    {
        Debug.Assert(m_OcclusionManager != null, "no occlusion manager");
        var subsystem = m_OcclusionManager.subsystem;
        if (subsystem == null)
        {
            if (m_ImageInfo != null)
            {
                m_ImageInfo.text = "Human Segmentation not supported.";
            }
            return;
        }

        StringBuilder sb = new StringBuilder();
        Texture2D humanStencil = m_OcclusionManager.humanStencilTexture;
        Texture2D humanDepth = m_OcclusionManager.humanDepthTexture;
        LogTextureInfo(sb, "stencil", humanStencil);
        LogTextureInfo(sb, "depth", humanDepth);

        if (m_ImageInfo != null)
        {
            m_ImageInfo.text = sb.ToString();
        }
        else
        {
            Debug.Log(sb.ToString());
        }

        // To use the stencil, be sure the HumanSegmentationStencilMode property on the AROcclusionManager is set to a
        // non-disabled value.
        m_RawImageStencil.texture = humanStencil;

        // To use the depth, be sure the HumanSegmentationDepthMode property on the AROcclusionManager is set to a
        /// non-disabled value.
        m_RawImageDepth.texture = humanDepth;

        _peoppleOcclusionMat.SetTexture("_BackgroundTex", _backgroundRT);
        _peoppleOcclusionMat.SetTexture("_StencilTex", humanStencil);
        _peoppleOcclusionMat.SetTexture("_DepthTex", humanDepth);
    }

    private void LateUpdate()
    {
        if (_arCameraBackground.material != null)
        {
            // Blit screen display to render texture
            Graphics.Blit(null, _backgroundRT, _arCameraBackground.material);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _peoppleOcclusionMat);
    }
}
