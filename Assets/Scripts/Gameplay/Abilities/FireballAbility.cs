using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAbility : Ability
{
    [SerializeField]
    GameObject fireballPrefab;

    // FUTURE:- Object pool these

    // Start is called before the first frame update
    void Start()
    {
        cooldownDuration = 0.2f;
        effectDuration = 2f;
        
    }

    // OnUpdate is called once per frame by `Ability` parent class
    protected override void OnUpdate() { }

    public override void Undo() { }

    void OnDestroy() { }
    
    public override void Execute()
    {
        if (!isReady) { return; }

        castStartTime = Time.time;
        cooldownProgress = cooldownDuration;

        Vector2 PointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        var movementVector = (PointerPosition - (Vector2)transform.position).normalized * 10;

        var fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        fireball.transform.right = (PointerPosition - (Vector2)transform.position).normalized;
        fireball.GetComponent<Rigidbody2D>().velocity += movementVector;
        StartCoroutine(DestroyAfterSeconds(fireball, 1));
    }



    // Temporary:- 
    IEnumerator DestroyAfterSeconds(GameObject fireball, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(fireball);
    }
}
