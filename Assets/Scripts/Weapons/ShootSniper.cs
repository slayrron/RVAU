using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShootSniper : MonoBehaviour
{
    public int ammoMag = 1;
    private int currentammo;

    public int damage;

    public TextMeshProUGUI text;
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    private Transform barrelLocation;
    [Tooltip("Specify time to destory the casing object")][SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] private float shotPower = 500f;

    public AudioSource source;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public AudioClip noammoSound;

    // Start is called before the first frame update
    void Start()
    {
        currentammo = ammoMag;
        XRGrabNetworkInteractable grabbable = GetComponent<XRGrabNetworkInteractable>();
        grabbable.activated.AddListener(FireBullet);
        if (barrelLocation == null)
        {
            barrelLocation = transform;
        }  
    }

    private void Update()
    {
        if (Vector3.Angle(transform.up, Vector3.up) > 100 && currentammo < ammoMag)
        {
            Reload();
        }
        text.text = currentammo.ToString();
    }

    void Reload()
    {
        currentammo = ammoMag;
        source.PlayOneShot(reloadSound);
    }


    void FireBullet(ActivateEventArgs arg)
    {
        if (currentammo > 0)
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
            GameObject spawnedBullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
            spawnedBullet.GetComponent<Rigidbody>().velocity = barrelLocation.forward * shotPower;

            spawnedBullet.transform.SetParent(this.transform);

            // Attach the BulletScript to the bullet
            BulletScript bulletScript = spawnedBullet.AddComponent<BulletScript>();
        }
        else
        {
            source.PlayOneShot(noammoSound);
        }
    }

    public void DealDamage(Zombie zombieComponent)
    {
        zombieComponent.TakeDamage(damage);

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
