using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtClean : MonoBehaviour
{
    [SerializeField] private Texture2D dirtMaskBase;
    [SerializeField] private Texture2D brush;
    [SerializeField] private int dirtAmountTotal;
    [SerializeField] private int cleanAmountTotal;
    private float _scaleFactor = 1.0f;
    private Texture2D _templateDirtMask;
    private Texture2D _templateDirtMaskClone;

    private Material _material;
    private string playerID="";

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        CreateTexture();
    }

    private void CreateTexture()
    {
        _templateDirtMask = new Texture2D(dirtMaskBase.width, dirtMaskBase.height);
        // int size = dirtMaskBase.width;
        // Color[] pixels = new Color[size * size];
        // for (int y = 0; y < size; y++)
        // {
        //     for (int x = 0; x < size; x++)
        //     {
        //         pixels[y * size + x] = dirtMaskBase.GetPixel(x, y);
        //     }
        // }

        _templateDirtMask.SetPixels(dirtMaskBase.GetPixels());
        _templateDirtMask.Apply();
        _material.SetTexture("_dirt_mask", _templateDirtMask);
        _templateDirtMaskClone = CopyTexture(_templateDirtMask);
        for (int i = 0; i < _templateDirtMaskClone.width; i++)
        {
            for (int j = 0; j < _templateDirtMaskClone.height; j++)
            {
                dirtAmountTotal++;
            }
        }
    }


    public void Clean(RaycastHit hit, float scalefactor, string playerID)
    {
        Vector2 textureCoord = hit.textureCoord;
        _scaleFactor = scalefactor;
        this.playerID = playerID;

        int pixelX = (int)(textureCoord.x * dirtMaskBase.width);
        int pixelY = (int)((textureCoord.y) * dirtMaskBase.height);
        int scaledWidth = (int)(brush.width * _scaleFactor);
        int scaledHeight = (int)(brush.height * _scaleFactor);
        int pixelXoffset = pixelX - (brush.width / 2);
        int pixelYoffset = pixelY - (brush.height / 2);


        for (int x = 0; x < scaledWidth; x++)
        {
            for (int y = 0; y < scaledHeight; y++)
            {
                int brushX = (int)(x / _scaleFactor);
                int brushY = (int)(y / _scaleFactor);

                int targetX = pixelXoffset + x;
                int targetY = pixelYoffset + y;
                if (targetX >= 0 && targetX < _templateDirtMask.width &&
                    targetY >= 0 && targetY < _templateDirtMask.height)
                {
                    Color pixelDirt = brush.GetPixel(brushX, brushY);
                    Color pixelDirtMask = _templateDirtMask.GetPixel(targetX, targetY);

                    _templateDirtMask.SetPixel(targetX, targetY,
                        new Color(0, pixelDirtMask.g * pixelDirt.g, 0));
                }
            }
        }

        _templateDirtMask.Apply();
        cleanAmountTotal = 0;
        for (int i = 0; i < _templateDirtMask.width; i++)
        {
            for (int j = 0; j < _templateDirtMask.height; j++)
            {
                Color pixelDirtMask = _templateDirtMask.GetPixel(i, j);
                if (pixelDirtMask.g <= 0.01f)
                {
                    cleanAmountTotal++;
                }
            }
        }
    }

    Texture2D CopyTexture(Texture2D originalTexture)
    {
        Texture2D copy = new Texture2D(originalTexture.width, originalTexture.height, originalTexture.format, false);
        Color[] pixels = originalTexture.GetPixels();
        copy.SetPixels(pixels);
        copy.Apply();
        return copy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
           GameManager.instance.CalculateScore(playerID,dirtAmountTotal - cleanAmountTotal);
        }
    }
}