using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    none,
    block_0,
    block_1,
    block_2,
    block_3,
    block_4,
    block_5,
    block_6,
    block_7,
    block_8,
    block_9,
    block_10,
    block_11,
    block_12,
    block_13,
    block_14,
    block_15,
    block_16,
    block_17,
    block_18,
    block_19,
    block_20,
    block_21,
    block_22,
    block_23,
    block_24,
    block_25,
    block_26,
    block_27,
    block_28,
    block_29,
    block_30,
    block_31,
    block_32,
    block_33,
    block_34,
    block_35,
}
public abstract class PoolMember : MonoBehaviour
{
    [SerializeField] protected PoolType _poolType;
    public PoolType poolType { get => _poolType; }
}