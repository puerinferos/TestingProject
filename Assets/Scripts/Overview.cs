using System;
using UnityEngine;

public abstract class Overview
{
    public static Action<Vector3> OnMove;
    public static Action OnMoveEnded;
    
    protected float sensitivity;
    public Vector2 InputValue;

    public Overview(float sensitivity)
    {
        this.sensitivity = sensitivity;
    }
    public abstract void ReadInput();
}