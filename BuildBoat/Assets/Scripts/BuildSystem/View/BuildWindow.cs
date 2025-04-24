using UnityEngine;

public class BuildWindow : AbstractWindowUi
{
    public float PlaceDistance = 5f; // Дистанция установки
    public LayerMask BlockLayer; // Слой блоков

    public override void Init()
    {
        Debug.Log("BuildWindow!");

    }

    public void Remove(BlockView blockView)
    {
        blockView.DestroyBlock();
    }
}