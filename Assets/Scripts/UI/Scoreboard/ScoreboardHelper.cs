using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/// <summary>
/// This class act as the base class for scoreboard table view
/// Because C# not support generic <*> we have this class to help us
/// </summary>
public class ScoreboardHelper
{
    public struct Data
    {
        public string title;
        public Color? backgroundColor;

        public Data(string title, Color? backgroundColor)
        {
            this.title = title;
            this.backgroundColor = backgroundColor;
        }
    }
    private ScoreboardType scoreboardType;
    private PointsScoreboardTableView[] pointsScoreboards;
    private LevelScoreboardTableView[] levelScoreboards;

    public void buildScoreboards(int scoreboards, Data[] datas, GameObject scoreboardPrefab, Transform parentTransform)
    {
        destroyScoreboards();
        var gm = GameObject.Instantiate(scoreboardPrefab, parentTransform);
        setScoreboardTypeByScoreboardGameObject(scoreboardPrefab);

        switch (scoreboardType)
        {
            case ScoreboardType.Points:
                pointsScoreboards = new PointsScoreboardTableView[scoreboards];
                pointsScoreboards[0] = gm.GetComponent<PointsScoreboardTableView>();
                pointsScoreboards[0].init(datas[0].title, datas[0].backgroundColor);
                break;
            case ScoreboardType.Level:
                levelScoreboards = new LevelScoreboardTableView[scoreboards];
                levelScoreboards[0] = gm.GetComponent<LevelScoreboardTableView>();
                levelScoreboards[0].init(datas[0].title, datas[0].backgroundColor);
                break;
        }

        for (int i = 1; i < scoreboards; i++)
        {
            switch (scoreboardType)
            {
                case ScoreboardType.Points:
                    pointsScoreboards[i] = GameObject.Instantiate(scoreboardPrefab, parentTransform).GetComponent<PointsScoreboardTableView>();
                    pointsScoreboards[i].init(datas[i].title, datas[i].backgroundColor);
                    break;
                case ScoreboardType.Level:
                    levelScoreboards[i] = GameObject.Instantiate(scoreboardPrefab, parentTransform).GetComponent<LevelScoreboardTableView>();
                    levelScoreboards[i].init(datas[i].title, datas[i].backgroundColor);
                    break;
            }
        }
    }

    public void destroyScoreboards()
    {
        switch (scoreboardType) {
            case ScoreboardType.Points:
                if (pointsScoreboards != null)
                {
                    foreach (PointsScoreboardTableView pointsScoreboard in pointsScoreboards)
                    {
                        GameObject.Destroy(pointsScoreboard.gameObject);
                    }
                    pointsScoreboards = null;
                }
                break;
            case ScoreboardType.Level:
                if (levelScoreboards != null)
                {
                    foreach (LevelScoreboardTableView gunMasterScoreboard in levelScoreboards)
                    {
                        GameObject.Destroy(gunMasterScoreboard.gameObject);
                    }
                    levelScoreboards = null;
                }
                break;
        }
    }

    private void setScoreboardTypeByScoreboardGameObject(GameObject scoreboardGameObject)
    {
        if (scoreboardGameObject.GetComponent<PointsScoreboardTableView>() != null)
        {
            this.scoreboardType = ScoreboardType.Points;
        } else if (scoreboardGameObject.GetComponent<LevelScoreboardTableView>() != null)
        {
            this.scoreboardType = ScoreboardType.Level;
        } else
        {
            throw new DataMisalignedException("Missing check for new Scoreboard");
        }
    }

    public void setInfos(int index, List<PlayerScoreboardInfo> infos)
    {
        switch(scoreboardType)
        {
            case ScoreboardType.Points:
                pointsScoreboards[index].setInfos(infos.Cast<PlayerPointsScoreboardInfo>().ToList());
                break;
            case ScoreboardType.Level:
                levelScoreboards[index].setInfos(infos.Cast<PlayerLevelScoreboardInfo>().ToList());
                break;
        }
    }

    public void addInfo(int index, PlayerScoreboardInfo info)
    {
        switch (scoreboardType)
        {
            case ScoreboardType.Points:
                pointsScoreboards[index].addInfo((PlayerPointsScoreboardInfo)info);
                break;
            case ScoreboardType.Level:
                levelScoreboards[index].addInfo((PlayerLevelScoreboardInfo)info);
                break;
        }
    }

    public PlayerScoreboardInfo removeInfo(int index, string playerId) =>
        scoreboardType switch
        {
            ScoreboardType.Points => pointsScoreboards[index].removeInfo(playerId),
            ScoreboardType.Level => levelScoreboards[index].removeInfo(playerId),
            _ => throw new NotImplementedException(),
        };
    

    public void addKill(int index, string id)
    {
        switch (scoreboardType)
        {
            case ScoreboardType.Points:
                pointsScoreboards[index].addKill(id);
                break;
            case ScoreboardType.Level:
                levelScoreboards[index].addKill(id);
                break;
        }
    }

    public void addDeath(int index, string id)
    {
        switch (scoreboardType)
        {
            case ScoreboardType.Points:
                pointsScoreboards[index].addDeath(id);
                break;
            case ScoreboardType.Level:
                levelScoreboards[index].addDeath(id);
                break;
        }
    }

    public void setPing(int index, string id, int ping)
    {
        switch(scoreboardType)
        {
            case ScoreboardType.Points:
                pointsScoreboards[index].setPing(id, ping);
                break;
            case ScoreboardType.Level:
                levelScoreboards[index].setPing(id, ping);
                break;
        }
    }

    public void addGameModePoints(int index, string id, int points)
    {
        switch(scoreboardType)
        {
            case ScoreboardType.Points:
                pointsScoreboards[index].addPoints(id, points);
                break;
            case ScoreboardType.Level:
                levelScoreboards[index].addLevels(id, points);
                break;
        }
    }

    public int scoreboardsCount() =>
        scoreboardType switch
        {
            ScoreboardType.Points => pointsScoreboards.Length,
            ScoreboardType.Level => levelScoreboards.Length,
            _ => throw new NotImplementedException(),
        };
}

public enum ScoreboardType
{
    Points, Level
}