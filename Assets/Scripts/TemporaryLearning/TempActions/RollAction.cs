using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAction : MonoBehaviour
{
    bool isReady = true;
    float cooldown = 3f;
    float duration = 0.3f;
    Rigidbody2D rb;

    bool isRolling = false;
    float rollStartTime = 0f;

    KeyboardController keyboardController;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        keyboardController = GetComponent<KeyboardController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRolling) 
        {
            HandleRollInProgress();
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isReady)
        {
            Vector2 PointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var movementVector = (PointerPosition - (Vector2)keyboardController.gameObject.transform.position).normalized * 10;
            CommandManager.Instance.AddCommand(
                new RollCommand(
                    transform,
                    movementVector
                ),
                keyboardController.rewinder
            );
            isReady = false;
            StartCoroutine(StartCooldown());
        }
    }

    // Called by the DashCommand Execute()
    public void Roll(Vector3 movementVector)
    {
        rollStartTime = Time.time;
        isRolling = true;
        keyboardController.SetMovementEnabled(false);
        rb.freezeRotation = false;
        rb.velocity = movementVector;
    }

    IEnumerator StartCooldown() 
    {
        yield return new WaitForSeconds(cooldown);
        isReady = true;
    }

    void HandleRollInProgress()
    {
        var timer = Time.time - rollStartTime;
        if (timer >= duration)
        {
            rollStartTime = 0;
            rb.velocity = Vector2.zero;
            rb.rotation = 0;
            rb.freezeRotation = true;
            isRolling = false;
            keyboardController.SetMovementEnabled(true);
            return;
        }
        
        float percentage = (timer / duration);
        // Future:- Set this to an animation instead of a fake rotation
        rb.rotation = percentage * 360;
    }
}
