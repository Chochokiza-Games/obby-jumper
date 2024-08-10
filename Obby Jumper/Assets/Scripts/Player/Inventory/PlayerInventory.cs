using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class PlayerInventory : MonoBehaviour
{
    public int BucketSize
    {
        get => _bucket.Count;
    }

    public int BucketCap
    {
        get => _bucketSizeCap;
    }

    [SerializeField] private int _bucketSizeCap;
    [SerializeField] private BaseInventoryItem[] _references;
    [SerializeField] private UnityEvent<int, BaseInventoryItem> _itemAdded; //id, scriptable object
    [SerializeField] private UnityEvent<int, BaseInventoryItem> _itemRemoved; //id, scriptable object

    private GameObject _errorToast;
    private Dictionary<int, BaseInventoryItem> _bucket = new Dictionary<int, BaseInventoryItem>();

    private BaseInventoryItem ConvertItemIdToItem(BaseInventoryItem.ItemId itemId)
    {
        foreach (BaseInventoryItem item in _references) 
        {
            if ((int)itemId == item.Id)
            {
                return Instantiate(item);
            }
        }
        return null;
    }

    public int PushItem(BaseInventoryItem.ItemId itemId)
    {
        BaseInventoryItem item = ConvertItemIdToItem(itemId);
        
        if (item == null)
        {
            Debug.LogError($"PlayerInventory: item id {itemId} not found");
            return -1;
        }

        if (_bucket.Count >= _bucketSizeCap)
        {
            RemoveById(_bucket.ElementAt(0).Key);
        }

        int id = Random.Range(0, int.MaxValue);
        _bucket[id] = item;
        _itemAdded.Invoke(id, item);
        if (Debug.isDebugBuild)
        {
            string bucketDump = "";
            foreach (var kv  in _bucket)
            {
                bucketDump += $"{kv.Key} -> {kv.Value}\n";
            }
            Debug.Log($"PlayerInventory: pushed item {itemId}\nBucket dump:\n{bucketDump}");
        }

        return id;
    }

    public bool HasSize()
    {
        return _bucket.Count < _bucketSizeCap;
    }

    public void RemoveById(int id)
    {
        BaseInventoryItem item = _bucket[id];
        _bucket.Remove(id);
        if (Debug.isDebugBuild)
        {
            string bucketDump = "";
            foreach (var kv in _bucket)
            {
                bucketDump += $"{kv.Key} -> {kv.Value}\n";
            }
            Debug.Log($"PlayerInventory: removed item on id {id}\nBucket dump:\n{bucketDump}");
        }
        _itemRemoved.Invoke(id, item);

        if (_errorToast != null && _bucket.Count < _bucketSizeCap - 1)
        {
            Destroy(_errorToast);
            _errorToast = null;
        }
    }

    public BaseInventoryItem GetItemById(int id)
    {
        if (!_bucket.ContainsKey(id))
        {
            Debug.LogError($"PlayerInventory: id {id} not found");
            return null;
        }

        return _bucket[id];
    }

    public BaseInventoryItem[] GetAllItems()
    {
        List<BaseInventoryItem> items = new List<BaseInventoryItem>();
        foreach (var item in _bucket)
        {
            items.Add(item.Value);
        }

        return items.ToArray();
    }

    public KeyValuePair<int, BaseInventoryItem> GetFirstElement()
    {
        return _bucket.ElementAt(_bucket.Count - 1);
    }
}
