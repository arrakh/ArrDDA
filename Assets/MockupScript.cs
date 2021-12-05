using System;
using System.Collections;
using System.Collections.Generic;
using Arr.DDA;
using Arr.DDA.Script;
using Arr.DDA.Script.Evaluators;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MockupScript : MonoBehaviour
{
    [SerializeField] private ChannelObject channel;
    [SerializeField] private MetricObject progression;

    private AdaptParameter parameter = new AdaptParameter();

    public void AddSkill()
    {
        var rand = Random.Range(0.1f, 0.6f);
        progression.SetDelta(rand);
        parameter.isSuccess = Random.Range(0, 2) == 0;
        
        Debug.Log($"Added Skill by {rand} with param success {parameter.isSuccess}");
        
        channel.Evaluate(parameter);
    }

    private IEnumerator Start()
    {
        channel.Initialize();

        while (true)
        {
            AddSkill();
            yield return new WaitForSeconds(1f);
        }
    }
}
