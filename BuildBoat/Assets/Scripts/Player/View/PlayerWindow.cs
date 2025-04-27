using UnityEngine;

public class PlayerWindow : AbstractWindowUi
{
    [SerializeField] private PlayerView _playerView;

    public PlayerView PlayerView => _playerView;

    public override void Init()
    {
        
    }
}