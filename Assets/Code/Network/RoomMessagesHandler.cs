using System;
using System.Collections.Generic;
using Code.General;
using Code.General.MessageFormatters;
using Fusion;
using UnityEngine;

namespace Code.Network
{
    public class RoomMessagesHandler : NetworkBehaviour
    {
        [SerializeField] private RoomBase _roomBase;

        private readonly List<string> _messages = new List<string>();
        private UserData _userData;
        private PlayerMessageFormatter _playerMessageFormatter;
        private NotificationMessageFormatter _notificationMessageFormatter;

        public IReadOnlyList<string> Messages => _messages;

        public event Action<string> OnMessageReceived;

        public override void Spawned()
        {
            _roomBase.RoomParticipantsHandler.LocalPlayerBeforeLeft += LocalPlayerLeft;
            _roomBase.RoomParticipantsHandler.LocalPlayerJoined += LocalPlayerJoined;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _roomBase.RoomParticipantsHandler.LocalPlayerBeforeLeft -= LocalPlayerLeft;
            _roomBase.RoomParticipantsHandler.LocalPlayerJoined -= LocalPlayerJoined;
        }

        public void Init(UserData userData)
        {
            _userData = userData;
            _notificationMessageFormatter = new NotificationMessageFormatter();
            _playerMessageFormatter = new PlayerMessageFormatter(_userData);
        }

        private void LocalPlayerLeft()
        {
            SendNotificationMessage($"{_userData.Username} left");
            _messages.Clear();
        }
        
        private void LocalPlayerJoined(RoomBase roomBase)
        {
            SendNotificationMessage($"{_userData.Username} joined");
        }

        public void SendPlayerMessage(string message)
        {
            SendMessage(message, _playerMessageFormatter);
        }

        public void SendNotificationMessage(string message)
        {
            SendMessage(message, _notificationMessageFormatter);
        }
        
        private void SendMessage(string message, IMessageFormatter messageFormatter)
        {
            if (messageFormatter != null)
            {
                message = messageFormatter.Format(message);
            }
            _messages.Add(message);
            OnMessageReceived?.Invoke(message);
            foreach (PlayerRef player in _roomBase.RoomParticipantsHandler.OtherPlayers)
            {
                RPC_SendMessage(message, player);
            }
        }
        
        public void SendMessageHistory(PlayerRef target)
        {
            foreach (var message in _messages)
            {
                RPC_SendMessage(message, target);
            }
        }
        
        [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
        private void RPC_SendMessage(string message, [RpcTarget] PlayerRef target)
        {
            _messages.Add(message);
            OnMessageReceived?.Invoke(message);
        }
    }
}