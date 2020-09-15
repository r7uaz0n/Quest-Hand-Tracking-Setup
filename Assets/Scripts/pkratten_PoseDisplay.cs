using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class pkratten_PoseDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPoseAnticipate(PoseAnticipateArgs args)
    {
        text.text = args.Percent.ToString();
    }

    public void OnPoseEnter(HandPose pose)
    {
        text.text = Enum.GetName(typeof(HandPoseName), pose.name);
        text.color = Color.green;
    }

    public void OnPoseExit(HandPose pose)
    {
        text.text = "";
        text.color = Color.white;
    }

}
