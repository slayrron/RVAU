using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door_animation : MonoBehaviour
{

    private Animator doorAnimator;
    private SphereCollider sc;

    // Start is called before the first frame update
    void Start()
    {
        sc = gameObject.AddComponent<SphereCollider>();
        sc.isTrigger = true;
        sc.radius = 2.0f;

        doorAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnimator.SetBool("character_nearby", true);
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnimator.SetBool("character_nearby", false);
        }
    }
}
