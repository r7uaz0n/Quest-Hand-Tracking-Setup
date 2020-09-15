using ByTheTale.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Welcome : State
{
    public override void Enter()
    {
        Quiz.quiz.text.text = "Thumbs Up to start the Quiz!";

        Quiz.quiz.Welcome.SetActive(true);
    }

    public override void Exit()
    {
        Quiz.quiz.Welcome.SetActive(false);
    }

    public override void Execute()
    {
        if(Quiz.quiz.currentPose.name == HandPoseName.ThumbsUp)
        {
            Quiz.quiz.ChangeState<ChooseQuestion>();
        }
    }
}
