using System;
using Fusion;
using UnityEngine;

namespace Code.General
{
    public class NetworkLauncher : MonoBehaviour
    {
        [SerializeField] private NetworkRunner _runner;
        [SerializeField] private NetworkEvents _networkEvents;
        
        public event Action<NetworkRunner> OnGameStarted = delegate { }; 
        public event Action<NetworkRunner> OnConnectionAttempt; 
        public event Action<NetworkRunner> OnShutdown; 

        private void Start()
        {
            StartNetwork();
            _networkEvents.OnShutdown.AddListener(HandleShutdown);
        }

        private void OnDestroy()
        {
            _networkEvents.OnShutdown.RemoveListener(HandleShutdown);
        }

        private void HandleShutdown(NetworkRunner runner, ShutdownReason reason)
        {
            Debug.LogError($"Disconnected. reason: {reason}");
            OnShutdown?.Invoke(runner);
        }

        private void StartNetwork()
        {
            OnConnectionAttempt?.Invoke(_runner);
            _runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                OnGameStarted = OnGameStarted.Invoke
            });
        }
    }
}