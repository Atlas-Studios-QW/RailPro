using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Connect GameHandler")]
    public GameHandler GH;

    private float CameraSpeed;
    private Savegame Savegame;
    private GameObject Player;
    private Vector3 PlayerPos;

    // Get data from GameHandler
    private void Start()
    {
        CameraSpeed = GH.CameraSpeed;
        Savegame = GH.Savegame;
        Player = GH.Player;
        // Set camera to center of map
        Player.transform.position = new Vector3(Savegame.mapSize.x / 2, 25f, Savegame.mapSize.y / 2);
    }

    void Update()
    {
        float FixedCameraSpeed = CameraSpeed;

        if (Input.GetButton("SpeedUp"))
        {
            FixedCameraSpeed = CameraSpeed * 2.5f;
        }
        else if (Input.GetButton("SpeedDown"))
        {
            FixedCameraSpeed = CameraSpeed * 0.5f;
        }


        PlayerPos = GH.Player.transform.position;
        Vector3 Movement = PlayerPos + new Vector3(Input.GetAxis("Horizontal") * FixedCameraSpeed * (PlayerPos.y / 100 + 0.1f) / 50, Input.GetAxis("ScrollWheel") * -FixedCameraSpeed, Input.GetAxis("Vertical") * FixedCameraSpeed * (PlayerPos.y / 100 + 0.1f) / 50);

        Vector2 MapSize = Savegame.mapSize;

        GH.Player.transform.position = Vector3.MoveTowards(PlayerPos, Movement, Time.deltaTime * CameraSpeed);

        PlayerPos = GH.Player.transform.position;

        // Limits camera movement
        if (PlayerPos.x < 0) { GH.Player.transform.position = new Vector3(0,PlayerPos.y,PlayerPos.z);}
        if (PlayerPos.x > MapSize.x) { GH.Player.transform.position = new Vector3(MapSize.x,PlayerPos.y,PlayerPos.z);}
        if (PlayerPos.y < 0.5f) { GH.Player.transform.position = new Vector3(PlayerPos.x,0.501f,PlayerPos.z);}
        if (PlayerPos.y > MapSize.x) { GH.Player.transform.position = new Vector3(PlayerPos.x,MapSize.x,PlayerPos.z);}
        if (PlayerPos.z < 0) { GH.Player.transform.position = new Vector3(PlayerPos.x,PlayerPos.y,0);}
        if (PlayerPos.z > MapSize.y) { GH.Player.transform.position = new Vector3(PlayerPos.x,PlayerPos.y,MapSize.y);}
    }
}
