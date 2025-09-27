using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Room[] rooms;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchToRoom(int _roomIdx)
    {
        for(int i = 0; i < rooms.Length; i++)
        {
            if (i == _roomIdx)
                rooms[i].gameObject.SetActive(true);
            else
                rooms[i].gameObject.SetActive(false);
        }
    }
}
