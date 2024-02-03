using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : MonoBehaviour
{

    public int price;
    private XRGrabNetworkInteractable networkInteractable;

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
        Transform playerTransform = interactor.transform.parent.parent;
        UnityEngine.Debug.Log(interactor.transform.name);

        Player playerScript = playerTransform.GetComponent<Player>();

        Transform weaponSlot = playerTransform.Find("WeaponSlot");

        if (weaponSlot != null)
        {
            // Set the player as the parent of the weapon
            transform.SetParent(weaponSlot);
            playerScript.BuyWeapon(price);
        }
        else
        {
            Debug.LogError("Weapon slot not found on the player!");
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