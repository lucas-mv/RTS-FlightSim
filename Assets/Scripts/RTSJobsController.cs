using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RTSJobsController : MonoBehaviour
{
    const int TOTAL_FRAME_TIME = 100;

    int _currentFrame = -1;
    int _hyperPeriod = 0;
    List<RTSJob> _jobs = new List<RTSJob>();
    Dictionary<string, List<int>> _skippedJobs;

    public void SetupJob(RTSJob job)
    {
        _jobs.Add(job);
        _jobs = _jobs.OrderBy(j => j.Priority).ToList();

        _hyperPeriod = Helpers.LeastCommonMultiple(_jobs.Select(j => j.FramePeriod).ToArray());
    }

    public string RunJobs()
    {
        if (!_jobs.Any()) return string.Empty;
        if (_currentFrame == -1 || _currentFrame > _hyperPeriod)
        {
            _skippedJobs = new Dictionary<string, List<int>>();
            foreach(var job in _jobs) _skippedJobs[job.Name] = new List<int>();
            _currentFrame = 1;
        }

        int frameRemainingTime = TOTAL_FRAME_TIME;
        foreach(var job in _jobs)
        {
            if (_currentFrame % job.FramePeriod != 0) continue;

            if(frameRemainingTime - job.Duration < 0)
            {
                _skippedJobs[job.Name].Add(_currentFrame);
                continue;
            }

            frameRemainingTime -= job.Duration;
            job.Execute.Invoke();
        }

        string skippedJobsText = _currentFrame == _hyperPeriod ? BuildSkippedJobsText() : string.Empty;
        _currentFrame++;
        return skippedJobsText;
    }

    string BuildSkippedJobsText()
    {
         var skippedJobsReport =  _skippedJobs
            .Where(sj => sj.Value.Any())
            .Select(sj => sj.Key + " skipped at frames " + string.Join(", ", sj.Value));

        return skippedJobsReport.Any() ?
            string.Join("\n", skippedJobsReport) :
            "Jobs correctly configured, no jobs were skipped during the hyperperiod";
    }

    public string GetHyperperiodText()
    {
        return "<b>Hyperperiod:</b> " + _hyperPeriod.ToString() + " frames";
    }
}
