using ByTheTale.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Quiz : MachineBehaviour
{
    static Quiz Instance;
    public static Quiz quiz { get { return Instance; } }

    public TextMeshProUGUI text;

    public GameObject Welcome, Selection, Question;

    [SerializeField]
    public HandPose currentPose = new HandPose();

    public UnityEvent AnswerCorrect;
    public UnityEvent AnswerWrong;
    public UnityEvent QuestionPopup;

    public override void AddStates()
    {
        AddState<Welcome>();
        AddState<ChooseQuestion>();
        AddState<Question1>();
        AddState<Question2>();
        AddState<Question3>();
        SetInitialState<Welcome>();
    }

    private void Awake()
    {
        Instance = this;
    }

    public override void Start()
    {
        base.Start();
        ChangeState<Welcome>();
    }

    private void Update()
    {
        base.Update();
    }

    public void OnPoseEnter(HandPose pose)
    {
        currentPose = pose;
    }

    public void OnPoseExit(HandPose pose)
    {
        currentPose = new HandPose();
    }
}
