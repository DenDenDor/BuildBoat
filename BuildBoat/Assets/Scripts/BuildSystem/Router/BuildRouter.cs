using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class BuildRouter : IRouter
{
    private BuildWindow Window => UiController.Instance.GetWindow<BuildWindow>();
    
   private BlockView _prefab;
   

    public void Init()
    {
        _prefab = FactoryController.Instance.FindPrefab<BlockView>();

        AddBlock(GetPlacePosition(new Vector3(-1, 247, 26)), BlockType.Earth);
        
        UpdateController.Instance.Add(OnUpdate);
    }

    private void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceBlock();
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            TryRemoveBlock();
        }
    }

    private void TryPlaceBlock()
    {
        if (InventoryController.Instance.CountSelectedBlocks <= 0)
        {
            return;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Window.PlaceDistance, Window.BlockLayer))
        {
            Vector3 placePosition = hit.point + hit.normal * 0.5f;

            placePosition = GetPlacePosition(placePosition);

            Vector3 halfExtents = new Vector3(0.499f, 0.499f, 0.499f);

            if (!Physics.CheckBox(placePosition, halfExtents, Quaternion.identity, Window.BlockLayer))
            {
                AddBlock(placePosition, InventoryController.Instance.Selected);
                
                InventoryController.Instance.TakeBlock();
                
                Window.PlayBuildAnimation();
            }
        }
    }

    private void AddBlock(Vector3 placePosition, BlockType blockType)
    {
        BlockView blockView = Object.Instantiate(_prefab, placePosition, Quaternion.identity);
        blockView.UpdateRenderer(InventoryController.Instance.GetMaterial(blockType));
        BuildController.Instance.Add(blockView, blockType);
    }

    private Vector3 GetPlacePosition(Vector3 placePosition)
    {
        placePosition = new Vector3(
            Mathf.Round(placePosition.x),
            Mathf.Round(placePosition.y),
            Mathf.Round(placePosition.z)
        );
        return placePosition;
    }

    private void TryRemoveBlock()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Window.PlaceDistance, Window.BlockLayer) && hit.collider.TryGetComponent(out BlockView blockView))
        {
            BlockType blockType = BuildController.Instance.GetBlockType(blockView);

            Window.Remove(blockView);
            
            InventoryController.Instance.AddBlock(blockType);
        }
    }

    public void Exit()
    {
        
    }
}