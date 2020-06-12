using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Clipping : VolumeComponent, IPostProcessComponent
{
    public IntParameter Width = new IntParameter(1920);
    public IntParameter Height = new IntParameter(1080);

    public bool IsActive()
    {
        return true;
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}
