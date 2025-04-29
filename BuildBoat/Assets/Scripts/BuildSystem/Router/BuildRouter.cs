using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

public class BuildRouter : IRouter
{
    private BuildWindow Window => UiController.Instance.GetWindow<BuildWindow>();
    
    private Vector3? _startPoint;
    private Vector3? _endPoint;

    private BlockCreatorVisitor _blockCreatorVisitor = new();
    
    public void Init()
    {
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
                        AddBlock(placePosition, BlockType.Grass, false);
                    }
                    else
                    {
                        AddBlock(placePosition, BlockType.Rock, false);
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
                AddBlock(placePosition, InventoryController.Instance.Selected, true);
                InventoryController.Instance.TakeBlock();
                Window.PlayBuildAnimation();
            }
        }
    }

    private void AddBlock(Vector3 placePosition, BlockType blockType, bool canBeDestroy)
    {
        if (blockType == BlockType.Motor)
        {
            Vector3 directionToPlayer = UiController.Instance.GetWindow<PlayerWindow>().PlayerView.transform.position - placePosition;
            directionToPlayer.y = 0;
        
            if (directionToPlayer != Vector3.zero)
            {
                float angle = Mathf.Round(Vector3.SignedAngle(Vector3.back, directionToPlayer, Vector3.up) / 90) * 90;
                Quaternion rotation = Quaternion.Euler(0, angle, 0);
                var blockView = _blockCreatorVisitor.Create(blockType, placePosition, rotation);
                BuildController.Instance.Add(blockView, new BlockInfo(blockType, canBeDestroy));
                return;
            }
        }
    
        var blockView1 = _blockCreatorVisitor.Create(blockType, placePosition);
        BuildController.Instance.Add(blockView1, new BlockInfo(blockType, canBeDestroy));
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
    
        if (Physics.Raycast(ray, out hit, Window.PlaceDistance, Window.BlockLayer) && 
            hit.collider.TryGetComponent(out BlockView blockView))
        {
            if (BuildController.Instance.Blocks.Contains(blockView)) // Check if block exists
            {
                BlockInfo blockInfo = BuildController.Instance.GetBlockType(blockView);

                if (blockInfo.CanBeDestroy)
                {
                    BlockType blockType = blockInfo.Type;
                    BuildController.Instance.Remove(blockView);
                    Window.Remove(blockView);
                    InventoryController.Instance.AddBlock(blockType);
                }
            }
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