using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;

public class StartGame : MonoBehaviour
{
    [SerializeField] NetworkManager manager;
    public static StartGame singleton;
    //private string url = "https://alpaclands.ru/server_getter.php";
    private string url = "";

    private List<int> topScoreDay = new List<int>();
    private List<int> topScoreWeek = new List<int>();
    private List<int> topScoreMonth = new List<int>();
    private List<int> topScoreAllTime = new List<int>();

    [SerializeField] private long topSeconds;
    [SerializeField] private long topHours;
    [SerializeField] private long topPoints;
    [SerializeField] private long topGames;
    [SerializeField] private long topAlpacas;
    [SerializeField] private long topPlants;

    public int DebugAdd;
    public bool AddValue;


    public int pointsCount;
    public float secondsPerLevel;
    public int plantsPlaced;
    public int alpacsCount;

    private void Awake()
    {
        singleton = this;
    }

    private void Update()
    {
        if (AddValue) {
            AddValue = false;
            AddNewTopScore(DebugAdd);
        }
    }

    void Start()
    {
        var i = Environment.GetCommandLineArgs();

        foreach (var c in i)
        {
            if (c.Equals("-serverClient"))
            {
                manager.StartServer();
                StartCoroutine(StartServerLoadTops());
                return;
            }
        }

        //StartCoroutine(SendTopDataToPHPToSQL(new DataMessageTop() { games = "1", total_time = "2", point = "3", alpaca = "4", plants = "5" }));
        //StartCoroutine(GetTopDataFromPHP());
        SceneManager.LoadScene(1);


    }

    IEnumerator StartServerLoadTops() {
        yield return GetTopScoreFromPHP();
        yield return GetTopDataFromPHP();
        yield return ClearTableEveryDay();
    }

    IEnumerator ClearTableEveryDay() 
    {
        while(true) 
        {
            topScoreMonth = new List<int>();
            for (var j = 0; j < 4; j++)
            {
                topScoreWeek = new List<int>();
                for (var i = 0; i < 7; i++)
                {
                    topScoreDay = new List<int>();
                    yield return ResendAllScore();
                    yield return new WaitForSeconds(86400);
                }
            }
        }
    }

    public void SaveNewData()
    {
        pointsCount = ProgressBar.singleton.PointsNow;
        secondsPerLevel = ProgressBar.singleton.T;
        plantsPlaced = PlantPlacer.singleton.PlantPlaced;
        alpacsCount = AlpacasManager.singleton.AlpacasCountNow;

        //Debug.Log($"Points: {pointsCount}; Seconds: {secondsPerLevel}; Plants: {plantsPlaced}; Alpacas: {alpacsCount}");

        if (KeyManager.GetKey("Legacy", 0) == 0)
            return;

        StartCoroutine(SendResultsToServer(pointsCount, secondsPerLevel, plantsPlaced, alpacsCount));

    }

    IEnumerator SendResultsToServer(int pointsCount, float secondsPerLevel, int plantsPlaced, int alpacsCount)
    {
        manager.StartClient();
        yield return new WaitForSeconds(5);

        //if (!manager.isNetworkActive)
        //    yield break;

        yield return new WaitForSeconds(5);
        manager.StopClient();
    }

    public void SaveNewData(int points, float time, int plantsPlaced, int alpacsCount)
    {
        topPoints = topPoints + points;
        topSeconds = topSeconds + Mathf.RoundToInt(time);
        topPlants = topPlants + plantsPlaced;
        topAlpacas = topAlpacas + alpacsCount;
        topGames = topGames + 1;

        if (topSeconds > 3600)
        {
            topHours += (topSeconds / 3600);
            topSeconds = 0;
        }

        StartCoroutine(SendTopDataToPHPToSQL(new DataMessageTop()
        {
            games = topGames.ToString(),
            total_time = topHours.ToString(),
            point = topPoints.ToString(),
            alpaca = topAlpacas.ToString(),
            plants = topPlants.ToString()
        }
        ));

        AddNewTopScore(points);
    }

    public void AddNewTopScore(int newScore) {
        var isDay = AddNewTopScore(topScoreDay, newScore);
        var isWeek = AddNewTopScore(topScoreWeek, newScore);
        var isMonth = AddNewTopScore(topScoreMonth, newScore);
        var isAllTime = AddNewTopScore(topScoreAllTime, newScore);

        if (isDay || isWeek || isMonth || isAllTime)
        {
            StartCoroutine(ResendAllScore());
        }
    }

    private string GetScoreFromList(List<int> list, int pos) {
        if(list.Count > pos)
            return list[pos].ToString();
        return "0";
    }

    private bool AddNewTopScore(List<int> list, int newScore) {
        var res = false;

        if (list.Count < 10 || list[9] < newScore)
        {
            list.Add(newScore);
            list.Sort();
            list.Reverse();
            if (list.Count > 10)
            {
                list.RemoveAt(10);
            }

            res = true;
        }

        return res;
    }

