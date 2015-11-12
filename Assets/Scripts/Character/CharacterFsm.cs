using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class CharacterFsm {

    Character character;
    CharacterInput input;
    CharacterController controller;
    Rigidbody rigidBody;

    State state;

    public enum State {
        OnGround,  //  State, Update will check for  
        OnAir,
        Dodging,
        OnLedge,
    }

    public CharacterFsm(Character character, CharacterInput input, CharacterController controller) {
        this.character = character;
        this.input = input;
        this.controller = controller;

        rigidBody = character.rigidbody;
        state = State.OnGround;
    }


    // jump
    int jumpCount = 0;
    bool jump { get { return (jumpCount < 2 && input.jump); } }

    // dodge
    private float dodgeTime = 0.416f;
    private float dodgeSickTime = 0.75f - 0.416f;
    private bool dodgeSick = false;

    private bool dodge { get { return (!dodgeSick && input.dodge); } }

    IEnumerator DodgeTimeout() {
        dodgeSick = true;
        yield return new WaitForSeconds(dodgeTime);

        if (controller.isGrounded) {
            EnterState(State.OnGround);
            yield break;
        }
        if (controller.canGrabLedge) {
            EnterState(State.OnLedge);
            yield break;
        }
        EnterState(State.OnAir);
    }
    IEnumerator DodgeSick() {
        yield return new WaitForSeconds(dodgeSickTime);
        dodgeSick = false;
    }

    // fsm
    public void FixedUpdate(float delta) {
        switch (state) {
            default:
            case State.OnGround:
                {
                    if (dodge) {
                        EnterState(State.Dodging);
                        break;
                    }
                    if (jump) {
                        EnterState(State.OnAir);
                        break;
                    }
                    controller.Move();
                    break;
                }
            case State.OnAir:
                {
                    if (dodge) {
                        EnterState(State.Dodging);
                        break;
                    }
                    if (jump) {
                        controller.Jump();
                        jumpCount++;
                        break;
                    }
                    controller.AirMove();

                    if (controller.isGrounded) {
                        EnterState(State.OnGround);
                        break;
                    }
                    if (controller.canGrabLedge) {
                        EnterState(State.OnLedge);
                        break;
                    }
                    break;
                }

            case State.Dodging:
                {
                    break;
                }
            case State.OnLedge:
                {
                    if (dodge) {
                        EnterState(State.Dodging);
                        break;
                    }
                    if (jump) {
                        EnterState(State.OnAir);
                        break;
                    }
                    break;
                }
        }
    }



    void EnterState(State newState) {
        Debug.Log("Enter=" + newState + " Exit=" + state);

        ExitState();
        state = newState;

        switch (state) {
            default:
            case State.OnGround:
                {
                    jumpCount = 0;
                    break;
                }
            case State.OnAir:
                {
                    controller.Jump();
                    jumpCount += 1;
                    break;
                }
            case State.Dodging:
                {
                    rigidBody.useGravity = false;
                    controller.Dodge();
                    character.StartCoroutine(DodgeTimeout());
                    break;
                }
            case State.OnLedge:
                {
                    break;
                }
        }
    }

    void ExitState() {
        switch (state) {
            default:
            case State.OnGround:
                {
                    break;
                }
            case State.OnAir:
                {
                    break;
                }

            case State.Dodging:
                {
                    rigidBody.useGravity = true;
                    character.StartCoroutine(DodgeSick());
                    break;
                }
            case State.OnLedge:
                {
                    break;
                }
        }
    }


}
