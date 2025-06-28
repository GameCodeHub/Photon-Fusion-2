using Fusion;
using UnityEngine;
using Spine.Unity;
using Fusion.Addons.FSM;
using System.Collections.Generic;

[RequireComponent(typeof(StateMachineController))]
public class PlayerController : NetworkBehaviour, IStateMachineOwner
{
    [SerializeField] private SkeletonAnimation skeletonAnimation; // Hoạt ảnh Spine Pro
    [SerializeField] private Rigidbody2D rigidbody2D; // Rigidbody2D để xử lý vật lý
    public float moveSpeed = 5f; // Tốc độ di chuyển
    public float jumpForce = 5f; // Lực nhảy

    public bool _isGrounded; // Trạng thái chạm đất
    public NetworkInputData _inputData; // Dữ liệu đầu vào mạng
    private StateMachine<StateBehaviour> _stateMachine;
   
    // Thiết lập hoạt ảnh Spine
    public void SetAnimation(string animationName, bool loop = true)
    {
        if (skeletonAnimation != null && skeletonAnimation.state != null)
        {
            skeletonAnimation.state.SetAnimation(0, animationName, loop);
        }
    }

    // Kiểm tra trạng thái đầu vào
    public bool HasMovementInput() => Mathf.Abs(_inputData.movement.x) > 0.1f;
    public bool IsJumping() => _inputData.jump && _isGrounded;
    public bool IsAttacking() => _inputData.attack;

    // Được gọi khi nhân vật spawn
    public override void Spawned()
    {
        RPC_SetAnimation("holdon", true); // Thiết lập hoạt ảnh đứng yên khi spawn
    }

    // Gọi từ FSM: Đồng bộ hoạt ảnh qua RPC
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SetAnimation(string animationName, bool loop)
    {
        SetAnimation(animationName, loop);
    }

    // Xử lý đầu vào mạng
    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority || !GetInput(out _inputData))
            return;

        // Cập nhật biến để xem trên Inspector
        movementX = _inputData.movement.x;
        isJumping = _inputData.jump && _isGrounded;
        isAttacking = _inputData.attack;

        // Nếu đang ở JumpState hoặc AttackState thì KHÔNG chuyển state ở đây!
        if (_stateMachine.ActiveState is JumpState || _stateMachine.ActiveState is AttackState)
            return;

        // Chỉ chuyển vào Jump/Attack khi nhận input
        if (IsAttacking())
            _stateMachine.TryActivateState<AttackState>();
        else if (IsJumping())
            _stateMachine.TryActivateState<JumpState>();
        else if (!HasMovementInput())
            _stateMachine.TryActivateState<IdleState>();
        else
            _stateMachine.TryActivateState<MoveState>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }

    // Thu thập các trạng thái
    void IStateMachineOwner.CollectStateMachines(List<IStateMachine> stateMachines)
    {
        _stateMachine = new StateMachine<StateBehaviour>("PlayerStateMachine",
            GetComponent<IdleState>(),
            GetComponent<MoveState>(),
            GetComponent<JumpState>(),
            GetComponent<AttackState>());

        stateMachines.Add(_stateMachine);
    }

    [SerializeField] public bool isJumping;
    [SerializeField] public bool isAttacking;
    [SerializeField] public float movementX;
}
