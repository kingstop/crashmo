using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap 
{
    Tile[,] mTiles;

    int mRowCount;
    int mColumnCount;

    float mTileLength;
    Vector3 mOrigin;
    Vector3 mEnd;
    GameObject mTileObj;

    List<KeyValuePair<int, int>> mTileObjs;    
    public void Create(GameObject TileObj, Vector3 origin, float tileLength )
    {
        int maxX = 0;
        int maxZ = 0;

        mTileObj = TileObj;
        foreach (Transform value in mTileObj.transform)
        {
            int x = (int)value.position.x;
            int z = (int)value.position.z;
            if(maxX < x)
            {
                maxX = x;
            }

            if(maxZ < z)
            {
                maxZ = z;
            }
            KeyValuePair<int, int> entity = new KeyValuePair<int, int>(z, x);
            mTileObjs.Add(entity);
        }

        InitMap(maxX, maxZ, tileLength, origin, false);
        foreach(KeyValuePair<int, int> entity in mTileObjs)
        {
            SetWalkable(entity.Key, entity.Value, true);
        }
        SetTileWalkable(GameObject.FindObjectsOfType<TileComponent>());
    }

    public TileMap()
    {
    }

    public  void InitMap(int rowCount, int columnCount, float tileLength, Vector3 origin, bool defaultWalkAble)
    {
        this.mRowCount = rowCount;
        this.mColumnCount = columnCount;
        this.mTileLength = tileLength;
        this.mOrigin = origin;
        this.mEnd = this.mOrigin + new Vector3(tileLength * (rowCount + 0.5f), 0, tileLength * (columnCount + 0.5f));

        this.mTiles = new Tile[rowCount, columnCount];
        int cullOff = 5;            //		剔除一定的区域。

        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                if ((row <= cullOff || row >= GameInfo.MapRow - cullOff) && (column <= cullOff || column >= GameInfo.MapColumn - cullOff))
                    continue;

                Tile tile = new Tile();
                tile.mRow = row;
                tile.mColumn = column;
                tile.mPosition = origin + tileLength * new Vector3(row + 0.5f, 0, column + 0.5f);
                tile.mIsWalkable = defaultWalkAble;
                this.mTiles[row, column] = tile;
            }
        }
    }
    public TileMap(int rowCount, int columnCount, float tileLength, Vector3 origin)
    {
        this.mRowCount = rowCount;
        this.mColumnCount = columnCount;
        this.mTileLength = tileLength;
        this.mOrigin = origin;
        this.mEnd = this.mOrigin + new Vector3(tileLength * (rowCount + 0.5f), 0, tileLength * (columnCount + 0.5f));

        this.mTiles = new Tile[rowCount, columnCount];
        int cullOff = 5;            //		剔除一定的区域。

        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                if ((row <= cullOff || row >= GameInfo.MapRow - cullOff) && (column <= cullOff || column >= GameInfo.MapColumn - cullOff))
                    continue;

                Tile tile = new Tile();
                tile.mRow = row;
                tile.mColumn = column;
                tile.mPosition = origin + tileLength * new Vector3(row + 0.5f, 0, column + 0.5f);
                tile.mIsWalkable = true;

                this.mTiles[row, column] = tile;
            }
        }
    }

    public void SetTileWalkable(TileComponent[] tileComponents)
    {
        for (int i = 0; i < tileComponents.Length; i++)
        {
            TileComponent com = tileComponents[i];

            Tile leftDown = GetTile(com.transform.position, com.mSize);
            if (leftDown == null)
                continue;

            for (int row = leftDown.mRow; row < leftDown.mRow + com.mSize; row++)
            {
                for (int column = leftDown.mColumn; column < leftDown.mColumn + com.mSize; column++)
                {
                    Tile t = GetTile(row, column);
                    t.mIsWalkable = com.mWalkable == TileComponent.ETileWalkable.Walkable;
                }
            }
        }
    }


    public void InitMapResource(int rowCount, int columnCount, float prefabLength, Vector3 origin)
    {
        MapLoader resource = new MapLoader();
        resource.Init(rowCount, columnCount, prefabLength, origin);
    }

    public void SetWalkable(int row, int column, bool walkable)
    {
        Tile tile = GetTile(row, column);
        if (tile != null)
        {
            tile.mIsWalkable = walkable;
        }
    }

    public Tile GetTile(int row, int column)
    {
        if (row < 0 || column < 0 || row >= mRowCount || column >= mColumnCount)
        {
            return null;
        }
        return mTiles[row, column];
    }

    public Tile GetTile(Vector3 position, int objectSize)
    {
        Vector3 leftDown = position - mOrigin - 0.5f * objectSize * mTileLength * new Vector3(1, 0, 1);

        if (leftDown.x < 0 || leftDown.z < 0)
            return null;

        int row = (int)(leftDown.x / mTileLength);
        int column = (int)(leftDown.z / mTileLength);

        return GetTile(row, column);
    }

    //		获取可以到达的邻居节点：是否忽略转角
    public List<Tile> GetNeighborsCanReach(Tile tile, int size, bool ignoreCorner)
    {
        List<Tile> neighbors = new List<Tile>();

        for (int row = tile.mRow - 1; row <= tile.mRow + 1; row++)
        {
            for (int column = tile.mColumn - 1; column <= tile.mColumn + 1; column++)
            {
                Tile neighbor = GetTile(row, column);
                if (neighbor == null || neighbor == tile || !IsWalkable(neighbor, size))
                    continue;
                else if (tile.mRow == neighbor.mRow || tile.mColumn == neighbor.mColumn)        //		not need check corners . 
                    neighbors.Add(neighbor);
                else if (!ignoreCorner)  //		check corners . 
                {
                    if (CanCornersReach(tile, neighbor, size))
                        neighbors.Add(neighbor);
                }
            }
        }

        return neighbors;
    }

    bool CanCornersReach(Tile tile, Tile neighbor, int size)
    {
        List<Tile> corners = GetCorners(tile, neighbor);

        for (int i = 0; i < corners.Count; i++)
        {
            Tile corner = corners[i];
            if (corner == null || !IsWalkable(corner, size))
                return false;
        }

        return true;
    }


    List<Tile> GetCorners(Tile tile, Tile neighbor)
    {
        if (tile.mRow == neighbor.mRow || tile.mColumn == neighbor.mColumn)
            return null;

        List<Tile> corners = new List<Tile>();

        int deltaRow = neighbor.mRow - tile.mRow;
        int deltaColumn = neighbor.mColumn - tile.mColumn;

        corners.Add(GetTile(tile.mRow, tile.mColumn + deltaColumn));
        corners.Add(GetTile(tile.mRow + deltaRow, tile.mColumn));

        return corners;
    }

    public bool IsWalkable(Tile tile, int size, Vector3 testPoint = default(Vector3))
    {
        if (tile == null || tile.mIsWalkable == false)
            return false;

        bool walkable = true;
        int sizezX = size;
        int sizeZ = size;

        //		修正
        if (testPoint != default(Vector3))
        {
            Vector3 leftDown = testPoint - new Vector3(size, 0, size) * 0.5f;
            if (leftDown.x - tile.mRow > 0.01f)
                sizezX++;
            if (leftDown.z - tile.mColumn > 0.01f)
                sizeZ++;
        }

        for (int row = tile.mRow; row < tile.mRow + sizezX; row++)
        {
            for (int column = tile.mColumn; column < tile.mColumn + sizeZ; column++)
            {
                Tile t = GetTile(row, column);
                if (t == null || !t.mIsWalkable)
                {
                    walkable = false;
                    break;
                }
            }
        }

        return walkable;
    }

    public bool IsOutOfMap(Vector3 position)
    {
        if (position.x < this.mOrigin.x || position.z < this.mOrigin.z || position.x > this.mEnd.x || position.z > this.mEnd.z)
        {
            return true;
        }
        return false;
    }
}
