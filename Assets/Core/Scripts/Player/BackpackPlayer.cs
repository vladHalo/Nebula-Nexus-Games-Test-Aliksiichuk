using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Scripts.Builds;
using Core.Scripts.Items;
using UnityEngine;

namespace Core.Scripts.Player
{
    public class BackpackPlayer : MonoBehaviour
    {
        [SerializeField] private int _capacityMax;
        [SerializeField] private float _delay;
        [SerializeField] private Transform _pointStartItems;
        [SerializeField] private MovementPlayer _movementPlayer;
        [SerializeField] private List<Item> _items;

        public IEnumerator GetItems(Transform firstPoint, List<Item> items)
        {
            while (items.Count != 0 && _items.Count <= _capacityMax)
            {
                var last = items.Last();
                var lastPoint = _items.Count == 0 ? _pointStartItems : _items.Last().transform;
                last.SetPointMove(firstPoint, lastPoint);
                last.enabled = true;
                _items.Add(last);
                items.Remove(last);

                if (_items.Count > 0)
                    _movementPlayer.SetLayerWeight(1, 1, 0.25f);
                yield return new WaitForSeconds(_delay);
            }
        }

        public IEnumerator SetItems(Transform lastPoint, List<Item> items)
        {
            while (_items.Count != 0)
            {
                var last = _items.Last();
                last.SetPointMove(_pointStartItems, lastPoint);
                last.enabled = true;
                items.Add(last);
                _items.Remove(last);

                if (_items.Count == 0)
                    _movementPlayer.SetLayerWeight(1, 0, 0.25f);
                yield return new WaitForSeconds(_delay);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out SpawnerBuild spawnerBuild))
            {
                StartCoroutine(GetItems(spawnerBuild.point, spawnerBuild.GetItems()));
            }

            if (other.TryGetComponent(out StockpileBuild stockpileBuild))
            {
                StartCoroutine(SetItems(stockpileBuild.point, stockpileBuild.GetItems()));
            }
        }
    }
}