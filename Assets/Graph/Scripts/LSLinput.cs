using System.Collections;
using UnityEngine;
using Assets.LSL4Unity.Scripts;
using Assets.LSL4Unity.Scripts.AbstractInlets;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Window_Graph;

public class LSLinput : InletFloatSamples
{
    public bool useX;
    public bool useY;
    public bool useZ;
    public int pointLimit;

    private bool pullSamplesContinuously = false;
    private List<float> First_para;
    private List<float> Second_para;
    private List<float> Third_para;
    private List<float> First_line;
    private List<float> Second_line;
    private List<float> Third_line;



    void Start()
    {
        // [optional] call this only, if your gameobject hosting this component
        // got instantiated during runtime
        Debug.Log("start recording...");

        registerAndLookUpStream();
        First_para = new List<float>() { 5, 98, 56, 45, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33, 19, 88, 45, 65, 19, 22, 44 };
        Second_para = new List<float>() { 5, 98, 56, 45, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33, 19, 88, 45, 65, 19, 22, 44 };
        Third_para = new List<float>() { 5, 98, 56, 45, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33, 19, 88, 45, 65, 19, 22, 44 };

    }

    protected override bool isTheExpected(LSLStreamInfoWrapper stream)
    {
        // the base implementation just checks for stream name and type
        var predicate = base.isTheExpected(stream);
        // add a more specific description for your stream here specifying hostname etc.
        //predicate &= stream.HostName.Equals("Expected Hostname");
        return predicate;
    }

    /// <summary>
    /// Override this method to implement whatever should happen with the samples...
    /// IMPORTANT: Avoid heavy processing logic within this method, update a state and use
    /// coroutines for more complexe processing tasks to distribute processing time over
    /// several frames
    /// </summary>
    /// <param name="newSample"></param>
    /// <param name="timeStamp"></param>
    protected override void Process(float[] newSample, double timeStamp)
    {
        //Assuming that a sample contains at least 3 values for x,y,z
        float x = useX ? newSample[0] : 1;
        float y = useY ? newSample[1] : 1;
        float z = useZ ? newSample[2] : 1;

        // we map the data to the scale factors
        // var targetScale = new Vector3(x, y, z);

        // apply the rotation to the target transform
        // targetTransform.localScale = targetScale;
        // targetText.text = string.Format("hr: {0}bpm\nrr: {1}ms\nstd dev rr: {2}ms", 1, 1, 1);

        First_para.Add(x);
        Second_para.Add(y);
        Third_para.Add(z);
    }

    protected override void OnStreamAvailable()
    {
        pullSamplesContinuously = true;
    }

    protected override void OnStreamLost()
    {
        pullSamplesContinuously = false;
    }

    private void Update()
    {
        if (pullSamplesContinuously)
        {
            pullSamples();
            First_line = First_para.GetRange(First_para.Count - pointLimit - 1, pointLimit);
            ShowGraph(First_line);
        }
    }
}