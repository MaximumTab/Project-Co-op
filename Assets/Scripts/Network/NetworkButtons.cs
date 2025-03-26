using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkButtons : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    void Start()
    {
        hostButton.onClick.AddListener(HostButtonOnClick);
        clientButton.onClick.AddListener(ClientButtonOnClick);
    }

    private void HostButtonOnClick()
    {
        Debug.Log("Host button clicked");
        hostButton.interactable = false;
        clientButton.interactable = false;
        NetworkManager.Singleton.StartHost();
    }

    private void ClientButtonOnClick()
    {
        Debug.Log("Client button clicked");
        hostButton.interactable = false;
        clientButton.interactable = false;
        NetworkManager.Singleton.StartClient();
    }
}

