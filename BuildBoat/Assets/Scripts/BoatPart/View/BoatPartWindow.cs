using UnityEngine;

public class BoatPartWindow : AbstractWindowUi
{
    [SerializeField] private BoatZone _boatZone;

    public BoatZone BoatZone => _boatZone;

    public override void Init()
    {
        
    }
}