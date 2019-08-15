using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour
{
    public float delay = 3f;
    float countdown;
    bool hasExploded = false;
    public GameObject explosionEffect;
    public float blastRadius = 1f;
    public float force = 700f;
    bool explosionHappen = false;
    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown<=0 && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] ColliderToDestroy= Physics.OverlapSphere(transform.position, blastRadius);


        foreach(Collider nearbyObject in ColliderToDestroy)
        {

            Destructible dest = nearbyObject.GetComponent<Destructible>();
            if(dest != null)
            {
                dest.Destroy();
            }
        }

        Collider[] ColliderToMove = Physics.OverlapSphere(transform.position, blastRadius);

        foreach(Collider nearbyObject in ColliderToMove)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null&&!explosionHappen)
            {
                explosionHappen = true;
                rb.AddExplosionForce(force, transform.position, blastRadius);
            }
        }

        Destroy(gameObject);
    }
}
