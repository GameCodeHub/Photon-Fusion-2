# Cấu Hình Photon Fusion Cho Host Mode

Hướng dẫn chi tiết cách triển khai **Photon Fusion** để sử dụng chế độ **Host Mode** trong Unity. **Host Mode** cho phép một thiết bị đóng vai trò vừa là máy chủ (host) vừa là client, giúp đơn giản hóa triển khai mạng cho các trò chơi đa người chơi nhỏ.

---

## 1. Giới Thiệu

### Host Mode là gì?  
**Host Mode** là chế độ trong Photon Fusion nơi một client hoạt động như một máy chủ, đồng thời quản lý kết nối và đồng bộ dữ liệu cho các client khác.  

Điểm nổi bật của Host Mode:  
- Không cần máy chủ trung tâm.  
- Giảm độ trễ cho host (do không phải thông qua máy chủ bên thứ ba).  
- Thích hợp cho các trò chơi có số lượng người chơi nhỏ.  

---

## 2. Cài Đặt

### Yêu Cầu  
- **Unity:** 2021.3 LTS hoặc mới hơn.  
- **Photon App ID:** Đăng ký tại [Photon Dashboard](https://dashboard.photonengine.com).  

### Các Bước Cài Đặt  

1. **Cài đặt Photon Fusion SDK**  
   - Tải Photon Fusion từ Unity Asset Store hoặc [Photon Dashboard](https://dashboard.photonengine.com).  
   - Nhập App ID của bạn vào `Photon Fusion Wizard`.  

2. **Cấu hình Unity**  
   - Tạo một scene mới hoặc sử dụng scene hiện tại trong dự án của bạn.  

---

## 3. Triển Khai

### **Tạo Script NetworkManager**  

Tạo một script mới có tên `NetworkManager.cs` để quản lý kết nối mạng và đồng bộ dữ liệu.

```csharp
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _runner;

    public async void StartGame()
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;
        _runner.AddCallbacks(this);

        // Thiết lập chế độ Host Mode
        GameMode mode = GameMode.Host;

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "RoomHost", // Tên phòng
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        Debug.Log("Host started successfully.");
    }

    // Callback khi một client tham gia phòng
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player joined: {player}");
    }

    // Callback khi một client rời phòng
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player left: {player}");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log($"NetworkRunner has shut down. Reason: {shutdownReason}");
    }

    // Các callback khác không sử dụng
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, System.Collections.Generic.Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
}
```
### Tạo Script GameManager
- Tạo một script mới có tên GameManager.cs để điều khiển logic chính của game.
```
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;

    void Start()
    {
        if (networkManager != null)
        {
            networkManager.StartGame();
            Debug.Log("Game started in Host Mode.");
        }
        else
        {
            Debug.LogError("NetworkManager not assigned.");
        }
    }
}

```
## 4. Tổ Chức Thư Mục

Sắp xếp dự án của bạn như sau:
```
/PhotonHostMode

NetworkManager.cs

GameManager.cs
```

---

## 5. Hướng Dẫn Chạy

1. Thêm `GameManager` vào một GameObject trong Scene chính.  
2. Chạy game trong Unity Editor.  
   - Nếu cấu hình đúng, Photon Fusion sẽ tạo một phòng với tên `RoomHost`.  
   - Các client khác có thể kết nối với host thông qua App ID của bạn.

   
![image](https://github.com/user-attachments/assets/aba974ad-38f6-47a2-b672-5fad64d5aaf8)

---

## 6. Mở Rộng

- **Tạo UI:** Thêm giao diện để người chơi nhập tên phòng hoặc chọn vai trò (host/client).  
- **Xử lý ngắt kết nối:** Thêm logic để xử lý khi host thoát hoặc client bị mất kết nối.  
- **Đồng bộ dữ liệu:** Sử dụng `[Networked]` để đồng bộ biến hoặc trạng thái giữa các client.  

---

## 7. Tham Khảo

- [Photon Fusion Documentation](https://doc.photonengine.com/fusion)  
- [Photon Dashboard](https://dashboard.photonengine.com)  
- [Unity Integration Guide](https://unity.com/learn)

### **Tác giả**  
**Pesinus**  
Liên hệ: [nguyenmanh2004devgame@gmail.com](mailto:nguyenmanh2004devgame@gmail.com)
