using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(LootGenerator))]
public class LootPanel : MonoBehaviour
{
    public static LootPanel Instance {get; private set;}
    [SerializeField] private Button _chest;
    [SerializeField] private Image _backLight;

    [SerializeField] private GameObject _lootPanel;
    [SerializeField] private GameObject _fingerObject;
    [SerializeField] private Transform _itemsBackLight;
    [SerializeField] private RectTransform _fingerEndPos;

    [SerializeField] private Color _offChestColor;
    [SerializeField] private Sprite _testOpenChest;
    
    private LootGenerator _lootGenerator;
    
    private Tween _fingerMoveTween;

    private Color _chestRareColor;

    private LootType _chestType;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    private void Start()
    {
        _lootGenerator = GetComponent<LootGenerator>();
    }

    public void LootPanelOpen(Sprite chestSprite, Color backLightColor, LootType chestRare)
    {
        _fingerObject.GetComponent<RectTransform>().DOAnchorPos(_fingerEndPos.anchoredPosition, 0.25f).SetLoops(-1, LoopType.Yoyo);
        
        _fingerObject.SetActive(true);

        _chestRareColor = backLightColor;

        _itemsBackLight.gameObject.SetActive(false);

        _lootPanel.SetActive(true);

        _chest.image.color = Color.white;
        _chest.image.sprite = chestSprite;
        _backLight.color = backLightColor;
        
        _chest.onClick.AddListener(ChestOpen);
    }

    private void ChestOpen()
    {
        _chest.onClick.RemoveListener(ChestOpen);
        _fingerObject.SetActive(false);

        DOTween.Sequence()
        .Append(_chest.transform.DOScale(1.2f, 0.09f))
        .Append(_chest.transform.DOScale(1, 0.05f))
        .AppendInterval(0.04f)
        .OnComplete(() =>
            {
                _chest.image.color = _offChestColor;
                GetLoot();
            });
    }

    private void GetLoot()
    {
        _itemsBackLight.localScale = Vector3.zero;
        _itemsBackLight.gameObject.SetActive(true);
        _itemsBackLight.DOScale(1f, 0.1f);

        _lootGenerator.GetLoot(_chestType, _chestRareColor);
    }
}
