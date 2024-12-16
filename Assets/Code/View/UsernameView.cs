using Code.General;
using TMPro;
using UnityEngine;

namespace Code.View
{
    public class UsernameView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private UserData _userData;

        private void Start()
        {
            _nameInputField.text = _userData.Username;
            _nameInputField.onValueChanged.AddListener(SetNickname);
        }

        private void OnDestroy()
        {
            _nameInputField.onValueChanged.RemoveListener(SetNickname);
        }

        private void SetNickname(string text)
        {
            _userData.SetUsername(text);
        }
    }
}