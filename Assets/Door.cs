using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Door : MonoBehaviour
{
    PhotonView view;
    public int price;

    public void Start()
    {
        view = GetComponent<PhotonView>();
    }

    public void RemoveDoors(GameObject obj)
    {
        //view.RPC("RemoveDoorsRPC", RpcTarget.All, obj);
        Destroy(obj);
    }

    [PunRPC]
    public void RemoveDoorsRPC(GameObject obj)
    {
        Destroy(obj);
    }

}
