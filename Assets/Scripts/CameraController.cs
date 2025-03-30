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

    [SerializeField] private float xSensitivity;
    [SerializeField] private float ySensitivity;

    [SerializeField] private bool invertX;
    private int xInvertedValue;

    private Vector3 desiredPos;

    private InputSystem_Actions input;

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
        Vector2 lookInput = input.Player.Look.ReadValue<Vector2>();
        yRotation += lookInput.x * ySensitivity;
        xRotation += lookInput.y * xSensitivity * xInvertedValue;
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
