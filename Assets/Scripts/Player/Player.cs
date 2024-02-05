using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using static ZombieSpawner;

public class Player : MonoBehaviour
{

    #region
    public static Transform transformInstance;
    public static Player gameObjectInstance;

    private void Awake()
    {
        transformInstance = this.transform;
        gameObjectInstance = this;

    }

    #endregion
    [SerializeField] float health, maxHealth = 3f;
    public int money = 0;
    [SerializeField] FloatingHealthBar healthBar;
    private Ressources ressources;
    private float lastTimeInjured = 0;
    public ActionBasedContinuousMoveProvider continuousMoveProvider;

    PhotonView view;

    public enum playerState { HEALTHY, KO };
    private playerState state = playerState.HEALTHY;

    // Start is called before the first frame update
    void Start()
    {
        ressources = GetComponent<Ressources>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == playerState.KO)
        {
            continuousMoveProvider.enabled = false;
        }
        else
        {
            if (health < maxHealth && Time.time - lastTimeInjured > 4)
            {
                health += maxHealth / 200;
                healthBar.UpdateHealthBar(health, maxHealth);
            }
            InputDevice leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            if (leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool isXButtonPressed) && isXButtonPressed)
            {
                Debug.Log("OK");
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        view.RPC("TakeDamageRPC", RpcTarget.All, damageAmount);
    }


    [PunRPC]
    public void TakeDamageRPC(float damageAmount)
    {
        lastTimeInjured = Time.time;
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    public void GainMoney(int amount)
    {
        money += amount;
        ressources.UpdateMoney(money);
    }

    public void BuyWeapon(int amount)
    {
        money -= amount;
        ressources.UpdateMoney(money);
    }
}
