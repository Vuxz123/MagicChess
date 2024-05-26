using System;
using com.ethnicthv.Inner;
using com.ethnicthv.Networking;
using com.ethnicthv.Other.Config;
using com.ethnicthv.Other.Ev;
using com.ethnicthv.Outer;
using UnityEngine;

namespace com.ethnicthv
{
    public class GameManager: MonoBehaviour
    {
        public static bool IsDebug { get; private set; }
        private static int _mainThreadId;
        
        private static GameManagerInner _gameManagerInner;
        private static GameManagerOuter _gameManagerOuter;
        
        private static bool _isInitialized = false;

        private void Awake()
        {
            if(_isInitialized) return;
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
            _isInitialized = true;
        }

        private void FixedUpdate()
        {
            SafeMechanism.Instance.DrainDispatchQueue();
            NetworkManager.Instance.Tick();
        }

        private void OnApplicationQuit()
        {
            NetworkManager.Instance.Dispose();
        }

        public static bool IsOnMainThread()
        {
            return System.Threading.Thread.CurrentThread.ManagedThreadId == _mainThreadId;
        }
    }
}