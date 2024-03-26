using System;
using com.ethnicthv.Inner;
using com.ethnicthv.Outer;
using com.ethnicthv.Util.Event;
using UnityEngine;

namespace com.ethnicthv
{
    public class GameManager: MonoBehaviour
    {
        private GameManagerInner _gameManagerInner;
        private GameManagerOuter _gameManagerOuter;

        private void Awake()
        {
            _gameManagerOuter = GameManagerOuter.instance;
            _gameManagerInner = GameManagerInner.instance;
            _gameManagerOuter.GameManagerInner = _gameManagerInner;
            _gameManagerInner.GameManagerOuter = _gameManagerOuter;
            
            EventManager.Instance.Init();
        }
    }
}