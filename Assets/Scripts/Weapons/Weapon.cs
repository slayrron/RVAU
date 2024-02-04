using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.UI.Image;

public class Weapon : MonoBehaviour
{
    public enum WeaponState { BUYABLE, SOLD };
    public WeaponState state;

    public int price;
    private XRGrabNetworkInteractable networkInteractable;

    public void Start()
    {
        state = WeaponState.BUYABLE;
    }

    private void OnEnable()
    {
        GetComponent<XRGrabInteractable>().onSelectEntered.AddListener(PickupWeapon);
    }

    private void OnDisable()
    {
        GetComponent<XRGrabInteractable>().onSelectEntered.RemoveListener(PickupWeapon);
    }

    public void PickupWeapon(XRBaseInteractor interactor)
    {
        // Get The player in the tree (Controller -> Camera -> Player XR Rig)
        GameObject player = GameObject.FindWithTag("Player");

        Player playerScript = player.GetComponent<Player>();

        if (state == WeaponState.BUYABLE)
        {
            state = WeaponState.SOLD;
            playerScript.BuyWeapon(price);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GameController")
        {
            networkInteractable = GetComponent<XRGrabNetworkInteractable>();

            if (networkInteractable != null)
            {
                Transform playerTransform = other.transform.parent.parent;
                if (playerTransform != null)
                {
                    // Now you have the player's transform
                    // You can access the player's components or perform actions with the player
                    Player playerScript = playerTransform.GetComponent<Player>();
                    if (playerScript.money < price && transform.parent == null)
                    {
                        networkInteractable.enabled = false;
                    }
                    else
                    {
                        networkInteractable.enabled = true;
                    }
                }
            }
            else
            {
                Debug.LogError("No Network Interactable found");
            }
        }
    }
}