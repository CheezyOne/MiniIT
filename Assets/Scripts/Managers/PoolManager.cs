using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using System.Collections;

namespace MiniIT.Managers
{
    public class PoolManager : Singleton<PoolManager>
    {
        [SerializeField] private Transform poolObjectsHolder = null;

        private List<ObjectPool> objectPools = new();
        private Dictionary<GameObject, Coroutine> activeCoroutines = new();

        private void OnDisable()
        {
            foreach (ObjectPool ObjectPool in objectPools)
            {
                ObjectPool.InactiveObjects.Clear();
            }

            foreach (var coroutine in activeCoroutines.Values)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
            }

            activeCoroutines.Clear();
        }

        public T InstantiateObject<T>(T objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, Transform parent = null) where T : Component
        {
            ObjectPool pool = null;

            foreach (ObjectPool objectPool in objectPools)
            {
                if (objectPool.PoolName == objectToSpawn.gameObject.name)
                {
                    pool = objectPool;
                    break;
                }
            }

            if (pool == null)
            {
                pool = new ObjectPool() { PoolName = objectToSpawn.gameObject.name };
                objectPools.Add(pool);
            }

            GameObject poolObject = pool.InactiveObjects.FirstOrDefault();
            T spawnableObject;

            if (poolObject == null)
            {
                if (parent == null)
                {
                    spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation, poolObjectsHolder);
                }
                else
                {
                    spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation, parent);
                }
            }
            else
            {
                spawnableObject = poolObject.GetComponent<T>();
                spawnableObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
                pool.InactiveObjects.Remove(spawnableObject.gameObject);
                spawnableObject.gameObject.SetActive(true);
            }

            return spawnableObject;
        }

        public void DestroyObject(GameObject objectToReturn, float delay = 0)
        {
            if (objectToReturn == null || !objectToReturn.activeInHierarchy)
            {
                return;
            }

            if (activeCoroutines.ContainsKey(objectToReturn))
            {
                StopCoroutine(activeCoroutines[objectToReturn]);
                activeCoroutines.Remove(objectToReturn);
            }

            if (delay == 0)
            {
                ReturnObjectToPool(objectToReturn);
            }
            else
            {
                Coroutine coroutine = StartCoroutine(DestroyObjectRoutine(objectToReturn, delay));
                activeCoroutines[objectToReturn] = coroutine;
            }
        }

        private void ReturnObjectToPool(GameObject objectToReturn)
        {
            if (objectToReturn == null)
            {
                return;
            }

            string realObjectName = objectToReturn.name.Replace("(Clone)", "");
            ObjectPool pool = objectPools.FirstOrDefault(p => p.PoolName == realObjectName);

            if (pool == null)
            {
                Debug.LogWarning($"There's no pool for: {realObjectName}");
                Destroy(objectToReturn);
            }
            else
            {
                objectToReturn.SetActive(false);
                if (!pool.InactiveObjects.Contains(objectToReturn))
                    pool.InactiveObjects.Add(objectToReturn);
            }
        }

        private IEnumerator DestroyObjectRoutine(GameObject objectToReturn, float delay)
        {
            yield return new WaitForSeconds(delay);

            if (activeCoroutines.ContainsKey(objectToReturn))
            {
                activeCoroutines.Remove(objectToReturn);
                ReturnObjectToPool(objectToReturn);
            }
        }
    }

    [Serializable]
    public class ObjectPool
    {
        public string PoolName;
        public List<GameObject> InactiveObjects = new();
    }
}