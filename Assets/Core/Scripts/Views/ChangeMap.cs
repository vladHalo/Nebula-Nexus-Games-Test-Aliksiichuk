using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts.Views
{
    public class ChangeMap : MonoBehaviour
    {
        [SerializeField] private Button _btn;
        [SerializeField] private GameObject[] _maps;

        private int _index;

        private void Start()
        {
            _btn.onClick.AddListener(Change);
        }

        [Button]
        private void Change()
        {
            _maps.ForEach(x => x.SetActive(false));

            _index++;
            if (_index >= _maps.Length) _index = 0;
            _maps[_index].SetActive(true);
        }
    }
}