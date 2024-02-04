using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
public class XRGrabNetworkInteractable : XRGrabInteractable
{
    private PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        view.RequestOwnership();
        base.OnSelectEntered(interactor);
    }

    protected override void OnActivate(XRBaseInteractor interactor)
    {
        base.OnActivate(interactor);

        //Added for inventory

        if (gameObject.GetComponent<Item>() == null)
        {
            return;
        }
        if (gameObject.GetComponent<Item>().inSlot)
        {
            grabMoneyFromInventory();
        }
    }

    public void grabMoneyFromInventory()
    {
        view.RPC("grabMoneyFromInventoryRPC", RpcTarget.All);
    }

    public void grabMoneyFromInventoryRPC()
    {
        gameObject.GetComponent<Item>().inSlot = false;
        gameObject.GetComponent<Item>().currentSlot.ResetColor();
        gameObject.GetComponent<Item>().currentSlot = null;
    }

}
