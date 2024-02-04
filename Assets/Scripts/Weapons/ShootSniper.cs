using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSniper : MonoBehaviour
{

    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;

    [Tooltip("Specify time to destory the casing object")][SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")][SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")][SerializeField] private float ejectPower = 150f;

    public AudioSource source;
    public AudioClip fireSound;

    // Start is called before the first frame update
    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;
    }

    public void PullTheTrigger()
    {
        {
            //Calls animation on the gun that has the relevant animation events that will fire
            gunAnimator.SetTrigger("Fire");
        }
    }

    void Shoot()
    {
        source.PlayOneShot(fireSound);
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
        GameObject bullet;
        bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        bullet.transform.SetParent(this.transform);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.AddForce(barrelLocation.forward * shotPower);
        }

        // Attach the BulletScript to the bullet
        BulletScript bulletScript = bullet.AddComponent<BulletScript>();

    }
}
