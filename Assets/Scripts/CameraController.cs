using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;


public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float lookheight;
    [SerializeField] private float posheight;

    private Quaternion targetRotation;

    private float yRotation;
    private float xRotation;
    private float xRotationClamped;

    [SerializeField] private float xRotationMin;
    [SerializeField] private float xRotationMax;

    [SerializeField] private float sensitivity = 2f;


    [SerializeField] private bool invertX;
    private int xInvertedValue;

    private Vector3 desiredPos;

    private InputSystem_Actions input;

    public float GetSensitivity() => sensitivity;
    public void SetSensitivity(float value) => sensitivity = value;
    private void Start()
    {
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        xInvertedValue = invertX ? -1 : 1;

        input = new InputSystem_Actions();
        input.Enable();
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;

        Vector2 lookInput = input.Player.Look.ReadValue<Vector2>();

        if (Mouse.current != null && Mouse.current.delta.IsActuated())
        {
            lookInput *= 0.2f; 
        }

        yRotation += lookInput.x * sensitivity;
        xRotation += lookInput.y * sensitivity * xInvertedValue;
        xRotation = Mathf.Clamp(xRotation, xRotationMin, xRotationMax);
    }
    private void LateUpdate()
    {
        xRotationClamped = Mathf.Clamp(xRotation, xRotationMin, xRotationMax);
        targetRotation = Quaternion.Euler(xRotationClamped, yRotation, 0.0f);

        desiredPos = target.position - targetRotation * offset + Vector3.up * posheight;

        transform.position = desiredPos;
        if(Time.timeScale != 0)
        {
            transform.LookAt(target.position + Vector3.up * lookheight);
        }
      
    }

    public Quaternion YRotation => Quaternion.Euler(0.0f, yRotation, 0.0f);
}
