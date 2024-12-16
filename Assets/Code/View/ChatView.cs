using System.Collections.Generic;
using Code.General;
using Code.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.View
{
    public class ChatView : MonoBehaviour
    {
        [SerializeField] private Transform _messageRoot;
        [SerializeField] private GameObject _messagePrefab;
        [SerializeField] private TMP_InputField _textInput;
        [SerializeField] private Button _backButton;
        [SerializeField] private GameStateHandler _gameStateHandler;
        
        private RoomBase _roomBase;
        private readonly List<GameObject> _messages = new List<GameObject>();
        
        private void ExitChatView()
        {
            
            if (!_roomBase.HasStateAuthority)
            {
                _roomBase.LeftRoom();
            }
            _gameStateHandler.OpenRoomsBrowser();
        }

        public void Init(RoomBase roomBase)
        {
            Deinit();
            _roomBase = roomBase;
            _roomBase.RoomParticipantsHandler.LocalPlayerLeft += ExitChatView;
            _roomBase.RoomMessagesHandler.OnMessageReceived += SpawnMessageUI;
            
            _textInput.onSubmit.AddListener(OnSubmit);
            _backButton.onClick.AddListener(ExitChatView);

            foreach (var message in _roomBase.RoomMessagesHandler.Messages)
            {
                SpawnMessageUI(message);
            }
        }

        private void Deinit()
        {
            _textInput.onSubmit.RemoveListener(OnSubmit);
            _backButton.onClick.RemoveListener(ExitChatView);
            if (_roomBase != null)
            {
                _roomBase.RoomParticipantsHandler.LocalPlayerLeft -= ExitChatView;
                _roomBase.RoomMessagesHandler.OnMessageReceived -= SpawnMessageUI;
            }
           

            foreach (GameObject message in _messages)
            {
                Destroy(message);
            }
        }

        private void OnDestroy()
        {
            Deinit();
        }

        private void OnSubmit(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                _roomBase.RoomMessagesHandler.SendPlayerMessage(text);
                _textInput.text = string.Empty;
            }
        }

        private void SpawnMessageUI(string message)
        {
            var messageGameObject = Instantiate(_messagePrefab, _messageRoot);
            TMP_Text messageText = messageGameObject.GetComponent<TMP_Text>();
            messageText.text = message;
            _messages.Add(messageGameObject);
        }
    }
}