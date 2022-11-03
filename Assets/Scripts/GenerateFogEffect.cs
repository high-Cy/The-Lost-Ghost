using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFogEffect : MonoBehaviour
{
    public Shader fogShader;
    private Material fogMaterial = null;

    [Range(0.0f, 1.0f)]
    public float _FogDensity = 0.1f;
    public Color _FogColor = Color.gray;

    private Camera myCamera;
    private Transform myCameraTransform;

    public Camera Camera
    {
        get
        {
            if (myCamera == null)
            {
                myCamera = GetComponent<Camera>();
            }
            return myCamera;
        }
    }

    public Transform CameraTransform
    {
        get
        {
            if (myCameraTransform == null)
            {
                myCameraTransform = Camera.transform;
            }
            return myCameraTransform;
        }
    }

    public Material material
    {
        get
        {
            if (fogShader && fogMaterial)
            {
                return fogMaterial;
            }

            if (fogShader) {
                fogMaterial = new Material(fogShader);
                fogMaterial.hideFlags = HideFlags.DontSave;
                if (fogMaterial)
                    return fogMaterial;
                else return null;
            }
            return null;
        }
    }

    public void setFogDensity(float n)
    {
        _FogDensity = n;
    }


    void Start()
    {
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
        {
            material.SetFloat("_FogDensity", _FogDensity);
            material.SetColor("_FogColor", _FogColor);

            Graphics.Blit(src, dest, material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}