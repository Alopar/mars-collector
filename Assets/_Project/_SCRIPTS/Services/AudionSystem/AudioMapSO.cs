using System.Collections.Generic;
using UnityEngine;

namespace Services.AudionSystem
{
    [CreateAssetMenu(fileName = "AudioMap_00", menuName = "Audio/AudioMap", order = 0)]
    public class AudioMapSO : ScriptableObject
    {
        #region FIELDS INSPECTOR
        [SerializeField] private string _name;
        [SerializeField] private List<AudionMapDTO> _clips;
        #endregion

        #region PROPERTIES
        public string Name => _name;
        public List<AudionMapDTO> Clips => _clips;
        #endregion
    }
}
