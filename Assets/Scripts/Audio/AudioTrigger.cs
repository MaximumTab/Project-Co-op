using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource audioSource;
    public GameObject linkedPrefab;

     private void Update()
    {
        
        if (linkedPrefab == null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !audioSource.isPlaying)
        audioSource.Play();
    }

        private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}

