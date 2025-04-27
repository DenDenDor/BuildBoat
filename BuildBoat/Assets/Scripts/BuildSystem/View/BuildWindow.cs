using UnityEngine;

public class BuildWindow : AbstractWindowUi
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private BuildWayButton _putBlock;
    [SerializeField] private BuildWayButton _destroyBlock;

    public BuildWayButton PutBlock => _putBlock;

    public BuildWayButton DestroyBlock => _destroyBlock;

    public float PlaceDistance = 5f; // Дистанция установки
    public LayerMask BlockLayer; // Слой блоков

    public Transform StartPoint => _startPoint;

    public Transform EndPoint => _endPoint;

    public override void Init()
    {
        Debug.Log("BuildWindow!");
    }

    public void PlayBuildAnimation()
    {
        _animator.SetTrigger("OnAttack2");
    }

    public void Remove(BlockView blockView)
    {
        blockView.DestroyBlock();
    }
}