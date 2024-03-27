using UnityEngine;

namespace com.ethnicthv.Outer.Util.Camera
{
    public class CameraConstance
    {
        public static readonly CameraPosition Pos1 = new CameraPosition(new Vector3(0, 9, -10.57f), Quaternion.Euler(26, 0, 0));

        public static readonly CameraPosition Pos2 = new CameraPosition(new Vector3(0, 9, 10.57f), Quaternion.Euler(26, 180, 0));
        
        public static readonly CameraPosition Pos3 = new CameraPosition(new Vector3(0, 12.8f, 0), Quaternion.Euler(90, 0, 0));
        
        public static readonly CameraPosition Pos4 = new CameraPosition(new Vector3(0, 12.8f, 0), Quaternion.Euler(90, 180, 0));

    }
}