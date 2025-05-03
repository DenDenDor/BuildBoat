using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
   public BlockType BlockType { get; set; }

   public InventoryItem(BlockType blockType)
   {
      BlockType = blockType;
   }
}
