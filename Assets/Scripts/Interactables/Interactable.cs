using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected float _distance = 1.5f;
    protected Transform _player;
    protected Character_Behaviour _behaviour;

    protected void Start()
    {
        _distance = GameManager.game_manager.player_transform.GetComponent<Character_Stats>().pick_up_distance;
        _behaviour = GameManager.game_manager.player_transform.GetComponent<Character_Behaviour>();
        _player = GameManager.game_manager.player_transform;
    }

    public virtual void Interact()
    {
        throw new System.NotImplementedException();
    }
}