using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class ScoreboardManager : MonoBehaviour
{
    private enum State
    {
        Regular, Single, MultiButSingle    
    }
    [SerializeField] private GameObject content;
    [SerializeField] private float minimumWidth = 400;
    private ScoreboardHelper scoreboardHelper;
    private State state;
    private Color[] colors;
    private Dictionary<string, int> playersTeams;

    private void Awake()
    {
        scoreboardHelper = new ScoreboardHelper();
        playersTeams = new Dictionary<string, int>();
    }

    public void setPlayers(List<PlayerScoreboardInfo> playerScoreboardInfos)
    {
        foreach (var item in playerScoreboardInfos)
        {
            setPlayer(item);
        }
        //switch (state)
        //{
        //    case State.MultiButSingle:
        //        foreach (var playerScoreboardInfo in playerScoreboardInfos)
        //        {
        //            playerScoreboardInfo.color = colors[playerScoreboardInfo.teamID];
        //        }
        //        goto case State.Single;
        //    case State.Single:
        //        scoreboardHelper.setInfos(0, playerScoreboardInfos);
        //        break;
        //    case State.Regular:
        //        foreach (var playerScoreboardInfo in playerScoreboardInfos)
        //        {
        //            playerScoreboardInfo.color = colors[playerScoreboardInfo.teamID];
        //        }
        //        var scoreboardsCount = scoreboardHelper.scoreboardsCount();
        //        for (int i = 0; i < scoreboardsCount; i++)
        //        {
        //            List<PlayerScoreboardInfo> players = new List<PlayerScoreboardInfo>();
        //            foreach (var item in playerScoreboardInfos)
        //            {
        //                if (item.teamID == i)
        //                {
        //                    players.Add(item);
        //                }
        //            }
        //            scoreboardHelper.setInfos(i, players);
        //        }
        //        break;
        //}
    }

    public void setPlayer(PlayerScoreboardInfo playerScoreboardInfo)
    {
        if (playersTeams.ContainsKey(playerScoreboardInfo.id))
        {
            playersTeams[playerScoreboardInfo.id] = playerScoreboardInfo.teamID;
        } else
        {
            playersTeams.Add(playerScoreboardInfo.id, playerScoreboardInfo.teamID);
        }
        switch(state)
        {
            case State.MultiButSingle:
                playerScoreboardInfo.color = colors[playerScoreboardInfo.teamID];
                goto case State.Single;
            case State.Single:
                scoreboardHelper.addInfo(0, playerScoreboardInfo);
                break;
            case State.Regular:
                playerScoreboardInfo.color = colors[playerScoreboardInfo.teamID];
                scoreboardHelper.addInfo(playerScoreboardInfo.teamID, playerScoreboardInfo);
                break;
        }
    }

    public void switchPlayerTeam(string id, int newTeamID)
    {
        var info = removePlayer(id);
        info.teamID = newTeamID;
        setPlayer(info);
    }

    public PlayerScoreboardInfo removePlayer(string playerId) =>
        state switch
        {
            State.Single => scoreboardHelper.removeInfo(0, playerId),
            State.MultiButSingle => scoreboardHelper.removeInfo(0, playerId),
            State.Regular => scoreboardHelper.removeInfo(playersTeams[playerId], playerId),
            _ => throw new System.NotImplementedException(),
        };
    

    public void addKill(string playerId)
    {
        switch(state)
        {
            case State.Single:
            case State.MultiButSingle:
                scoreboardHelper.addKill(0, playerId);
                break;
            case State.Regular:
                scoreboardHelper.addKill(playersTeams[playerId], playerId);
                break;
        }
    }

    public void addDeath(string playerId)
    {
        switch(state)
        {
            case State.Single:
            case State.MultiButSingle:
                scoreboardHelper.addDeath(0, playerId);
                break;
            case State.Regular:
                scoreboardHelper.addDeath(playersTeams[playerId], playerId);
                break;
        }
    }

    public void setPing(string playerId, int ping)
    {
        switch (state)
        {
            case State.Single:
            case State.MultiButSingle:
                scoreboardHelper.setPing(0, playerId, ping);
                break;
            case State.Regular:
                scoreboardHelper.setPing(playersTeams[playerId], playerId, ping);
                break;
        }
    }

    public void addGameModePoints(string playerId, int points)
    {
        switch(state)
        {
            case State.Single:
            case State.MultiButSingle:
                scoreboardHelper.addGameModePoints(0, playerId, points);
                break;
            case State.Regular:
                scoreboardHelper.addGameModePoints(playersTeams[playerId], playerId, points);
                break;
        }
    }

    public void buildScoreboards(int numberOfScoreboards, GameObject scoreboardPrefab)
    {
        StartCoroutine(buildScoreboardsCourotine(numberOfScoreboards, scoreboardPrefab));
    }

    public IEnumerator buildScoreboardsCourotine(int numberOfScoreboards, GameObject scoreboardPrefab)
    {
        destroyScoreboards();
        yield return null;
        GameObject[] gameObjects = new GameObject[numberOfScoreboards];
        int scoreboards = 1;
        for (int i = 0; i < numberOfScoreboards; i++)
        {
            GameObject gm = new GameObject($"gm{i}", typeof(RectTransform));
            gm.transform.SetParent(content.transform);
            gameObjects[i] = gm;
        }
        yield return null;
        if (numberOfScoreboards == 1)
        {
            state = State.Single;
        }
        else if (gameObjects[0].GetComponent<RectTransform>().rect.width < minimumWidth)
        {
            state = State.MultiButSingle;
        }
        else
        {
            state = State.Regular;
            scoreboards = gameObjects.Length;
        }

        foreach (var gm in gameObjects)
        {
            Destroy(gm);
        }
        yield return null;
        ScoreboardHelper.Data[] datas = new ScoreboardHelper.Data[scoreboards];
        switch (state)
        {
            case State.MultiButSingle:
                setColors();
                goto case State.Single;
            case State.Single:
                datas[0] = new ScoreboardHelper.Data("Scoreboard", null);
                break;
            case State.Regular:
                setColors();
                for (int i = 0; i < scoreboards; i++)
                {
                    char letter = (char)(65 + i);
                    datas[i] = new ScoreboardHelper.Data($"Team {letter}", colors[i]);
                }
                break;
        }
        
        scoreboardHelper.buildScoreboards(scoreboards, datas, scoreboardPrefab, content.transform);
    }

    public void destroyScoreboards()
    {
        scoreboardHelper.destroyScoreboards();
    }

    private void setColors()
    {
        colors = new Color[16];
        colors[0] = createColor(g: 155, b: 155);//Pinkish
        colors[1] = createColor(153, b: 153);//Grinish
        colors[2] = createColor(b: 153);//Yellowish
        colors[3] = createColor(153);//Cyan
        colors[4] = createColor(g:51, b:51);//Red
        colors[5] = createColor(r:102, b:204);//Green
        colors[6] = createColor(160,160,160);//Grey
        colors[7] = createColor(153,0,76,80);//Bourdeaux
        colors[8] = createColor(51,51);//Blue
        colors[9] = createColor(g:102, b:178);//Pink
        colors[10] = createColor(204,204,0);//Gold
        colors[11] = createColor(193,53,193);//Purple
        colors[12] = createColor(130,125,72);//Army green
        colors[13] = createColor(241,150,31);//Carrot orange
        colors[14] = createColor(g: 187, b:97);//Orangish
        colors[15] = createColor();//White
    }

    private Color createColor(int r = 255, int g = 255, int b = 255, int a = 100)
    {
        return new Color(r / 255f, g / 255f, b / 255f, a / 100f);
    }
}