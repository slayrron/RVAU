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
    private float explosionRadius = 2.5f;
    public GameObject Weapon { get; set; }
    public AudioClip _explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        explosion = Resources.Load<GameObject>("Explosion");
    }


    private void OnCollisionEnter(Collision collision)
    {
        AudioSource.PlayClipAtPoint(_explosionSound, transform.position);
        GameObject explosion_instance = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(explosion_instance, 1.9f);
        var colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(var obj in colliders)
        {
            if (obj.TryGetComponent<Zombie>(out Zombie zombieComp))
            {
                if (Weapon != null)
                {
                    ShootRocket shootingScript = Weapon.GetComponent<ShootRocket>();
                    if (shootingScript != null)
                    {
                        shootingScript.DealDamage(zombieComp);
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
            if (!obj.gameObject.CompareTag("Weapon") && !obj.gameObject.CompareTag("ZombieDoor"))
            {
                if (obj.gameObject.TryGetComponent<Door>(out Door door))
                {
                    Debug.Log("TRUE");
                    door.RemoveDoors(obj.gameObject);
                    //Destroy(collision.gameObject);
                }
                Destroy(gameObject);
            }
        }
        if (collision.gameObject.TryGetComponent<Zombie>(out Zombie zombieComponent))
        {
            if (Weapon != null)
            {
                ShootRocket shootingScript = Weapon.GetComponent<ShootRocket>();
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