    IEnumerator ResendAllScore() { 
        yield return ClearTopScoreFromPHP();
        for (var j = 0; j < 10; j++)
        {
            yield return SendTopScoreToPHPToSQL(new DataMessageTopScore() { 
                day = GetScoreFromList(topScoreDay, j), 
                week = GetScoreFromList(topScoreWeek, j), 
                month = GetScoreFromList(topScoreMonth, j), 
                all_time = GetScoreFromList(topScoreAllTime, j)
            });
        }
    }

    private IEnumerator SendUserDataToPHPToSQL(DataMessage data)
    {
        WWWForm form = new WWWForm();
        //form.AddField("command", "set_user_data");
        //
        //form.AddField("point", data.point);
        //form.AddField("total_time", data.total_time);
        //form.AddField("plants", data.plants);
        //form.AddField("alpaca", data.alpaca);

        WWW www = new WWW(url, form);
        yield return StartCoroutine(WaitForResquet(www));
    }

    private IEnumerator SendTopDataToPHPToSQL(DataMessageTop data)
    {
        WWWForm form = new WWWForm();
        //form.AddField("command", "set_top_data");
        //
        //form.AddField("games", data.games);
        //form.AddField("point", data.point);
        //form.AddField("total_time", data.total_time);
        //form.AddField("plants", data.plants);
        //form.AddField("alpaca", data.alpaca);

        WWW www = new WWW(url, form);
        yield return StartCoroutine(WaitForResquet(www));
    }
    private IEnumerator SendTopScoreToPHPToSQL(DataMessageTopScore data)
    {
        WWWForm form = new WWWForm();
        //form.AddField("command", "set_top_score");
        //
        //form.AddField("day", data.day);
        //form.AddField("week", data.week);
        //form.AddField("month", data.month);
        //form.AddField("all_time", data.all_time);

        WWW www = new WWW(url, form);
        yield return StartCoroutine(WaitForResquet(www));
    }

    private IEnumerator GetTopScoreFromPHP()
    {
        WWWForm form = new WWWForm();
        //form.AddField("command", "get_top_score");

        WWW www = new WWW(url, form);
        yield return StartCoroutine(WaitForResquet(www));

        var text = www.text;

        var topPoses = text.Split(',');
        foreach (var e in topPoses) {
            if (e == "")
                break;
            var values = e.Split(' ');
            topScoreDay.Add(int.Parse(values[0]));
            topScoreWeek.Add(int.Parse(values[1]));
            topScoreMonth.Add(int.Parse(values[2]));
            topScoreAllTime.Add(int.Parse(values[3]));
        }
    }

    private IEnumerator GetTopDataFromPHP()
    {
        WWWForm form = new WWWForm();
        //form.AddField("command", "get_top_data");

        WWW www = new WWW(url, form);
        yield return StartCoroutine(WaitForResquet(www));

        var text = www.text;
        //Hours, Islands, Points, Plants, Alpacas

        var values = text.Split(' ');
        Debug.Log(text);
        topHours = int.Parse(values[0]);
        topGames = int.Parse(values[1]);
        topPoints = int.Parse(values[2]);
        topPlants = int.Parse(values[3]);
        topAlpacas = int.Parse(values[4]);
    }

    private IEnumerator ClearTopScoreFromPHP()
    {
        WWWForm form = new WWWForm();
        //form.AddField("command", "clear_top_score");

        WWW www = new WWW(url, form);
        yield return StartCoroutine(WaitForResquet(www));
    }


    private IEnumerator WaitForResquet(WWW www)
    {
        yield return www;
        Debug.Log(www.text);
    }


    public struct DataMessage
    {
        public string games;
        public string point;
        public string total_time;
        public string plants;
        public string alpaca;

        //    public override string ToString()
        //    {
        //        return $"<{time}> {name} : {text}";
        //    }
        //
        //    public static DataMessage Parse(string info)
        //    {
        //        var sp = info.Split('"');
        //
        //        return new DataMessage() { time = sp[3], name = sp[7], text = sp[11] };
        //    }
    }

    public struct DataMessageTop
    {
        public string games;
        public string point;
        public string total_time;
        public string plants;
        public string alpaca;

        //    public override string ToString()
        //    {
        //        return $"<{time}> {name} : {text}";
        //    }
        //
        //    public static DataMessage Parse(string info)
        //    {
        //        var sp = info.Split('"');
        //
        //        return new DataMessage() { time = sp[3], name = sp[7], text = sp[11] };
        //    }
    }
    public struct DataMessageTopScore
    {
        public string day;
        public string week;
        public string month;
        public string all_time;

        //    public override string ToString()
        //    {
        //        return $"<{time}> {name} : {text}";
        //    }
        //
        //    public static DataMessage Parse(string info)
        //    {
        //        var sp = info.Split('"');
        //
        //        return new DataMessage() { time = sp[3], name = sp[7], text = sp[11] };
        //    }
    }

}
