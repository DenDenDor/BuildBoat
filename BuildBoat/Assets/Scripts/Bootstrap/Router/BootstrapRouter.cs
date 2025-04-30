using System.Collections;
using UnityEngine;

public class BootstrapRouter : IRouter
{
    public void Init()
    {
        DeviceData.IsMobile = SDKMediator.Instance.IsMobile;
        
        CoroutineController.Instance.RunCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);
        
        SceneController.Instance.LoadScene(SceneList.Battle);
    }

    public void Exit()
    {
        
    }
}
