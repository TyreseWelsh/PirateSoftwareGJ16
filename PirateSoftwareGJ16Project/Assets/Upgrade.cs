using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private int pickupIndex;
    private SphereCollider pickupRadius;

    private Coroutine pickupCoroutine;

    private Coroutine stopCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        pickupRadius = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (pickupCoroutine == null)
            {
                if (stopCoroutine != null)
                {
                    StopCoroutine(stopCoroutine);
                    stopCoroutine = null;
                }
                pickupCoroutine = StartCoroutine(pickupTick());
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(pickupCoroutine);
            pickupCoroutine = null;
            stopCoroutine = StartCoroutine(dropTick());
        }
    }

    private IEnumerator pickupTick()
    {
        if (pickupIndex == 10)
        {
            Destroy(this.gameObject);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        pickupIndex++;
        pickupCoroutine = StartCoroutine(pickupTick());
    }

    private IEnumerator dropTick()
    {

       while (pickupIndex > 0)
       {
           pickupIndex--;
           yield return new WaitForSeconds(1);
           yield return null;
       }
       Debug.Log("NO longer ticking down");
       StopCoroutine(stopCoroutine);
       
    }
}


