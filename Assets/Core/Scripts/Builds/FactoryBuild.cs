using System;
using UnityEngine;

namespace Core.Scripts.Builds
{
    public class FactoryBuild : Build
    {
        [SerializeField] private float _conveerSpeed = 2f;
        [SerializeField] private Material _conveerMaterial;
        [SerializeField] private Transform[] _points;

        private void FixedUpdate()
        {
            //if()
            //Vector2 offset = new Vector2(_conveerSpeed * Time.fixedDeltaTime, 0);
            //_conveerMaterial.mainTextureOffset -= offset;
        }
    }
}