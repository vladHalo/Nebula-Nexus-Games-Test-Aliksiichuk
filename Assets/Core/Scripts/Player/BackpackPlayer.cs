using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackpackPlayer : MonoBehaviour
{
    [SerializeField] private int _capacityMax;
    [SerializeField] private Transform _pointStartItems;
    [SerializeField] private MovementPlayer _movementPlayer;
    [SerializeField] private List<Item> _items;

    public IEnumerator GetItems(List<Item> items, Transform firstPoint)
    {
        while ( /*items.Count != 0 && */_items.Count <= _capacityMax)
        {
            var last = items.Last();
            var lastPoint = _items.Count == 0 ? _pointStartItems : _items.Last().transform;
            last.SetPointMove(firstPoint, lastPoint);
            _items.Add(last);
            items.Remove(last);
            yield return new WaitForSeconds(.1f);
        }

        if (_items.Count > 0)
            _movementPlayer.SetLayerWeight(1, 1, 0.25f);
    }

    public IEnumerator SetItems(Transform lastPoint, TypeZoneForItems typeZoneForItems)
    {
        while (_items.Count != 0)
        {
            var last = _items.Last();
            last.SetPointMove(last.transform, lastPoint);
            _items.Remove(last);
            yield return new WaitForSeconds(.1f);
        }

        if (_items.Count == 0)
            _movementPlayer.SetLayerWeight(1, 0, 0.25f);
    }
}