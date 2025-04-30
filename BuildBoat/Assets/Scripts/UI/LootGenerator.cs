using DG.Tweening;
using UnityEngine;
using System.Collections;

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
    [SerializeField] private Sprite _testItemSprite;
    
    public void GetLoot(LootType lootType, Color rareColor)
    {
        _itemViewer.gameObject.SetActive(true);
        _itemViewer.View(_testItemSprite,2, rareColor);
        int itemsCount = 4;
        StartCoroutine(ItemsViewing(itemsCount));
    }

    private IEnumerator ItemsViewing(int itemsCount)
    {
        for (int i = 0; i < itemsCount; i++)
        {
            DOTween.Sequence()
                .Append(_itemViewer.transform.DOScale(1.2f, 0.2f))
                .Append(_itemViewer.transform.DOScale(1f, 0.05f));
            
            _itemViewer.View(_testItemSprite,2, Color.blue);
            
            yield return new WaitForSeconds(1f);
        }
    }
}
