using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillDamager : AbstractCollisionDamager
{
    public override float CalculateDamage(Collision collision)
    {
        return Mathf.Infinity;
    }
}
