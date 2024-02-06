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
    [SerializeField] GameObject koScreen;
    private Ressources ressources;
    private float lastTimeInjured;
    public ActionBasedContinuousMoveProvider continuousMoveProvider;

    PhotonView view;

    public enum playerState { HEALTHY, KO };
    private playerState state = playerState.HEALTHY;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        ressources = GetComponent<Ressources>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        health = maxHealth;
        koScreen = GameObject.Find("KO Screen");
        koScreen.SetActive(false);
        koScreen.activeInHierarchy.Equals(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == playerState.KO)
        {
            koScreen.SetActive(true);
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
                RaycastHit hit;
                if (Physics.Raycast(transform.position, -transform.right, out hit, 2f))
                {
                    // Check if the collided object has a GameObject
                    GameObject collidedObject = hit.collider.gameObject;
                    if (collidedObject.tag == "Door")
                    {
                        Door door = collidedObject.GetComponent<Door>();
                        if (money >= door.price)
                        {
                            LoseMoney(door.price);
                            door.RemoveDoors(collidedObject);
                        }
                    }
                }
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        view.RPC("TakeDamageRPC", RpcTarget.All, damageAmount);
        lastTimeInjured = Time.time;
    }

    [PunRPC]
    public void TakeDamageRPC(float damageAmount)
    {
        if (health <= 0)
        {
            state = playerState.KO;
        }
        else
        {     
            health -= damageAmount;
            healthBar.UpdateHealthBar(health, maxHealth);
        }
    }

    public void GainMoney(int amount)
    {
        money += amount;
        ressources.UpdateMoney(money);
    }

    public void LoseMoney(int amount)
    {
        money -= amount;
        ressources.UpdateMoney(money);
    }
}
