using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Lander : MonoBehaviour {
    private const float GRAVITY_SCALE_NORMAL = 0.8f;
    private const float FUEL_CAPACITY = 100f;
    private const float FUEL_CONSUMPTION_AMOUNT_NORMAL = 10f;
    private const float FUEL_CONSUMPTION_AMOUNT_CARGO  = 5f;


    public enum States {
        WaitingToStart,
        Normal,
        GameOver
    };

    public enum LandingTypes {
        Successful,
        Crashed,
    }

    public class LandingData : EventArgs {
        public LandingTypes landingType;
        public float landingSpeed;
        public float landingAngle;
        public float landingPadMultiplier;
    }

    private Rigidbody2D landerRigidBody2D;
    private LandingPad landingPad;

    //Events
    public event EventHandler OnNoForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler<LandingData> OnLanding;
    public event EventHandler<float> OnCoinPickup;
    public event EventHandler OnFuelPickup;

    //Lander variables
    public States State { get; private set; }
    public  float FuelAmount { get; private set; }
    private float force;
    private float turnSpeed;    
    private float softLandingDotVector;
    private float dotVector;
    private float softLandingCollisionMagnitude;

    //Conditions
    private bool collidedWithLandingPad;
    private bool collidedWithCargo;
    private bool collidedWithFuel;
    private bool collidedWithCoin;

    private void Awake() {
        landerRigidBody2D = GetComponent<Rigidbody2D>();
        
        SetState(States.WaitingToStart);
    }

    private void FixedUpdate() {
        if (FuelAmount <= 0) { 
            FuelAmount = 0;
            return; 
        }

        if (GameInput.Instance.IsUpActionPressed() || GameInput.Instance.IsLeftActionPressed() || GameInput.Instance.IsRightActionPressed()) {
            switch (State) {
                case States.GameOver:
                    return;
            }

            SetState(States.Normal);
            ConsumeFuel();
        } else {
            OnNoForce?.Invoke(this, EventArgs.Empty);
        }


        if (GameInput.Instance.IsUpActionPressed()) {
            OnUpForce?.Invoke(this, EventArgs.Empty);

            force = 700f;
            landerRigidBody2D.AddForce(transform.up * (force * Time.deltaTime));    
        }

        if (GameInput.Instance.IsLeftActionPressed()) {
            OnLeftForce?.Invoke(this, EventArgs.Empty);
            
            turnSpeed = 100f;
            landerRigidBody2D.AddTorque(turnSpeed * Time.deltaTime);
        }

        if (GameInput.Instance.IsRightActionPressed()) {
            OnRightForce?.Invoke(this, EventArgs.Empty);
            
            turnSpeed = -100f;
            landerRigidBody2D.AddTorque(turnSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        dotVector = Vector2.Dot(Vector2.up, transform.up);
        softLandingCollisionMagnitude = 3f;
        softLandingDotVector = 0.97f;

        collidedWithLandingPad = collision.gameObject.TryGetComponent(out landingPad);

        if ((collision.relativeVelocity.magnitude > softLandingCollisionMagnitude) || (dotVector < softLandingDotVector) || !collidedWithLandingPad) {
            OnLanding?.Invoke(this, new LandingData {
                landingType = LandingTypes.Crashed,
                landingSpeed = collision.relativeVelocity.magnitude,
                landingAngle = dotVector,
                landingPadMultiplier = 0
            } );
        }
        else if (collidedWithLandingPad) {
            OnLanding?.Invoke(this, new LandingData {
                landingType = LandingTypes.Successful,
                landingSpeed = collision.relativeVelocity.magnitude,
                landingAngle = dotVector,
                landingPadMultiplier = landingPad.Multiplier
            });
        }

        SetState(States.GameOver);
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collidedWithLandingPad) {
            collidedWithLandingPad = false;
            landingPad = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        collidedWithFuel = collision.gameObject.TryGetComponent(out Fuel fuel);
        collidedWithCoin = collision.gameObject.TryGetComponent(out Coin coin);
        collidedWithCargo = collision.gameObject.TryGetComponent(out Cargo cargo);

        if (collidedWithFuel) {
            if ((FuelAmount + fuel.GetFuelAmount()) > FUEL_CAPACITY) {
                FuelAmount = FUEL_CAPACITY;
            }
            else {
                FuelAmount += fuel.GetFuelAmount();
            }

            OnFuelPickup?.Invoke(this, System.EventArgs.Empty);
            fuel.DestroySelf();
        }

        if (collidedWithCoin) {
            OnCoinPickup?.Invoke(this, coin.GetCoinScore());
            coin.DestroySelf();
        }

        if (collidedWithCargo) {
            Debug.Log("Cargoo");
        }
    }

    private void ConsumeFuel() {
        FuelAmount -= FUEL_CONSUMPTION_AMOUNT_NORMAL * Time.deltaTime;
    }

    public float GetFuelAmount() {
        return FuelAmount;
    }

    public float GetFuelCapacity() {
        return FUEL_CAPACITY;
    }

    public float GetVelocityX() {
        return landerRigidBody2D.linearVelocityX;
    }

    public float GetVelocityY() {
        return landerRigidBody2D.linearVelocityY;
    }

    public void SetState(States state) {
        this.State = state;

        switch (this.State) {
            case States.WaitingToStart:
                FuelAmount = 100f;
                landerRigidBody2D.gravityScale = 0f;
                break;

            case States.Normal:
                landerRigidBody2D.gravityScale = GRAVITY_SCALE_NORMAL;
                break;
        }
    }
}