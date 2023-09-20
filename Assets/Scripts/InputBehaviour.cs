using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class InputBehaviour : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    private Overview overview;
    private void Start()
    {
#if UNITY_EDITOR
        overview = new PCInput(sensitivity);
#else
        overview = new MobileInput(sensitivity);
#endif    
    }

    private void Update()
    {
        overview.ReadInput();
    }
}