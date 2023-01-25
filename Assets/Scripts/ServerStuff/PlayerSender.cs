using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSender : NetworkBehaviour
{
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        Debug.Log(1);
        if (!isServer)
        {
            Debug.Log(2);
            var s = StartGame.singleton;
            CmdSaveNewData(s.pointsCount, s.secondsPerLevel, s.plantsPlaced, s.alpacsCount);
        }
    }

    [Command]
    public void CmdSaveNewData(int points, float time, int plantsPlaced, int alpacsCount)
    {
        StartGame.singleton.SaveNewData(points, time, plantsPlaced, alpacsCount);
        Debug.Log($"Points: {points}; Seconds: {time}; Plants: {plantsPlaced}; Alpacas: {alpacsCount}");
    }
}
