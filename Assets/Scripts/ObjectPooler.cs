using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoomBloxDemo
{
    public class ObjectPooler : MonoBehaviour
    {
        private GameObject _pooledItem;
        private int _initialSize;
        private int _maxSize;

        private List<GameObject> _items; //actualItem, isInUse

        private GameObject _parentObject;

        private bool _isInit = false;
        public void Init(GameObject pooledItem, int initialSize = 20, int maxSize = 50)
        {
            if (_isInit) return;
            _isInit = true;

            _pooledItem = pooledItem;
            _initialSize = initialSize;
            _maxSize = maxSize;
            _items = new List<GameObject>();
            _parentObject = new GameObject("_pool_" + pooledItem.name);
            StartCoroutine(InitAsync());
        }

        IEnumerator InitAsync()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                var item = Instantiate(_pooledItem, _parentObject.transform);
                item.SetActive(false);
                _items.Add(item);
                yield return null;
            }
        }

        public GameObject Instantiate()
        {
            foreach (var item in _items)
            {
                if (item.activeInHierarchy != true)
                {
                    item.SetActive(true);
                    return item;
                }
            }
            //no items found, increase size of pool
            if (_items.Count < _maxSize)
            {
                var item = Instantiate(_pooledItem, _parentObject.transform);
                _items.Add(item);
                return item;
            }
            else
                return null;
        }

        public void Destroy(GameObject item)
        {
            item.SetActive(false);
        }
    }
}
