using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopupHandler : MonoBehaviour
{
    [SerializeField] private GameObject DamagePopupPrefab;

    public DamagePopup Create(Vector3 position, int damageAmount)
    {
        GameObject popup = Instantiate(DamagePopupPrefab, position, Quaternion.identity);
        DamagePopup damagePopup = popup.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, sortingOrder++);

        return damagePopup;
    }

    private int sortingOrder = 0;
}
