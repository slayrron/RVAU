using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class Money : MonoBehaviour
{

    PhotonView view;
    public int amount;

    public AudioSource source;
    public AudioClip registerSound;

    void Start()
    {
        amount = 0;
        view = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        GetComponent<XRGrabNetworkInteractable>().onActivate.AddListener(AddMoney);
    }

    private void OnDisable()
    {
        GetComponent<XRGrabNetworkInteractable>().onActivate.RemoveListener(AddMoney);
    }


    private void deletePrefab()
    {
        view.RPC("deletePrefabRPC", RpcTarget.All);
    }

    [PunRPC]
    private void deletePrefabRPC()
    {
        Destroy(gameObject);
    } 

    public void SetAmount(int number)
    {
        amount = number;
    }
    private void AddMoney(XRBaseInteractor interactor)
    {
        source.PlayOneShot(registerSound);
        GameObject player = GameObject.FindWithTag("Player");

        Player playerScript = player.GetComponent<Player>();
        playerScript.GainMoney(amount);
        deletePrefab();
    }
}