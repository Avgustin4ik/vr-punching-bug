namespace MageSurvivor.Code.Core.Pool
{
    using System;
    using UnityEngine;
    using UnityEngine.Pool;
    using System.Collections.Generic;

    public abstract class PooledMonoBehaviour : MonoBehaviour
    {
        [SerializeField] private int prewarmCount = 10;
        
        private static Dictionary<System.Type, ObjectPool<PooledMonoBehaviour>> Pools = new();

        private ObjectPool<PooledMonoBehaviour> _myPool;

        [RuntimeInitializeOnLoadMethod]
        public static void ReleaseAll()
        {
            foreach (var pool in Pools)
            {
                pool.Value.Clear();
            }
            GC.Collect();
        }

        private void InitializePool()
        {
            System.Type type = GetType();
            if (Pools.TryGetValue(type, out _myPool)) return;
            _myPool = new ObjectPool<PooledMonoBehaviour>(
                createFunc: () => Instantiate(this),
                actionOnGet: OnGet,
                actionOnRelease: OnRelease,
                actionOnDestroy: OnDestroyPooledObject,
                defaultCapacity: prewarmCount,
                maxSize: 100
            );
            Pools[type] = _myPool;

            Prewarm(prewarmCount);
        }

        public T Spawn<T>(Vector3 transformPosition, Quaternion lookRotation) where T : PooledMonoBehaviour
        {
            T instance = Spawn<T>();
            instance.transform.position = transformPosition;
            instance.transform.rotation = lookRotation;
            return instance;
        }

        public T Spawn<T>() where T : PooledMonoBehaviour
        {
            if (_myPool != null)
            {
                PooledMonoBehaviour instance = _myPool.Get();
                return (T)instance;
            }
            else
            {
                InitializePool();
                PooledMonoBehaviour instance = _myPool.Get();
                return (T)instance;
            }
        }

        public void Release()
        {
            _myPool ??= Pools[GetType()];
            _myPool.Release(this);
        }

        private void OnGet(PooledMonoBehaviour obj)
        {
            if (obj == null || obj.Equals(null)) return;
            obj.gameObject.SetActive(true);
        }

        private void OnRelease(PooledMonoBehaviour obj)
        {
            if (obj == null || obj.Equals(null)) return;
            obj.gameObject.SetActive(false);
        }

        private void OnDestroyPooledObject(PooledMonoBehaviour obj)
        {
            if (obj == null || obj.Equals(null)) return;
            Destroy(obj.gameObject);
        }

        private void Prewarm(int count)
        {
            PooledMonoBehaviour[] prewarmedObjects = new PooledMonoBehaviour[count];
            for (int i = 0; i < count; i++)
            {
                prewarmedObjects[i] = _myPool.Get();
            }
            for (int i = 0; i < count; i++)
            {
                prewarmedObjects[i].Release();
            }
        }
    }
}