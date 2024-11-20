using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;
    [SerializeField] private GameObject buttonsPlane;

    private void Start()
    {
        startHostButton.onClick.AddListener(StartHost);
        startHostButton.onClick.AddListener(() => buttonsPlane.SetActive(false));

        startClientButton.onClick.AddListener(StartClient);
        startClientButton.onClick.AddListener(() => buttonsPlane.SetActive(false));
    }

    private void StartHost() => NetworkManager.Singleton.StartHost();
    private void StartClient() => NetworkManager.Singleton.StartClient();

}
