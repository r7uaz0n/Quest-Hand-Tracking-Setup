﻿using ByTheTale.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question3 : State
{
    public override void Enter()
    {
        Quiz.quiz.text.text = "What do you do when exiting the museum?";

        Quiz.quiz.Question.SetActive(true);
    }

    public override void Exit()
    {
        Quiz.quiz.Question.SetActive(false);
    }

    public override void Execute()
    {
        if (Quiz.quiz.currentPose.name == HandPoseName.Three)
        {
            Quiz.quiz.AnswerCorrect.Invoke();
            Quiz.quiz.currentPose = new HandPose();
            Quiz.quiz.ChangeState<Welcome>();
        }
        else if (Quiz.quiz.currentPose.name != HandPoseName.None)
        {
            Quiz.quiz.AnswerWrong.Invoke();
        }
    }
}
