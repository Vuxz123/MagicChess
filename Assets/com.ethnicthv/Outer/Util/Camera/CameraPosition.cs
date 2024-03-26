using com.ethnicthv.Outer.Behaviour.Movement;
using UnityEngine;

namespace com.ethnicthv.Outer.Util.Camera
{
    public class CameraPosition
    {
        private static PositionMovementBehaviour _positionMovementBehaviour;
        private static RotationMovementBehaviour _rotationMovementBehaviour;

        public static void SetUp()
        {
            UnityEngine.Camera current;
            _positionMovementBehaviour = ((current = UnityEngine.Camera.main)!).GetComponent<PositionMovementBehaviour>();
            _rotationMovementBehaviour = current.GetComponent<RotationMovementBehaviour>();
        }
        
        private Vector3 Position { get; }

        private Quaternion Rotation { get; }

        public CameraPosition(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public void MakeCameraGoTo()
        {
            _positionMovementBehaviour.MoveTo(Position);
            _rotationMovementBehaviour.MoveTo(Rotation);
        }
    }
}