using System.Collections;
using System.Linq;
using Core.Scripts.Items;
using Core.Scripts.Player;
using Lean.Pool;
using UnityEngine;

namespace Core.Scripts.Builds
{
    public class SpawnerBuild : Build
    {
        [SerializeField] private Transform _startPoint;

        private BackpackPlayer _backpack;

        private void Start()
        {
            StartCoroutine(SpawnItems());
        }

        private IEnumerator SpawnItems()
        {
            while (true)
            {
                yield return new WaitForSeconds(_time);
                if (items.Count < _maxCapacity)
                {
                    var item = LeanPool.Spawn(_prefab, _parent).GetComponent<Item>();
                    item._itemMoveType = ItemMoveType.Bezier;

                    if (_backpack != null && _backpack.СapacityСheck())
                    {
                        item.FinishMoveBezier += () => { item._itemMoveType = ItemMoveType.Follow; };
                        _backpack.GetItem(item, _startPoint);
                        items.Remove(item);
                    }
                    else
                    {
                        item.FinishMoveBezier += () =>
                        {
                            items.Add(item);
                            item._itemMoveType = ItemMoveType.None;
                        };
                        var lastPoint = items.Count == 0 ? finishPoint : items.Last().transform;
                        item.SetPointMove(_startPoint, lastPoint);
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BackpackPlayer backpackPlayer))
                _backpack = backpackPlayer;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out BackpackPlayer backpackPlayer))
                _backpack = null;
        }
    }
}