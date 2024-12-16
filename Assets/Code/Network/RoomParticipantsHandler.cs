using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

namespace Code.Network
{
    public class RoomParticipantsHandler : NetworkBehaviour
    {
        [SerializeField] private RoomBase _roomBase;
        
        private List<PlayerRef> _otherPlayers = new List<PlayerRef>();

        public List<PlayerRef> OtherPlayers => _otherPlayers;
        private List<PlayerRef> AllPlayers => _otherPlayers.Concat(new PlayerRef[] { Runner.LocalPlayer}).ToList();
        private bool IsConnected => _otherPlayers.Count != 0;
        
        public event Action<RoomBase> LocalPlayerJoined;
        public event Action LocalPlayerBeforeLeft; 
        public event Action LocalPlayerLeft; 
        
        public void JoinRoom()
        {
            RPC_JoinRoom(Runner.LocalPlayer);
        }

        [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
        private void RPC_JoinRoom(PlayerRef player)
        {
            if (HasStateAuthority)
            {
                foreach (PlayerRef playerRef in _otherPlayers)
                {
                    RPC_NotifyPlayerJoined(player, playerRef);
                }
            }
            _otherPlayers.Add(player);
            _roomBase.RoomMessagesHandler.SendMessageHistory(player);
            RPC_AcceptJoin(AllPlayers.ToArray(), player); 
           
        }
        
        [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
        private void RPC_NotifyPlayerJoined(PlayerRef player, [RpcTarget] PlayerRef target)
        {
            _otherPlayers.Add(player);
        }
        
        [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
        private void RPC_AcceptJoin(PlayerRef[] participants, [RpcTarget] PlayerRef target)
        {
            _otherPlayers = participants.Except(new []{Runner.LocalPlayer}).ToList();
            LocalPlayerJoined?.Invoke(_roomBase);
        }

        public void LeftRoom()
        {
            if (!IsConnected)
            {
                return;
            }
            if (HasStateAuthority)
            {
                Debug.LogError($"Can't left your room. You can remove it instead");
            }
            LocalPlayerBeforeLeft?.Invoke();
            foreach (PlayerRef playerRef in _otherPlayers)
            {
                RPC_NotifyPlayerLeft(Runner.LocalPlayer, playerRef);
            }
            _otherPlayers.Clear();
            LocalPlayerLeft?.Invoke();
        }

        [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
        private void RPC_NotifyPlayerLeft(PlayerRef player, [RpcTarget] PlayerRef target)
        {
            _otherPlayers.Remove(player);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _otherPlayers.Clear();
           
            LocalPlayerLeft?.Invoke();
        }
    }
}