using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShootRocket : MonoBehaviour
{
    public int ammoMag = 1;
    private int currentammo;

    public int damage;

    public TextMeshProUGUI text;
    public GameObject bulletPrefab;

    

    [SerializeField] private Transform barrelLocation;
    [Tooltip("Bullet Speed")] private float shotPower = 10f;

    public AudioSource source;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public AudioClip explosionSound;

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

            //cancels if there's no bullet prefeb
            if (!bulletPrefab)
            { return; }

            // Create a bullet and add force on it in direction of the barrel
            GameObject spawnedBullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation * Quaternion.Euler(90, 0, 0));
            spawnedBullet.GetComponent<Rigidbody>().velocity = barrelLocation.forward * shotPower;

            // Attach the BulletScript to the bullet
            RocketScript rocketScript = spawnedBullet.AddComponent<RocketScript>();
            rocketScript.Weapon = this.gameObject;
            rocketScript._explosionSound = explosionSound;
        }
    }

    public void DealDamage(Zombie zombieComponent)
    {
        zombieComponent.TakeDamage(damage);

        Player playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.GainMoney(50);
        }
        else
        {
            Debug.LogError("ParentScript not found on the parent GameObject.");
        }
    }
}
