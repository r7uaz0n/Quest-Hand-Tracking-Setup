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
            switch (bone.Id)
            {
                case OVRSkeleton.BoneId.Invalid:
                    break;
                //case OVRSkeleton.BoneId.Hand_Start:
                    break;
                case OVRSkeleton.BoneId.Hand_WristRoot:
                    hand[0].position = bone.Transform.position;
                    hand[0].rotation = bone.Transform.rotation;
                    break;
                case OVRSkeleton.BoneId.Hand_ForearmStub:
                    break;
                case OVRSkeleton.BoneId.Hand_Thumb0:
                    break;
                case OVRSkeleton.BoneId.Hand_Thumb1:
                case OVRSkeleton.BoneId.Hand_Thumb2:
                case OVRSkeleton.BoneId.Hand_Thumb3:
                case OVRSkeleton.BoneId.Hand_Index1:
                case OVRSkeleton.BoneId.Hand_Index2:
                case OVRSkeleton.BoneId.Hand_Index3:
                case OVRSkeleton.BoneId.Hand_Middle1:
                case OVRSkeleton.BoneId.Hand_Middle2:
                case OVRSkeleton.BoneId.Hand_Middle3:
                case OVRSkeleton.BoneId.Hand_Ring1:
                case OVRSkeleton.BoneId.Hand_Ring2:
                case OVRSkeleton.BoneId.Hand_Ring3:
                case OVRSkeleton.BoneId.Hand_Pinky0:
                case OVRSkeleton.BoneId.Hand_Pinky1:
                case OVRSkeleton.BoneId.Hand_Pinky2:
                case OVRSkeleton.BoneId.Hand_Pinky3:
                //case OVRSkeleton.BoneId.Hand_MaxSkinnable:
                    //break;
                case OVRSkeleton.BoneId.Hand_ThumbTip:
                case OVRSkeleton.BoneId.Hand_IndexTip:
                case OVRSkeleton.BoneId.Hand_MiddleTip:
                case OVRSkeleton.BoneId.Hand_RingTip:
                case OVRSkeleton.BoneId.Hand_PinkyTip:
                    hand[(int)bone.Id - 2].position = bone.Transform.position;
                    hand[(int)bone.Id - 2].rotation = bone.Transform.rotation;
                    break;
                //case OVRSkeleton.BoneId.Hand_End:
                    //break;
                case OVRSkeleton.BoneId.Max:
                    break;
                default:
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        FixedUpdate();
    }
}
