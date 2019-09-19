using UnityEngine;

public class GrenadeJob : MonoBehaviour
{
    public float delay = 3f;
    public float blasteradius = 1f;
    float countdown;
    public float force = 700f;
    bool hasExploded = false;
    public GameObject explosionEffect;

    public GameObject GrenadeTerrainDeform;
    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }
    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;

        if(countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Collider[] coll = Physics.OverlapSphere(transform.position,blasteradius);
        foreach(Collider nearbyObject in coll)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(force, transform.position,blasteradius);
                //Code here               
                Instantiate(GrenadeTerrainDeform, transform.position, transform.rotation);
            }
        }
        Destroy(gameObject);
    }
}
