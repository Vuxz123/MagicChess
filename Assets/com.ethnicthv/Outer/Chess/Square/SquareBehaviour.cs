using System;
using System.Threading;
using com.ethnicthv.Outer.Util;
using UnityEngine;

namespace com.ethnicthv.Outer.Chess.Square
{
    public class SquareBehaviour: MonoBehaviour, ISquare
    {
        private CbPos _pos;
        private CbType _type;
        private GameObject _highlight;
        private MeshRenderer _renderer;
        
        private bool _isSelected;
        private Color _highlightColor;
        private int _isDirty;
        
        private int _emmisionColor = Shader.PropertyToID("Emmission Color");

        private void Start()
        {
            if (_highlight != null) return;
            _highlight = Instantiate(Resources.Load<GameObject>("Prefab/Chess/Board/Cell"));
            _highlight.transform.localScale = Vector3.one * 1.23f;
            _renderer = _highlight.GetComponent<MeshRenderer>();
            var transform1 = transform;
            var position = transform1.position;
            _highlight.transform.position = new Vector3(position.x,2.9f,position.z);
            _highlight.SetActive(false);
        }

        private void Update()
        {
            if (!IsDirty()) return;
            SetHighlightColor(_highlightColor);
            _highlight.SetActive(_isSelected);
        }

        public void SetPos(int x, int y)
        {
            _pos = new CbPos
            {
                X = x, Y = y
            };
            _type = (x + y) % 2 == 0 ? CbType.Black : CbType.White;
        }
        
        private void SetHighlightColor(Color color)
        {
            _renderer.material.SetColor(_emmisionColor, color);
        }
        
        /// <summary>
        /// Highlight the square. <br/>
        /// </summary>
        /// <param name="color"> Color of the highlight </param>
        public void Highlight(Color color)
        {
            MarkDirty();
            _isSelected = true;
            _highlightColor = color;
        }

        /// <summary>
        /// Unhighlight the square. <br/>
        /// </summary>
        public void UnHighlight()
        {
            if (!_isSelected) return;
            MarkDirty();
            _isSelected = false;
        }
        
        /// <summary>
        /// Mark the object as dirty. <br/>
        /// This method is thread safe.
        /// </summary>
        public void MarkDirty()
        {
            //prevent multiple threads from accessing the same variable
            Interlocked.Exchange(ref _isDirty, 1);
        }

        /// <summary>
        /// Check if the object is dirty. <br/>
        /// This action must be performed by the main thread only, if u use this method in another thread, pls make sure of what u gonna do. <br/>
        /// After calling this method, the object will be marked as clean. <br/>
        /// This method is thread safe. <br/>
        /// </summary>
        /// <returns>
        /// <list type="dotted">
        ///     <item> <description>true: the object is dirty</description> </item>
        ///     <item> <description>false: the object is clean</description> </item>
        /// </list>
        /// </returns>
        public bool IsDirty()
        {
            //prevent multiple threads from accessing the same variable
            return Interlocked.CompareExchange(ref _isDirty, 0, 1) == 1;
        }

        public CbPos Pos => _pos;
        public CbType Type => _type;

        public override string ToString()
        {
            return $"{nameof(_pos)}: {_pos}, {nameof(_type)}: {_type}";
        }
    }
}