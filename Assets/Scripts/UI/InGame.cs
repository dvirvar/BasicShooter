using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ExtensionMethods;

public class InGame : MonoBehaviour
{    
    private SocketHandler socketHandler;
    private SpawnUI spawnUI;
    private ScoreboardManager scoreboardManager;

    private void Awake()
    {
        socketHandler = FindObjectOfType<SocketHandler>();
    }

    private void Update() {
        if (Input.GetButtonDown(StaticStrings.Input.tab))
        {
            scoreboardManager.gameObject.SetActive(true);
        }
        else if (Input.GetButtonUp(StaticStrings.Input.tab)) {
            scoreboardManager.gameObject.SetActive(false);
        }
    }

    public void setSpawnUI(SpawnUI spawnUIPrefab)
    {
        destroySpawnUI();
        this.spawnUI = Instantiate(spawnUIPrefab);
        this.spawnUI.transform.SetParent(transform);// Settings parent after, so it will first instantiate the object and will call Awake, Awake wont be called from inactive object
        #region Fix for size
        this.spawnUI.GetComponent<RectTransform>().setStreched();
        #endregion
        this.spawnUI.onSpawnPressed.AddListener(delegate(Loadout loadout)
        {
            JSONObject jSONObject = JSONObject.obj;
            jSONObject.AddField("id", User.currentUser().id);
            if (loadout != null)
            {
                jSONObject.AddField("loadout", JSONObject.Create(JsonConvert.SerializeObject(loadout)));
            }
            socketHandler.Emit("requestSpawn", jSONObject);
        });
    }

    private void destroySpawnUI()
    {
        if (spawnUI != null)
        {
            this.spawnUI.onSpawnPressed.RemoveAllListeners();
            Destroy(spawnUI.gameObject);
        }
    }

    public void showSpawnUI(bool show)
    {
        spawnUI.gameObject.SetActive(show);
    }

    public SpawnUI getSpawnUI()
    {
        return spawnUI;
    }

    public void setScoreboardManager(ScoreboardManager scoreboardManager)
    {
        this.scoreboardManager = scoreboardManager;
        this.scoreboardManager.transform.SetParent(transform);
        #region Fix for size
        this.scoreboardManager.GetComponent<RectTransform>().setStreched();
        #endregion

    }

    private void destroyScoreboardsUI()
    {
        if (scoreboardManager != null)
        {
            scoreboardManager.destroyScoreboards();
            scoreboardManager = null;
        }
    }

    //public void setUI(SpawnUI spawnUI, ScoreboardManager scoreboardManager)
    //{
    //    setSpawnUI(spawnUI);
    //    setScoreboardManager(scoreboardManager);
    //}

    public void cleanUI()
    {
        destroySpawnUI();
        destroyScoreboardsUI();
    }
}
