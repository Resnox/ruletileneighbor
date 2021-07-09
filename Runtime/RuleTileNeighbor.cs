using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine.Serialization;

namespace UnityEngine
{
    [CreateAssetMenu(menuName = "2D/Tiles/Rule Tile Neighbor/New Rule Tile")]
    public class RuleTileNeighbor : RuleTile<RuleTileNeighbor.Neighbor>
    {
        public RuleTileNeighborGroup similarTileGroup;

        public class Neighbor : RuleTile.TilingRule.Neighbor
        {
            public const int Similar = 3;
            public const int NotSimilar = 4;
        }

        public override bool RuleMatch(int neighbor, TileBase tile)
        {
            if (tile is RuleOverrideTile)
                tile = (tile as RuleOverrideTile).m_InstanceTile;

            switch (neighbor)
            {
                case TilingRule.Neighbor.This: return tile == this;
                case TilingRule.Neighbor.NotThis: return tile != this && !similarTileGroup.similarTiles.Contains(tile);
                case Neighbor.Similar: return (tile != null && similarTileGroup.similarTiles.Contains(tile)) || tile == this;
                case Neighbor.NotSimilar: return !similarTileGroup.similarTiles.Contains(tile);
            }
            return true;
        }
    }
}
