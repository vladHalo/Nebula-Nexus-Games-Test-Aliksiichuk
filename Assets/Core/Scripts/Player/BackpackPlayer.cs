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
        [SerializeField] private Transform _pointStartItems;
        [SerializeField] private List<Item> _items;

        public MovementPlayer movementPlayer;

        public IEnumerator GetItems(List<Item> items, Transform firstPoint)
        {
            while (_items.Count < _capacityMax)
            {
                if (items.Count != 0)
                {
                    var last = items.Last();
                    if (_items.Count < _capacityMax)
                    {
                        var lastPoint = _items.Count == 0 ? _pointStartItems : _items.Last().transform;
                        last.itemMoveType = ItemMoveType.Bezier;
                        last.SetPointMove(firstPoint, lastPoint);
                        _items.Add(last);
                    }

                    items.Remove(items.Last());
                }

                if (_items.Count == 1)
                {
                    _items[0].FinishMoveBezier += () => { movementPlayer.SetLayerWeight(1, 1, 0.25f); };
                }

                yield return new WaitForSeconds(_delay);
            }
        }

        public IEnumerator SetItems(Build build, ItemType itemType, params Transform[] startPoints)
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                if (_items[i].itemType != itemType) continue;
                int index = i;
                if (startPoints.Length == 1) index = 0;
                build.items.Add(_items[i]);
                _items[i].itemMoveType = ItemMoveType.Bezier;
                _items[i].SetPointMove(_pointStartItems, startPoints[index]);
                _items.Remove(_items[i]);

                if (itemType == ItemType.Sword)
                {
                    var last = build.items.Last();
                    last.FinishMoveBezier += () =>
                    {
                        LeanPool.Despawn(last, 3);
                        build.items.Remove(last);
                        build.WorkBuildWithItems();
                    };
                }

                if (_items.Count == 0)
                    movementPlayer.SetLayerWeight(1, 0, 0.25f);
                RefreshPosition();
                yield return new WaitForSeconds(_delay);
            }

            if (build.items.Count == 0) yield break;
            if (itemType == ItemType.Metal)
                build.items.Last().FinishMoveBezier += () => { build.WorkBuildWithItems(); };
        }

        public int CountItems() => _items.Count;

        private void RefreshPosition()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                var result = i == 0 ? _pointStartItems : _items[i - 1].transform;
                _items[i].SetPointMove(null, result);
            }
        }

        [Button]
        private void ClearBackpack()
        {
            _items.ForEach(x => LeanPool.Despawn(x));
            _items.Clear();
            movementPlayer.SetLayerWeight(1, 0, 0.25f);
        }
    }
}