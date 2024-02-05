using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Photon.Pun;
using UnityEditor;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RemoveDoors(GameObject obj)
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
                /*else
                {   //TEMP
                    ShootSniper shootsniper = transform.parent.GetComponent<ShootSniper>();
                    shootsniper.DealDamage(zombieComponent);
                    //Debug.LogError("ParentScript not found on the parent GameObject.");
                }*/
            }
            else
            {
                Debug.LogError("This GameObject has no parent.");
            }

        }
        if (!collision.gameObject.CompareTag("Weapon") && !collision.gameObject.CompareTag("ZombieDoor"))
        {
            if (collision.gameObject.CompareTag("Door"))
            {
                RemoveDoors(collision.gameObject);
                Destroy(collision.gameObject);
            }
            Debug.Log(collision.gameObject.name);
            Destroy(gameObject);
        }
    }
}
