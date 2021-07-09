using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEngine
{
    [CreateAssetMenu(menuName = "2D/Tiles/Rule Tile Neighbor/New Group")]
    public class RuleTileNeighborGroup : ScriptableObject
    {
        public TileBase[] similarTiles;
    }
}
