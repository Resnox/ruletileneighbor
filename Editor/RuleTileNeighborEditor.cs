using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace UnityEditor
{
    /// <summary>
    /// The Editor for a RuleTile.
    /// </summary>
    [CustomEditor(typeof(RuleTileNeighbor), true)]
    [CanEditMultipleObjects]
    public class RuleTileNeighborEditor : RuleTileEditor
    {
        private const string s_XIconString = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAACjSURBVDhP3ZDBDYMwDEUxK5QDp0iMAWIuhmAuVMZAyolDO0PIt2woxCB65UmOHcvfTpw9hLaqPjC5MlYuAQXvug4wLbZySi4+oS/LF4rhJZVA4lfOBN08f4dpKuTKJGJwbGAJwe3JzThKtEK7P1sT4SF0zu0sEk4Xpk/VBor3nmAxJPPZ8Ic/BhwykUEDc2FXxAbcKIrtbRuE36ngHzHgiRsZLe5iVCDSgouuAAAAAElFTkSuQmCC";
        private const string s_ArrowNeighbour0 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAADaSURBVDhP1ZHBDgExEIY73X2DFWfZfQeHJXgfB44uXDi4kngJz+CA4CmsuJLsAwjd0anRpF2JK99l5m/7t50Z8Z8AR0M7Tq8CAPVyiFggAChEDACkPocPgQib06HKxz1z0rgMo57d9Jnkc7HOdtYjOTIQclJimi8cI+GY6aucOpBRqVuNpcUxU42cOgyirlBSHllavJcx4LTEqNIPW3F6Z2nwXqauvqCv+pgLksaZpd8wPQ7Nu8ZxPnN6oLXYZvtS7QaacydpOgY9vmJVX6KOH5v5FbqA059AiCdhI0zEl73RMgAAAABJRU5ErkJggg==";
        private const string s_ArrowNeighbour1 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAMAAAAMCGV4AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJUExURT8jO7w2qgAAAM7JFZcAAAADdFJOU///ANfKDUEAAAAJcEhZcwAADsIAAA7CARUoSoAAAAA+SURBVBhXhYwBCgAwCAKr/z+6TBs0GJMyT9gstl58XfMJdHcV7UAVMCKL2sEuMCVAqVkvlOgfxh8KPEebIxKqSAFmPbvxGAAAAABJRU5ErkJggg==";
        private const string s_ArrowNeighbour2 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsIAAA7CARUoSoAAAADZSURBVDhP1ZI/DoIwFMb7gBuQ6GrgDg5gwPuY6Oqgiy6OOngJz+AgiXoL1JnIAYzAs328IA3+WfW39Puafk37teI/AR6J0PESAYBy2kIsEAByRDQBDLkOM4EI0enY4uV6uO/2cGIP2TWZp6skig9ttsLgkdjFe1ika3avAIsFoYUVeX7rvNtAXYUl0QgXpnke2wN2OqoDloQWDl2/mNojrYc6qjyWRBV+F6xfoWz9SRWWLRqyTXYls+sy0zuQz/UJeQLcdjcYON6dpwj1jPQPvhG4/oXlTyLEA8FpTZmpK20cAAAAAElFTkSuQmCC";
        private const string s_ArrowNeighbour3 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAMAAAAMCGV4AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJUExURT8jO7w2qgAAAM7JFZcAAAADdFJOU///ANfKDUEAAAAJcEhZcwAADsEAAA7BAbiRa+0AAAA9SURBVBhXdc4LCgAgCAPQ6f0PnZ8ZTWgE+RZCcM3P4DCXiWHioCFCJyuBPNR4v3fx7FfRAx2Fev/vRu1+AKhoAWaay6E7AAAAAElFTkSuQmCC";
        private const string s_ArrowNeighbour5 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAMAAAAMCGV4AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJUExURT8jO7w2qgAAAM7JFZcAAAADdFJOU///ANfKDUEAAAAJcEhZcwAADsEAAA7BAbiRa+0AAAA8SURBVBhXdYtRDgAgCEKV+x86dVDZkg/g4TR0DWzaxa7CcA7pFnIOYdlLdbs4h8/9/a8gHlZhMMVbnYEFqCgBZo2r3RsAAAAASUVORK5CYII=";
        private const string s_ArrowNeighbour6 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsIAAA7CARUoSoAAAADeSURBVDhP1ZExDsIwDEXjBIkDFDFDewcGilrugwSMDMDCwsrAJTgDSwc4BaDOSD0AgtTUrRuRVOzwFtuRv+3Y4reJgjBl9zuxP7yPgxFyWBL5w+dxcMA4CK13osW2AgCX3rR4VKj1o6elvKw78zJn5c3IYHI9ATmEZMtAmbjwJkKpdloLa6hAMUHOoS1GzM1oVMBlk+2os9FYYgDQ7DZwhYTTGRW7FttsL6TWfQ4NTmdplvFJvQMODZa46P1ipwEVcM9odaI707lo67Q82gF9pZqoKIwIye3c5fT/RIg3/tRMutEI+8UAAAAASUVORK5CYII=";
        private const string s_ArrowNeighbour7 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAMAAAAMCGV4AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJUExURT8jO7w2qgAAAM7JFZcAAAADdFJOU///ANfKDUEAAAAJcEhZcwAADsEAAA7BAbiRa+0AAAA7SURBVBhXfYwLCgAwCEKz+x96TSuIfSTMF5j51IPNKsgBZJL/OLpk/tgTIIn7wMxS4+0fV2LxsUuT3RemSAFmXTNtmgAAAABJRU5ErkJggg==";
        private const string s_ArrowNeighbour8 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsIAAA7CARUoSoAAAADbSURBVDhP1ZExDoJAEEV3AOMBSGwl4hmMESOaeBsTbS20sbK08RKewUYSuYWE2ugNdEdmGDCrWNjpa/YP8GZ3WPVbhH6gJX5HJuK+s8PXBkO/j2Grd5aSsWRlSFy6M87ZCtRg5HWbYz/EhTtVCgD5pVDKAz9IC7GAGth2PZ27E3kCjgSmlKPT0VtdNjcpS56iUoi6emciSuJaVYMCALhLZAyZsLVur69bqUwQ0ZbIvMvGjCYAFkhkDJmu45OYg8ZIRie+R74OcOjn0Ix01HzHTESEQxI35PP/RKkHIs1JDLhKY+cAAAAASUVORK5CYII=";

        private static Texture2D[] s_editorTextures;

        /// <summary>
        /// Array of arrow textures used for marking positions for Rule matches
        /// </summary>
        public static Texture2D[] editorTexture
        {
            get
            {
                if (s_editorTextures == null)
                {
                    s_editorTextures = new Texture2D[10];
                    s_editorTextures[0] = Base64ToTexture(s_ArrowNeighbour0);
                    s_editorTextures[1] = Base64ToTexture(s_ArrowNeighbour1);
                    s_editorTextures[2] = Base64ToTexture(s_ArrowNeighbour2);
                    s_editorTextures[3] = Base64ToTexture(s_ArrowNeighbour3);
                    s_editorTextures[5] = Base64ToTexture(s_ArrowNeighbour5);
                    s_editorTextures[6] = Base64ToTexture(s_ArrowNeighbour6);
                    s_editorTextures[7] = Base64ToTexture(s_ArrowNeighbour7);
                    s_editorTextures[8] = Base64ToTexture(s_ArrowNeighbour8);
                    s_editorTextures[9] = Base64ToTexture(s_XIconString);
                }
                return s_editorTextures;
            }
        }

        public override void RuleOnGUI(Rect rect, Vector3Int position, int neighbor)
        {
            switch (neighbor)
            {
                case RuleTile.TilingRule.Neighbor.This:
                    GUI.DrawTexture(rect, arrows[GetArrowIndex(position)]);
                    break;
                case RuleTileNeighbor.Neighbor.Similar:
                    GUI.DrawTexture(rect, editorTexture[GetArrowIndex(position)]);
                    break;
                case RuleTile.TilingRule.Neighbor.NotThis:
                    GUI.DrawTexture(rect, arrows[9]);
                    break;
                case RuleTileNeighbor.Neighbor.NotSimilar:
                    GUI.DrawTexture(rect, editorTexture[9]);
                    break;
                default:
                    var style = new GUIStyle();
                    style.alignment = TextAnchor.MiddleCenter;
                    style.fontSize = 10;
                    GUI.Label(rect, neighbor.ToString(), style);
                    break;
            }
        }
    }
}