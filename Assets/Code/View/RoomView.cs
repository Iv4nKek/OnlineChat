using Code.General;
using Code.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.View
{
    public class RoomView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _roomName;
        [SerializeField] private Button _joinButton;
        [SerializeField] private Button _removeButton;
        [SerializeField] private TMP_Text _joinButtonText;
        [SerializeField] private string _ownerJoinName = "Open";
        [SerializeField] private string _guestJoinName = "Join";
        
        
        private GameStateHandler _gameStateHandler;
        private RoomBase _roomBase;
        

        public void Init(RoomBase roomBase, GameStateHandler gameStateHandler)
        {
            _gameStateHandler = gameStateHandler;
            if (_roomBase != null)
            {
                DisposeView();
            }
            _roomBase = roomBase;
            InitView();
        }

        private void OnDestroy()
        {
            DisposeView();
        }

        private void InitView()
        {
            UpdateRoomBaseName(_roomBase.RoomName);
            _roomBase.RoomNameChanged += UpdateRoomBaseName;
            _roomBase.RoomRemoved += DestroyView;
            _joinButton.onClick.AddListener(OnJoinRoomClicked);
            _joinButtonText.text = _roomBase.HasStateAuthority ? _ownerJoinName : _guestJoinName;

            if (_roomBase.HasStateAuthority)
            {
                _removeButton.onClick.AddListener(_roomBase.RemoveRoom);
            }
            else
            {
                _removeButton.gameObject.SetActive(false);
            }
        }

        private void DestroyView()
        {
            Destroy(gameObject);
        }

        private void OnJoinRoomClicked()
        {
            if (_roomBase.HasStateAuthority)
            {
                _gameStateHandler.OpenRoom(_roomBase);
            }
            else
            {
                _roomBase.JoinRoom();
            }
        }

        private void DisposeView()
        {
            _roomBase.RoomNameChanged -= UpdateRoomBaseName;
            _roomBase.RoomRemoved -= DestroyView;
            _joinButton.onClick.RemoveListener(_roomBase.JoinRoom);
            _removeButton.onClick.RemoveListener(_roomBase.LeftRoom);
        }

        private void UpdateRoomBaseName(string roomName)
        {
            _roomName.text = roomName;
        }
    }
}