using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sides {
    Bottom,
    Right,
    Left,
    Top
}    

public class Tile {

    public int id = 0;
    public Tile[] neighbours = new Tile[4];
    public int autoTileID;
    public bool visited;
    public int fowAutoTileId;

    public void addNeighbour(Sides side, Tile tile) {
        neighbours[(int)side] = tile;
        CalculateAutoTileID();
    }

    public void removeNeighbour(Tile tile) {
        var total = neighbours.Length;

        for (var i = 0; i < total; i++) {
            if (neighbours[i] != null) {
                if(neighbours[i].id == tile.id) {
                    neighbours[i] = null;
                }
            }
        }
        CalculateAutoTileID();
    }

    public void ClearNeighbours() {
        var total = neighbours.Length;

        for(var i = 0; i < total; i++) {
            var tile = neighbours[i];
            if(tile != null) {
                tile.removeNeighbour(this);
                neighbours[i] = null;
            }
        }
        CalculateAutoTileID();
    }

    private void CalculateAutoTileID() {
        var sideValues = new StringBuilder();

        foreach(Tile tile in neighbours) {
            sideValues.Append(tile == null ? "0" : "1");
        }

        autoTileID = Convert.ToInt32(sideValues.ToString(), 2);
    }

    public void CalculateFoWAutotileID() {
        var sideValues = new StringBuilder();

        foreach (Tile tile in neighbours) {
            if(tile == null) {
                sideValues.Append("1");
            } else {
                sideValues.Append(tile.visited ? "0" : "1");
            }
        }

        fowAutoTileId = Convert.ToInt32(sideValues.ToString(), 2);
    }
}


