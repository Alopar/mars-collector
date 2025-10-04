using System;
using System.Collections;
using System.Collections.Generic;
using GameApplication.Gameplay.Models;
using UnityEngine;

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
            StartCoroutine(ProcessTurnSequence(cargo));
        }

        private IEnumerator ProcessTurnSequence(ShipCargo cargo)
        {
            SetPhase(GamePhase.Traveling);
            MarsManager.Instance.ClearPreview();

            yield return new WaitForSeconds(ShipTravelDuration);

            SetPhase(GamePhase.Processing);

            MarsManager.Instance.ApplyCargoDelivery(cargo);
            CurrentGameState.AddShipSent();

            yield return new WaitForSeconds(0.5f);

            TurnEvent currentEvent = EventManager.Instance.GetNextEvent();
            if (currentEvent != null)
            {
                Debug.Log($"Применяется событие: {currentEvent.title}");
                var eventChanges = currentEvent.GetResourceChanges();
                
                foreach (var change in eventChanges)
                {
                    Debug.Log($"  {change.Key}: {change.Value}");
                }
                
                MarsManager.Instance.ApplyChanges(eventChanges);
            }
            else
            {
                Debug.LogWarning("Нет события для применения!");
            }

            yield return new WaitForSeconds(0.3f);

            MarsManager.Instance.ApplyPeopleConsumption(Config.WeaponsPerPerson, Config.SuppliesPerPerson);

            yield return new WaitForSeconds(0.3f);

            if (!CurrentGameState.IsGameOver)
            {
                CurrentGameState.IncrementTurn();
                OnTurnChanged?.Invoke(CurrentGameState.CurrentTurn);

                if (CurrentGameState.CheckVictory())
                {
                    EndGame(true, $"Победа! Вы продержались {Config.TurnsToWin} ходов!");
                    yield break;
                }

                ShipManager.Instance.ClearCargo();
                SetPhase(GamePhase.Loading);
                ShowNextTurnPreview();
            }
        }

        private void ShowNextTurnPreview()
        {
            TurnEvent nextEvent = EventManager.Instance.PeekNextEvent();
            
            if (nextEvent != null)
            {
                var preview = nextEvent.GetResourceChanges();
                
                int currentPeople = MarsManager.Instance.ColonyState.People;
                int weaponsConsumption = Mathf.RoundToInt(currentPeople * Config.WeaponsPerPerson);
                int suppliesConsumption = Mathf.RoundToInt(currentPeople * Config.SuppliesPerPerson);
                
                if (preview.ContainsKey(ResourceType.Weapons))
                    preview[ResourceType.Weapons] -= weaponsConsumption;
                else
                    preview[ResourceType.Weapons] = -weaponsConsumption;
                
                if (preview.ContainsKey(ResourceType.Supplies))
                    preview[ResourceType.Supplies] -= suppliesConsumption;
                else
                    preview[ResourceType.Supplies] = -suppliesConsumption;
                
                MarsManager.Instance.SetPreview(preview);
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
            
            SetPhase(GamePhase.Loading);
            OnTurnChanged?.Invoke(0);
            ShowNextTurnPreview();
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

