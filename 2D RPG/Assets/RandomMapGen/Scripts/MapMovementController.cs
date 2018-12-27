﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovementController : MonoBehaviour {

    public Map map;
    public Vector2 tileSize;
    public int currentTile;
    public float speed = 1f;
    public bool moving;

    private int tempX;
    private int tempY;
    private int tempIndex;
    private float moveTime;
    private Vector2 startPos;
    private Vector2 endPos;

    public void MoveTo(int index, bool animate = false) {
        currentTile = index;

        PosUtil.CalculatePos(index, map.columns, out tempX, out tempY);

        tempX *= (int)tileSize.x;
        tempY *= -(int)tileSize.y;

        var newPos = new Vector3(tempX, tempY, 0);

        if (!animate) {
            transform.position = newPos;
        } else {
            startPos = transform.position;
            endPos = newPos;
            moveTime = 0;
            moving = true;
        }

    }

    public void MoveInDirection(Vector2 dir) {
        PosUtil.CalculatePos(currentTile, map.columns, out tempX, out tempY);

        tempX += (int)dir.x;
        tempY += (int)dir.y;

        PosUtil.CalculateIndex(tempX, tempY, map.columns, out tempIndex);
        MoveTo(tempIndex, true);
    }

    private void Update() {
        if (moving) {
            moveTime += Time.deltaTime;
            if(moveTime > speed) {
                moving = false;
                transform.position = endPos;
            }

            transform.position = Vector2.Lerp(startPos, endPos, moveTime / speed);
        }
    }
}
