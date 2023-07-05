using Core.Scripts.Builds;
using UnityEngine;

namespace Core.Scripts.Player
{
    public class TriggerBackpackPlayer : MonoBehaviour
    {
        [SerializeField] private BackpackPlayer _backpack;

        private Coroutine _currentCoroutine;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out FactoryBuild factory))
            {
                var result = factory.ColliderСheck(other);

                if (result == FactoryColliderType.Set)
                {
                    if (factory.items.Count > 0) return;
                    StartCoroutine(_backpack.SetItems(factory, ItemType.Metal, factory.startPoints));
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (_currentCoroutine != null || !other.TryGetComponent(out Build build)) return;

            if (other.TryGetComponent(out SpawnerBuild spawner))
            {
                _currentCoroutine = StartCoroutine(_backpack.GetItems(spawner.items, spawner.finishPoint));
            }

            if (other.TryGetComponent(out FactoryBuild factory))
            {
                var result = factory.ColliderСheck(other);

                if (result == FactoryColliderType.Get)
                {
                    _currentCoroutine =
                        StartCoroutine(_backpack.GetItems(factory.itemsSword, factory.finishPoint));
                }
            }

            if (other.TryGetComponent(out StockpileBuild stockpile))
            {
                _currentCoroutine = StartCoroutine(_backpack.SetItems(stockpile,
                    ItemType.Sword, stockpile.finishPoint));
                _backpack.movementPlayer.SetLayerWeight(1, 0, 0.25f);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Build build))
            {
                if (_currentCoroutine != null)
                {
                    StopCoroutine(_currentCoroutine);
                    _currentCoroutine = null;
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