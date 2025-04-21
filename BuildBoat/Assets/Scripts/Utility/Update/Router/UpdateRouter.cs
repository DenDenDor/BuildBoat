using System.Collections;
using UnityEngine;

public class UpdateRouter : IRouter
{
    private UpdateWindow Window => UiController.Instance.GetWindow<UpdateWindow>();

    private Coroutine _coroutine;

    public void Init()
    {
        OnContinueMovingTime();
        
        UpdateController.Instance.ContinueMovingTime += OnContinueMovingTime;
        UpdateController.Instance.StoppedTime += OnStopTime;
    }

    private void OnStopTime()
    {
        Window.StopCoroutine(_coroutine);
    }

    private void OnContinueMovingTime()
    {
        _coroutine = Window.StartCoroutine(Update());
    }

    private IEnumerator Update()
    {
        while (true)
        {
            UpdateController.Instance.UpdateActions();

            yield return null;
        }
    }

    public void Exit()
    {
        OnStopTime();
    }
}