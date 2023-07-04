using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Scripts.Builds;
using Core.Scripts.Items;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Scripts.Player
{
    public class BackpackPlayer : MonoBehaviour
    {
        [SerializeField] private int _capacityMax;
        [SerializeField] private float _delay;
        [SerializeField] private MovementPlayer _movementPlayer;
        [SerializeField] private Transform _pointStartItems;
        [SerializeField] private List<Item> _items;

        private Coroutine currentCoroutine;

        public IEnumerator GetItems(List<Item> items, Transform firstPoint)
        {
            while (_items.Count < _capacityMax)
            {
                if (items.Count != 0)
                {
                    GetItem(items.Last(), firstPoint);
                    items.Remove(items.Last());
                }

                yield return new WaitForSeconds(_delay);
            }

            currentCoroutine = null;
        }

        public void GetItem(Item item, Transform firstPoint)
        {
            if (_items.Count < _capacityMax)
            {
                var lastPoint = _items.Count == 0 ? _pointStartItems : _items.Last().transform;
                item._itemMoveType = ItemMoveType.Bezier;
                item.SetPointMove(firstPoint, lastPoint);
                _items.Add(item);

                if (_items.Count > 0)
                    _movementPlayer.SetLayerWeight(1, 1, 0.25f);
            }
        }

        public IEnumerator SetItems(List<Item> items, Transform lastPoint, ItemType itemType,
            Action finishAction = null)
        {
            while (_items.Count != 0)
            {
                if (_items.Last()._itemType == itemType)
                {
                    var last = SetItem(lastPoint);
                    items.Add(last);
                }

                yield return new WaitForSeconds(_delay);
            }

            items.Last().FinishMoveBezier += () => { finishAction?.Invoke(); };
        }

        private Item SetItem(Transform lastPoint)
        {
            var last = _items.Last();
            last._itemMoveType = ItemMoveType.Bezier;
            last.SetPointMove(_pointStartItems, lastPoint);
            _items.Remove(last);

            if (_items.Count == 0)
                _movementPlayer.SetLayerWeight(1, 0, 0.25f);
            return last;
        }

        public IEnumerator SetItems(FactoryBuild factoryBuild)
        {
            int index = 0;
            while (_items.Count != 0 && factoryBuild.items.Count < factoryBuild.startPoints.Length)
            {
                var last = SetItem(factoryBuild.startPoints[index]);
                factoryBuild.items.Add(last);
                index++;

                if (_items.Count == 0)
                {
                    factoryBuild.items.Last().FinishMoveBezier += () => { factoryBuild.OnMoveItemsToFactory(); };
                }

                yield return new WaitForSeconds(_delay);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (currentCoroutine != null) return;

            if (other.TryGetComponent(out SpawnerBuild spawnerBuild))
            {
                currentCoroutine = StartCoroutine(GetItems(spawnerBuild.items, spawnerBuild.finishPoint));
            }

            if (other.TryGetComponent(out FactoryBuild factoryBuild))
            {
                var result = factoryBuild.ColliderСheck(other);

                if (result == FactoryColliderType.Set)
                {
                    currentCoroutine = StartCoroutine(SetItems(factoryBuild));
                }
                else
                {
                    Debug.Log(FactoryColliderType.Get);
                }
            }

            if (other.TryGetComponent(out StockpileBuild stockpileBuild))
            {
                currentCoroutine = StartCoroutine(SetItems(stockpileBuild.items, stockpileBuild.finishPoint,
                    stockpileBuild._itemTypeSet, stockpileBuild.DespawnItems));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Build build))
            {
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);
                    currentCoroutine = null;
                }
            }
        }

        public bool СapacityСheck() => _items.Count < _capacityMax;

        [Button]
        private void ClearBackpack()
        {
            _items.ForEach(x => LeanPool.Despawn(x));
            _items.Clear();
            _movementPlayer.SetLayerWeight(1, 0, 0.25f);
        }
    }
}