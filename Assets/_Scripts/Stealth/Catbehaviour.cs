using UnityEngine;
using Unity.AI;
using System;
using UnityEngine.AI;

public class CatBehaviour : MonoBehaviour
{
    [SerializeField] private NavMeshAgent cat;
    [SerializeField] private float jumpValue;
    [SerializeField] private Transform mouse;
    [SerializeField] private bool canMove;
    private Vector3 prevPosition;
    private Transform catPosition;
    [SerializeField] private Animator catAnimator;
    public bool CanMove
    {
        get { return canMove; }
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

    // This method makes the cat follow the mouse's position using the NavMeshAgent component.
    // It also checks if the cat is falling or walking based on its previous and current positions, and triggers the appropriate animations.
    private void Update() 
    {
        if (canMove)
        {
            cat.SetDestination(mouse.position);
            if (prevPosition.y > transform.position.y)
            {
                Debug.Log("Cae");
                catAnimator.SetBool("Falling", true);
            }
            else
                catAnimator.SetBool("Falling", false);
            if ((prevPosition.x != transform.position.x || prevPosition.z != transform.position.z) && prevPosition.y == transform.position.y)
            {
                Debug.Log("Se mueve");
                catAnimator.SetBool("Walking", true);
            }
            else
                catAnimator.SetBool("Walking", false);
            prevPosition = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Navmesh jump"))
        {
            Debug.Log("Salta");
            catAnimator.SetTrigger("Jumped");
        }
        else if (other.CompareTag("Sitting zone"))
        {
            catAnimator.SetBool("Sitting", true);
        }
    }
}
