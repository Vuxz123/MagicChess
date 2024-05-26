using System;
using com.ethnicthv.Inner;
using com.ethnicthv.Outer.Util;
using UnityEngine;
using Debug = com.ethnicthv.Other.Debug;

namespace com.ethnicthv.Outer.Behaviour.Chess.Square
{
    public class SquareBehaviour: OuterObjectAbstract, ISquare
    {
        private CbPos _pos;
        private CbType _type;
        private GameObject _highlight;
        private MeshRenderer _highlightRenderer;
        
        private bool _isSelected;
        private Color _highlightColor;
        private int _isDirty;
        
        private readonly int _emissionColor = Shader.PropertyToID("_Emission_Color");

        private void Start()
        {
            if (_highlight != null) return;
            _highlight = Instantiate(Resources.Load<GameObject>("Prefab/Chess/Board/Cell"));
            _highlight.transform.localScale = Vector3.one * 1.23f;
            _highlightRenderer = _highlight.GetComponent<MeshRenderer>();
            var transform1 = transform;
            var position = transform1.position;
            _highlight.transform.position = new Vector3(position.x,2.9075f,position.z);
            _highlight.SetActive(false);
        }

        public void Update()
        {
            BaseUpdate();
        }

        protected override void Cleaning()
        {
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
            _highlightRenderer.material.SetColor(_emissionColor, color);
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
        /// Check if the square has a piece. <br/>
        /// </summary>
        /// <returns>  
        /// <list type="bullet">
        ///     <item> <description>true: the square has a piece</description> </item>
        ///     <item> <description>false: the square is empty</description> </item>
        /// </list>
        /// </returns>
        public bool HasPiece()
        {
            return GameManagerInner.Instance.Board[GameManagerInner.ConvertOuterToInnerPos(_pos)] != null;
        }

        public CbPos Pos => _pos;
        public CbType Type => _type;

        public override string ToString()
        {
            return $"{nameof(_pos)}: {_pos}, {nameof(_type)}: {_type}";
        }
    }
}