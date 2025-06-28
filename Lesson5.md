## Thông tin chi tiết về NetworkRunner

### Scene Loading
- **Xem thêm**: [Scene Loading](#scene-loading) để hiểu thêm về cách tải và chuyển đổi Scene khi sử dụng NetworkRunner.

### Lưu ý quan trọng
- **Mỗi NetworkRunner chỉ sử dụng một lần**: 
  - Nếu bị ngắt kết nối hoặc thất bại, cần hủy NetworkRunner hiện tại và tạo mới để bắt đầu một phiên khác.

---

### Các thành phần hỗ trợ của NetworkRunner

#### Hành vi mô phỏng và Callbacks
- **SimulationBehaviour**: 
  - Các component con kế thừa lớp này sẽ nhận các callback như `FixedUpdateNetwork()` và `Render()`.
- **INetworkRunnerCallbacks**: 
  - Các component con implement interface này sẽ tự động được đăng ký và nhận các callback liên quan.

#### Các thành phần tích hợp sẵn
- **RunnerAOIGizmos**: Hiển thị vùng quan tâm (Area Of Interest).
- **HitboxManager**: Tự động quản lý Hitbox và cơ chế bù độ trễ (Lag Compensation).
- **RunnerLagCompensationGizmos**: Hiển thị cơ chế Lag Compensation.
- **RunnerEnableVisibility**: Kích hoạt kiểm soát hiển thị trong chế độ Multi-Peer.

#### Tạo thành phần tùy chỉnh
- Kế thừa từ `SimulationBehaviour` hoặc implement `INetworkRunnerCallbacks` để tạo các component hỗ trợ riêng cho NetworkRunner.

---

### Chức năng chính của NetworkRunner

#### Kết nối mạng
- **Quản lý kết nối**: Tới Matchmaking Server, Room Server và Game Server.
- **PlayerRef**: Xác định quyền sở hữu Input và State của các NetworkObject.

#### Quản lý Tick
- **Tick Simulation**: 
  - Xác định số lượng Tick cần mô phỏng dựa trên thời gian đã trôi qua.
- **Đồng bộ Client**: 
  - Điều chỉnh tốc độ Tick của Client để đảm bảo đồng bộ với Server.

# NetworkRunner trong Photon Fusion

## Tổng quan
`NetworkRunner` là thành phần trung tâm của Photon Fusion, đại diện cho một **Peer** trong hệ thống mạng. Thành phần này chịu trách nhiệm quản lý toàn bộ:
- Messaging
- Matchmaking
- Kết nối
- Spawn đối tượng
- Mô phỏng (Simulation)
- Đồng bộ trạng thái (State Replication)

Một ứng dụng Unity có thể chạy nhiều NetworkRunner cùng lúc, mỗi cái đại diện cho một Peer riêng biệt. Xem thêm về [Multi-Peer Mode](#multi-peer-mode).

---

## Cách sử dụng

### Tạo NetworkRunner
`NetworkRunner` có thể được tạo theo 3 cách:
1. **Từ Prefab khi runtime.**
2. **Thêm trực tiếp vào Scene** như một GameObject.
3. **Tạo động qua code** bằng cách thêm NetworkRunner vào một GameObject.

### Khởi động và Kết nối
Sau khi tạo, NetworkRunner có thể:
- Kết nối vào hệ thống Matchmaking.
- Tạo hoặc tham gia Room.

#### Tạo hoặc Tham gia Room
- **Phương thức `StartGame()`**: Tạo một Peer và tham gia hoặc tạo một Room dựa trên tham số `StartGameArgs`.
- **Chế độ Single Mode**: Không kết nối tới Photon Server và không tạo Room.

#### Tải Scene
- **Scene hiện tại**: Dùng `NetworkSceneInfo` để bắt đầu mô phỏng trong Scene hiện tại.
- **Scene khác**: NetworkRunner có thể tải Scene mới và spawn tất cả các NetworkObject trong đó.

Ví dụ: Bắt đầu NetworkRunner trong Scene hiện tại:
```csharp
var sceneRef = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
NetworkSceneInfo info = new NetworkSceneInfo();
info.AddSceneRef(sceneRef, LoadSceneMode.Single);

runner.StartGame(new StartGameArgs()
{
    Scene = info,
    GameMode = GameMode.Shared,
});
```
#### Quản lý Scene và Đối tượng
- Trong chế độ **Multi-Peer**, tất cả Scene Object và Spawned Object là con của một GameObject chuyên biệt, được thêm vào `PhysicsScene` hoặc `PhysicsScene2D`.

---

### Multi-Peer Mode
- **Cho phép nhiều NetworkRunner** hoạt động trong một instance Unity.
- **Quản lý riêng biệt**:
  - Mỗi Runner hoạt động trong một Scene và quản lý các NetworkObject riêng.

---

### Lưu ý
- **Thứ tự thực thi Callback**:
  - Các Callback như `OnPlayerJoined` sẽ không được gọi cho các Player đã tham gia trước đó.
- **Sử dụng đúng cách**:
  - Chỉ sử dụng một NetworkRunner cho mỗi phiên, và cần tạo mới khi phiên kết thúc.

---

NetworkRunner là công cụ cốt lõi trong Photon Fusion, cung cấp các giải pháp mạnh mẽ để đồng bộ hóa và tối ưu hóa trải nghiệm mạng. Sử dụng đúng cách sẽ đảm bảo hiệu suất và độ tin cậy cao.
