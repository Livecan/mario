using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerUpCrate : MonoBehaviour
{
    public GameObject powerUp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyTile(Vector2 tilePosition2D)
    {
        // TODO: REFACTORING1: Destroy tile is relevant for other Tilemap objects too, extract and reuse
        // TODO: REFACTORING2: Figure out how to do the positions nicely and avoid magic numbers

        var tilemap = GetComponent<Tilemap>();

        var tilePosition = tilemap.WorldToCell(tilePosition2D);

        var powerUpInstance = Instantiate(powerUp, new Vector3(tilePosition.x + 0.5f, tilePosition.y + 1.5f, 0), powerUp.transform.rotation);
        //powerUpInstance.GetComponent<>

        GetComponent<Tilemap>().SetTile(tilePosition, null);
    }
}
