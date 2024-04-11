using System;
using com.ethnicthv.Inner;
using com.ethnicthv.Inner.Object.Piece;
using com.ethnicthv.Outer;
using com.ethnicthv.Outer.Event;
using com.ethnicthv.Outer.Event.Listener;
using com.ethnicthv.Outer.Util.Camera;
using com.ethnicthv.Util.Networking;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = com.ethnicthv.Util.Debug;
using Object = UnityEngine.Object;

namespace com.ethnicthv
{
    public class Test: MonoBehaviour
    {
        public TMP_Text cameraText;
        public TMP_Text actionText;
        
        private int _cameraPos = 1;
        
        //dirtiness
        private bool _isDirty = true;
        private void Start()
        {
            int b = 0b_1000_1000_1000_1000;
            byte a = 0b_1111_1000;
            byte c = 0b_101;
            byte test = 0b_0000_11;
		
            byte d = BytesUtil.AppendByte(c, a, 4, 3);
		
            Debug.Log($"{Convert.ToString(d, toBase: 2)}");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _cameraPos = 1;
                CameraConstance.Pos1.MakeCameraGoTo();
                _isDirty = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _cameraPos = 2;
                CameraConstance.Pos2.MakeCameraGoTo();
                _isDirty = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _cameraPos = 3;
                CameraConstance.Pos3.MakeCameraGoTo();
                _isDirty = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _cameraPos = 4;
                CameraConstance.Pos4.MakeCameraGoTo();
                _isDirty = true;
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                GameManagerInner.Instance.TestInput();
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                OnSquareSelectedListener.Action = OnSquareSelectedListener.SelectingAction.Move;
                _isDirty = true;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                OnSquareSelectedListener.Action = OnSquareSelectedListener.SelectingAction.Attack;
                _isDirty = true;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                OnSquareSelectedListener.Action = OnSquareSelectedListener.SelectingAction.Defend;
                _isDirty = true;
            }
        }

        private void FixedUpdate()
        {
            if (!_isDirty) return;
            cameraText.text = $"Camera Pos: {_cameraPos}";
            actionText.text = $"Action Type: {OnSquareSelectedListener.Action}";
            _isDirty = false;
        }
    }
}