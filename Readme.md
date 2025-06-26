# Photon Fusion 2

Photon Fusion 2 là một giải pháp mạnh mẽ, dễ dàng tích hợp và hiệu suất cao để phát triển các trò chơi hoặc ứng dụng thời gian thực đa người chơi. Đây là thế hệ thứ hai của Fusion, được tối ưu hóa với các tính năng nâng cao để đáp ứng nhu cầu của các nhà phát triển.

## Giới thiệu

Photon Fusion 2 được thiết kế để mang đến một nền tảng đa người chơi hiệu quả, giúp các nhà phát triển dễ dàng xây dựng các trò chơi từ PvP thời gian thực đến các trò chơi co-op dựa trên trạng thái đồng bộ.

Fusion cung cấp ba chế độ đồng bộ hóa:

- **State Replication (Đồng bộ trạng thái):** Đồng bộ toàn bộ trạng thái giữa server và client.
- **Event Streaming (Luồng sự kiện):** Tối ưu hóa gửi sự kiện khi không cần đồng bộ trạng thái.
- **Simulation Ticks (Mô phỏng):** Hỗ trợ logic game phức tạp và tính toán phi tập trung.

## Tính năng nổi bật

- Hiệu suất cao: Tối ưu hóa cho cả máy chủ và thiết bị khách.
- Tích hợp Unity: Dễ dàng cài đặt và sử dụng trong Unity Editor.
- Mô phỏng thời gian thực: Hỗ trợ cả client-host và server-host.
- Scale-up tốt: Hỗ trợ từ các trận đấu nhỏ đến các môi trường hàng nghìn người chơi.
- Khả năng mở rộng: Kết hợp dễ dàng với Photon Cloud hoặc tự triển khai máy chủ riêng.

## Yêu cầu hệ thống

- **Unity:** Phiên bản 2021.3 LTS trở lên.
- **Photon AppId:** Đăng ký tại [Photon Engine Dashboard](https://dashboard.photonengine.com).
- **Nền tảng hỗ trợ:** Windows, macOS, Android, iOS, WebGL.

## Cài đặt

1. **Cài đặt qua Unity Package Manager (UPM):**
   - Truy cập `Edit > Project Settings > Package Manager`.
   - Thêm URL: `https://package.photonengine.com/fusion`.

2. **Import thủ công:**
   - Tải Fusion SDK từ [Photon Download Page](https://www.photonengine.com/fusion).
   - Giải nén và import vào Unity qua `Assets > Import Package`.

3. **Cấu hình Photon Fusion:**
   - Tạo tài khoản trên [Photon Dashboard](https://dashboard.photonengine.com).
   - Lấy **AppId** và gán trong `PhotonAppSettings`.

## Cách sử dụng

### Tạo một game đa người chơi cơ bản

1. **Tạo NetworkRunner:**
   - Thêm component `NetworkRunner` vào GameObject chính.
   - Chọn chế độ vận hành: `Server`, `Client`, hoặc `Host`.

2. **Xử lý logic đồng bộ:**
   - Sử dụng các thuộc tính như `[Networked]` để đánh dấu biến cần đồng bộ.
   - Implement `INetworkRunnerCallbacks` để xử lý sự kiện mạng.

3. **Chạy game:**
   - Triển khai và kết nối các thiết bị thông qua máy chủ Photon.

## Hỗ trợ

Nếu bạn gặp bất kỳ vấn đề nào, hãy tham khảo:

- [Tài liệu chính thức](https://doc.photonengine.com/fusion).
- Tham gia [Photon Fusion Forum](https://forum.photonengine.com).
- Email hỗ trợ: `support@photonengine.com`.

## Tài liệu tham khảo

- [Fusion API Documentation](https://doc-api.photonengine.com/fusion).
- [Unity Integration Guide](https://unity.com/learn).

## Giấy phép

Photon Fusion 2 được phân phối theo giấy phép của Photon Engine. Để biết thêm chi tiết, hãy tham khảo tại [Photon Licensing](https://www.photonengine.com/en-US/Legal/Terms).
