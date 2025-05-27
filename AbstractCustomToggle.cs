using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SA.CustomToggle
{
    public abstract class AbstaractCustomToggle : MonoBehaviour, ICustomToggle
    {
        //public bool interactable;
        public bool isOn;

        [Header("Toggle group")]
        public CustomToggleGroup toggleGroup;

        [Header("Target button")]
        [Tooltip("In case this field will be empty this script will search for a button on this object")]
        public Button targetButton;

        [Header("Target graffic objects")]
        [Tooltip("This gameObjects will be disabled depending on the toggle state")]
        public GameObject chosen;
        public GameObject notChosen;

        [Space(10)]
        public UnityEvent onValueChanged;
        public UnityEvent onTurnedOn;
        public UnityEvent onTurnedOff;

        private bool subscribed = false;

        public void OnEnable()
        {
            if (targetButton == null)
                targetButton = GetComponent<Button>();
            if (targetButton != null && !subscribed)
            {
                targetButton.onClick.AddListener(OnClick);
                subscribed = true;
            }
            if (toggleGroup != null)
            {
                toggleGroup.RegisterButton(this);
            }
        }

        public void OnDestroy()
        {
            if (targetButton != null)
                targetButton.onClick.RemoveListener(OnClick);
            if (toggleGroup != null)
            {
                toggleGroup.UnregisterButton(this);
            }
        }

        public void Toggle(bool value)
        {
            ToggleGraffic(value);
            isOn = value;
            if (isOn)
                onTurnedOn?.Invoke();
            else
                onTurnedOff?.Invoke();
            onValueChanged?.Invoke();
        }

        public virtual void ToggleGraffic(bool value)
        {
            if (chosen != null)
                chosen.SetActive(value);
            if (notChosen != null)
                notChosen.SetActive(!value);
        }

        public virtual void OnClick()
        {
            //    SLDebug.Log("Toggle clicked " + gameObject.name);
            if (toggleGroup != null)
                toggleGroup.ButtonClicked(this);
            else
            {
                Toggle(!isOn);
            }
        }

        public abstract object GetToggleValue();

        public bool IsOn()
        {
            return isOn;
        }
    }
}