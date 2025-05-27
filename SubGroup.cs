using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubGroup : MonoBehaviour
{
    protected List<ICustomToggle> buttons;
    public List<ICustomToggle> chosenButtons = new List<ICustomToggle>();

    public virtual void Init()
    {
        if (buttons == null)
            buttons = new List<ICustomToggle>();
    }

    private IEnumerator Start()
    {
        Init();

        // Wait for toggles to register
        yield return null;
        yield return null;
    }

    public void ButtonClicked(ICustomToggle button)
    {
        // Here we check button and if needed uncheck other toggles in group

        // So, if toggle is off - that mean before click if was on - we will do nothing 
        // And if it was on we disable all others
        // We should be carefull if we will do something on "off" that can create infinite recursion
        if (button.IsOn())
        {
            if (buttons != null)
            {
                foreach (var i in buttons)
                {
                    if (i.GetHashCode() != button.GetHashCode() && i.IsOn())
                    {
                        // Button on state "on" and we imitate click on it so the button will be disabled
                        // by main group and main group will have actual info about this button state
                        i.OnClick();

                    }
                }
            }
        }
    }

    public void RegisterButton(ICustomToggle newB)
    {
        if (buttons == null)
            buttons = new List<ICustomToggle>();
        if (!buttons.Contains(newB))
            buttons.Add(newB);
    }

    public void UnregisterButton(ICustomToggle newB)
    {
        if (buttons != null)
            if (buttons.Contains(newB))
            {
                buttons.Remove(newB);
            }
    }
}
