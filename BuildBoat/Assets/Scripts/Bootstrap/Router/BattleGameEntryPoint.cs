using System.Collections.Generic;
using UnityEngine;

public class BattleGameEntryPoint : AbstractGameEntryPoint
{
    protected override List<IRouter> Routers => new List<IRouter>()
    {
        new UpdateRouter(),
        new ShipRouter(),
        new FactoryRouter(),
        new BuildRouter(),
        new InventoryRouter(),
        new FPSRouter(),
    };
}