using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static event Action OnNextLevel;
    private GameObject flagMain;
    private List<GameObject> flagArray = new();
    public Transform player;
    public FloorAction floorAction;
    private AsyncOperation loadedScene;
    private string scencName = "Scene0";
    [Header("属性")]
    public bool isManual;
    public bool isClear;
    private bool isRestart;
    private bool isRestartData;
    private bool isRestartLevel;
    public int currentLevel;
    public int levelMax;
    private void Start()
    {
        //手动模式，需要搭配currentLevel来调整关卡
        if (isManual)
        {
            flagMain = FindObjInScene(scencName + currentLevel, "FlagMain");
            LoadChildObjInList(flagMain, flagArray);
        }
        else//Build模式，不要忘记关闭手动模式和关卡调整为1
        {
            loadedScene = LoadScene(scencName + currentLevel);
            isRestart = true;
            isRestartData = true;
        }
    }
    private void Update()
    {
        if (isRestartLevel)
        {
            SphereCollider coll = player.GetComponent<SphereCollider>();
            coll.enabled = false;
            if (player.position.y <= -10)
            {
                isRestart = true;
            }
            if (player.position.y >= 60)
            {
                coll.enabled = true;
                isRestartLevel = false;
            }
        }
        ClearCheck();
    }

    //通关检查
    private void ClearCheck()
    {
        //检查是否可以过关了
        if (!isRestart)
        {
            foreach (var item in flagArray)
            {
                FlagAction flagAction = item.GetComponent<FlagAction>();
                if (flagAction.isFlag)
                    isClear = true;
                else
                    isClear = false;
            }
            if (isClear)
                floorAction.DoorOpen();
            else
                floorAction.DoorClose();
        }
        else
        {
            if (!isRestartData)
            {
                OnNextLevel?.Invoke();
                isClear = false;
                UnLoadScene(scencName + currentLevel);
                if (!isRestartLevel)
                {
                    currentLevel++;
                    if (currentLevel > levelMax)
                        currentLevel = 1;
                }
                loadedScene = LoadScene(scencName + currentLevel);
                isRestartData = true;
            }
            else
            {
                if (loadedScene != null && loadedScene.isDone)
                {
                    flagMain = FindObjInScene(scencName + currentLevel, "FlagMain");
                    flagArray.Clear();
                    LoadChildObjInList(flagMain, flagArray);
                    isRestart = false;
                    isRestartData = false;
                }
            }
        }
    }

    public void LevelRestart()
    {
        isRestartLevel = true;
    }

    public AsyncOperation LoadScene(string sceneName)
    {
        return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
    public void UnLoadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    public GameObject FindObjInScene(string sceneName, string tagName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        GameObject[] gameObjects = scene.GetRootGameObjects();
        foreach (var item in gameObjects)
        {
            if (item.CompareTag(tagName))
                return item;
        }
        return null;
    }

    public void LoadChildObjInList(GameObject parent, List<GameObject> list)
    {
        foreach (Transform item in parent.transform)
        {
            list.Add(item.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && !isRestartLevel)
            isRestart = true;
    }

    private void OnEnable()
    {
        PlayerControl.OnRestartLevel += LevelRestart;
    }

    private void OnDisable()
    {
        PlayerControl.OnRestartLevel -= LevelRestart;
    }
}
