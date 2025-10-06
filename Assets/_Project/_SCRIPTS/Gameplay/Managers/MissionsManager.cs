using GameApplication.Gameplay.Models;
using GameApplication.Gameplay.Models.Missions;
using GameApplication.UI;
using UnityEngine;

namespace GameApplication.Gameplay.Managers
{
    public class MissionsManager : MonoBehaviour
    {
        [SerializeField] private MissionButtonView _missionButtonView;
        [SerializeField] private MissionDescriptionView _missionDescriptionView;
        [SerializeField] private MissionResultView _missionResultView;
        [SerializeField] private MissionsChainConfig _missionChain;

        private int _currentMissionIndex = -1;
        private bool _missionIsActive = false;

        public int CurrentMissionIndex => _currentMissionIndex;

        public static MissionsManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void Initialize()
        {
            _missionResultView.Hide();
            _missionButtonView.Hide();

            CargoManager.Instance.OnResourcesChanged += HandleResourcesChanged;
            GameFlowManager.Instance.OnTurnChanged += OnTurnChanged;
            GameFlowManager.Instance.OnGameEnded += GameEnded;
        }

        private void GameEnded(bool _, string __)
        {
            _currentMissionIndex = -1;
            _missionIsActive = false;
            _missionButtonView.Hide();
        }

        public void CheckMissionComplete()
        {
            if (_currentMissionIndex == -1) return;
            if (_missionIsActive == false) return;
            if (!_missionChain.Missions[_currentMissionIndex].CheckRequirements()) return;

            if (_currentMissionIndex == _missionChain.Missions.Length - 1)
                // secret ending
                return;

            _missionResultView.Show();
            _missionResultView.SetText(_missionChain.Missions[_currentMissionIndex].MissionResult);
            _currentMissionIndex++;
            _missionIsActive = false;
            _missionButtonView.Hide();
        }

        public bool SecretEnding()
        {
            if (_missionIsActive == false) return false;
            if (!_missionChain.Missions[_currentMissionIndex].CheckRequirements()) return false;

            if (_currentMissionIndex == _missionChain.Missions.Length - 1)
                return true;

            return false;
        }

        public void CheckMissionCanStart()
        {
            if (_currentMissionIndex == -1) return;
            if (_missionIsActive) return;
            if (_missionChain.Missions[_currentMissionIndex].MinPeopleNeededToStart > MarsManager.Instance.ColonyState.People) return;

            _missionIsActive = true;
            _missionButtonView.Show();
            _missionButtonView.SetNew();
            _missionButtonView.SetMissionText(_missionChain.Missions[_currentMissionIndex].MissionName);
            _missionDescriptionView.SetMissionText(_missionChain.Missions[_currentMissionIndex].MissionDescription);
        }

        private void HandleResourcesChanged(ResourceType resource, int amount)
        {
            // change mission availability state
        }

        private void OnTurnChanged(int turn)
        {
            _missionResultView.Hide();

            if (turn == 2)
                StartMissionsChain();
        }

        private void StartMissionsChain()
        {
            _missionIsActive = false;
            _currentMissionIndex = 0;
            _missionButtonView.Show();
            _missionButtonView.SetNew();
            _missionButtonView.SetMissionText(_missionChain.Missions[_currentMissionIndex].MissionName);
            _missionDescriptionView.SetMissionText(_missionChain.Missions[_currentMissionIndex].MissionDescription);
        }
    }
}