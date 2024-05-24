using System;
using System.Diagnostics;
using System.Linq;
using com.ethnicthv.Inner;
using com.ethnicthv.Inner.Event;
using com.ethnicthv.Inner.Object.Piece;
using com.ethnicthv.Networking;
using com.ethnicthv.Other.Network.Client.P;
using com.ethnicthv.Outer;
using com.ethnicthv.Outer.Event;
using com.ethnicthv.Outer.Event.Listener;
using com.ethnicthv.Outer.Util.Camera;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = com.ethnicthv.Other.Debug;
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
            //TestPacketWriterReader();

            TestNetworkConversion();
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
        
        private void TestPacketWriterReader()
        {
            var a = new byte[]
            {
                0b_0111_0001,
                0b_1110_0001,
            };
            var v = new byte[]
            {
                0b_0000_0000,
                0b_1100_0111,
                0b_1101_1010,
                0b_1111_1001,
            };
            
            const byte add = 0b_111;
            
            // var temp = BytesUtil.GetByte(a[0] , 6, 2, true);
            // Debug.Log("Bytes: " + Convert.ToString(temp, 2).PadLeft(8, '0'));
            
            // var temp = BytesUtil.AppendBytes(a , v, 10, 16);
            // Debug.Log("Bytes: \n" + string.Join("\n", temp.Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))));
            {
                System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                var packet = PacketWriter.Create()
                    .Write((byte)10)
                    .Write(false)
                    .Write(true)
                    .Write((short) 3103)
                    .Write((byte) Byte.MaxValue)
                    .GetPacket();
                Debug.Log(watch.Elapsed);
                // Debug.Log("Packet: \n" + string.Join("\n", packet.GetBytes().Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))));
            
                watch = System.Diagnostics.Stopwatch.StartNew();
                var reader = PacketReader.Create(packet);
                var temp1 = reader.ReadByte();
                var temp2 = reader.ReadBool();
                var temp3 = reader.ReadBool();
                var temp4 = reader.ReadShort();
                var temp5 = reader.ReadByte();
                Debug.Log(watch.Elapsed);
                reader.Close();
            }
            // Debug.Log($"Read: {temp1}");
            // Debug.Log("Read: " + temp2);
            // Debug.Log("Read: " + temp3);
            // Debug.Log("Read: " + temp4);
            // Debug.Log("Read: " + temp5);
            
            {
                System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                var packet = PacketWriter.Create()
                    .Write((byte)10)
                    .Write(false)
                    .Write(true)
                    .Write((short) 3103)
                    .Write((byte) Byte.MaxValue)
                    .GetPacket();
                Debug.Log(watch.Elapsed);
                // Debug.Log("Packet: \n" + string.Join("\n", packet.GetBytes().Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))));
            
                watch = System.Diagnostics.Stopwatch.StartNew();
                var reader = PacketReader.Create(packet);
                var temp1 = reader.ReadByte();
                var temp2 = reader.ReadBool();
                var temp3 = reader.ReadBool();
                var temp4 = reader.ReadShort();
                var temp5 = reader.ReadByte();
                Debug.Log(watch.Elapsed);
                reader.Close();
            }
            
            {
                System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                var packet = PacketWriter.Create()
                    .Write((byte)10)
                    .Write(false)
                    .Write(true)
                    .Write((short) 3103)
                    .Write((byte) Byte.MaxValue)
                    .GetPacket();
                Debug.Log(watch.Elapsed);
                // Debug.Log("Packet: \n" + string.Join("\n", packet.GetBytes().Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))));
            
                watch = System.Diagnostics.Stopwatch.StartNew();
                var reader = PacketReader.Create(packet);
                var temp1 = reader.ReadByte();
                var temp2 = reader.ReadBool();
                var temp3 = reader.ReadBool();
                var temp4 = reader.ReadShort();
                var temp5 = reader.ReadByte();
                Debug.Log(watch.Elapsed);
                reader.Close();
            }

            {
                System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                for (int i = 0; i < 100000; i++)
                {
                    var packet = PacketWriter.Create()
                        .Write((byte)10)
                        .Write(false)
                        .Write(true)
                        .Write((short) 3103)
                        .Write((byte) Byte.MaxValue)
                        .GetPacket();
            
                    var reader = PacketReader.Create(packet);
                    var temp1 = reader.ReadByte();
                    var temp2 = reader.ReadBool();
                    var temp3 = reader.ReadBool();
                    var temp4 = reader.ReadShort();
                    var temp5 = reader.ReadByte();
                    reader.Close();
                }
                Debug.Log(watch.Elapsed);
            }
        }

        private void TestNetworkConversion()
        {
            for (int v = 0; v < 10; v++)
            {
                var ev = new ChessBoardEvent(ChessBoardEvent.EventType.Check);
                var i = NetworkManager.Instance;
                Stopwatch watch = Stopwatch.StartNew();
                var packet = i.PacketizeEvent(e: ev);
                Debug.Log(watch.Elapsed);
                watch = Stopwatch.StartNew();
                var nev = i.ResolvePacket(packet);
                Debug.Log(watch.Elapsed);
            }
        }
    }
}