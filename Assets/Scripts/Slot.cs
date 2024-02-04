using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
public class Slot : MonoBehaviour
{
    public GameObject ItemInSlot;
    //public Image slotImage;
    Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        //slotImage = GetComponent<Image>();
        //originalColor = slotImage.color;

        if (this.name == "Slot 1")
        {
            InsertItem(GameObject.FindWithTag("Money"));
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

    void InsertItem(GameObject obj)
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
