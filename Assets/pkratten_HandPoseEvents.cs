using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public struct PoseAnticipateArgs
{
    public PoseAnticipateArgs(HandPose pose, float percent)
    {
        Pose = pose;
        Percent = percent;
    }
    public HandPose Pose;
    public float Percent;
}

[Serializable]
public class HandPoseEvent : UnityEvent<HandPose> { }
[Serializable]
public class PoseAnticipateEvent : UnityEvent<PoseAnticipateArgs> { }

public class pkratten_HandPoseEvents : MonoBehaviour
{
    public pkratten_HandPose HandPose;
    public float delay = 1.5f;

    [SerializeField]
    public PoseAnticipateEvent PoseAnticipate;
    public HandPoseEvent PoseEnter;
    public HandPoseEvent PoseStay;
    public HandPoseEvent PoseExit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    HandPose previous;
    float time = 0;
    bool pose = false;

    // Update is called once per frame
    void Update()
    {
        if(HandPose.currentPose.name == previous.name)
        {
            time += Time.deltaTime;
            if(time > delay)
            {
                if (pose == false)
                {
                    PoseEnter.Invoke(HandPose.currentPose);
                    pose = true;
                }
                else PoseStay.Invoke(HandPose.currentPose);
            }
            else
            {
                PoseAnticipateArgs args = new PoseAnticipateArgs(HandPose.currentPose, time/delay);
                PoseAnticipate.Invoke(args);
            }
        }
        else
        {
            if (pose) PoseExit.Invoke(previous);
            previous = new HandPose();
            time = 0;
            pose = false;
            PoseAnticipate.Invoke(new PoseAnticipateArgs(previous, 0));
        }

        previous = HandPose.currentPose;
    }
}
