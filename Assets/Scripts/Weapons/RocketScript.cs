using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Photon.Pun;
using UnityEditor;
public class RocketScript : MonoBehaviour
{
    PhotonView view;
    private GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        explosion = Resources.Load<GameObject>("Explosion");
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject explosion_instance = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(explosion_instance, 1.9f);
        if (collision.gameObject.TryGetComponent<Zombie>(out Zombie zombieComponent))
        {
            if (transform.parent != null)
            {
                ShootRocket shootingScript = transform.parent.GetComponent<ShootRocket>();
                if (shootingScript != null)
                {
                    shootingScript.DealDamage(zombieComponent);
                }
                else if (shootingScript != null)
                {
                    shootingScript.DealDamage(zombieComponent);
                }
                else
                {
                    Debug.LogError("ParentScript not found on the parent GameObject.");
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
            if (collision.gameObject.TryGetComponent<Door>(out Door door))
            {
                Debug.Log("TRUE");
                door.RemoveDoors(collision.gameObject);
                //Destroy(collision.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
