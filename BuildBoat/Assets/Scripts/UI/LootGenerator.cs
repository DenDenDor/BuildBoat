using System;
using DG.Tweening;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public enum LootType
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public class LootGenerator : MonoBehaviour
{
    [SerializeField] private ItemViewer _itemViewer;
    [SerializeField] private List<ChestData> _chestDatas;

    [Header("Block Icons")]
    [SerializeField] private List<BlockIconPair> _blockIconList = new List<BlockIconPair>();

    private Dictionary<BlockType, Sprite> _blockIconsDictionary = new Dictionary<BlockType, Sprite>();

    private LootPanel _lootPanel;
    
    private void Awake()
    {
        _blockIconsDictionary = _blockIconList.ToDictionary(pair => pair.GetBlockType, pair => pair.GetIcon);
    }

    private void Start()
    {
        _lootPanel = GetComponent<LootPanel>();
    }
    
    public void GetLoot(LootType lootType)
    {
        ChestData chestData = _chestDatas.FirstOrDefault(data => data.GetLootType == lootType);

        if (chestData == null)
        {
            Debug.LogError("No ChestData found for LootType: " + lootType);
            return;
        }

        List<KeyValuePair<BlockType, int>> droppedItems = GenerateLoot(chestData);

        _itemViewer.gameObject.SetActive(true);

        StartCoroutine(ShowLoot(droppedItems, chestData.RareColor));
    }
    
    private List<KeyValuePair<BlockType, int>> GenerateLoot(ChestData chestData)
    {
        List<KeyValuePair<BlockType, int>> droppedItems = new List<KeyValuePair<BlockType, int>>();

        foreach (LootItem lootItem in chestData.GuaranteedLoot)
        {
            int amount = Random.Range(lootItem.MinAmount, lootItem.MaxAmount + 1);
            droppedItems.Add(new KeyValuePair<BlockType, int>(lootItem.GetBlockType, amount));
        }

        int possibleItemsCount = 0;
        foreach (LootItem lootItem in chestData.PossibleLoot)
        {
            if (possibleItemsCount < chestData.MaxPossibleItems && Random.value <= lootItem.DropChance)
            {
                int amount = Random.Range(lootItem.MinAmount, lootItem.MaxAmount + 1);
                droppedItems.Add(new KeyValuePair<BlockType, int>(lootItem.GetBlockType, amount));
                possibleItemsCount++;
            }
        }

        return droppedItems;
    }

    
    private IEnumerator ShowLoot(List<KeyValuePair<BlockType, int>> droppedItems, Color rareColor)
    {
        foreach (var item in droppedItems)
        {
            DOTween.Sequence()
                .Append(_itemViewer.transform.DOScale(1.3f, 0.3f))
                .Append(_itemViewer.transform.DOScale(1f, 0.12f))
                .AppendInterval(0.25f)
                .Append(_itemViewer.transform.DOScale(0f, 0.2f));

            Sprite icon = GetIconForBlockType(item.Key);

            for (int i = 0; i < item.Value; i++)
            {
                InventoryController.Instance.AddBlock(item.Key);
            }

            _itemViewer.View(icon, item.Value, rareColor);
            yield return new WaitForSeconds(0.7f);
        }

        _lootPanel.ClosePanel();
    }
    
    private Sprite GetIconForBlockType(BlockType blockType) => _blockIconsDictionary[blockType];
}
