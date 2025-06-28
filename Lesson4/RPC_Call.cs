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
