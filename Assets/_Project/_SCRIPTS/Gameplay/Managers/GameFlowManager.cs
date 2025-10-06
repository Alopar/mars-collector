using GameApplication.Gameplay.Models;
using GameApplication.Utility;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace GameApplication.Gameplay.Managers
{
    public class GameFlowManager : MonoBehaviour
    {
        public static GameFlowManager Instance { get; private set; }

        [Header("Configuration")]
        public GameConfig Config;

        public GameState CurrentGameState { get; private set; }
        public GamePhase CurrentPhase { get; private set; }

        public event Action<GamePhase> OnPhaseChanged;
        public event Action<int> OnTurnChanged;
        public event Action<bool, string> OnGameEnded;

        [Header("Settings")]
        public float ShipTravelDuration = 2f;
        public PlayableDirector ShipTravelDirector;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            if (Config == null)
            {
                Debug.LogError("GameConfig is not assigned!");
                return;
            }

            if (ShipManager.Instance == null)
            {
                Debug.LogError("ShipManager is not in the scene!");
                return;
            }

            if (MarsManager.Instance == null)
            {
                Debug.LogError("MarsManager is not in the scene!");
                return;
            }

            if (EventManager.Instance == null)
            {
                Debug.LogError("EventManager is not in the scene!");
                return;
            }

            CurrentGameState = new GameState();
            CurrentGameState.Reset(Config.TurnsToWin);

            ShipManager.Instance.Initialize(Config.ShipCapacity);
            MarsManager.Instance.Initialize(Config.StartingWeapons, Config.StartingSupplies, Config.StartingPeople);
            EventManager.Instance.LoadEvents(Config.EventsJson);
            
            if (CargoManager.Instance != null && Config.CargoDatabase != null)
            {
                CargoManager.Instance.Initialize(Config.GridWidth, Config.GridHeight, Config.CargoDatabase);
            }
            MissionsManager.Instance.Initialize();

            ShipManager.Instance.OnShipLaunched += HandleShipLaunched;
            MarsManager.Instance.OnGameOver += HandleMarsGameOver;

            SetPhase(GamePhase.Loading);
            OnTurnChanged?.Invoke(0);
            ShowNextTurnPreview();
        }

        private void OnDestroy()
        {
            if (ShipManager.Instance != null)
                ShipManager.Instance.OnShipLaunched -= HandleShipLaunched;
            
            if (MarsManager.Instance != null)
                MarsManager.Instance.OnGameOver -= HandleMarsGameOver;
        }

        private void HandleShipLaunched(ShipCargo cargo)
        {
            if (CargoManager.Instance != null && CargoManager.Instance.HasAnyShapes())
            {
                cargo = CargoManager.Instance.ConvertToShipCargo();
            }
            
            StartCoroutine(ProcessTurnSequence(cargo));
        }

        private IEnumerator ProcessTurnSequence(ShipCargo cargo)
        {
            ShipTravelDirector.Play();
            yield return new WaitForSeconds(ShipTravelDuration);

            SetPhase(GamePhase.Traveling);
            MarsManager.Instance.ClearPreview();

            SetPhase(GamePhase.Processing);

            MarsManager.Instance.ApplyCargoDeliverySilent(cargo);
            MarsManager.Instance.ApplyPeopleConsumptionSilent(Config.GetWeaponsPerPerson(), Config.GetSuppliesPerPerson());
            
            TurnEvent currentEvent = EventManager.Instance.GetNextEvent();
            if (currentEvent != null)
            {
                var eventChanges = currentEvent.GetResourceChanges();
                MarsManager.Instance.ApplyChangesSilent(eventChanges);
            }
            else
            {
                Debug.LogWarning("Нет события для применения!");
            }
            
            MarsManager.Instance.NotifyStateChanged();
            CurrentGameState.AddShipSent();

            ShipTravelDirector.time = 0;
            ShipTravelDirector.Evaluate();
            ShipTravelDirector.Stop();

            if (!CurrentGameState.IsGameOver)
            {
                CurrentGameState.IncrementTurn();
                OnTurnChanged?.Invoke(CurrentGameState.CurrentTurn);

                if (CurrentGameState.CheckVictory())
                {
                    EndGame(true, $"Win! You've lasted {Config.TurnsToWin} turns!");
                    yield break;
                }

                MissionsManager.Instance.CheckMissionComplete();
                MissionsManager.Instance.CheckMissionCanStart();

                ShipManager.Instance.ClearCargo();
                
                if (CargoManager.Instance != null)
                {
                    CargoManager.Instance.ClearCargo();
                }
                
                SetPhase(GamePhase.Loading);
                ShowNextTurnPreview();
            }
        }

        private void ShowNextTurnPreview()
        {
            TurnEvent currentEvent = EventManager.Instance.GetCurrentEvent();
            
            if (currentEvent != null)
            {
                var preview = currentEvent.GetResourceChanges();
                MarsManager.Instance.SetPreview(preview);
            }
            else
            {
                Debug.LogWarning("ShowNextTurnPreview: currentEvent is null!");
            }
        }

        private void HandleMarsGameOver(string reason)
        {
            EndGame(false, reason);
        }

        public void EndGame(bool victory, string message)
        {
            CurrentGameState.IsGameOver = true;
            CurrentGameState.IsVictory = victory;
            SetPhase(GamePhase.GameOver);
            OnGameEnded?.Invoke(victory, message);
        }

        public void RestartGame()
        {
            CurrentGameState.Reset(Config.TurnsToWin);
            MarsManager.Instance.ResetColony(Config.StartingWeapons, Config.StartingSupplies, Config.StartingPeople);
            ShipManager.Instance.ClearCargo();
            EventManager.Instance.Reset();
            
            if (CargoManager.Instance != null)
            {
                CargoManager.Instance.ClearCargo();
            }
            
            SceneManager.LoadScene(1);
            
            // SetPhase(GamePhase.Loading);
            // OnTurnChanged?.Invoke(0);
            // ShowNextTurnPreview();
        }

        private void SetPhase(GamePhase newPhase)
        {
            CurrentPhase = newPhase;
            OnPhaseChanged?.Invoke(newPhase);
        }
    }

    public enum GamePhase
    {
        Loading,
        Traveling,
        Processing,
        GameOver
    }
}

