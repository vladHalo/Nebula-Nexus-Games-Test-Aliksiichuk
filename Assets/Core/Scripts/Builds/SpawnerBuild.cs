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
        
        private Coroutine _currentCoroutine;

        private void Start()
        {
            _currentCoroutine = StartCoroutine(SpawnItems());
        }

        private IEnumerator SpawnItems()
        {
            while (true)
            {
                yield return new WaitForSeconds(_time);
                if (items.Count < _maxCapacity)
                {
                    var item = LeanPool.Spawn(_prefab, _parent).GetComponent<Item>();
                    item.itemMoveType = ItemMoveType.Bezier;
                    item.FinishMoveBezier += () => { item.itemMoveType = ItemMoveType.Follow; };
                    var lastPoint = items.Count == 0 ? finishPoint : items.Last().transform;
                    item.SetPointMove(_startPoint, lastPoint);
                    items.Add(item);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BackpackPlayer backpackPlayer))
            {
                StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out BackpackPlayer backpackPlayer))
                _currentCoroutine = StartCoroutine(SpawnItems());
        }
    }
}