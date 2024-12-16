using System;
using Code.Network;
using Code.View;
using Fusion;
using UnityEngine;

namespace Code.General
{
    public class GameStateHandler : MonoBehaviour
    {
        [SerializeField] private NetworkLauncher _networkLauncher;
        [SerializeField] private GameObject _roomsMenu;
        [SerializeField] private ChatView _chat;
        [SerializeField] private GameObject _connectingLabel;

        private void Start()
        {
            _networkLauncher.OnGameStarted +=  HandleGameStarted;
            _networkLauncher.OnConnectionAttempt += ShowConnectingLabel;
            _networkLauncher.OnShutdown += HandleShutdown;
        }

        private void OnDestroy()
        {
            _networkLauncher.OnGameStarted -=  HandleGameStarted;
            _networkLauncher.OnConnectionAttempt -= ShowConnectingLabel;
            _networkLauncher.OnShutdown -= HandleShutdown;
        }

        private void HandleShutdown(NetworkRunner networkRunner)
        {
            _roomsMenu.SetActive(false);
            _chat.gameObject.SetActive(false);
            _connectingLabel.SetActive(false);
        }

        private void ShowConnectingLabel(NetworkRunner networkRunner)
        {
            _connectingLabel.SetActive(true);
        }

        private void HandleGameStarted(NetworkRunner networkRunner)
        {
            _connectingLabel.SetActive(false);
            OpenRoomsBrowser();
        }

        public void OpenRoom(RoomBase roomBase)
        {  
            _roomsMenu.SetActive(false);
            _chat.Init(roomBase);
            _chat.gameObject.SetActive(true);
        }

        public void OpenRoomsBrowser()
        {
            _roomsMenu.SetActive(true);
            _chat.gameObject.SetActive(false);
        }
    }
}