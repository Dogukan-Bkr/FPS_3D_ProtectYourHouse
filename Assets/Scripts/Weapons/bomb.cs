using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public float power = 10f;
    public float range = 5f;
    public float upSidePower = 1f;
    public ParticleSystem expEffect;
    AudioSource bombSound;

    void Start()
    {

        bombSound = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (expEffect != null)
            {
                
                Destroy(gameObject, .5f);
                exp();
            }

        }
    }

    void exp()
    {
        Vector3 expPosition = transform.position;
        Instantiate(expEffect, transform.position, transform.rotation);
        bombSound.Play();
        Collider[] colliders = Physics.OverlapSphere(expPosition, range);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (hit != null && rb)
            {
                if (hit.gameObject.CompareTag("Enemy"))
                {
                    hit.transform.gameObject.GetComponent<Enemy>().dead();
                }
                rb.AddExplosionForce(power, expPosition, range, 1, ForceMode.Impulse);
            }
        }

    }
}
