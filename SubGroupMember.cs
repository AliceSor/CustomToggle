using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.CustomToggle
{
    [RequireComponent(typeof(AbstaractCustomToggle))]
    public class SubGroupMember : MonoBehaviour
    {
        [SerializeField] protected bool showDebug = false;

        public SubGroup subGroup;
        private AbstaractCustomToggle toggle;

        private void Start()
        {
            toggle = GetComponent<AbstaractCustomToggle>();
            toggle.onValueChanged.AddListener(OnValueChanged);

            if (showDebug)
                Debug.Log($"{name}: Start called, toggle={toggle}, subGroup={subGroup}", this);

            if (subGroup != null)
                subGroup.RegisterButton(toggle);
        }

        private void OnDestroy()
        {
            if (showDebug)
                Debug.Log($"{name}: OnDestroy called, toggle={toggle}, subGroup={subGroup}", this);

            if (toggle != null)
                toggle.onValueChanged.RemoveListener(OnValueChanged);

            if (subGroup != null)
                subGroup.UnregisterButton(toggle);
        }

        public void OnValueChanged()
        {
            if (showDebug)
                Debug.Log($"{name}: OnValueChanged called, toggle={toggle}, subGroup={subGroup}", this);

            if (subGroup != null)
                subGroup.ButtonClicked(toggle);
        }
    }
}