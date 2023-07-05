using System;
using Core.Scripts.Player;
using Lean.Pool;
using TMPro;
using UnityEngine;

namespace Core.Scripts.Builds
{
    public class StockpileBuild : Build
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI countSwordUI;
        [SerializeField] private int countSword;

        public void DespawnItems()
        {
            items.ForEach(x => LeanPool.Despawn(x));
            countSword += items.Count;
            items.Clear();
            countSwordUI.text = $"{countSword}";
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out MovementPlayer movementPlayer))
                panel.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out MovementPlayer movementPlayer))
                panel.SetActive(false);
        }
    }
}