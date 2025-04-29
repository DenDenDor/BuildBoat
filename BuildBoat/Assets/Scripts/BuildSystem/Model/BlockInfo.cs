using UnityEngine;

public class BlockInfo
{
    public BlockType Type;
    public bool CanBeDestroy;

    public BlockInfo(BlockType type, bool canBeDestroy)
    {
        Type = type;
        CanBeDestroy = canBeDestroy;
    }
}
