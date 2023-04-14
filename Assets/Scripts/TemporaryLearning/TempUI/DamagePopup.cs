using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTimer = 0.5f;
    private float moveSpeed = 2f;

    // Start is called before the first frame update
    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, moveSpeed) * Time.deltaTime;
        disappearTimer -= Time.deltaTime;

        if (disappearTimer < 0)
        {
            Destroy(gameObject);
        }
    }

    public void Setup(int damageAmount, int sortOrder)
    {
        textMesh.text = damageAmount.ToString();
        textMesh.sortingOrder = sortOrder;
    }
}
