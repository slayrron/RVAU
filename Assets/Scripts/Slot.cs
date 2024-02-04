using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
public class Slot : MonoBehaviour
{
    public GameObject ItemInSlot;
    public GameObject money;
    //public Image slotImage;
    Color originalColor;
    PhotonView view;
    bool test = false;

    // Start is called before the first frame update
    void Start()
    {
        //slotImage = GetComponent<Image>();
        //originalColor = slotImage.color;

        view = GetComponent<PhotonView>();
        if (view == null )
        {
            Debug.Log("null");
        }
    }

    private void Update()
    {
        if( PhotonNetwork.CurrentRoom.PlayerCount == 2 && test == false && this.name == "Slot 1")
        {
            InsertMoney();
            test = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (ItemInSlot != null) return;
        GameObject obj = other.gameObject;
        if (!IsItem(obj)) return;
        InputDevice leftController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        
        if (leftController.TryGetFeatureValue(CommonUsages.gripButton, out bool isGrabButtonPressed) && !isGrabButtonPressed)
        {
            Debug.Log("ahhh");
            InsertItem(obj);
        }
    }

    bool IsItem(GameObject obj)
    {
        return obj.GetComponent<Item>();
    }

    public void InsertMoney()
    {
        Debug.Log("InsertMoney");
        view.RPC("InsertMoneyRPC", RpcTarget.All);
       
    }

    [PunRPC]
    public void InsertMoneyRPC() 
    {
        Debug.Log("RPC");
        GameObject m = PhotonNetwork.Instantiate("10000dol", transform.position, transform.rotation);
        m.GetComponent<Rigidbody>().isKinematic = true;
        m.transform.SetParent(gameObject.transform, true);
        m.transform.localPosition = Vector3.zero;
        m.transform.localEulerAngles = m.GetComponent<Item>().slotRotation;
        m.GetComponent<Item>().inSlot = true;
        m.GetComponent<Item>().currentSlot = this;
        ItemInSlot = m;
    }


    public void InsertItem(GameObject obj)
    {
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.transform.SetParent(gameObject.transform, true);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = obj.GetComponent<Item>().slotRotation;
        obj.GetComponent<Item>().inSlot = true;
        obj.GetComponent<Item>().currentSlot = this;
        ItemInSlot = obj;
        //slotImage.color = Color.gray;
    }

    public void ResetColor()
    {
        //slotImage.color = originalColor;
    }

}
