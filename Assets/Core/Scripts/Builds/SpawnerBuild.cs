using System.Collections;
using System.Linq;
using Core.Scripts.Items;
using Lean.Pool;
using UnityEngine;

namespace Core.Scripts.Builds
{
    public class SpawnerBuild : Build
    {
        private void Start()
        {
            StartCoroutine(SpawnItems());
        }

        private IEnumerator SpawnItems()
        {
            while (true)
            {
                yield return new WaitForSeconds(_delay);
                if (_items.Count < _maxCapacity)
                {
                    var item = LeanPool.Spawn(_prefab, _parent).GetComponent<Item>();
                    Transform lastPoint = _items.Count == 0 ? point : _items.Last().transform;
                    item.SetPointMove(transform, lastPoint);
                    _items.Add(item);
                }
            }
        }
    }
}