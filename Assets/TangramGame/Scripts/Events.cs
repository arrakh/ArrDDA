using System;
using UnityEngine;
using UnityEngine.Events;

namespace TangramGame.Scripts
{
    public static class Events
    {
        public static Action<GameTimer> OnNewGameTimer;
        public static Action<GameDifficulty> OnNewRound;
    }
}