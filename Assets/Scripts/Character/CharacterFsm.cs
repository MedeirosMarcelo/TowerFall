using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class CharacterFsm {

    Character character;
    Rigidbody rigidBody;

    CharacterInput input;
    CharacterController controller;


    State state;



    public enum State {
        Idle,
        Jumping,
        Dodging,
        Hanging,
    }

    public CharacterFsm(Character character) {
        this.character = character;
        rigidBody = character.rigidbody;
        input = character.input;
        controller = character.controller;
        state = State.Idle;
    }


    // jump
    int jumpCount = 0;
    bool jump { get { return (jumpCount < 2 && input.jump); } }

    // dodge
    private float dodgeTime = 0.416f;
    private float dodgeSickTime = 0.75f - 0.416f;
    private bool dodgeSick = false;

    private bool dodge { get { return (!dodgeSick && input.dash); } }

    IEnumerator DodgeTimeout() {
        dodgeSick = true;
        yield return new WaitForSeconds(dodgeTime);
        EnterState(State.Idle);
    }
    IEnumerator DodgeSick() {
        yield return new WaitForSeconds(dodgeSickTime);
        dodgeSick = false;
    }

    // fsm
    public void Update(float delta) {
        switch (state) {
            default:
            case State.Idle:
                {
                    if (dodge) {
                        EnterState(State.Dodging);
                        break;
                    }
                    if (jump) {
                        EnterState(State.Jumping);
                        break;
                    }
                    controller.Move();
                    break;
                }
            case State.Jumping:
                {
                    if (dodge) {
                        EnterState(State.Dodging);
                        break;
                    }
                    if (jump) {
                        controller.Jump();
                        break;
                    }
                    controller.Move();
                    break;
                }

            case State.Dodging:
                {
                    break;
                }
            case State.Hanging:
                {
                    if (dodge) {
                        EnterState(State.Dodging);
                        break;
                    }
                    if (jump) {
                        EnterState(State.Jumping);
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
            case State.Idle:
                {
                    jumpCount = 0;
                    break;
                }
            case State.Jumping:
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
            case State.Hanging:
                {
                    break;
                }
        }
    }

    void ExitState() {
        switch (state) {
            default:
            case State.Idle:
                {
                    break;
                }
            case State.Jumping:
                {
                    break;
                }

            case State.Dodging:
                {
                    rigidBody.useGravity = true;
                    character.StartCoroutine(DodgeSick());
                    break;
                }
            case State.Hanging:
                {
                    break;
                }
        }
    }


}
