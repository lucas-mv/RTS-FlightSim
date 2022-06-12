using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RTSJobsController : MonoBehaviour
{
    [SerializeField] int _totalTime;

    List<RTSJob> _jobs = new List<RTSJob>();

    public void SetupJob(RTSJob job)
    {
        _jobs.Add(job);
        _jobs = _jobs.OrderBy(j => j.Priority).ToList();
    }

    public string RunJobs()
    {
        if (!_jobs.Any()) return string.Empty;

        List<string> skippedJobs = new List<string>();
        int remainingTime = _totalTime;
        foreach(var job in _jobs)
        {
            if(remainingTime - job.Duration < 0)
            {
                Debug.Log("Skipping job " + job.Name +" due to duration being longer than remaining time");
                skippedJobs.Add(job.Name);
                continue;
            }

            remainingTime -= job.Duration;
            job.Execute.Invoke();
        }

        return string.Join(", ", skippedJobs.ToArray());
    }
}
