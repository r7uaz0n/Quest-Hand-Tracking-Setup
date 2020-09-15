using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pkratten_LeapHands : pkratten_HandsBase
{
    public List<Transform> skeletonRight;
    public List<Transform> skeletonLeft;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        UpdateTransforms(skeletonRight, HandRight, true);
        UpdateTransforms(skeletonLeft, HandLeft, false);
    }

    void UpdateTransforms(List<Transform> skeleton, List<Transform> hand, bool inverse)
    {
        for (int i = 0; i < skeleton.Count; i++)
        {
            hand[i].position = skeleton[i].position;
            if (inverse) hand[i].rotation = Quaternion.LookRotation(-skeleton[i].forward, -skeleton[i].up);
            else hand[i].rotation = Quaternion.LookRotation(skeleton[i].forward, skeleton[i].up);
        }
    }

    // Update is called once per frame
    void Update()
    {
        FixedUpdate();
    }
}
