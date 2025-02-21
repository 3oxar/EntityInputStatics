using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

partial class InputSystem : SystemBase
{
    private InputAction _moveAction;
    private InputAction _jerkAction;
    private InputAction _fireAction;

    private float2 _moveInput;

    private float _jerkInput;
    private float _jerkReload;
    private float _fireInput;
    protected override void OnStartRunning()
    {
        _moveAction = new InputAction("move", binding: "<Gamepad>/rightStick");
        _jerkAction = new InputAction("jerk", binding: "<Keyboard>/space");
        _fireAction = new InputAction("fire", binding: "<Mouse>/rightButton");

        _moveAction.AddCompositeBinding("Dpad")
            .With("Up", binding: "<Keyboard>/w")
            .With("Down", binding: "<Keyboard>/s")
            .With("Left", binding: "<Keyboard>/a")
            .With("Right", binding: "<Keyboard>/d");

        _moveAction.performed += context => { _moveInput = context.ReadValue<Vector2>(); };
        _moveAction.canceled += context => { _moveInput = context.ReadValue<Vector2>(); };

        _jerkAction.performed += context => { _jerkInput = context.ReadValue<float>(); };
        _jerkAction.canceled += context => { _jerkInput = context.ReadValue<float>(); };

        _fireAction.performed += context => { _fireInput = context.ReadValue<float>(); };
        _fireAction.canceled += context => { _fireInput = context.ReadValue<float>(); };

        _jerkAction.Enable();
        _moveAction.Enable();
        _fireAction.Enable();
    }

    protected override void OnUpdate()
    {
        foreach (var (input, player, entity) in SystemAPI.Query<RefRW<InputComponent>, RefRO<PlayerTag>>().WithEntityAccess())
        {
            input.ValueRW.Move = _moveInput;
            //input.ValueRW.Shoot = _fireInput;

            if (_jerkInput != 0 && _jerkReload == 0)//если нажата кнопка и время перезарядки рывка 0 секунд
            {
                input.ValueRW.Jerk = _jerkInput;
                _jerkInput = 0;
                _jerkReload = 5f;//устанавливаем время перезарядки рывка 
            }
            else if (_jerkInput == 0)
            {
                input.ValueRW.Jerk = _jerkInput;
            }

            if (_jerkReload > 0)
            {
                _jerkReload -= SystemAPI.Time.DeltaTime;//отсчитываем время рывка 
            }
            else if (_jerkReload < 0)
                _jerkReload = 0;
        }
    }

    protected override void OnStopRunning()
    {
        _jerkAction.Disable();
        _moveAction.Disable();
        _fireAction.Disable();
    }
}


