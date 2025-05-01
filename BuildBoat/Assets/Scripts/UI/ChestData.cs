using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct LootItem
{
    public BlockType GetBlockType => _blockType;
    public int MinAmount => _minAmount;
    public int MaxAmount => _maxAmount;
    public float DropChance => _dropChance;
    
    [SerializeField] private BlockType _blockType;
    [SerializeField] private int _minAmount;
    [SerializeField] private int _maxAmount;
    [Range(0f, 1f)] [SerializeField] private float _dropChance;
}

[CreateAssetMenu(fileName = "ChestData", menuName = "Scriptable Objects/Chest Data")]
public class ChestData : ScriptableObject
{
    public LootType GetLootType => _lootType;
    public Color RareColor => _rareColor;
    public List<LootItem> GuaranteedLoot => _guaranteedLoot;
    public List<LootItem> PossibleLoot => _possibleLoot;
    public int MaxPossibleItems => _maxPossibleItems;
    
    [SerializeField] private LootType _lootType;
    [SerializeField] private Color _rareColor;

    [SerializeField] private List<LootItem> _guaranteedLoot;

    [SerializeField] private List<LootItem> _possibleLoot;
    
    [SerializeField] private int _maxPossibleItems = 3;
}
