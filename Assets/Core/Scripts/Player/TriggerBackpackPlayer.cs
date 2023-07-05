using Core.Scripts.Builds;
using UnityEngine;

namespace Core.Scripts.Player
{
    public class TriggerBackpackPlayer : MonoBehaviour
    {
        [SerializeField] private BackpackPlayer _backpack;

        private Coroutine currentCoroutine;

        private void OnTriggerStay(Collider other)
        {
            if (currentCoroutine != null || !other.TryGetComponent(out Build build)) return;

            if (other.TryGetComponent(out SpawnerBuild spawnerBuild))
            {
                currentCoroutine = StartCoroutine(_backpack.GetItems(spawnerBuild.items, spawnerBuild.finishPoint));
            }

            if (other.TryGetComponent(out FactoryBuild factoryBuild))
            {
                var result = factoryBuild.ColliderСheck(other);

                if (result == FactoryColliderType.Set)
                {
                    if (factoryBuild.items.Count > 0) return;
                    currentCoroutine = StartCoroutine(_backpack.SetItems(factoryBuild.items, ItemType.Metal,
                        factoryBuild.OnMoveItemsToFactory, factoryBuild.startPoints));
                }
                else
                {
                    currentCoroutine =
                        StartCoroutine(_backpack.GetItems(factoryBuild.itemsSword, factoryBuild.finishPoint));
                }
            }

            if (other.TryGetComponent(out StockpileBuild stockpileBuild))
            {
                currentCoroutine = StartCoroutine(_backpack.SetItems(stockpileBuild.items,
                    ItemType.Sword, stockpileBuild.DespawnItems, stockpileBuild.finishPoint));
                _backpack.movementPlayer.SetLayerWeight(1, 0, 0.25f);
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

            if (other.TryGetComponent(out StockpileBuild stockpileBuild))
            {
                if (_backpack.CountItems() > 0)
                    _backpack.movementPlayer.SetLayerWeight(1, 1, 0.25f);
            }
        }
    }
}