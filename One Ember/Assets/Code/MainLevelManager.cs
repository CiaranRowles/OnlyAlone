﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLevelManager : MonoBehaviour
{

    public static MainLevelManager instance;

    private PlayerRaycastingManager raycastMan;

    private PlayerInputManager inputMan;

    private PlayerTextureManager texManager;

    private Vector2[] allTextureLocations;
    private float[] allPixelDistances;

    [SerializeField]
    private Transform centerObj;

    public int nextLevelIndex = 2;

    public bool hasWon = false;


    /// <summary>
    /// Done this way to make the editor cleaner, getComponent has a very low performance cost.
    /// </summary>
    private void Awake()
    {
        instance = this;
        raycastMan = GetComponent<PlayerRaycastingManager>();
        inputMan = GetComponent<PlayerInputManager>();
        texManager = GetComponent<PlayerTextureManager>();
    }

    void Start()
    {
        allTextureLocations = texManager.getWorldSpaceLocationsOfTextures(texManager.getTexture().height, texManager.getTexture().width);
        allPixelDistances = texManager.allPixelDistancesFromCenter(allTextureLocations);
        Texture2D tex2D = (Texture2D)texManager.getTexture();
    }

    void Update()
    {
        RaycastHit2D[] hit2Ddata = raycastMan.sendRayInEveryDirection(centerObj.up, centerObj.position);
        float[] allHitDataDistances = raycastMan.getHitDataDistances(hit2Ddata);
        bool[] arePixelVisible = texManager.arePixelsVisible(hit2Ddata, allHitDataDistances, allTextureLocations, allPixelDistances);
        texManager.repaintTexture(arePixelVisible);
    }

    public void nextLevel()
    {
        Debug.Log("load next level");
        SceneManager.LoadScene(nextLevelIndex);
    }

    public void restartLevel()
    {
        if (hasWon == false) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
