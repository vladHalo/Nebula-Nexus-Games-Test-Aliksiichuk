using Sirenix.OdinInspector;
using UnityEngine;

public class ZonaShopController : MonoBehaviour
{
    [SerializeField] TypeZoneForItems _typeZoneForItems;

    [SerializeField] BackpackPlayer _holderPlayerController;

    ItemSpawnController _itemSpawnController;

    private void Start()
    {
        if (_typeZoneForItems == TypeZoneForItems.Buy) _itemSpawnController = GetComponent<ItemSpawnController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MovementPlayer i))
        {
            // if (_typeZoneForItems == TypeZoneForItems.Buy) _shopView.OnOffShop(true);
            // else StartCoroutine(_holderPlayerController.SetItems(transform, _typeZoneForItems));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out MovementPlayer i))
        {
            if (_typeZoneForItems == TypeZoneForItems.Buy)
            {
                //_shopView.OnOffShop(false);
                StartCoroutine(_holderPlayerController.GetItems(_itemSpawnController._items,
                    _itemSpawnController._spawnPosition));
            }
        }
    }
}