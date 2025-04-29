using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoatZone : MonoBehaviour
{
    private List<BlockView> _blockViews = new();

    public List<BlockView> BlockViews => _blockViews.Where(x=>x != null).ToList();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BlockView blockView))
        {
            _blockViews.Add(blockView);
        }
    }
}
