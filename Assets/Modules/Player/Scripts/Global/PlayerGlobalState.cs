using UnityEngine.InputSystem;

namespace Modules.Player.Scripts.Global
{
    public static class PlayerGlobalState
    {
        public static PlayerInput[] AllPlayersCurrent = new PlayerInput[3];
        public static int activePlayers;
    }
}