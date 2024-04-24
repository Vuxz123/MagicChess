using System;
using System.Linq;
using com.ethnicthv.Inner;
using com.ethnicthv.Inner.Object.Piece;
using com.ethnicthv.Outer;
using com.ethnicthv.Outer.Event;
using com.ethnicthv.Outer.Event.Listener;
using com.ethnicthv.Outer.Util.Camera;
using com.ethnicthv.Util.Networking.Packet;
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
            var a = new byte[]
            {
                0b_0111_0001,
                0b_1110_0001,
            };
            var v = new byte[]
            {
                0b_0000_0000,
                0b_0000_0111,
                0b_1101_1010,
                0b_1111_1001,
            };

            const byte add = 0b_111;
            
            // var temp = BytesUtil.AppendBytes(a , v, 0, 11);
            // Debug.Log("Bytes: \n" + string.Join("\n", temp.Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))));

            var packet = PacketWriter.Create().Write((byte)10).Write(false).Write(true).Write((short) 3103).GetPacket();
            Debug.Log("Packet: \n" + string.Join("\n", packet.GetBytes().Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))));
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