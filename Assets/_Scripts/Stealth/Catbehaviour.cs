using UnityEngine;
using Unity.AI;
using System;
using UnityEngine.AI;

public class Catbehaviour : MonoBehaviour
{
    [SerializeField] private NavMeshAgent cat;
    [SerializeField] private float jumpValue;
    private Vector3 prevPosition;
    [SerializeField] private Transform mouse;

    private Transform catPosition;
    private Animator catAnimator;
    [SerializeField] private bool canMove;
    public bool CanMove
    {
        get {return canMove; }
        set { canMove = value; }
    }

    private void Start()
    {
        catPosition = gameObject.transform;
        catAnimator = gameObject.GetComponent<Animator>();
        GameObject target = GameObject.FindGameObjectWithTag("Player");
        mouse = target.transform;
        prevPosition = transform.position;
    }

   
    private void Update()
    {
        if (canMove)
        {
            prevPosition = transform.position;
            cat.SetDestination(mouse.position);
            if (prevPosition.y > transform.position.y)
            {
                catAnimator.SetTrigger("Falling");
            }
            if ((prevPosition.x != transform.position.x || prevPosition.z != transform.position.z) && prevPosition.y == transform.position.y)
            {
                catAnimator.SetTrigger("Walking");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Navmesh jump"))
        {
            catAnimator.SetTrigger("Jumped");
        }
        else if (other.CompareTag("Sitting zone"))
        {
            catAnimator.SetBool("Sitting", true);
        }
    }
}
