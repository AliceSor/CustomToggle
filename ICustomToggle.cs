using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICustomToggle
{
    void Toggle(bool value);
    void OnClick();
    object GetToggleValue();
    bool IsOn();
}
