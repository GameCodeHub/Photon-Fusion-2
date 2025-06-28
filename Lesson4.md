## RPC trong Photon Fusion

### RPC (Remote Procedure Call)
RPC là một tính năng trong Photon Fusion cho phép gọi các phương thức từ xa giữa các client hoặc server, giúp đồng bộ hóa hành động và sự kiện trong game.

---

### Các thành phần chính của RPC
1. **`RpcSources`**:
   - Xác định nguồn gốc của lời gọi (client, server, hoặc authority cụ thể).
2. **`RpcTargets`**:
   - Xác định đích đến của lời gọi (một client, tất cả client, hoặc server).

---

### Ví dụ mã sử dụng RPC
Dưới đây là ví dụ về cách sử dụng RPC để thực hiện các hành động trong game:

#### Script: `RPCExample.cs`
```csharp
using Fusion;
using UnityEngine;

public class RPCExample : NetworkBehaviour
{
    // RPC dùng để gửi tin nhắn
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SendMessage(string message)
    {
        Debug.Log($"Message received: {message}");
    }

    // RPC dùng để phát hiệu ứng âm thanh
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_PlaySound(Vector3 position)
    {
        Debug.Log($"Play sound at position: {position}");
        // Thêm logic phát âm thanh tại vị trí
    }

    // RPC dùng để yêu cầu mua vật phẩm
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_BuyItem(string itemName)
    {
        Debug.Log($"Request to buy: {itemName}");
        // Logic xử lý mua vật phẩm tại server
    }

    // RPC thông báo bắt đầu trận đấu
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_StartGame()
    {
        Debug.Log("Game has started!");
        // Logic bắt đầu trận đấu
    }

    private void Update()
    {
        // Gửi tin nhắn khi nhấn T
        if (Input.GetKeyDown(KeyCode.T) && Object.HasInputAuthority)
        {
            RPC_SendMessage("Hello from player!");
        }

        // Phát âm thanh khi nhấn G
        if (Input.GetKeyDown(KeyCode.G) && Object.HasInputAuthority)
        {
            RPC_PlaySound(transform.position);
        }

        // Gửi yêu cầu mua vật phẩm khi nhấn B
        if (Input.GetKeyDown(KeyCode.B) && Object.HasInputAuthority)
        {
            RPC_BuyItem("Sword");
        }

        // Bắt đầu trận đấu khi nhấn S (chỉ host)
        if (Input.GetKeyDown(KeyCode.S) && Object.HasStateAuthority)
        {
            RPC_StartGame();
        }
    }
}
```
### Chức năng của các RPC trong script

#### 1. Gửi tin nhắn (`RPC_SendMessage`)
- **Mô tả**: Gửi tin nhắn từ một client tới tất cả các client khác.
- **Kích hoạt**: Khi nhấn phím `T`.
- **Ứng dụng**: Gửi thông báo, trò chuyện trong game, hoặc các thông điệp chung.

#### 2. Phát âm thanh (`RPC_PlaySound`)
- **Mô tả**: Gửi yêu cầu phát âm thanh tại một vị trí cụ thể tới tất cả client.
- **Kích hoạt**: Khi nhấn phím `G`.
- **Ứng dụng**: Phát hiệu ứng âm thanh như bước chân, tiếng súng, hoặc thông báo trạng thái trong game.

#### 3. Yêu cầu mua vật phẩm (`RPC_BuyItem`)
- **Mô tả**: Gửi yêu cầu mua vật phẩm từ client tới server để xử lý.
- **Kích hoạt**: Khi nhấn phím `B`.
- **Ứng dụng**: Xử lý các giao dịch mua bán vật phẩm trong trò chơi, chỉ được thực hiện bởi server.

#### 4. Bắt đầu trận đấu (`RPC_StartGame`)
- **Mô tả**: Server gửi thông báo bắt đầu trận đấu tới tất cả client.
- **Kích hoạt**: Khi nhấn phím `S` (chỉ dành cho host).
- **Ứng dụng**: Đồng bộ hóa trạng thái bắt đầu trận đấu giữa các client trong game.

---

### Gợi ý mở rộng

#### 1. **Tối ưu hóa mạng**
- Chỉ gửi dữ liệu cần thiết qua RPC để giảm tải băng thông.
- Ví dụ: Tránh gửi dữ liệu trạng thái không cần thiết hoặc dữ liệu có thể được dự đoán bởi client.

#### 2. **Đồng bộ hóa trạng thái**
- Kết hợp RPC với các cơ chế như FSM để đồng bộ trạng thái trò chơi một cách hiệu quả.
- Ví dụ: Đồng bộ trạng thái người chơi hoặc cập nhật kết quả trận đấu.

#### 3. **Bảo mật RPC**
- Kiểm tra quyền hạn trước khi thực hiện hành động quan trọng.
- Ví dụ: Xác minh yêu cầu mua vật phẩm chỉ được gửi từ client hợp lệ để tránh gian lận.
