using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

public class BuildRouter : IRouter
{
    private BuildWindow Window => UiController.Instance.GetWindow<BuildWindow>();
    
    private BlockView _prefab;
    private Vector3? _startPoint;
    private Vector3? _endPoint;
    private bool _isSettingArea = false;

    public void Init()
    {
        _prefab = FactoryController.Instance.FindPrefab<BlockView>();

        _startPoint = Window.StartPoint.position;
        _endPoint = Window.EndPoint.position;
        FillAreaWithBlocks();

        UpdateController.Instance.Add(OnUpdate);

        Window.PutBlock.Clicked += () => SetMode(true);
        Window.DestroyBlock.Clicked += () => SetMode(false);
        SetMode(true, true);

    }

    private bool _isPutMode = true;

    private void SetMode(bool isPutMode, bool forceSet = false)
    {
        if (_isPutMode == isPutMode && !forceSet) return;

        PlayerView playerView = UiController.Instance.GetWindow<PlayerWindow>().PlayerView;
        
        _isPutMode = isPutMode;
        
        Window.PutBlock.Increase();
        Window.DestroyBlock.Decrease();
        
        if (isPutMode)
        {
            playerView.Pencil.Appear();
            playerView.Scissors.Disappear();
            
            Window.PutBlock.Increase();
            Window.DestroyBlock.Decrease();
        }
        else
        {
            playerView.Scissors.Appear();
            playerView.Pencil.Disappear();

            Window.PutBlock.Decrease();
            Window.DestroyBlock.Increase();
        }
    }

    private void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Проверяем, не кликнули ли мы по UI элементу
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (_isPutMode)
            {
                TryPlaceBlock();
            }
            else
            {
                TryRemoveBlock();
            }
        }
    }

    
    private void FillAreaWithBlocks()
    {
        if (!_startPoint.HasValue || !_endPoint.HasValue) return;

        Vector3 start = _startPoint.Value;
        Vector3 end = _endPoint.Value;

        // Ensure start has the smaller coordinates
        float minX = Mathf.Min(start.x, end.x);
        float minY = Mathf.Min(start.y, end.y);
        float minZ = Mathf.Min(start.z, end.z);
        
        float maxX = Mathf.Max(start.x, end.x);
        float maxY = Mathf.Max(start.y, end.y);
        float maxZ = Mathf.Max(start.z, end.z);

        for (float x = minX; x <= maxX; x++)
        {
            for (float y = minY; y <= maxY; y++)
            {
                for (float z = minZ; z <= maxZ; z++)
                {
                    Vector3 position = new Vector3(x, y, z);
                    Vector3 placePosition = GetPlacePosition(position);

                    if (y == maxY)
                    {
                        AddBlock(placePosition, BlockType.Grass);
                    }
                    else
                    {
                        AddBlock(placePosition, BlockType.Stone);
                    }
                }
            }
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
        blockView.UpdateRenderer(InventoryController.Instance.GetMaterial(blockType), InventoryController.Instance.GetDestroyColor(blockType));
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

public class BuildPosition
{
    public List<Vector3> Get()
    {
        return new List<Vector3>()
        {
            new Vector3(-1, 246, 26),
            new Vector3(0, 246, 26),
            new Vector3(1, 246, 26),
            new Vector3(2, 246, 26),
            new Vector3(3, 246, 26),
            new Vector3(4, 246, 26),
            new Vector3(5, 246, 26),
            new Vector3(-1, 246, 27),
            new Vector3(0, 246, 27),
            new Vector3(1, 246, 27),
            new Vector3(2, 246, 27),
            new Vector3(3, 246, 27),
            new Vector3(4, 246, 27),
            new Vector3(5, 246, 27),
        };
    }
}