using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileInput : Overview
{
    private Dictionary<int, (bool,Vector2)> fingersTouchUI = new Dictionary<int, (bool,Vector2)>();
    public MobileInput(float sensitivity) : base(sensitivity)
    {
    }

    public override void ReadInput()
    {
        if(UnityEngine.Input.touchCount < 1)
            return;

        for (int i = 0; i < UnityEngine.Input.touchCount; i++)
        {
            if (UnityEngine.Input.GetTouch(i).phase == TouchPhase.Began)
            {
                if (!fingersTouchUI.ContainsKey(Input.GetTouch(i).fingerId))
                    fingersTouchUI.Add(Input.GetTouch(i).fingerId, (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId),Input.GetTouch(i).position));
                else
                    fingersTouchUI[Input.GetTouch(i).fingerId] = (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId),Input.GetTouch(i).position);
            }

            var fingerInfo = fingersTouchUI[Input.GetTouch(i).fingerId];
            if(fingerInfo.Item1)
                continue;

            if (Input.GetTouch(i).phase == TouchPhase.Moved)
            {
                Vector3 deltaDirection = Input.GetTouch(i).position - fingerInfo.Item2;
                deltaDirection *= sensitivity * Time.deltaTime;

                OnMove?.Invoke(deltaDirection);

                fingerInfo.Item2 = Input.GetTouch(i).position;
                fingersTouchUI[Input.GetTouch(i).fingerId] = fingerInfo;
            }
            if(Input.GetTouch(i).phase == TouchPhase.Ended)
                OnMoveEnded?.Invoke();
        }
    }
}