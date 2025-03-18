using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

partial class InputSystem : SystemBase
{
    private InputAction _moveAction;
    private InputAction _jerkAction;
    private InputAction _fireAction;
    private InputAction _createPrefabAction;

    private float2 _moveInput;

    private float _jerkInput;
    private float _jerkReload;
    private float _fireInput;
    private float _createPrefabInput;
    private float _createPrefabReload;

    protected override void OnStartRunning()
    {
        _moveAction = new InputAction("move", binding: "<Gamepad>/rightStick");
        _jerkAction = new InputAction("jerk", binding: "<Keyboard>/space");
        _fireAction = new InputAction("fire", binding: "<Mouse>/leftButton");
        _createPrefabAction = new InputAction("createPrefab", binding: "<Keyboard>/tab");

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

        _createPrefabAction.performed += context => { _createPrefabInput = context.ReadValue<float>(); };
        _createPrefabAction.canceled += context => { _createPrefabInput = context.ReadValue<float>(); };

        _jerkAction.Enable();
        _moveAction.Enable();
        _fireAction.Enable();
        _createPrefabAction.Enable();
    }

    protected override void OnUpdate()
    {
        foreach (var input in SystemAPI.Query<RefRW<InputComponent>>())
        {
            input.ValueRW.Move = _moveInput;
            input.ValueRW.Fire = _fireInput;

            //рывок
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

            //Создание игрока
            if(_createPrefabInput != 0 && _createPrefabReload == 0)
            {
                input.ValueRW.CreatePrefab = _createPrefabInput;
                _createPrefabInput = 0;
                _createPrefabReload = 10f;
            }
            else if(_createPrefabInput == 0)
            {
                input.ValueRW.CreatePrefab = 0;
            }

            if(_createPrefabReload > 0)
            {
                _createPrefabReload -= SystemAPI.Time.DeltaTime;
            }
            else if (_createPrefabReload < 0)
                _createPrefabReload = 0;
        }
    }

    protected override void OnStopRunning()
    {
        _jerkAction.Disable();
        _moveAction.Disable();
        _fireAction.Disable();
        _createPrefabAction.Disable();
    }
}


