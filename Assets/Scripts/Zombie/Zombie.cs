using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Zombie : MonoBehaviour
{
    // Start is called before the first frame update

   // Player[] players;
   // Player nearestPlayer;

    [SerializeField] float health, maxHealth = 3f;
    [SerializeField] FloatingHealthBar healthBar;

    PhotonView view;

    [SerializeField] private float stoppingDistance = 3.0f;
    private bool hasEeachedPlayer = false;
    private float timeOfLastAttack = 0.0f;
    private float attackSpeed = 1.5f;

    [SerializeField] private Animator zombieAnimator;

    public Transform player;
    public Player playerScript;


    public float followDistance = 10f;
    private NavMeshAgent agent;

    public AudioSource source;
    public AudioClip zombieSound;
    public AudioClip zombieSoundAtk;


    void Start()
    {
        /*players = FindObjectsOfType<Player>();
        if (players.Length > 1 ) {
            Debug.Log("OK");
        }*/
        view = GetComponent<PhotonView>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        player = Player.transformInstance;
        playerScript = Player.gameObjectInstance;

        if (zombieAnimator == null)
            zombieAnimator = GetComponentInChildren<Animator>();
        source.PlayOneShot(zombieSound);
    }

    void Update()
    {
        /*float distanceOne = Vector3.Distance(transform.position, players[0].transform.position);
        float distanceTwo = Vector3.Distance(transform.position, players[1].transform.position);
        float distance = distanceOne;*/
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < followDistance)
        {
            agent.SetDestination(player.position);
            zombieAnimator.SetBool("Walk", true);

            if (distance < stoppingDistance) 
            {
                if (!hasEeachedPlayer)
                {
                    // Give the player a last chance to try to escape
                    hasEeachedPlayer = true;
                    timeOfLastAttack = Time.time;
                }
                if (Time.time >= timeOfLastAttack + attackSpeed)
                {
                    timeOfLastAttack = Time.time;
                    zombieAnimator.SetBool("Attack", true);
                    source.PlayOneShot(zombieSoundAtk);
                    playerScript.TakeDamage(1);
                }
            }
            else
            {
                zombieAnimator.SetBool("Attack", false);
                hasEeachedPlayer = false;
            }
        }
        else
        {
            zombieAnimator.SetBool("Walk", false);
        }
    }

    public void DealDamage(float damageAmount)
    {
        playerScript.TakeDamage(1);
    }

    public void TakeDamage(float damageAmount)
    {
        view.RPC("TakeDamageRPC", RpcTarget.All, damageAmount);
    }

    [PunRPC]

    public void TakeDamageRPC(float damageAmount)
    {
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);

        if (health <=0)
        {
            //zombieAnimator.SetTrigger("Die");
            Destroy(gameObject);
            //return 50;
        }
        //return 0;
    }
}
