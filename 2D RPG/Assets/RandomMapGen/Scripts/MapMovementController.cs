using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovementController : MonoBehaviour {

    public Map map;
    public Vector2 tileSize;

    private int tempX;
    private int tempY;

    public void MoveTo(int index) {
        PosUtil.CalculatePos(index, map.columns, out tempX, out tempY);

        tempX *= (int)tileSize.x;
        tempY *= -(int)tileSize.y;

        transform.position = new Vector3(tempX, tempY, 0);
    }
}
