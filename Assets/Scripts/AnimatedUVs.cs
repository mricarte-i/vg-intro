using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedUVs : MonoBehaviour
{
    public int  materialIndex = 0;
    public Vector2 uvAnimationRate = new Vector2(1.0f, 0.0f);
    public string textureName = "_MainTex";

    private Vector2 _uvOffset = Vector2.zero;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void LateUpdate()
    {
        _uvOffset += (uvAnimationRate * Time.deltaTime);
        if (_renderer.enabled)
        {
            _renderer.materials[materialIndex].SetTextureOffset(textureName, _uvOffset);
        }
    }
}
