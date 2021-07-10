using System;
using Libs;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public event Action OnFireButtonPressed;
    public event Action OnConstructButtonDown;
    public event Action<float> OnRotationAxisUpdate;
    public event Action<float, float> OnMovementAxisUpdate;
    public event Action<Vector3> OnWorldSpacePointerPositionUpdate;

    [Range(0, 1)]
    [SerializeField] private float _rotationDelta = 0.1f;

    private float _rotationAxis;

    private enum InputNames 
    {
        Vertical,
        Horizontal,
        PlusRotationButton,
        MinusRotationButton,
        Fire1,
        ConstructModeButton
    }

    private void Update()
    {
        OnWorldSpacePointerPositionUpdate?.Invoke(Input.mousePosition);
        OnMovementAxisUpdate?.Invoke(Input.GetAxis(InputNames.Horizontal.ToString()), Input.GetAxis(InputNames.Vertical.ToString()));

        bool plusRotationButtonPressed = Input.GetButton(InputNames.PlusRotationButton.ToString());
        bool minusRotationButtonPressed = Input.GetButton(InputNames.MinusRotationButton.ToString());

        if (plusRotationButtonPressed || minusRotationButtonPressed)
        {
            if (plusRotationButtonPressed)
            {
                _rotationAxis += _rotationDelta;

                if (_rotationAxis > 0)
                {
                    _rotationAxis = 1;
                }
            }

            if (Input.GetButton(InputNames.MinusRotationButton.ToString()))
            {
                _rotationAxis -= _rotationDelta;

                if (_rotationAxis < -1)
                {
                    _rotationAxis = -1;
                }
            }
        }
        else
        {
            _rotationAxis = 0;
        }

        OnRotationAxisUpdate?.Invoke(_rotationAxis);
        

        if (Input.GetButton(InputNames.Fire1.ToString()))
        {
            OnFireButtonPressed?.Invoke();
        }

        if (Input.GetButtonDown(InputNames.ConstructModeButton.ToString()))
        {
            OnConstructButtonDown?.Invoke();
        }
    }
}
