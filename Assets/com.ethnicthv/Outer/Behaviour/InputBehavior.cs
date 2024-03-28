using System;
using com.ethnicthv.Outer.Event.Listener;
using UnityEngine;
using Debug = com.ethnicthv.Util.Debug;

namespace com.ethnicthv.Outer.Behaviour
{
    public class InputBehavior: MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Right Clicked");
                OnSquareSelectedListener.CancelSelecting();
            }
        }
    }
}