using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class CharacterFsm {

    Character character;
    CharacterInput input;
    CharacterController controller;

    State state;

    bool dashSick = false;
    int jumpCount = 0;

    public enum State {
        Idle,
        Jumping,
        Dashing,
        Hanging,
    }

    public CharacterFsm(Character character) {
        this.character = character;
        input = character.input;
        controller = character.controller;
        state = State.Idle;
    }

    bool dash { get { return (!dashSick && input.dash); } }
    bool jump { get { return (jumpCount < 2 && input.jump); } }
    IEnumerator Dashing(float delayTime) {
        yield return new WaitForSeconds(delayTime);
        dashSick = false;
        EnterState(State.Idle);
    }

    public void Update() {
        switch (state) {
            default:
            case State.Idle:
                {
                    if (dash) {
                        EnterState(State.Dashing);
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
                    if (dash) {
                        EnterState(State.Dashing);
                        break;
                    }
                    if (jump) {
                        controller.Jump();
                        break;
                    }
                    controller.Move();
                    break;
                }

            case State.Dashing:
                {
                    break;
                }
            case State.Hanging:
                {
                    if (dash) {
                        EnterState(State.Dashing);
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


    public class Timer {

        float elapsed = 0f;
        public Timer(float time) {
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

            case State.Dashing:
                {
                    controller.Dash();
                    character.StartCoroutine(Dashing(1f));
                    dashSick = true;
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

            case State.Dashing:
                {
                    break;
                }
            case State.Hanging:
                {
                    break;
                }
        }
    }


}
