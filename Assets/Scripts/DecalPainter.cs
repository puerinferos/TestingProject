using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalPainter : MonoBehaviour
{
    [SerializeField] private int maxDecals;
    [SerializeField] private float decalSizeScaler;
    [SerializeField] private Texture2D stampTexture;
    private Renderer renderer;
    private float posX = 256f;
    private float posY = 256f;
    RenderTexture rt;
    private List<Vector2> stampPositions = new List<Vector2>();

    void Start()
    {
        renderer = GetComponent<Renderer>();
        rt = new RenderTexture(512, 512, 32);
        renderer.material.SetTexture("_BaseMap", rt);
        RenderTexture.active = rt;
        GL.Clear(true,true,Color.white);
        RenderTexture.active = null;

    }

    public void DrawDecal(Vector2 rectPosition)
    {
        RenderTexture.active = rt;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, 512, 512, 0);

        posX = rectPosition.x * rt.width;
        posY = rectPosition.y * rt.height;
        stampPositions.Add(new Vector2(posX, posY));
        
        if (stampPositions.Count < maxDecals)
            Graphics.DrawTexture(
                new Rect(posX - (stampTexture.width*decalSizeScaler) / 2, (rt.height - posY) - (stampTexture.height*decalSizeScaler) / 2,
                    stampTexture.width*decalSizeScaler, stampTexture.height*decalSizeScaler), stampTexture);    
        else
        {
            GL.Clear(true,true,Color.white);
            stampPositions.RemoveAt(0);
            for (int i = 0; i < stampPositions.Count; i++)
            {
                Graphics.DrawTexture(
                    new Rect(stampPositions[i].x - (stampTexture.width*decalSizeScaler) / 2, (rt.height - stampPositions[i].y) - (stampTexture.height*decalSizeScaler) / 2,
                        stampTexture.width*decalSizeScaler, stampTexture.height*decalSizeScaler), stampTexture);    
            } 
        }

        GL.PopMatrix();
        RenderTexture.active = null;
    }
}