using UnityEngine;
using Unity.AI;
using System;
using UnityEngine.AI;

public class Catbehaviour : MonoBehaviour
{
    [SerializeField] private NavMeshAgent cat;
    private Transform catPosition;
    private Rigidbody catRB;
    [SerializeField] private float jumpValue;
    [SerializeField] private Vector3 jumpVector;
    [SerializeField] private Transform mouse;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        catPosition = gameObject.transform;
        catRB = gameObject.GetComponent<Rigidbody>();
        GameObject target = GameObject.FindGameObjectWithTag("Player");
        mouse = target.transform;
        jumpVector = new Vector3(0, jumpValue, 0);
    }

    // Update is called once per frame
    void Update()
    {
        cat.SetDestination(mouse.position);
        if(catPosition.transform.position.y < mouse.position.y || Input.GetKeyDown(KeyCode.L))
        {
            //this won't work if nav agent is active
            catRB.AddForce(jumpVector, ForceMode.Impulse);
        }
    }
}
