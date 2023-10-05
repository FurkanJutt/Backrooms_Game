using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;


public enum GenerationState
{
    Idle,
    GeneratingRooms,
    GeneratingLights,
    GeneratingBarrier,
    GeneratingSpawn,
    GeneratingExit
}

public class GenerationManager : MonoBehaviour
{
    public static GenerationManager instance;

    #region Variables

    [Header("References")]
    [SerializeField] private Transform WorldGrid; // Parent gameobject of the rooms generated.
    [SerializeField] private List<GameObject> RoomTypes;
    [SerializeField] private GameObject EmptyRoom, SpawnRoom, ExitRoom, Barrier;
    [SerializeField] private List<GameObject> LightTypes;

    [SerializeField] private GameObject PlayerObject, MainCameraObject;

    [Header("Settings")]
    [Tooltip("Increase chance of spawning EmptyRoom.")]
    [Range(0, 18)]
    public int mapEmptiness = 4; // The chance of an EmptyRoom spawning.

    [Tooltip("Increase chance of spawning EmptyRoom.")]
    [Range(0, 18)]
    public int mapBrightiness = 4; // The chance of an LightType spawning.

    [Tooltip("Add a perfect square root number.")]
    [Range(0,18)]
    [SerializeField] private int _sizeValue;

    [Space]
    public List<GameObject> RoomsGenerated; // Store rooms that are already generated.
    [SerializeField] private int _mapSize = 16; // Actual size of the map.

    public int mapSize
    {
        get { return _mapSize; }
        set {
            value = _sizeValue;
            _mapSize = (int)Mathf.Pow(value, 4);
            _mapSizeSquare = (int)Mathf.Sqrt(_mapSize);
        }
    }

    [SerializeField] private int _mapSizeSquare; // The square root of the mapsize.

    public GenerationState currentState;

    private float _currentPosX, _currentPosZ;
    private float _currentPosTracker; // Keeps track of current pos of the generated room.
    private int _currentRoom; // Keeps track of current room of the generated grid.
    private Vector3 _currentPos; // Current position of the room to be generated.
    private float roomSize = 7;

    #endregion Variables

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // TODO: Remove Start() function.
    private void Start()
    {
        PlayerObject.SetActive(false);
    }

    public void UpdateMapSize()
    {
        mapSize = _mapSize;
    }

    public void ReloadWorld() // Reload the world, so you can make a new one.
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GenerateWorld() // Creates the world for the first time.
    {
        UpdateMapSize();

        for (int i = 0; i < mapEmptiness; i++)
        {
            RoomTypes.Add(EmptyRoom); // Adds EmptyRooms to the RoomTypes List.
        }

        for (int state = 0; state < 6; state++)
        {
            for (int i = 0; i < mapSize; i++)
            {
                if (_currentPosTracker == _mapSizeSquare)
                {
                    if (currentState == GenerationState.GeneratingBarrier)
                    {
                        Destroy(RoomsGenerated[_currentRoom]);
                        GenerateBarrier(); // Right(Bottom) of the map. [inside "()" is the real orientation]
                    }

                    _currentPosX = 0;
                    _currentPosTracker = 0;

                    _currentPosZ += roomSize; // Moves up.

                    if (currentState == GenerationState.GeneratingBarrier)
                        GenerateBarrier(); // Left(Top) of the map.
                }
                _currentPos = new(_currentPosX, 0, _currentPosZ);

                switch (currentState)
                {
                    case GenerationState.GeneratingRooms:
                        RoomsGenerated.Add(Instantiate(RoomTypes[Random.Range(0, RoomTypes.Count)], _currentPos, Quaternion.identity, WorldGrid)); // Instantiate the room type at the current position.
                        break;
                    case GenerationState.GeneratingLights:
                        int lightSpawn = Random.Range(-1, mapBrightiness);

                        if(lightSpawn == 0)
                            Instantiate(LightTypes[Random.Range(0, LightTypes.Count)], _currentPos, Quaternion.identity, WorldGrid); // Instantiate the room type at the current position.
                        break;
                    case GenerationState.GeneratingBarrier:
                        if (_currentRoom <= _mapSizeSquare && _currentRoom >= 0)
                        {
                            Destroy(RoomsGenerated[_currentRoom]);
                            GenerateBarrier(); // Bottom(Right) of map.
                        }

                        if (_currentRoom <= _mapSize && _currentRoom >= _mapSize - _mapSizeSquare)
                        {
                            Destroy(RoomsGenerated[_currentRoom]);
                            GenerateBarrier(); // Top(Left) of map.
                        }
                        break;
                }

                _currentRoom++;
                _currentPosTracker++;
                _currentPosX += roomSize; // Moves right.
            }
            NextState();

            switch (currentState)
            {
                case GenerationState.GeneratingExit:
                    int roomToReplace = Random.Range(0, RoomsGenerated.Count);

                    GameObject exitRoom = Instantiate(ExitRoom, RoomsGenerated[roomToReplace].transform.position, Quaternion.identity, WorldGrid);
                    DestroyImmediate(RoomsGenerated[roomToReplace]);

                    RoomsGenerated[roomToReplace] = exitRoom;
                    break;
                case GenerationState.GeneratingSpawn:
                    int _roomToReplace = Random.Range(0, RoomsGenerated.Count);

                    spawnRoom = Instantiate(SpawnRoom, RoomsGenerated[_roomToReplace].transform.position, Quaternion.identity, WorldGrid);
                    DestroyImmediate(RoomsGenerated[_roomToReplace]);

                    RoomsGenerated[_roomToReplace] = spawnRoom;
                    break;
            }
        }
    }

    [HideInInspector] public GameObject spawnRoom;

    public void SpawnPlayer()
    {
        PlayerObject.transform.position = new(spawnRoom.transform.position.x, 2f, spawnRoom.transform.position.z);

        MainCameraObject.SetActive(false);
        PlayerObject.SetActive(true);
    }

    private void NextState()
    {
        currentState++; // Goes to next state.

        // Reseting variables.
        _currentRoom = 0;
        _currentPosX = 0;
        _currentPosZ = 0;
        _currentPos = Vector3.zero;
        _currentPosTracker = 0;
    }

    public void GenerateBarrier() // Generate the barrier at the current position.
    {
        _currentPos = new(_currentPosX, 0, _currentPosZ);

        GameObject barrier = Instantiate(Barrier, _currentPos, Quaternion.identity, WorldGrid);
        //DestroyImmediate()
    }

    public void WinGame()
    {
        MainCameraObject.SetActive(true);
        PlayerObject.SetActive(false);

        Debug.Log($"{GetType().Name}-> Player has exited the game!");
    }
}
