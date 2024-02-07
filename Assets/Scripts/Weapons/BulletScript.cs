using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Photon.Pun;
using UnityEditor;

public class BulletScript : MonoBehaviour
{
    PhotonView view;
    public GameObject Weapon { get; set; }

    void Start()
    {
        view = GetComponent<PhotonView>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Zombie>(out Zombie zombieComponent))
        {
            if (Weapon != null)
            {
                SimpleShoot shootingScript = Weapon.GetComponent<SimpleShoot>();
                ShootSniper shootsniper = Weapon.GetComponent<ShootSniper>();
                if (shootingScript != null)
                {
                    shootingScript.DealDamage(zombieComponent);
                }
                else if (shootsniper != null)
                {
                    shootsniper.DealDamage(zombieComponent);
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
