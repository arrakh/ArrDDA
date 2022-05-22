using System;
using UnityEngine;
using UnityEngine.Events;

namespace TangramGame.Scripts
{
    public static class Events
    {
        //Custom Data Events
        public static Action<GameTimer> OnNewGameTimer;
        public static Action<RoundResult> OnRoundCompleted;
        
        //Primitive Data Events
        //todo: Describe the parameters or consider making it a Custom Data Action
        public static Action<int> OnHealthChanged;          //int: newHealth
        public static Action<int> OnScoreChanged;          //int: newScore
        
        //Void Events
        public static Action OnGameOver;
        public static Action OnPreRoundOver;
    }
}