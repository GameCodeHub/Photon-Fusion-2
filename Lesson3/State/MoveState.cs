using UnityEngine;
using Fusion.Addons.FSM;

public class MoveState : StateBehaviour
{
    private PlayerController _player;

    protected override void OnEnterState()
    {
        _player = GetComponent<PlayerController>();
        if (_player.HasStateAuthority)
        {
            _player.RPC_SetAnimation("move", true); // Đồng bộ hoạt ảnh di chuyển cho tất cả client
        }
    }
    protected override void OnFixedUpdate()
    {
        if (!_player.HasStateAuthority || !GetInput(out _player._inputData))
            return;
        Vector2 moveDirection = new Vector2(_player._inputData.movement.x * _player.moveSpeed, _player.GetComponent<Rigidbody2D>().linearVelocity.y);
        _player.GetComponent<Rigidbody2D>().linearVelocity = moveDirection;

        if (!_player.HasMovementInput())
            Machine.TryActivateState<IdleState>();
        else if (_player.IsJumping())
            Machine.TryActivateState<JumpState>();
        else if (_player.IsAttacking())
            Machine.TryActivateState<AttackState>();
        
    }


}

