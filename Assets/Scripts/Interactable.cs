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

    protected void Update()
    {
        if (Vector3.Distance(transform.position, _player.position) <= _distance)
            _behaviour.AddToInteractableList(this);
        else
            _behaviour.RemoveFromInteractableList(this);
    }
    private void OnDestroy()
    {
        _behaviour.RemoveFromInteractableList(this);
    }

    public virtual void Interact()
    {
        throw new System.NotImplementedException();
    }
}