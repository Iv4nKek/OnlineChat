using Code.General;
using Code.Network;
using Code.View;
using UnityEngine;

namespace Code.Factory
{
    public class RoomViewFactory : MonoBehaviour
    {
        [SerializeField] private GameStateHandler _gameStateHandler;
        [SerializeField] private GameObject _roomViewPrefab;
        [SerializeField] private Transform _roomParent;
        
        private void Awake()
        {
            RoomRegistry.OnRoomAdded += CreateRoom;
        }

        private void OnDestroy()
        {
            RoomRegistry.OnRoomAdded -= CreateRoom;
        }

        private void CreateRoom(RoomBase roomBase)
        {
            RoomView roomView = Instantiate(_roomViewPrefab, _roomParent).GetComponent<RoomView>();
            roomView.Init(roomBase,_gameStateHandler);
        }
    }
}