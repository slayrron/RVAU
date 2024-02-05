using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShootSniper : MonoBehaviour
{

    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    private Transform barrelLocation;
    [Tooltip("Specify time to destory the casing object")][SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] private float shotPower = 500f;

    public AudioSource source;
    public AudioClip fireSound;

    // Start is called before the first frame update
    void Start()
    {
        XRGrabNetworkInteractable grabbable = GetComponent<XRGrabNetworkInteractable>();
        grabbable.activated.AddListener(FireBullet);
        if (barrelLocation == null)
        {
            barrelLocation = transform;
        }
            
    }
    

    void FireBullet(ActivateEventArgs arg)
    {
        //source.PlayOneShot(fireSound);
        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        GameObject spawnedBullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        spawnedBullet.GetComponent<Rigidbody>().velocity = barrelLocation.forward * shotPower;

        spawnedBullet.transform.SetParent(this.transform);

        // Attach the BulletScript to the bullet
        BulletScript bulletScript = spawnedBullet.AddComponent<BulletScript>();

    }

    public void DealDamage(Zombie zombieComponent)
    {
        zombieComponent.TakeDamage(12);

        Player playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.GainMoney(100);
        }
        else
        {
            Debug.LogError("ParentScript not found on the parent GameObject.");
        }
    }
}
