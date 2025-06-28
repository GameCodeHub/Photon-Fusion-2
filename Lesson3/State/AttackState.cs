using UnityEngine;
using Fusion.Addons.FSM;

public class AttackState : StateBehaviour
{
    private PlayerController _player;

    protected override void OnEnterState()
    {
        _player = GetComponent<PlayerController>();
        if (_player.HasStateAuthority)
        {
            _player.RPC_SetAnimation("att", true); // Đồng bộ hoạt ảnh tấn công cho tất cả client
        }
    }

    protected override void OnFixedUpdate()
    {
        // Kiểm tra nếu thời gian hoạt ảnh đã hoàn thành
        if (Machine.StateTime > 0.7f) // 0.7f là thời gian tấn công
        {
            Machine.TryActivateState<IdleState>();
        }
    }
}
