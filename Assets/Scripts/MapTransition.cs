using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] Collider2D mapBoundary;
    CinemachineConfiner confiner;
    [SerializeField] Direction direction; 
    public float offset =2f;
    enum Direction { Up, Down, Left, Right}

    private void Awake() {
        confiner = FindObjectOfType<CinemachineConfiner>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) {
            confiner.m_BoundingShape2D= mapBoundary;
            UpdatePlayerPosition(collision.gameObject);

            //MapControllerManual.Instance?.HighlightArea(mapBoundary.name);
            MapControllerDynamic.Instance?.updateCurrentArea(mapBoundary.name);
        }
    }
    private void UpdatePlayerPosition(GameObject player) {
        Vector3 newPos = player.transform.position;
        switch(direction) {
            case Direction.Up:
                newPos.y+= offset;
                break;
            case Direction.Down:
                newPos.y-=offset;
                break;
            case Direction.Left:
                newPos.x-=offset;
                break;
            case Direction.Right:
                newPos.x+=offset;
                break;
        }
        player.transform.position=newPos;
    }
}
