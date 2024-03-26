using UnityEngine;

namespace com.ethnicthv.Outer.Behaviour.Movement
{
    public class ModifiablePositionMovementBehaviour: MonoBehaviour
    {
        private Vector3 _target;
        private Vector3 _start = Vector3.negativeInfinity;
        private float _distance;
        private float _walked;

        public float speed = 10;
        
        private InterpolationFunction _function = InterpolationFunctions.Linear;

        private void Start()
        {
            _target = transform.position;
        }

        private void FixedUpdate()
        {
            //Điều kiện cửa đảm bảo nếu đã tới nơi thì ko làm gì nữa;
            if (Vector3.Distance(transform.position, _target) < 0.001f) return;

            //-- Mở đầu Code chính --

            //Tính toán delta là bước đi hiện tại của camera.
            var delta = Time.deltaTime * speed;
            //Add vào _walked để tính khoảng cách đã đi được.
            _walked += delta;
            //Tính toán vị trí hiện tại rồi set vào pos để dịch chuyển vị trí.
            transform.position = _function(_start, _target, _walked, _distance, delta);
            //Điều kiện kết thúc, nếu pos hiện tại đã đạt đến target thì reset;
            if (Vector3.Distance(transform.position, _target) < 0.001f) ResetMovementAtTarget();
        }

        private void ResetMovement(InterpolationFunction function = null)
        {
            _distance = 0;
            _walked = 0;
            _start = transform.position;
        }

        private void ResetMovementAtTarget()
        {
            transform.position = _target;
            ResetMovement();
        }
        
        private void RecalculateDistance() => _distance = Vector3.Distance(transform.position, _target);

        public void MoveTo(Vector3 target, float s, InterpolationFunction function = null)
        {
            if(function != null) _function = function;
            speed = s;
            MoveTo(target);
        }
        
        public void MoveTo(Vector3 target)
        {
            _target = target;
            ResetMovement();
            RecalculateDistance();
        }

        public void Cancel() => MoveTo(_start);
    }
}