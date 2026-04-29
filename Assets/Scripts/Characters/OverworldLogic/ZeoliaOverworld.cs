using UnityEngine;
using UnityEngine.AI;

public class ZeoliaOverworld : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody2D rb;

    private int direction = 5; // for animation

     void Awake()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if(GameManager.GameState == GameStates.Overworld)
        {
            agent.SetDestination(target.position);

            Vector3 move = agent.desiredVelocity;
            int tempDir = 5;

            // surprisingly not the same order as the input map so I can't just reuse
            if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
            {
                if (Mathf.Sign(move.x) > 0)
                {
                    tempDir = (int)CardinalDirections.North;
                }
                else
                {
                    tempDir = (int)CardinalDirections.South;
                }
            }
            else
            {
                if (Mathf.Sign(move.y) > 0)
                {
                    tempDir = (int)CardinalDirections.East;
                }
                else
                {
                    tempDir = (int)CardinalDirections.West;
                }
            }

            if (tempDir != direction)
            {
                direction = tempDir;
                animator.SetInteger("direction", direction);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player playerScript))
        {
            GameManager.StartCombat(enemy1: CharManager.chars[CharNames.Zeolia], doAfterCombat: () => {
                Destroy(gameObject);
            });
        }
    }
}
