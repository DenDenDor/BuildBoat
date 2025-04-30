using System.Collections.Generic;
using UnityEngine;

public class BoostrapGameEntryPoint : AbstractGameEntryPoint
{
    protected override List<IRouter> Routers => new List<IRouter>()
    {
        new BootstrapRouter()
    };
}