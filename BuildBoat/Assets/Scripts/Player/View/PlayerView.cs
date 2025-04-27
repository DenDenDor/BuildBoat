using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private HandView _scissors;
    [SerializeField] private HandView _pencil;

    public HandView Scissors => _scissors;

    public HandView Pencil => _pencil;
}
