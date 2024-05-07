using System;
using com.ethnicthv.Inner;
using com.ethnicthv.Other.Config;
using com.ethnicthv.Other.Ev;
using com.ethnicthv.Other.Networking;
using com.ethnicthv.Outer;
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
            
            #pragma warning disable CS0618 // Type or member is obsolete
            _gameManagerOuter = GameManagerOuter.instance;
            _gameManagerInner = GameManagerInner.instance;
            #pragma warning restore CS0618 // Type or member is obsolete
            
            _gameManagerOuter.GameManagerInner = _gameManagerInner;
            _gameManagerInner.GameManagerOuter = _gameManagerOuter;
            
            ConfigProvider.Init();
            
            EventManager.Instance.Init();
            
            SafeMechanism.Init();
            
            NetworkManager.Instance.Init();
        }

        private void FixedUpdate()
        {
            SafeMechanism.Instance.DrainDispatchQueue();
        }

        public static bool IsOnMainThread()
        {
            return System.Threading.Thread.CurrentThread.ManagedThreadId == _mainThreadId;
        }
    }
}