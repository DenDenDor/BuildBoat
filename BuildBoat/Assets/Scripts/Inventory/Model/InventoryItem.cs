using UnityEngine;

public class InventoryItem
{
   public BlockType BlockType { get; set; }

   public InventoryItem(BlockType blockType)
   {
      BlockType = blockType;
   }
}
