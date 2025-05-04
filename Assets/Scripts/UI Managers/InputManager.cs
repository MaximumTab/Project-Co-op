using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public InputSystem_Actions Actions { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Actions = new InputSystem_Actions();
        Actions.Enable();
    }

    private void OnDestroy()
    {
        if (Actions != null)
        {
            // Disable every map manually
            Actions.Player.Disable();
            Actions.UI.Disable();

            Actions.Disable(); // Redundant safety
            Actions.Dispose();
            Actions = null;
        }

        if (Instance == this)
            Instance = null;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoCreate()
    {
        if (Instance == null)
        {
            GameObject go = new GameObject("InputManager (Auto)");
            go.AddComponent<InputManager>();
        }
    }
}
