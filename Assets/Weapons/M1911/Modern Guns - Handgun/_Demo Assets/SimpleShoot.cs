using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    public int ammoMag = 7;
    private int currentammo;

    private float reloadTimer = 0.4f;

    public TextMeshProUGUI text;
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;

    public AudioSource source;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public AudioClip noammoSound;

    void Start()
    {
        currentammo = ammoMag;
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();
    }

    void Reload()
    {
        currentammo = ammoMag;
        source.PlayOneShot(reloadSound);
    }

    public void PullTheTrigger()
    {
        {
            //Calls animation on the gun that has the relevant animation events that will fire
            if (currentammo > 0)
            {
                gunAnimator.SetTrigger("Fire");
            }
            else
            {
                source.PlayOneShot(noammoSound);
            }
        }
    }

    private void Update()
    {
       if (Vector3.Angle(transform.up, Vector3.up) > 100 && currentammo < ammoMag)
        {
            // Start the reload timer
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0f)
            {
                Reload();
                reloadTimer = 0.4f; // Reset the reload timer
            }
        }
       text.text = currentammo.ToString();
    }


    //This function creates the bullet behavior
    void Shoot()
    {
        currentammo--;
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

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.AddForce(barrelLocation.forward * shotPower);
        }

        // Attach the BulletScript to the bullet
        BulletScript bulletScript = bullet.AddComponent<BulletScript>();
        bulletScript.Weapon = this.gameObject;
    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

    public void DealDamage(Zombie zombieComponent)
    {
        zombieComponent.TakeDamage(1);
        
        Player playerScript =  GameObject.FindWithTag("Player").GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.GainMoney(10);
        }
        else
        {
            Debug.LogError("ParentScript not found on the parent GameObject.");
        }
    }
}
