﻿using System.Collections.Generic;
using Core.Scripts.Items;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;

namespace Core.Scripts.Builds
{
    public class FactoryBuild : Build
    {
        [SerializeField] private Material _conveerMaterial;
        [SerializeField] private SkinnedMeshRenderer mesh;
        [SerializeField] private SphereCollider[] _collider;

        [SerializeField] private Transform _pointDestroyItem;

        [SerializeField] private List<Item> itemsSword;

        public Transform[] startPoints;

        private void Update()
        {
            if (items.Count > 0)
            {
                Vector2 offset = new Vector2(Time.fixedDeltaTime, 0);
                _conveerMaterial.mainTextureOffset -= offset;
            }
            //mesh.SetBlendShapeWeight(0, mesh.GetBlendShapeWeight(0) + index);
        }

        public void OnMoveItemsToFactory()
        {
            int index = 0;
            for (int i = 0; i < items.Count; i++)
            {
                items[i]._itemMoveType = ItemMoveType.None;
                items[i].transform.DOMove(_pointDestroyItem.position, _time * i).OnComplete(() =>
                {
                    LeanPool.Despawn(items[index]);
                    items.Remove(items[index]);
                    index++;
                    var sword = LeanPool.Spawn(_prefab, _pointDestroyItem.position, _prefab.transform.rotation, _parent)
                        .GetComponent<Item>();
                    sword.transform.DOMove(finishPoint.position, 1)
                        .OnComplete(() => { itemsSword.Add(sword); }).SetEase(Ease.Linear);
                }).SetEase(Ease.Linear);
            }
        }

        public FactoryColliderType ColliderСheck(Collider collider) =>
            collider == _collider[0] ? FactoryColliderType.Set : FactoryColliderType.Get;
    }
}