using com.ethnicthv.Outer.Util;
using UnityEngine;

namespace com.ethnicthv.Outer.Movement
{
    public class RotationMovementBehaviour : MonoBehaviour
    {
        private Quaternion _target;
        private Quaternion _start = Quaternion.Euler(new Vector3(0, 0, 0));
        private float _distance;
        private float _walked;

        public float speed = 10;

        private void Start()
        {
            _target = transform.rotation;
        }

        private void FixedUpdate()
        {
            //Điều kiện cửa đảm bảo nếu đã tới nơi thì ko làm gì nữa;
            if (transform.rotation == _target) return;

            //-- Mở đầu Code chính --
            //Nếu _distance là 0 (tức là chưa bắt đầu quá trình di chuển) thì:
            //- đặt _start là pos hiện tại.
            //- set _distance thành khoảng cách để đi đến mục tiêu.
            if (_distance == 0)
            {
                _start = transform.rotation;
                _distance = Quaternion.Angle(_start, _target);
                _walked = 0;
            }

            //Tính toán delta là bước đi hiện tại của camera.
            var delta = Time.deltaTime * speed;
            //Add vào _walked để tính khoảng cách đã đi được.
            _walked += delta;
            //Tính toán vị trí hiện tại rồi set vào pos để dịch chuyển vị trí.
            transform.rotation =
                Quaternion.RotateTowards(_start, _target, EasingFunction.EaseOutQuartic(_walked) * _distance);
            //Điều kiện kết thúc, nếu pos hiện tại đã đạt đến target thì reset;
            if (transform.rotation == _target) ResetMovement();
        }

        private void ResetMovement() => _distance = 0;

        public void MoveTo(Quaternion target)
        {
            _target = target;
            ResetMovement();
        }

        public void Cancel() => MoveTo(_start);
    }
}