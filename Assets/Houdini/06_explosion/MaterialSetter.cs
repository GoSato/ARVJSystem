using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MaterialSetter : MonoBehaviour
{
    [SerializeField] private Material _insideMat;
    [SerializeField] private Material _outsideMat;

    [ContextMenu("Set Material")]
    private void SetMaterial()
    {
        var insideMatName = _insideMat.name;
        var outsideMatName = _outsideMat.name;

        var meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in meshRenderers)
        {
            var materials = renderer.sharedMaterials;
            var mats = new Material[2];

            if (materials[0].name == insideMatName)
            {
                mats[0] = _insideMat;
                mats[1] = _outsideMat;
            }
            else
            {
                mats[0] = _outsideMat;
                mats[1] = _insideMat;
            }
            renderer.materials = mats;
        }
    }
}
