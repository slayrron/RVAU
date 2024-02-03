using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject cylinderPrefab;

    public float delayShoot = 2.0f;
    public float delayBurst = 2.0f;
    private float lastShoot = 0.0f;
    private bool isBursting = false;
    private int burstCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator DestroyBullet(GameObject instantiatedCylinder)
    {
        yield return new WaitForSeconds(1.6f);
        Destroy(instantiatedCylinder);
    }

    IEnumerator Burst()
    {
        while (burstCount < 3)
        {
            yield return new WaitForSeconds(0.1f);
            if (Input.GetMouseButton(1))
            {
                Vector3 spawnPosition = transform.position + transform.forward;
                Quaternion spawnRotation = Quaternion.LookRotation(transform.forward) * Quaternion.Euler(0, 90, 0); ;
                GameObject instantiatedCylinder = Instantiate(cylinderPrefab, spawnPosition, spawnRotation);

                Rigidbody cylinderRigidbody = instantiatedCylinder.GetComponent<Rigidbody>();
                cylinderRigidbody.AddForce(transform.forward * 80f, ForceMode.Impulse);

                StartCoroutine(DestroyBullet(instantiatedCylinder));

                burstCount++;
            }
            else
            {
                // Si le bouton de la souris est relâché, sortir de la boucle de rafale
                break;
            }
        }
        isBursting = false;
        burstCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Time.time - lastShoot > delayShoot)
            {
                Vector3 spawnPosition = transform.position + transform.forward;
                Quaternion spawnRotation = Quaternion.LookRotation(transform.forward) * Quaternion.Euler(0, 90, 0); ;
                GameObject instantiatedCylinder = Instantiate(cylinderPrefab, spawnPosition, spawnRotation);

                Rigidbody cylinderRigidbody = instantiatedCylinder.GetComponent<Rigidbody>();
                cylinderRigidbody.AddForce(transform.forward * 80f, ForceMode.Impulse);
                lastShoot = Time.time;
                
                StartCoroutine(DestroyBullet(instantiatedCylinder));
            }
        }
        if (Input.GetMouseButton(1))
        {
            if (!isBursting && (Time.time - lastShoot > delayBurst))
            {
                isBursting = true;
                StartCoroutine(Burst());
                lastShoot = Time.time;
            }
        }
        else
        {
            isBursting = false;
            burstCount = 0;
        }
    }
}
