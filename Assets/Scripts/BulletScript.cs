using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Zombie>(out Zombie zombieComponent))
        {
            if (transform.parent != null)
            {
                SimpleShoot shootingScript = transform.parent.GetComponent<SimpleShoot>();
                if (shootingScript != null)
                {
                    shootingScript.DealDamage(zombieComponent);
                }
                else
                {
                    Debug.LogError("ParentScript not found on the parent GameObject.");
                }
            }
            else
            {
                Debug.LogError("This GameObject has no parent.");
            }

        }

        //Temp
        if (collision.gameObject.name != "M1911")
        { 
            Destroy(gameObject);
        }
    }
}
