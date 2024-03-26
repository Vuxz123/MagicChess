using System;
using com.ethnicthv.Inner;
using com.ethnicthv.Outer;
using com.ethnicthv.Util.Event;
using UnityEngine;

namespace com.ethnicthv
{
    public class GameManager: MonoBehaviour
    {
        public static bool IsDebug { get; private set; }
        private static int _mainThreadId;
        
        private GameManagerInner _gameManagerInner;
        private GameManagerOuter _gameManagerOuter;

        private void Awake()
        {
            _mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            
            IsDebug = Debug.isDebugBuild;
            
            _gameManagerOuter = GameManagerOuter.instance;
            _gameManagerInner = GameManagerInner.instance;
            _gameManagerOuter.GameManagerInner = _gameManagerInner;
            _gameManagerInner.GameManagerOuter = _gameManagerOuter;
            
            EventManager.Instance.Init();
        }
        
        public static bool IsOnMainThread()
        {
            return System.Threading.Thread.CurrentThread.ManagedThreadId == _mainThreadId;
        }
    }
}