using UnityEngine;

namespace Core.Scripts.Globals
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : class
    {
        public static T instance { get; private set; }

        protected virtual void Awake()
        {
            instance = this as T;
        }
    }
}