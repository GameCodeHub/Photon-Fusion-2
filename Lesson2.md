# Xây Dựng Hệ Thống Người Chơi Trong Photon Fusion Với Spine Pro Và InputSystem

---

## **1. Mục Tiêu**

- Hiểu cách xây dựng hệ thống người chơi với **Photon Fusion**.  
- Tích hợp **InputSystem** để thu thập và áp dụng đầu vào.  
- Đồng bộ hóa chuyển động và hoạt ảnh từ **Spine Pro** giữa các client.  
- Tích hợp và sử dụng **Fusion Physics Addon** để quản lý đồng bộ hóa vật lý.

---

## **2. Tích Hợp Fusion Physics Addon**

### **2.1. Yêu Cầu Hệ Thống**

- **Unity Version:** Unity 2021.3 hoặc cao hơn.  
- **Photon Fusion AppId:** 
  - Tạo AppId từ [Photon Dashboard](https://dashboard.photonengine.com).  
  - Dán AppId vào **App Id Fusion** trong **Real Time Settings** (menu Fusion trong Unity).

---

### **2.2. Tải Addon**

| **Phiên Bản** | **Ngày Phát Hành** | **Liên Kết Tải**                                           |
|---------------|---------------------|-----------------------------------------------------------|
| **2.0.3**     | Mar 03, 2025       | [Tải Fusion Physics 2.0.3](https://doc.photonengine.com/fusion/current/addons/physics/download) |
| **2.0.2**     |                     | [Tải Fusion Physics 2.0.2](https://doc.photonengine.com/fusion/current/addons/physics/download) |
| **2.0.1**     |                     | [Tải Fusion Physics 2.0.1](https://doc.photonengine.com/fusion/current/addons/physics/download) |

---

### **2.3. Cài Đặt Addon**

#### **Bước 1: Xóa Bản Addon Cũ (Nếu Có)**

1. Truy cập thư mục:  
   **Assets > Photon > FusionAddons > Physics**.  
2. Xóa toàn bộ thư mục **Physics**.

#### **Bước 2: Nhập Addon Mới**

1. Tải tệp `.unitypackage` của phiên bản mới.  
2. Kéo tệp `.unitypackage` vào cửa sổ **Project** trong Unity.  
3. Nhấn **Import** và đảm bảo tất cả các tệp được chọn.

---

## **3. Thành Phần Bắt Buộc Trong Fusion Physics**

| **Thành Phần**            | **Mục Đích**                                                                                          |
|---------------------------|-------------------------------------------------------------------------------------------------------|
| **NetworkRigidbody2D/3D** | Đồng bộ trạng thái vật lý của Rigidbody (2D/3D) giữa các client.                                      |
| **BoxCollider2D/3D**      | Phát hiện va chạm cho các đối tượng vật lý.                                                          |
| **NetworkObject**         | Quản lý các đối tượng trên mạng, cho phép đồng bộ trạng thái.                                        |

---

## **4. Định Nghĩa Các Thành Phần Của `INetworkRunnerCallbacks`**

| **Hàm Bắt Buộc**           | **Định Nghĩa**                                                                                       |
|----------------------------|----------------------------------------------------------------------------------------------------|
| `OnPlayerJoined`           | Được gọi khi người chơi tham gia phòng. Spawn nhân vật cho người chơi này.                          |
| `OnPlayerLeft`             | Được gọi khi người chơi rời phòng. Xóa các tài nguyên liên quan.                                   |
| `OnInput`                  | Thu thập đầu vào từ `InputSystem` và gửi lên máy chủ.                                               |
| `OnInputMissing`           | Được gọi khi thiếu đầu vào từ một người chơi.                                                       |
| `OnShutdown`               | Được gọi khi `NetworkRunner` dừng hoạt động. Dọn dẹp tài nguyên.                                   |
| `OnConnectedToServer`      | Xác nhận kết nối thành công đến máy chủ.                                                           |
| `OnDisconnectedFromServer` | Được gọi khi mất kết nối với máy chủ.                                                              |
| `OnConnectRequest`         | Xử lý yêu cầu kết nối từ một client khác.                                                          |
| `OnConnectFailed`          | Được gọi khi kết nối với máy chủ thất bại.                                                         |
| `OnReliableDataReceived`   | Nhận dữ liệu đáng tin cậy từ người chơi khác.                                                      |
| `OnReliableDataProgress`   | Xử lý tiến trình gửi dữ liệu đáng tin cậy.                                                         |
| `OnUserSimulationMessage`  | Xử lý tin nhắn mô phỏng do người dùng định nghĩa.                                                  |
| `OnSessionListUpdated`     | Được gọi khi danh sách session được cập nhật.                                                      |
| `OnCustomAuthenticationResponse` | Xử lý phản hồi xác thực tùy chỉnh từ máy chủ.                                               |
| `OnHostMigration`          | Xử lý khi host bị thay đổi (Host Migration).                                                       |
| `OnSceneLoadStart`         | Được gọi khi một cảnh mới bắt đầu tải.                                                             |
| `OnSceneLoadDone`          | Được gọi khi cảnh mới đã tải xong.                                                                 |
| `OnObjectEnterAOI`         | Được gọi khi một đối tượng xuất hiện trong vùng quan tâm của người chơi (AOI - Area of Interest).  |
| `OnObjectExitAOI`          | Được gọi khi một đối tượng rời khỏi vùng quan tâm của người chơi.  

## **5. Hướng Dẫn Triển Khai**

1. **Tạo Prefab Nhân Vật:**  
   - Thêm các thành phần: `NetworkObject`, `NetworkRigidbody2D`, `BoxCollider2D`.  
   - Gán script `PlayerController` để điều khiển logic nhân vật.

2. **Cấu Hình Vật Lý Trong `PlayerController`:**  
   - Thêm `NetworkRigidbody2D` để đồng bộ hóa trạng thái vật lý của nhân vật.  
   - Sử dụng `BoxCollider2D` để phát hiện va chạm.

3. **Thu Thập Đầu Vào Với `InputSystem`:**  
   - Cấu hình hành động di chuyển, nhảy và tấn công trong `InputSystem`.

---

## **6. Kết Quả**

- Khi người chơi tham gia, nhân vật được spawn và đồng bộ hóa trạng thái vật lý qua mạng.  
- Input từ người chơi được thu thập và đồng bộ hóa qua mạng.  
- Vật lý và hoạt ảnh nhân vật được đồng bộ mượt mà giữa các client.

---

## **7. Bài Tập Mở Rộng**

- **Tích hợp hiệu ứng vật lý:** Thêm hiệu ứng như lực va chạm hoặc mô phỏng trọng lực.  
- **Tối ưu vật lý:** Sử dụng cấu hình vật lý để giảm độ trễ và cải thiện hiệu suất.  
- **Kết hợp Unity Physics:** Kết hợp Unity Physics với Fusion Physics để mở rộng khả năng mô phỏng.

---

Chúc bạn triển khai thành công! 🚀
