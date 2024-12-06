using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emptyHive : MonoBehaviour
{
    AudioSource dropedDown;
    
    void Start()
    {
        dropedDown = GetComponent<AudioSource>();
        Destroy(gameObject, 2f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Road"))
        {
            dropedDown.Play();
            if (!dropedDown.isPlaying) { Destroy(gameObject, 1f); }
        }

       
    }
}
