using System;

namespace GameApplication.Gameplay.Models
{
    [Serializable]
    public class GameState
    {
        public int CurrentTurn = 0;
        public int ShipsSent = 0;
        public bool IsGameOver = false;
        public bool IsVictory = false;
        public int TurnsToWin = 20;

        public void IncrementTurn()
        {
            CurrentTurn++;
        }

        public void AddShipSent()
        {
            ShipsSent++;
        }

        public bool CheckVictory()
        {
            if (CurrentTurn >= TurnsToWin)
            {
                IsVictory = true;
                IsGameOver = true;
                return true;
            }
            return false;
        }

        public void Reset(int turnsToWin)
        {
            CurrentTurn = 0;
            ShipsSent = 0;
            IsGameOver = false;
            IsVictory = false;
            TurnsToWin = turnsToWin;
        }
    }
}

