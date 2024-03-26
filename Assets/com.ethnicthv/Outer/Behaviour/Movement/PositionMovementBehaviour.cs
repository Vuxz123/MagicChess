using com.ethnicthv.Outer.Util;
using UnityEngine;

namespace com.ethnicthv.Outer.Behaviour.Movement
{
    public class PositionMovementBehaviour : MonoBehaviour
    {
        private Vector3 _target;
        private Vector3 _start = Vector3.negativeInfinity;
        private float _distance;
        private float _walked;

        public float speed = 10;

        private void Start() => _target = transform.position;

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
            transform.position = Vector3.MoveTowards(_start, _target, EasingFunction.EaseOutQuart(_walked) * _distance);
            //Điều kiện kết thúc, nếu pos hiện tại đã đạt đến target thì reset;
            if (Vector3.Distance(transform.position, _target) < 0.001f) ResetMovement();
        }

        private void ResetMovement()
        {
            _distance = 0;
            _walked = 0;
            _start = transform.position;
        }
        
        private void RecalculateDistance() => _distance = Vector3.Distance(transform.position, _target);

        public void MoveTo(Vector3 target)
        {
            _target = target;
            ResetMovement();
            RecalculateDistance();
        }

        public void Cancel() => MoveTo(_start);
    }
}