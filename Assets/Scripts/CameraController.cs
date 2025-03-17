using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float height;

    private Quaternion targetRotation;
    private float yRotation;
    private float xRotation;
    private float xRotationClamped;
    [SerializeField]private float xRotationMin;
    [SerializeField]private float xRotationMax;

   
  
    private void Update() 
    {
        yRotation += Input.GetAxis("Mouse X");
        xRotation += Input.GetAxis("Mouse Y");
    }
    void LateUpdate()
    {
        xRotationClamped = Mathf.Clamp(xRotation, xRotationMin, xRotationMax);
        targetRotation = Quaternion.Euler(xRotation, yRotation, 0.0f);
        transform.position = target.position - targetRotation * offset + Vector3.up;
    }
}
