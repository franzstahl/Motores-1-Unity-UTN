using UnityEngine;
using Unity.AI;
using System;
using UnityEngine.AI;

public class Catbehaviour : MonoBehaviour
{
    [SerializeField] private NavMeshAgent cat;
    [SerializeField] private float jumpValue;
    [SerializeField] private Vector3 jumpVector;
    [SerializeField] private Transform mouse;

    private Transform catPosition;
    private Rigidbody catRB;
    [SerializeField] private bool canMove;
    public bool CanMove
    {
        get {return canMove; }
        set { canMove = value; }
    }

    private void Start()
    {
        catPosition = gameObject.transform;
        catRB = gameObject.GetComponent<Rigidbody>();
        GameObject target = GameObject.FindGameObjectWithTag("Player");
        mouse = target.transform;
        jumpVector = new Vector3(0, jumpValue, 0);
    }

   
    private void Update()
    {
        if(canMove)
            cat.SetDestination(mouse.position);
        // if(catPosition.transform.position.y < mouse.position.y || Input.GetKeyDown(KeyCode.L))
        // {
        //     cat.enabled = false;
        //     catRB.AddForce(jumpVector, ForceMode.Impulse);
        //     cat.enabled = true;
        // }
    }
}
