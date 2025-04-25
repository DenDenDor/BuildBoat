using UnityEngine;

public class BuildWindow : AbstractWindowUi
{
    [SerializeField] private Animator _animator;
    
    public float PlaceDistance = 5f; // Дистанция установки
    public LayerMask BlockLayer; // Слой блоков

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