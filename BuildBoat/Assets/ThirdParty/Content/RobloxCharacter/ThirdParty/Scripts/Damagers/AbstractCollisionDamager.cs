using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCollisionDamager : MonoBehaviour
{
    public abstract float CalculateDamage(Collision collision);
}
