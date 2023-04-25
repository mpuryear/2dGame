using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldController : MonoBehaviour
{
    private Transform suckTo;
    private float moveSpeed = 4f;
    private CurrencyView view;
    private GoldVacuum vacuum;
    void Update()
    {
        if(!suckTo) { return; }

        transform.position = Vector3.MoveTowards(transform.position, suckTo.position, moveSpeed * Time.deltaTime);

        if((transform.position - suckTo.position).sqrMagnitude < 0.005f)
        {
            vacuum.PlayGoldSound();
            view.OnPickupGold(1);
            Destroy(this.gameObject);
        }
    }

    public void SuckTo(Transform target, CurrencyView view, GoldVacuum vacuum, float withSpeed)
    {
        this.moveSpeed = withSpeed;
        this.view = view;
        this.vacuum = vacuum;
        suckTo = target;
    }
}
