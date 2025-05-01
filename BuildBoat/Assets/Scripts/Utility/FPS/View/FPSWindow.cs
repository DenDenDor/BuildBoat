using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FPSWindow : AbstractWindowUi
{
    [SerializeField] private PostProcessVolume _volume;
    [SerializeField] private PostProcessProfile _mobile;
    [SerializeField] private PostProcessProfile _pc;
    
    public override void Init()
    {
        
    }

    public void UpdatePCProfile()
    {
        _volume.profile = _pc;
    }   
    
    public void UpdateMobileProfile()
    {
        _volume.profile = _mobile;
    }
}