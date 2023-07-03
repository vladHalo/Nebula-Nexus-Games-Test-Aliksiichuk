using System.Collections.Generic;
using System.Linq;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;
/*using Zenject;*/

public class ItemSpawnController : MonoBehaviour
{
    public List<Item> _items;
    public Transform _spawnPosition;

    // [Button]
    // public void Spawn(Item prefab)
    // {
    //     if (prefab._typeItem == TypeItem.Gear &&
    //         int.Parse(_shopView._gearPriceUI.text) >= _moneyController._money) return;
    //
    //     if (prefab._typeItem == TypeItem.Battery &&
    //         int.Parse(_shopView._batteryPriceUI.text) >= _moneyController._money) return;
    //
    //     var item = LeanPool.Spawn(prefab);
    //     item.transform.position = _spawnPosition.position;
    //     if (_items.Count != 0)
    //         item.transform.position =
    //             new Vector3(_spawnPosition.position.x, _items.Last().transform.position.y + item._offset.y,
    //                 _spawnPosition.position.z);
    //     _items.Add(item);
    //     if (item._typeItem == TypeItem.Gear)
    //         _moneyController.MinusMoney(int.Parse(_shopView._gearPriceUI.text));
    //     else _moneyController.MinusMoney(int.Parse(_shopView._batteryPriceUI.text));
    //     _shopView.RefreshPrice();
    // }
}