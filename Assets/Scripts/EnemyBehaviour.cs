using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
internal class EnemyBehaviour : MonoBehaviour
{
    bool _isAgro = false;
    Transform _playerTransform;
    CharacterController _characterController;
    CharacterStates _state;
    private void Start()
    {
        _playerTransform = GameManager.game_manager.player_transform;
        _characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        CheckCharacterState();

        if (_state == CharacterStates.Moving)
        {
            transform.LookAt(_playerTransform);
            _characterController.Move(_playerTransform.position);
        }
        else if (_state == CharacterStates.Attacking)
            Attack();

    }

    private void CheckCharacterState()
    {
        float _distanceFromPlayer = Vector2.Distance(transform.position, _playerTransform.position);

        if ((_distanceFromPlayer >= 10))
        {
            if (_distanceFromPlayer <= 1f)
                _state = CharacterStates.Attacking;
            else
                _state = CharacterStates.Moving;
        }
        else
            _state = CharacterStates.None;
    }

    private void Attack()
    {
        Ray _ray = new Ray(transform.position, transform.forward * 3);

        if (Physics.Raycast(_ray, 5))
        {

        }
    }
}