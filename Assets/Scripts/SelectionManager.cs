using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] float maxRange = 3.5f;
    [SerializeField] LayerMask mask;
    //Transform selectableTemp;
    void Update()
    {
        Selecting();
    }
    void Selecting()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxRange, mask))
        {
            var selectable = hit.transform;
            ISelectable selectableCode = selectable.gameObject.GetComponent<ISelectable>();
            if (selectableCode != null)
            {
                //Hover action
                //selectableCode.Hover();

                if (Input.GetButtonDown("Fire1"))
                {
                    selectableCode.Select();
                }
            }
        }
    }
}
