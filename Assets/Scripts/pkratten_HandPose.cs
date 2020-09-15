using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.XR;

[Serializable]
public struct HandPose
{
    public HandPoseName name;
    [HideInInspector]
    public List<Vector3> localPositions;
    [HideInInspector]
    public List<Quaternion> localRotations;
    [HideInInspector]
    public List<Vector3> Positions;
    [HideInInspector]
    public List<Quaternion> Rotations;
    [HideInInspector]
    public List<Vector3> relativePositions;
    [HideInInspector]
    public List<Vector3> relativeDirections;
    public Handedness handedness;
}

public enum HandPoseName
{
    None,
    New,
    One,
    Two,
    Three,
    Four,
    Five,
    Stop,
    Open,
    ThumbsUp,
    Pinch,
    PinchGrab
}

[Flags]
public enum Handedness : byte
{
    /// <summary>
    /// No hand specified by the SDK for the controller
    /// </summary>
    None = 0 << 0,
    /// <summary>
    /// The controller is identified as being provided in a Left hand
    /// </summary>
    Left = 1 << 0,
    /// <summary>
    /// The controller is identified as being provided in a Right hand
    /// </summary>
    Right = 1 << 1,
    /// <summary>
    /// The controller is identified as being either left and/or right handed.
    /// </summary>
    Both = Left | Right,
    /// <summary>
    /// Reserved, for systems that provide alternate hand state.
    /// </summary>
    Other = 1 << 2,
    /// <summary>
    /// Global catchall, used to map actions to any controller (provided the controller supports it)
    /// </summary>
    /// <remarks>Note, by default the specific hand actions will override settings mapped as both</remarks>
    Any = Other | Left | Right,
}

public enum DetectionMethod
{
    RelativePositions,
    RelativeDirections
}

public class pkratten_HandPose : MonoBehaviour
{
    public HandPose currentPose;
    public float Tolerance;
    public pkratten_HandsBase Hands;
    public DetectionMethod detectionMethod;
    public int ReferenceIndex;
    public Record record = Record.None;
    public List<HandPose> Poses;

    public enum Record
    {
        None,
        Right,
        Left
    }

    Handedness ComparePose(HandPose pose)
    {
        Handedness handedness = Handedness.None;

        int countR = 0;
        int countL = 0;

        for (int i = 0; i < pose.Positions.Count; i++)
        {
            switch (detectionMethod)
            {
                case DetectionMethod.RelativePositions:
                    Vector3 relativePosition = Hands.HandRight[ReferenceIndex].transform.InverseTransformPoint(Hands.HandRight[i].position);
                    if (Vector3.Distance(relativePosition, pose.relativePositions[i]) < Tolerance) countR++;
                    relativePosition = Vector3.Reflect(relativePosition, Vector3.right);
                    if (Vector3.Distance(relativePosition, pose.relativePositions[i]) < Tolerance) countL++;
                    break;
                case DetectionMethod.RelativeDirections:
                    Vector3 relativeDirection = Hands.HandRight[ReferenceIndex].transform.InverseTransformDirection(Hands.HandRight[i].right);
                    if (Vector3.Distance(relativeDirection, pose.relativeDirections[i]) < Tolerance) countR++;
                    relativeDirection = Hands.HandLeft[ReferenceIndex].transform.InverseTransformDirection(Hands.HandLeft[i].right);
                    if (Vector3.Distance(relativeDirection, pose.relativeDirections[i]) < Tolerance) countL++;
                    break;
            }

            
        }

        if (countR == pose.Positions.Count) handedness = Handedness.Right;
        if (countL == pose.Positions.Count) handedness = Handedness.Left;

        return handedness;
    }

    void SavePose()
    {
        if (record == Record.None) return;

        HandPose pose = new HandPose();
        pose.name = HandPoseName.New;
        pose.localPositions = new List<Vector3>();
        pose.localRotations = new List<Quaternion>();
        pose.Positions = new List<Vector3>();
        pose.Rotations = new List<Quaternion>();
        pose.relativePositions = new List<Vector3>();
        pose.handedness = Handedness.None;

        List<Transform> hand;
        if (record == Record.Right) hand = Hands.HandRight;
        else hand = Hands.HandLeft;

        for (int i = 0; i < hand.Count; i++)
        {
            Transform current = hand[i];
            pose.Positions.Add(current.position);
            pose.Rotations.Add(current.rotation);
            pose.localPositions.Add(current.localPosition);
            pose.localRotations.Add(current.localRotation);
            pose.relativePositions.Add(hand[ReferenceIndex].transform.InverseTransformPoint(current.position));
            pose.relativeDirections.Add(hand[ReferenceIndex].transform.InverseTransformDirection(current.right));
        }
        if(record == Record.Left)
        {
            for (int i = 0; i < pose.relativePositions.Count; i++)
            {
                pose.relativePositions[i] = Vector3.Reflect(pose.relativePositions[i], Vector3.right);
            }
        }

        Poses.Add(pose);
    }

    HandPose None = new HandPose();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SavePose();
        }

        foreach (var pose in Poses)
        {
            Handedness handedness = ComparePose(pose);
            if(handedness!= Handedness.None)
            {
                currentPose = pose;
                currentPose.handedness = handedness;
                return;
            }
        }

        currentPose = None;
    }
}
