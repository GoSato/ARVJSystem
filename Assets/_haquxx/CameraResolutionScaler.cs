using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraResolutionScaler : MonoBehaviour
{
    [Range(0.01f, 1.0f)]
    [SerializeField]
    private float _renderScale = 1.0f;

    [SerializeField]
    private FilterMode _filterMode = FilterMode.Bilinear;

    private Rect _originalRect;
    private Rect _scaledRect;

    private void OnDestroy()
    {
        Camera.main.rect = _originalRect;
    }

    private void OnPreRender()
    {
        _originalRect = Camera.main.rect;
        _scaledRect.Set(_originalRect.x, _originalRect.y, _originalRect.width * _renderScale, _originalRect.height * _renderScale);
        Camera.main.rect = _scaledRect;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Camera.main.rect = _originalRect;
        source.filterMode = _filterMode;
        Graphics.Blit(source, destination);
    }
}
