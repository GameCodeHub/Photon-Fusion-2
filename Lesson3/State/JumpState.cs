using UnityEngine;
using Fusion.Addons.FSM;

public class JumpState : StateBehaviour
{
    private PlayerController _player;

    protected override void OnEnterState()
    {
        _player = GetComponent<PlayerController>();
        if (_player.HasStateAuthority)
        {
            _player.RPC_SetAnimation("back", true);
            _player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * _player.jumpForce, ForceMode2D.Impulse);
          
        }
    }

    protected override void OnFixedUpdate()
    {
        if (_player._isGrounded)
            Machine.TryActivateState<IdleState>();
    }
}
