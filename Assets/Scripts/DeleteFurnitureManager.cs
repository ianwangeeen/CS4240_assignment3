using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteFurnitureManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPressed()
    {
        audioSource.PlayOneShot(audioClip);
    }
}
