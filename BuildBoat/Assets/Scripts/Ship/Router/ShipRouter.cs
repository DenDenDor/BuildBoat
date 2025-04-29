using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipRouter : IRouter
{
    private ShipWindow Window => UiController.Instance.GetWindow<ShipWindow>();

    public void Init()
    {
        Window.ShipButton.Clicked += OnClicked;
    }

    private void OnClicked()
    {
        List<BlockView> listInZone = UiController.Instance.GetWindow<BoatPartWindow>().BoatZone.BlockViews;
        List<BlockView> allBlocks = BuildController.Instance.Blocks;

        HashSet<BlockView> connectedBlocks = GetConnectedBlocks(listInZone, allBlocks);

        ShipView shipView = Object.FindObjectOfType<ShipView>();
        if (shipView == null)
        {
            GameObject shipObject = new GameObject("Ship");
            shipView = shipObject.AddComponent<ShipView>();
            shipObject.AddComponent<Rigidbody>();
        }

        foreach (BlockView block in connectedBlocks)
        {
            block.transform.SetParent(shipView.transform);
        }

        shipView.CalculateCenterOfMass(new List<BlockView>(connectedBlocks));
        shipView.motorAttachmentPoint = connectedBlocks.FirstOrDefault(x=>x is MotorView)?.transform;

        Rigidbody shipRigidbody = shipView.GetComponent<Rigidbody>();
        shipRigidbody.mass = connectedBlocks.Count * 0.1f; 
        shipRigidbody.linearDamping = 0.5f;
        shipRigidbody.angularDamping = 0.5f;
    }

    private HashSet<BlockView> GetConnectedBlocks(List<BlockView> listInZone, List<BlockView> allBlocks)
    {
        HashSet<BlockView> connectedBlocks = new HashSet<BlockView>();
        Queue<BlockView> blocksToProcess = new Queue<BlockView>();

        foreach (BlockView block in listInZone)
        {
            if (!connectedBlocks.Contains(block))
            {
                connectedBlocks.Add(block);
                blocksToProcess.Enqueue(block);
            }
        }

        while (blocksToProcess.Count > 0)
        {
            BlockView currentBlock = blocksToProcess.Dequeue();

            foreach (BlockView candidateBlock in allBlocks)
            {
                if (!connectedBlocks.Contains(candidateBlock) && IsAdjacent(currentBlock, candidateBlock))
                {
                    connectedBlocks.Add(candidateBlock);
                    blocksToProcess.Enqueue(candidateBlock);
                }
            }
        }

        return connectedBlocks;
    }

    private bool IsAdjacent(BlockView block1, BlockView block2)
    {
        Vector3 pos1 = block1.transform.position;
        Vector3 pos2 = block2.transform.position;

        float distance = Vector3.Distance(pos1, pos2);
        return distance <= Mathf.Sqrt(3) + 0.001f; 
    }

    public void Exit()
    {
        Window.ShipButton.Clicked -= OnClicked;
    }
}