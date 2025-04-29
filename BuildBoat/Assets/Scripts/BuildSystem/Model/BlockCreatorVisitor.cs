using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class BlockCreatorVisitor
{
    private BlockParticle _blockParticle;
    
    private Dictionary<BlockType, BlockView> _prefabs = new Dictionary<BlockType, BlockView>();

    public BlockCreatorVisitor()
    {
        _blockParticle = FactoryController.Instance.FindPrefab<BlockParticle>();
        
        _prefabs.Add(BlockType.Grass, FactoryController.Instance.FindPrefab<SimpleBlockView>());
        _prefabs.Add(BlockType.Motor, FactoryController.Instance.FindPrefab<MotorView>());
    }

    public BlockView Create(BlockType blockType, Vector3 placePosition, Quaternion rotation = default)
    {
        BlockView prefab = GetBlockPrefab(blockType);
        Quaternion finalRotation = rotation == default ? Quaternion.identity : rotation;
    
        BlockView created = Object.Instantiate(prefab, placePosition, finalRotation);
        created.UpdateColor(InventoryController.Instance.GetDestroyColor(blockType), _blockParticle);

        switch (created)
        {
            case MotorView motorView:
                break;
            case SimpleBlockView simpleBlockView:
                simpleBlockView.UpdateRenderer(InventoryController.Instance.GetMaterial(blockType));
                break;
        }

        return created;
    }

    private BlockView GetBlockPrefab(BlockType blockType)
    {
        if (_prefabs.ContainsKey(blockType))
        {
            return _prefabs[blockType];
        }

        return _prefabs[BlockType.Grass];
    }
}
