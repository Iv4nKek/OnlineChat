using Code.General;
using Code.Network;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Factory
{
    public class RoomFactory : MonoBehaviour
    {
        [SerializeField] private NetworkRunner _runner;
        [SerializeField] private UserData _userData;
        
        [SerializeField] private GameStateHandler _gameStateHandler;
        [SerializeField] private Button _createRoomButton;
        [SerializeField] private GameObject _roomPrefab;

        private RoomBase _room;

        private void OnEnable()
        {
            RoomRegistry.OnRoomAdded += InitRoom;
            _createRoomButton.onClick.AddListener(CreateRoom);
        }
        
        private void OnDisable()
        {
            RoomRegistry.OnRoomAdded -= InitRoom;
            _createRoomButton.onClick.RemoveListener(CreateRoom);
            if (_room != null)
            {
                _room.RoomParticipantsHandler.LocalPlayerJoined -= _gameStateHandler.OpenRoom;
            }
        }
        
        private void OnDestroy()
        {
            RoomRegistry.Dispose();
        }

        private void InitRoom(RoomBase room)
        {
            _room = room;
            room.RoomMessagesHandler.Init(_userData);
            room.RoomParticipantsHandler.LocalPlayerJoined += _gameStateHandler.OpenRoom;
        }
        
        private void CreateRoom()
        {
            NetworkObject spawned = _runner.Spawn(_roomPrefab);
            RoomBase roomBase = spawned.GetComponent<RoomBase>();
            roomBase.RoomName = _userData.Username;

        }

    }
}