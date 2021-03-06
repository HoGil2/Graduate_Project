﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickedMainMenu : MonoBehaviour
{
    public GameObject main;
    public GameObject option;
    public GameObject tpcheck;
    public GameObject replay;
    public GameObject assist;

    private void Start()
    {
        if (PlayerPrefs.GetInt("tpmode", 0) == 0)
        {
            tpcheck.SetActive(false);
        }
        else
        {
            tpcheck.SetActive(true);
        }
        if (PlayerPrefs.GetInt("assist", 0) == 0)
        {
            assist.SetActive(false);
        }
        else
        {
            assist.SetActive(false);
        }
    }

    public void Replay_btn_clicked()
    {
        Debug.Log("option");
        main.SetActive(false);
        replay.SetActive(true);
    }

    public void Option_btn_clicked()
    {
        Debug.Log("option");
        main.SetActive(false);
        option.SetActive(true);
    }

    public void Back_btn_clicked()
    {
        Debug.Log("back");
        main.SetActive(true);
        option.SetActive(false);
        replay.SetActive(false);
    }

    public void Teleport_btn_clicked()
    {
        Debug.Log("Teleport button");
        if (tpcheck.activeSelf)
        {
            tpcheck.SetActive(false);
            PlayerPrefs.SetInt("tpmode", 0);
        }
        else
        {
            tpcheck.SetActive(true);
            PlayerPrefs.SetInt("tpmode", 1);
        }
    }

    public void Assist_btn_clicked()
    {
        Debug.Log("Assist button");
        if (assist.activeSelf)
        {
            assist.SetActive(false);
            PlayerPrefs.SetInt("assist", 0);
        }
        else
        {
            assist.SetActive(true);
            PlayerPrefs.SetInt("assist", 1);
        }
    }

    public void start_btn_clicked()
    {
        Debug.Log("start click");
        SceneManager.LoadScene(1);
    }

    public void Exit_btn_clicked()
    {
        Debug.Log("exit click");
        PlayerPrefs.Save();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
