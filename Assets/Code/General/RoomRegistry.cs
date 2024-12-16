using System;
using System.Collections.Generic;
using Code.Network;

namespace Code.General
{
    public static class RoomRegistry
    {
        private static List<RoomBase> _rooms = new List<RoomBase>();

        public static List<RoomBase> Rooms => _rooms;
        
        public static event Action<RoomBase> OnRoomAdded;

        public static void RegisterRoom(RoomBase roomBase)
        {
            _rooms.Add(roomBase);
            OnRoomAdded?.Invoke(roomBase);
        }

        public static void Dispose()
        {
            _rooms = null;
        }
    }
}