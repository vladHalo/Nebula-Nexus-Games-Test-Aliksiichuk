using Core.Scripts.Player;
using TMPro;
using UnityEngine;

namespace Core.Scripts.Builds
{
    public class StockpileBuild : Build
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TextMeshProUGUI _countSwordUI;

        private int _countSword;

        public override void WorkBuildWithItems()
        {
            _countSword++;
            _countSwordUI.text = $"{_countSword}";
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out MovementPlayer movementPlayer))
                _panel.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out MovementPlayer movementPlayer))
                _panel.SetActive(false);
        }
    }
}