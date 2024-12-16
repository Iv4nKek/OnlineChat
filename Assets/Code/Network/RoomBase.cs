using System;
using Code.General;
using Fusion;
using UnityEngine;

namespace Code.Network
{
    [RequireComponent(typeof(RoomMessagesHandler))]
    public class RoomBase : NetworkBehaviour
    {
        [SerializeField] private RoomParticipantsHandler _roomParticipantsHandler;
        [SerializeField] private RoomMessagesHandler _roomMessagesHandler;
        
        [Networked, OnChangedRender(nameof(NameChanged))] 
        public string RoomName { get; set; }
        
        public RoomParticipantsHandler RoomParticipantsHandler => _roomParticipantsHandler;
        public RoomMessagesHandler RoomMessagesHandler => _roomMessagesHandler;
        
        public event Action<string> RoomNameChanged;
        public event Action RoomRemoved;

        void NameChanged()
        {
            RoomNameChanged?.Invoke(RoomName);
        }

        public override void Spawned()
        {
           RoomRegistry.RegisterRoom(this);
        }

        public void JoinRoom()
        {
            _roomParticipantsHandler.JoinRoom();
        }
        public void LeftRoom()
        {
            _roomParticipantsHandler.LeftRoom();
        }

        public void RemoveRoom()
        {
            if (!HasStateAuthority)
            {
                Debug.LogError($"Error removing room: You are not the owner of this room.");
                return;
            }
            Runner.Despawn(Object);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            RoomRemoved?.Invoke();
        }
    }
}