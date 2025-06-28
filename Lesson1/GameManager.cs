using Fusion;
using UnityEngine;

public class CustomGameManager : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [System.Obsolete("This method is obsolete. Use StartGame() in NetworkManager instead.")]
    void Start()
    {
       
        if (networkManager != null)
        {
            networkManager.StartGame();
            Debug.Log("Game started with NetworkManager.");
        }
        else
        {
            Debug.LogError("NetworkManager not found in the scene.");
        }
    }
}
