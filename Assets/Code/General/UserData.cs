using System;
using UnityEngine;

namespace Code.General
{
    public class UserData : MonoBehaviour
    {
        private string _username;

        public string Username => _username;
        
        public event Action<string> UsernameChanged;

        public void SetUsername(string username)
        {
            _username = username;
            UsernameChanged?.Invoke(_username);
        }

        private void Awake()
        {
            _username = Environment.MachineName;
        }
    }
}