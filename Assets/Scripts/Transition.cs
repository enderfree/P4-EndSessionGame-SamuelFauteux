using System.Collections;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private CardinalDirections activationDirection;
    [SerializeField] private Transform destination;
    [SerializeField] private RoomNames destinationRoom;

    private bool playerOnTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player playerScript))
        {
            playerOnTrigger = true;
            StartCoroutine(DoesPlayerWantToTransition(collision.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player playerScript))
        {
            playerOnTrigger = false;
        }
    }

    private IEnumerator DoesPlayerWantToTransition(GameObject player)
    {
        Animator animator = player.GetComponent<Animator>();

        while (playerOnTrigger)
        { 
            if (animator.GetBool("isWalking") && animator.GetInteger("direction") == (int)activationDirection)
            {
                GameManager.CurrentRoom = RoomManager.rooms[destinationRoom];
                player.transform.position = destination.position;

                playerOnTrigger = false;
            }

            yield return null;
        }
    }
}
