using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnedPlayerPrefab;
    public GameObject spawn1;
    public GameObject spawn2;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (PhotonNetwork.IsMasterClient)
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", spawn1.transform.position, spawn1.transform.rotation);
        }
        else
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", spawn2.transform.position, spawn2.transform.rotation);

        }

        spawnedPlayerPrefab.tag = "NetworkPlayer";
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
    }
}
