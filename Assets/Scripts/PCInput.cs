using UnityEngine;
using UnityEngine.EventSystems;

public class PCInput:Overview
{
    private Vector3 _startTouchPosition;
    private bool startedTouchOverUI;

    public override void ReadInput()
    {
        if (UnityEngine.Input.GetMouseButtonDown(0))
        {
            startedTouchOverUI = EventSystem.current.IsPointerOverGameObject();
            _startTouchPosition = UnityEngine.Input.mousePosition;
        }

        if(startedTouchOverUI)
            return;

        if (UnityEngine.Input.GetMouseButton(0))
        {
            Vector3 deltaDirection = Input.mousePosition - _startTouchPosition;
            deltaDirection *= sensitivity * Time.deltaTime;

            OnMove?.Invoke(deltaDirection);

            _startTouchPosition = Input.mousePosition;     
        }

        if (UnityEngine.Input.GetMouseButtonUp(0))
            OnMoveEnded?.Invoke();
    }

    public PCInput(float sensitivity) : base(sensitivity)
    {
    }
}