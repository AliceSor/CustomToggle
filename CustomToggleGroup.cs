using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SA.CustomToggle
{
    public class CustomToggleGroup : MonoBehaviour
    {
        [SerializeField] protected bool showDebug = false;

        public UnityEvent onValuesChanged;
        public UnityEvent onCurrentToggleChanged;
        public UnityEvent onToggleClicked;
        [Tooltip("Second click will uncheck toggle")]
        public bool allowSwitchOff;
        public bool allowOnlyOne;

        [SerializeField] private Transform togglesRoot;

        [Header("Default")]
        [Tooltip("If this value not null - that component will be chosen as default each time toggle group enabled")]
        public AbstaractCustomToggle defaultToggle;

        public ICustomToggle current;
        public List<ICustomToggle> Buttons { get => buttons; }
        public List<ICustomToggle> chosenButtons = new List<ICustomToggle>();

        protected List<ICustomToggle> buttons = new List<ICustomToggle>();

        public Transform TogglesRoot { get => togglesRoot; set => togglesRoot = value; }

        public virtual void Init()
        {
            if (showDebug)
                Debug.Log($"{name}: Init called", this);

            if (buttons == null)
                buttons = new List<ICustomToggle>();

            StartCoroutine(TryUncheckDefault());

            UpdateChosenButtons();
        }

        protected void UpdateChosenButtons()
        {
            if (showDebug)
                Debug.Log($"{name}: UpdateChosenButtons called", this);

            //Debug.Log("UpdateChosenButtons");
            chosenButtons.Clear();
            foreach (var i in buttons)
                if (i.IsOn() && !chosenButtons.Contains(i))
                    chosenButtons.Add(i);
                else
                {

                    i.Toggle(false);
                    if (chosenButtons.Contains(i))
                        chosenButtons.Remove(i);
                }

            string cb = "";

            foreach (var i in chosenButtons)
                cb += i + " | ";
            //Debug.Log("Chosen button initialy: " + cb);
        }

        private void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            yield return null;
            UpdateChosenButtons();
        }

        //TODO: rename Uncheck on Check
        private IEnumerator TryUncheckDefault()
        {
            yield return null;
            if (defaultToggle != null && defaultToggle is ICustomToggle i)
                ButtonClicked(i);
        }

        public virtual object GetValue()
        {
            return null;
        }

        public void RegisterButton(ICustomToggle newB)
        {
            if (showDebug)
                Debug.Log($"{name}: RegisterButton {newB}", this);

            if (buttons == null)
                buttons = new List<ICustomToggle>();
            if (!buttons.Contains(newB))
                buttons.Add(newB);
        }

        public void UnregisterButton(ICustomToggle newB)
        {
            if (showDebug)
                Debug.Log($"{name}: UnregisterButton {newB}", this);

            if (buttons != null)
                if (buttons.Contains(newB))
                    buttons.Remove(newB);
        }

        public void ButtonClicked(ICustomToggle button)
        {
            if (showDebug)
                Debug.Log($"{name}: ButtonClicked {button}", this);

            if (allowOnlyOne)
                ButtonClickedForSingleMode(button);
            else
                ButtonClickedForMultipleMode(button);
            onToggleClicked?.Invoke();
        }

        private void ButtonClickedForMultipleMode(ICustomToggle button)
        {
            if (showDebug)
                Debug.Log($"{name}: ButtonClickedForMultipleMode {button}", this);

            // Check if we have buttons
            // if allowSwitch of we just switch
            // else we check if any other options left. If they are we can switch off else do nothing
            // if something was changed updated chosen buttons we fire event
            bool stateChanged = false;

            if (buttons == null)
                return;

            if (allowSwitchOff)
            {
                button.Toggle(!button.IsOn());
                stateChanged = true;

                if (button.IsOn())
                    chosenButtons.Add(button);
                else
                    if (chosenButtons.Contains(button))
                    chosenButtons.Remove(button);
            }
            else
                // if button if off we can just turn it on
                // if on but we have other enabled buttons also can just turn it off
                // else if button on we do nothing

                if (!button.IsOn() || button.IsOn() && chosenButtons.Count > 1)
            {
                button.Toggle(!button.IsOn());
                stateChanged = true;

                if (button.IsOn())
                    chosenButtons.Add(button);
                else
                    if (chosenButtons.Contains(button))
                    chosenButtons.Remove(button);
            }
            else
            {
                // Do nothing
            }

            if (stateChanged)
                onValuesChanged?.Invoke();

        }

        private void ButtonClickedForSingleMode(ICustomToggle button)
        {
            if (showDebug)
                Debug.Log($"{name}: ButtonClickedForSingleMode {button}", this);

            // Check if we have buttons
            // if allowSwitch of we just switch
            // else if can switch then switch else do nothing
            // if something was changed updated chosen buttons we fire event

            bool stateChanged = false;

            if (buttons == null)
                return;

            if (allowSwitchOff)
            {
                bool isOn = button.IsOn();

                if (buttons.Count > 0)
                    foreach (ICustomToggle i in buttons)
                        i.Toggle(false);
                button.Toggle(!isOn);

                stateChanged = true;

                chosenButtons.Clear();
                if (button.IsOn())
                {
                    chosenButtons.Add(button);
                    current = button;
                }
                else
                    current = null;
            }
            else
                if (!button.IsOn())
            {
                bool isOn = button.IsOn();

                if (buttons.Count > 0)
                    foreach (ICustomToggle i in buttons)
                        i.Toggle(false);
                button.Toggle(!isOn);
                stateChanged = true;

                chosenButtons.Clear();
                chosenButtons.Add(button);
                current = button;
            }
            else
            {
                // Do nothing
            }

            if (stateChanged)
            {
                onValuesChanged?.Invoke();
                onCurrentToggleChanged?.Invoke();
            }
        }

        public void UnclickAll()
        {
            if (showDebug)
                Debug.Log($"{name}: UnclcikAll called", this);

            if (buttons != null)
                foreach (ICustomToggle i in buttons)
                    i.Toggle(false);
        }

        public void DisableAll()
        {
            if (showDebug)
                Debug.Log($"{name}: DisableAll called", this);

            if (buttons != null)
                foreach (ICustomToggle i in buttons)
                    i.Toggle(false);
            if (defaultToggle != null && defaultToggle is ICustomToggle j)
                defaultToggle.Toggle(true);
            if (chosenButtons != null)
                chosenButtons.Clear();
        }
    }
}