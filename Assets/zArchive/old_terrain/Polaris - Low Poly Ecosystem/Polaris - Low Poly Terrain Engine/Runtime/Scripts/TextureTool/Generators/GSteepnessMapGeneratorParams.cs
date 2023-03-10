using UnityEngine;

namespace Pinwheel.Griffin.TextureTool
{
    [System.Serializable]
    public struct GSteepnessMapGeneratorParams
    {
        [SerializeField]
        private GStylizedTerrain terrain;
        public GStylizedTerrain Terrain
        {
            get
            {
                return terrain;
            }
            set
            {
                terrain = value;
            }
        }

        [SerializeField]
        private GNormalMapMode mode;
        public GNormalMapMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
            }
        }
    }
}
