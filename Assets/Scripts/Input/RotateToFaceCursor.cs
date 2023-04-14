using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToFaceCursor : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector2 PointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = (PointerPosition - (Vector2)transform.position).normalized;
    }
}
