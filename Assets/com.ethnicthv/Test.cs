using com.ethnicthv.Inner;
using com.ethnicthv.Outer;
using com.ethnicthv.Outer.Util.Camera;
using UnityEngine;

namespace com.ethnicthv
{
    public class Test: MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CameraConstance.Pos1.MakeCameraGoTo();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CameraConstance.Pos2.MakeCameraGoTo();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CameraConstance.Pos3.MakeCameraGoTo();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                CameraConstance.Pos4.MakeCameraGoTo();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                GameManagerInner.Instance.TestInput();
            }
        }
    }
}