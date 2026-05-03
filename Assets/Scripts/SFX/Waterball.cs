using System;
using UnityEngine;

public class Waterball : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float damping;
    [SerializeField] float detectionRadius;

    [NonSerialized] public Vector3 target;
    [NonSerialized] public Action callback;
    
    private bool shouldMove = false;
    private Vector3 smoothDampVelocity = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void FixedUpdate()
    {
        if (shouldMove)
        {
            MoveToTarget();
        }
    }

    public void BeginToMove()
    {
        shouldMove = true;
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target, ref smoothDampVelocity, damping);

        if (Vector3.Distance(transform.position, target) < detectionRadius)
        {
            if (callback != null)
            {
                callback();
            }

            animator.SetTrigger("Pop");
        }
    }
}
