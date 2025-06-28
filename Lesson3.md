# Bài học: Finite State Machine (FSM) trong Photon Fusion

Finite State Machine (FSM) là một mô hình toán học mạnh mẽ, đặc biệt hữu ích trong việc quản lý trạng thái và hành vi phức tạp của các đối tượng trong game. Trong Photon Fusion, FSM giúp tổ chức và quản lý hành vi của các thực thể trong môi trường mạng một cách hiệu quả.

---

## 1. Tổng quan về FSM
FSM là một mô hình gồm:
- **Trạng thái (States):** Các trạng thái của đối tượng.
- **Chuyển tiếp (Transitions):** Quy tắc và điều kiện chuyển giữa các trạng thái.
- **Sự kiện (Events):** Tác nhân kích hoạt sự chuyển tiếp.

### Tính năng của FSM trong Photon Fusion:
- **Hỗ trợ nhiều máy trạng thái:** Nhiều FSM có thể chạy song song trên cùng một đối tượng.
- **Cấu trúc phân cấp:** Cho phép tổ chức máy trạng thái con bên trong máy trạng thái cha.
- **Quản lý trạng thái:** Tự động chuyển đổi trạng thái dựa trên điều kiện hoặc ưu tiên.

---

## 2. Cấu trúc cơ bản của FSM trong Fusion

### Thành phần chính:
1. **StateMachineController:**
   - Được thêm vào đối tượng game để quản lý và đồng bộ hóa trạng thái qua mạng.
   - Đảm bảo tất cả client đều biết trạng thái hiện tại của đối tượng.

2. **StateMachine:**
   - Lưu trữ danh sách các trạng thái.
   - Xử lý logic chuyển đổi giữa các trạng thái thông qua các phương pháp như `TryActivateState` hoặc `ForceActivateState`.

3. **State:**
   - Đại diện cho một trạng thái cụ thể và chứa logic liên quan.
   - Kế thừa từ `StateBehaviour` để tích hợp với Photon Fusion.

---

## 3. Cách triển khai FSM

### Cài đặt FSM trên đối tượng Player
```csharp
[RequireComponent(typeof(StateMachineController))]
public class PlayerController : NetworkBehaviour, IStateMachineOwner
{
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private Rigidbody2D rigidbody2D;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public bool _isGrounded;

    private StateMachine<StateBehaviour> _stateMachine;

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;

        if (_stateMachine.ActiveState is JumpState || _stateMachine.ActiveState is AttackState)
            return;

        // Chuyển trạng thái dựa trên đầu vào
        if (_isGrounded && Input.GetKey(KeyCode.Space))
            _stateMachine.TryActivateState<JumpState>();
        else if (_isGrounded && Input.GetKey(KeyCode.F))
            _stateMachine.TryActivateState<AttackState>();
        else
            _stateMachine.TryActivateState<IdleState>();
    }

    void IStateMachineOwner.CollectStateMachines(List<IStateMachine> stateMachines)
    {
        _stateMachine = new StateMachine<StateBehaviour>("PlayerFSM",
            GetComponent<IdleState>(),
            GetComponent<JumpState>(),
            GetComponent<AttackState>());
        stateMachines.Add(_stateMachine);
    }
}
```
### Trạng thái Idle (Đứng yên):
```
public class IdleState : StateBehaviour
{
    private PlayerController _player;

    protected override void OnEnterState()
    {
        _player = GetComponent<PlayerController>();
        if (_player.HasStateAuthority)
            _player.RPC_SetAnimation("idle", true);
    }

    protected override void OnFixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
            Machine.TryActivateState<JumpState>();
    }
}
```
### Trạng thái Jump (Nhảy):
```
public class JumpState : StateBehaviour
{
    private PlayerController _player;

    protected override void OnEnterState()
    {
        _player = GetComponent<PlayerController>();
        if (_player.HasStateAuthority)
        {
            _player.RPC_SetAnimation("jump", true);
            _player.rigidbody2D.AddForce(Vector2.up * _player.jumpForce, ForceMode2D.Impulse);
        }
    }

    protected override void OnFixedUpdate()
    {
        if (_player._isGrounded)
            Machine.TryActivateState<IdleState>();
    }
}
```
### Trạng thái tấn công (Attack)
```
public class AttackState : StateBehaviour
{
    private PlayerController _player;

    protected override void OnEnterState()
    {
        _player = GetComponent<PlayerController>();
        if (_player.HasStateAuthority)
            _player.RPC_SetAnimation("attack", true);
    }

    protected override void OnFixedUpdate()
    {
        if (Machine.StateTime > 0.5f) // Hoàn thành hoạt ảnh tấn công
            Machine.TryActivateState<IdleState>();
    }
}
```
### trạng thái di chuyển (Move)
```
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
```
## 4. Cách hoạt động của FSM

### Xây dựng máy trạng thái
- Các trạng thái `IdleState`, `JumpState`, và `AttackState` được thêm vào máy trạng thái `PlayerFSM`.

### Quyết định trạng thái
- Mỗi trạng thái kiểm tra điều kiện trong `OnFixedUpdate` để quyết định khi nào nên chuyển đổi.

### Đồng bộ hóa qua mạng
- Dữ liệu trạng thái và hoạt ảnh được đồng bộ qua RPC (`RPC_SetAnimation`).

---

## 5. Ưu điểm của FSM trong Fusion

### Tăng hiệu quả tổ chức code
- Tách biệt logic theo từng trạng thái, giảm sự phức tạp của phương thức `Update`.

### Dễ mở rộng
- Dễ dàng thêm trạng thái mới hoặc tổ chức logic phức tạp.

### Tương thích mạng
- Sử dụng `StateMachineController` để đảm bảo đồng bộ trạng thái giữa các client.

---

## 6. Gợi ý mở rộng

### Thêm trạng thái phức tạp
- Ví dụ: `DodgeState`, `DeadState`, hoặc `SpecialAttackState`.

### Tích hợp AI
- Dùng FSM để điều khiển hành vi AI, như tuần tra (Patrol), đuổi theo (Chase), và tấn công (Attack).

### Tối ưu hóa mạng
- Chỉ đồng bộ dữ liệu cần thiết (như hoạt ảnh hoặc vị trí).

FSM trong Photon Fusion là công cụ mạnh mẽ để quản lý trạng thái đối tượng trong môi trường mạng. Với cấu trúc rõ ràng và khả năng đồng bộ hóa tự động, FSM giúp bạn xây dựng các hành vi phức tạp một cách hiệu quả và dễ mở rộng.
