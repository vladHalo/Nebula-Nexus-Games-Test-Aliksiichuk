using System.Collections.Generic;
using Core.Scripts.Items;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Scripts.Builds
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(SphereCollider))]
    public abstract class Build : MonoBehaviour
    {
        [SerializeField] protected int _maxCapacity;
        [SerializeField] protected GameObject _prefab;

        [HideIf("_maxCapacity", 0)] [SerializeField]
        protected float _time;

        [HideIf("_maxCapacity", 0)] [SerializeField]
        protected Transform _parent;

        public Transform finishPoint;
        public List<Item> items;

        public virtual void WorkBuildWithItems()
        {
        }
    }
}