using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine.Serialization;

namespace UnityEngine
{
    [CreateAssetMenu(menuName = "2D/Tiles/Rule Tiles Neighbour", order = 101)]
    public class RuleTileNeighbor : RuleTile<RuleTileNeighbor.Neighbor>
    {
        public TileBase[] similarTile = new TileBase[0];

        public class Neighbor : RuleTile.TilingRule.Neighbor
        {
            public const int Similar = 3;
        }

        public override bool RuleMatch(int neighbor, TileBase tile)
        {
            switch (neighbor)
            {
                case Neighbor.Similar: return (tile != null && similarTile.Contains(tile)) || tile == this;
            }
            return base.RuleMatch(neighbor, tile);
        }
    }
}
