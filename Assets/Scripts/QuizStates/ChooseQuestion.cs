using ByTheTale.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseQuestion : State
{    public override void Enter()
    {
        Quiz.quiz.text.text = "Choose a Question!";

        Quiz.quiz.Selection.SetActive(true);
    }

    public override void Exit()
    {
        Quiz.quiz.Selection.SetActive(false);
    }

    public override void Execute()
    {
        switch (Quiz.quiz.currentPose.name)
        {
            case HandPoseName.None:
                break;
            case HandPoseName.New:
                break;
            case HandPoseName.One:
                Quiz.quiz.currentPose = new HandPose();
                Quiz.quiz.ChangeState<Question1>();
                break;
            case HandPoseName.Two:
                Quiz.quiz.currentPose = new HandPose();
                Quiz.quiz.ChangeState<Question2>();
                break;
            case HandPoseName.Three:
                Quiz.quiz.currentPose = new HandPose();
                Quiz.quiz.ChangeState<Question3>();
                break;
            case HandPoseName.Four:
                break;
            case HandPoseName.Five:
                break;
            case HandPoseName.ThumbsUp:
                break;
            case HandPoseName.Pinch:
                break;
            case HandPoseName.PinchGrab:
                break;
            default:
                break;
        }
    }
}
