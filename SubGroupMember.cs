using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AbstaractCustomToggle))]
public class SubGroupMember : MonoBehaviour
{
    public SubGroup subGroup;
    private AbstaractCustomToggle toggle;

    private void Start()
    {
        toggle = GetComponent<AbstaractCustomToggle>();
        toggle.onValueChanged.AddListener(OnValueChanged);

        if (subGroup != null)
        {
            subGroup.RegisterButton(toggle);
        }
    }

    private void OnDestroy()
    {
        if (toggle != null)
        {
            toggle.onValueChanged.RemoveListener(OnValueChanged);
        }

        if (subGroup != null)
        {
            subGroup.UnregisterButton(toggle);
        }
    }

    public void OnValueChanged()
    {
        if (subGroup != null)
        {
            //Debug.Log("Toggle value changed" + toggle.GetToggleValue().ToString() + " : " + toggle.isOn.ToString());
            subGroup.ButtonClicked(toggle);
        }
    }
}
