using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pkratten_OculusHands : pkratten_HandsBase
{
    public OVRSkeleton skeletonRight;
    public OVRSkeleton skeletonLeft;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        UpdateTransforms(skeletonRight, HandRight);
        UpdateTransforms(skeletonLeft, HandLeft);
    }

    void UpdateTransforms(OVRSkeleton skeleton, List<Transform> hand)
    {
        foreach (var bone in skeleton.Bones)
        {
            int i = -1;
            switch (bone.Id)
            {
                case OVRSkeleton.BoneId.Invalid:
                    break;
                //case OVRSkeleton.BoneId.Hand_Start:
                    break;
                case OVRSkeleton.BoneId.Hand_WristRoot:
                    i = 0;
                    break;
                case OVRSkeleton.BoneId.Hand_ForearmStub:
                    break;
                case OVRSkeleton.BoneId.Hand_Thumb0:
                    break;
                case OVRSkeleton.BoneId.Hand_Thumb1:
                    break;
                case OVRSkeleton.BoneId.Hand_Thumb2:
                    i = 1;
                    break;
                case OVRSkeleton.BoneId.Hand_Thumb3:
                    i = 2;
                    break;
                case OVRSkeleton.BoneId.Hand_Index1:
                    i = 4;
                    break;
                case OVRSkeleton.BoneId.Hand_Index2:
                    i = 5;
                    break;
                case OVRSkeleton.BoneId.Hand_Index3:
                    i = 6;
                    break;
                case OVRSkeleton.BoneId.Hand_Middle1:
                    i = 8;
                    break;
                case OVRSkeleton.BoneId.Hand_Middle2:
                    i = 9;
                    break;
                case OVRSkeleton.BoneId.Hand_Middle3:
                    i = 10;
                    break;
                case OVRSkeleton.BoneId.Hand_Ring1:
                    i = 12;
                    break;
                case OVRSkeleton.BoneId.Hand_Ring2:
                    i = 13;
                    break;
                case OVRSkeleton.BoneId.Hand_Ring3:
                    i = 14;
                    break;
                case OVRSkeleton.BoneId.Hand_Pinky0:
                    break;
                case OVRSkeleton.BoneId.Hand_Pinky1:
                    i = 16;
                    break;
                case OVRSkeleton.BoneId.Hand_Pinky2:
                    i = 17;
                    break;
                case OVRSkeleton.BoneId.Hand_Pinky3:
                    i = 18;
                    break;
                //case OVRSkeleton.BoneId.Hand_MaxSkinnable:
                //break;
                case OVRSkeleton.BoneId.Hand_ThumbTip:
                    i = 3;
                    break;
                case OVRSkeleton.BoneId.Hand_IndexTip:
                    i = 7;
                    break;
                case OVRSkeleton.BoneId.Hand_MiddleTip:
                    i = 11;
                    break;
                case OVRSkeleton.BoneId.Hand_RingTip:
                    i = 15;
                    break;
                case OVRSkeleton.BoneId.Hand_PinkyTip:
                    i = 19;
                    break;
                //case OVRSkeleton.BoneId.Hand_End:
                    //break;
                case OVRSkeleton.BoneId.Max:
                    break;
                default:
                    break;
            }

            if (i == -1)
            {
                continue;
            }

            hand[i].position = bone.Transform.position;
            hand[i].rotation = Quaternion.LookRotation(bone.Transform.right, bone.Transform.up);
        }
    }

    // Update is called once per frame
    void Update()
    {
        FixedUpdate();
    }
}
