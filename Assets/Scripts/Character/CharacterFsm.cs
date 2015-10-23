using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class CharacterFsm {

    public enum State {
        Idle,
        Run,
        Shoot,
        Jump,
        Crouch,
        Dash,
        GrabLedge,
        Climb,
        SlideDown,
        Fall,
        Die
    }

    Character character;
    public State state;

    public CharacterFsm(Character character) {
        this.character = character;
        state = State.Idle;
    }


    void Update() {
        switch (state) {
            case State.Idle:
                break;
            case State.Run:
                break;
            case State.Shoot:
                break;
            case State.Jump:
                break;
            case State.Crouch:
                break;
            case State.Dash:
                break;
            case State.GrabLedge:
                break;
            case State.Climb:
                break;
            case State.SlideDown:
                break;
            case State.Fall:
                break;
            case State.Die:
                break;
        }
    }

    void EnterState(State newState) {
        ExitState();
        state = newState;

        switch (state) {
            case State.Idle:
                break;
            case State.Run:
                break;
            case State.Shoot:
                break;
            case State.Jump:
                break;
            case State.Crouch:
                break;
            case State.Dash:
                break;
            case State.GrabLedge:
                break;
            case State.Climb:
                break;
            case State.SlideDown:
                break;
            case State.Fall:
                break;
            case State.Die:
                break;
        }
    }

    void ExitState() {
        switch (state) {
            case State.Idle:
                break;
            case State.Run:
                break;
            case State.Shoot:
                break;
            case State.Jump:
                break;
            case State.Crouch:
                break;
            case State.Dash:
                break;
            case State.GrabLedge:
                break;
            case State.Climb:
                break;
            case State.SlideDown:
                break;
            case State.Fall:
                break;
            case State.Die:
                break;
        }
    }


}
