using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Clipping : VolumeComponent, IPostProcessComponent
{
    public BoolParameter Enable = new BoolParameter(false);
    public IntParameter Width = new IntParameter(1920);
    public IntParameter Height = new IntParameter(1080);

    public bool IsActive()
    {
        return Enable.value;
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}
