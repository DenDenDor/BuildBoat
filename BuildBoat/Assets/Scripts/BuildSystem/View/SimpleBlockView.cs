using UnityEngine;

public class SimpleBlockView : BlockView
{
    [SerializeField] private MeshRenderer _meshRenderer;

    public void UpdateRenderer(Material newMaterial)
    {
        _meshRenderer.material = newMaterial;
    }
}
