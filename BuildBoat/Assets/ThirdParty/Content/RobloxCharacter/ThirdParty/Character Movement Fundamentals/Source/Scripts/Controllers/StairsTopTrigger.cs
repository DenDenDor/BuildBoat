namespace CMF
{
    public class StairsTopTrigger : StairsTrigger
    {
        protected override void OnPlayerEnter(StairsWalkerController stairsController)
        {
            if (!stairsController.IsPlayerOnStair) return;
            base.OnPlayerEnter(stairsController);
        }
    }
}