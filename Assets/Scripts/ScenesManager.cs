using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    private GameObject flagMain;
    private List<GameObject> flagArray = new();
    public Transform player;
    public FloorAction floorAction;
    private AsyncOperation loadedScene;
    private string scencName = "Scene0";
    public SceneAsset sceneMain;
    public SceneAsset sceneAsset01;
    public SceneAsset sceneAsset02;
    public SceneAsset sceneAsset03;
    public SceneAsset sceneAsset04;
    [Header("属性")]
    public bool isClear;
    public bool isRestart;
    public bool isRestartData;
    public int currentLevel;
    public int levelMax;
    private void Start()
    {
        currentLevel = 1;
        flagMain = FindObjInScene(sceneAsset01.name, "FlagMain");
        LoadChildObjInList(flagMain, flagArray);
    }
    private void Update()
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
                isClear = false;
                UnLoadScene(scencName + currentLevel);
                currentLevel++;
                if (currentLevel > levelMax)
                    currentLevel = 1;
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
        if (other.transform.CompareTag("Player"))
            isRestart = true;
    }
}
