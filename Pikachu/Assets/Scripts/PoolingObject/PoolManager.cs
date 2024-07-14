using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PoolManager;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public PoolType tag;
        public PoolMember prefab;
        public int size;
    }
    [SerializeField] protected List<Pool> list;
    [SerializeField] protected Transform holder;
    protected Dictionary<PoolType, Queue<PoolMember>> poolDictionary;
    public static PoolManager instance;
    protected virtual void Awake()
    {
        instance = this;
        this.poolDictionary = new Dictionary<PoolType, Queue<PoolMember>>();
        foreach (Pool pool in list)
        {
            this.poolDictionary[pool.tag] = new Queue<PoolMember>();
            for (int i = 0; i < pool.size; i++)
            {
                this.pushPrefabToPool(pool.tag, pool.prefab);
            }
        }
    }
    protected void pushPrefabToPool(PoolType name, PoolMember prefab)
    {
        PoolMember clone = Instantiate(prefab);
        clone.gameObject.SetActive(false);
        this.poolDictionary[name].Enqueue(clone);
    }
    public Boolean hasPool(PoolType name)
    {
        return poolDictionary.ContainsKey(name);
    }

    public Queue<PoolMember> getPool(PoolType name)
    {
        if (!this.hasPool(name))
        {
            this.poolDictionary.Add(name, new Queue<PoolMember>());
        }
        return this.poolDictionary[name];
    }
    public void expand(PoolType name)
    {
        PoolMember tmp = this.findPrefab(name);
        this.pushPrefabToPool(name, tmp);
    }
    public PoolMember findPrefab(PoolType name)
    {
        foreach (Pool pool in this.list)
        {
            if (pool.tag == name)
            {
                return pool.prefab;
            }
        }
        return null;
    }
    public PoolMember spawn(PoolType name, Vector3 pos, Quaternion rot)
    {
        Queue<PoolMember> tmp = this.getPool(name);
        if (tmp.Count == 0)
        {
            this.expand(name);
        }
        PoolMember clone = tmp.Dequeue();
        clone.gameObject.SetActive(true);
        clone.transform.position = pos;
        clone.transform.rotation = rot;
        clone.transform.SetParent(this.holder);
        return clone;
    }
    public T spawnT<T>(PoolType name, Vector3 pos, Quaternion rot) where T : PoolMember
    {
        return spawn(name, pos, rot) as T;
    }
    public void despawn(PoolMember clone)
    {
        if (clone.gameObject.activeSelf)
        {
            clone.gameObject.SetActive(false);
            this.getPool(clone.poolType).Enqueue(clone);
        }
    }
    public void collectAll()
    {
        foreach (Transform child in this.holder)
        {
            despawn(child.GetComponent<PoolMember>());
        }
    }
}