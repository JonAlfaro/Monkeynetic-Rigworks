using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Serialization;
using RenderSettings = UnityEngine.RenderSettings;

public class EnvriomentController : MonoBehaviour
{
    private TreePrototype[] _treeCache;
    private TreePrototype[] _activeTree;
    private GameObject[] _grassArr;
    private int _grassArrIndex = 0;
    public Color SkyboxColorDefault;
    public Color SkyboxColorElderCSummon;
    [FormerlySerializedAs("StageTerrain")] public Terrain stageTerrain;
    [FormerlySerializedAs("StageLight")] public Light stageLight;
    [FormerlySerializedAs("PlayerCamera")] public Camera playerCamera;
    [FormerlySerializedAs("GrassDefault")] public GameObject grassDefault;
    [FormerlySerializedAs("GrassWind1")] public GameObject grassWind1;
    [FormerlySerializedAs("GrassWind2")] public GameObject grassWind2;

    // Start is called before the first frame update
    void Start()
    {
        _treeCache = stageTerrain.terrainData.treePrototypes;
        _activeTree = stageTerrain.terrainData.treePrototypes;
        _grassArr = new[] {grassDefault, grassWind1, grassWind2};
        SetSkyboxColor(SkyboxColorDefault);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("z"))
        {
            RotateGrassWind();
            SetSkyboxColor(SkyboxColorElderCSummon);
        }
    }

    public void SetSkyboxColor(Color color)
    {
        if (RenderSettings.skybox.HasProperty("_Tint"))
        {
            RenderSettings.skybox.SetColor("_Tint", color);
        }
        else if (RenderSettings.skybox.HasProperty("_SkyTint"))
        {
            RenderSettings.skybox.SetColor("_SkyTint", color);
        }
    }
    void OnApplicationQuit()
    {
        
        stageTerrain.terrainData.treePrototypes = _treeCache;
        playerCamera.backgroundColor = Color.blue;
    }

    public void IncreaseGrassWind()
    {
        _grassArrIndex = _grassArrIndex == 2 ? _grassArrIndex : _grassArrIndex + 1;
        _activeTree[0].prefab =  _grassArr[_grassArrIndex];
        stageTerrain.terrainData.treePrototypes = _activeTree;
    }
    
    public void DecreaseGrassWind()
    {
        _grassArrIndex = _grassArrIndex == 0 ? _grassArrIndex : _grassArrIndex - 1;
        _activeTree[0].prefab =  _grassArr[_grassArrIndex];
        stageTerrain.terrainData.treePrototypes = _activeTree;
    }
    
    public void RotateGrassWind()
    {
        _grassArrIndex = _grassArrIndex == 2 ? 0 : _grassArrIndex + 1;
        _activeTree[0].prefab =  _grassArr[_grassArrIndex];
        stageTerrain.terrainData.treePrototypes = _activeTree;
    }
    
}
