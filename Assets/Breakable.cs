using Unity.VisualScripting;
using UnityEngine;


[SelectionBase] public class Breakable : MonoBehaviour 
{
[SerializeField] GameObject wholeObject;
[SerializeField] GameObject brokenObject;

BoxCollider bc;

    private void Awake()
    {
        wholeObject.SetActive(true);
        brokenObject.SetActive(false);

        bc = GetComponent<BoxCollider>(); 
    }

    private void OnMouseDown()
    {
        Break();      
    }

    private void Break()
    {
        wholeObject.SetActive(false);
        brokenObject.SetActive(true);

        bc = GetComponent<BoxCollider>(); 
    }
}

