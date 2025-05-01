using System;
using UnityEngine;

[Serializable]
public class BlockIconPair
{
    public BlockType GetBlockType => _blockType;
    public Sprite GetIcon => _icon;
    
    [SerializeField] private BlockType _blockType;
    [SerializeField] private Sprite _icon;
}
