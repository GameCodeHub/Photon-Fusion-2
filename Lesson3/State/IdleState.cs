using UnityEngine;
using Fusion.Addons.FSM;

public class IdleState : StateBehaviour
{
    private PlayerController _player;
    protected override void OnEnterState()
    {
        _player = GetComponent<PlayerController>();
        if (_player.HasStateAuthority)
        {
            _player.RPC_SetAnimation("holdon", true); // Đồng bộ hoạt ảnh đứng yên cho tất cả client
        }
    }
    protected override void OnFixedUpdate()
    {
        if (_player.HasMovementInput())
        {
            Machine.TryActivateState<MoveState>();
        }
        else if (_player.IsJumping())
        {
            Machine.TryActivateState<JumpState>();
        }
        else if (_player.IsAttacking())
        {
            Machine.TryActivateState<AttackState>();
        }
    }
}


