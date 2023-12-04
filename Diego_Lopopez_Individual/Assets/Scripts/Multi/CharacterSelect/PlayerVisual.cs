using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    public SkinnedMeshRenderer surface;

    private Material material;

    private void Awake()
    {
        material = new Material(surface.material);
        surface.material = material;
    }

    public void SetPlayerColor(Color color)
    {
        material.color = color;
    }
}
