using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
public class XRGrabNetworkInteractable : XRGrabInteractable
{
    private PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        photonView.RequestOwnership();
        base.OnSelectEntered(interactor);
    }

    protected override void OnActivate(XRBaseInteractor interactor)
    {
        base.OnActivate(interactor);

        //Added for inventory

        if (gameObject.GetComponent<Item>() == null) return;
        if (gameObject.GetComponent<Item>().inSlot)
        {
            gameObject.GetComponent<Item>().inSlot = false;
            gameObject.GetComponent<Item>().currentSlot.ResetColor();
            gameObject.GetComponent<Item>().currentSlot = null;
            
        }
    }

}
