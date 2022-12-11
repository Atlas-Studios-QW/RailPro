using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Connect GameHandler")]
    public GameHandler GH;

    [Header("Settings")]
    public float CameraSpeed = 10;
    
    [Header("Game Objects")]
    public GameObject Player;

    private Savegame Savegame;

    private void Start()
    {
        Savegame = GH.Savegame;
        Player.transform.position = new Vector3(GH.Savegame.mapSize.x / 2, 25f, Savegame.mapSize.y / 2);
    }

    void Update()
    {
        Vector3 Movement = Player.transform.position + new Vector3(Input.GetAxis("Horizontal") * CameraSpeed * (Player.transform.position.y / 100), Input.GetAxis("ScrollWheel") * -1000 * CameraSpeed, Input.GetAxis("Vertical") * CameraSpeed * (Player.transform.position.y / 100));

        Player.transform.position = Vector3.MoveTowards(Player.transform.position, Movement, Time.deltaTime * CameraSpeed);
        
        Vector3 PlayerPos = Player.transform.position;
        if (PlayerPos.x > Savegame.mapSize.x) { Player.transform.position = new Vector3(Savegame.mapSize.x, PlayerPos.y, PlayerPos.z); }
        if (PlayerPos.x < 0) { Player.transform.position = new Vector3(0, PlayerPos.y, PlayerPos.z); }
        if (PlayerPos.y > 100f) { Player.transform.position = new Vector3(PlayerPos.x,100f,PlayerPos.z); }
        if (PlayerPos.y < 1) { Player.transform.position = new Vector3(PlayerPos.x, 1, PlayerPos.z); }
        if (PlayerPos.z > Savegame.mapSize.y) { Player.transform.position = new Vector3(PlayerPos.x, PlayerPos.y, Savegame.mapSize.y); }
        if (PlayerPos.z < 0) { Player.transform.position = new Vector3(PlayerPos.x, PlayerPos.y, 0); }
    }
}
