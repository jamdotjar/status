using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float boxMoveSpeed = 70.0f;

    public float maxGrabDistance;
    public LayerMask wallLayerMask;
    private BoxCollider2D boxCollider;
    public LayerMask boxLayerMask;
    public float grabDistance = 2.5f;
    public float hoverRadius = 2.2f;
    public KeyCode grabKey = KeyCode.E;
    
    private Rigidbody2D grabbedBox;
    private bool isGrabbing;

 private void Update()
{

    Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    if (Input.GetKeyDown(grabKey))
    {
        if (!isGrabbing)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, mouseWorldPosition - (Vector2)transform.position, grabDistance, boxLayerMask);
            if (hit.collider != null)
            {
                // Check if there's a wall between the player and the box
                RaycastHit2D wallHit = Physics2D.Linecast(transform.position, hit.transform.position, wallLayerMask);
                if (wallHit.collider == null)
                {
                    grabbedBox = hit.collider.gameObject.GetComponent<Rigidbody2D>();
                    boxCollider = hit.collider.gameObject.GetComponent<BoxCollider2D>();
                    if (grabbedBox != null)
                    {
                        grabbedBox.gravityScale = 0;
                        isGrabbing = true;
                        Physics2D.IgnoreCollision(boxCollider, GetComponent<Collider2D>(), true); // Ignore collision with player
                    }
                }
            }
        }
        else
        {
            grabbedBox.gravityScale = 1;
            Physics2D.IgnoreCollision(boxCollider, GetComponent<Collider2D>(), false); // Re-enable collision with player
            grabbedBox = null;
            boxCollider = null;
            isGrabbing = false;
        }
    }
    if (isGrabbing)
    {
        Vector2 hoverDirection = (mouseWorldPosition - (Vector2)transform.position).normalized;
        float actualDistance = Vector2.Distance(mouseWorldPosition, (Vector2)transform.position);
        float positionFactor = Mathf.Min(1.0f, actualDistance / hoverRadius);

        Vector2 targetPosition = (Vector2)transform.position + hoverDirection * hoverRadius * positionFactor;

        // Check for wall collisions
        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, hoverDirection, hoverRadius, wallLayerMask);
        if (wallHit.collider != null)
        {
            targetPosition = wallHit.point - (hoverDirection * 0.01f); // Slightly move the box away from the wall
        }

        float step = boxMoveSpeed * Time.deltaTime;
        grabbedBox.MovePosition(Vector2.MoveTowards(grabbedBox.transform.position, targetPosition, step));

        // Check if the box is too far from the player
        if (Vector2.Distance(transform.position, grabbedBox.transform.position) > maxGrabDistance)
        {
            grabbedBox.gravityScale = 1;
            grabbedBox = null;
            isGrabbing = false;
        }
    }
}
}