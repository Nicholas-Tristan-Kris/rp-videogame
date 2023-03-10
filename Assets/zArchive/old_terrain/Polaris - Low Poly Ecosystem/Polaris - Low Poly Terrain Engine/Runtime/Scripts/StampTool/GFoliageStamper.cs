using System.Collections.Generic;
using UnityEngine;

namespace Pinwheel.Griffin.StampTool
{
    [System.Serializable]
    [ExecuteInEditMode]
    public class GFoliageStamper : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private bool editor_ShowLivePreview = true;
        public bool Editor_ShowLivePreview
        {
            get
            {
                return editor_ShowLivePreview;
            }
            set
            {
                editor_ShowLivePreview = value;
            }
        }

        [SerializeField]
        private bool editor_ShowBounds = true;
        public bool Editor_ShowBounds
        {
            get
            {
                return editor_ShowBounds;
            }
            set
            {
                editor_ShowBounds = value;
            }
        }
#endif

        [SerializeField]
        private bool enableTerrainMask;
        public bool EnableTerrainMask
        {
            get
            {
                return enableTerrainMask;
            }
            set
            {
                enableTerrainMask = value;
            }
        }

        [SerializeField]
        private int groupId;
        public int GroupId
        {
            get
            {
                return groupId;
            }
            set
            {
                groupId = value;
            }
        }

        [SerializeField]
        private Vector3 position;
        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                transform.position = value;
            }
        }

        [SerializeField]
        private Quaternion rotation;
        public Quaternion Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
                transform.rotation = value;
            }
        }

        [SerializeField]
        private Vector3 scale;
        public Vector3 Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                transform.localScale = value;
            }
        }

        [SerializeField]
        private Texture2D mask;
        public Texture2D Mask
        {
            get
            {
                return mask;
            }
            set
            {
                mask = value;
            }
        }

        [SerializeField]
        private AnimationCurve falloff;
        public AnimationCurve Falloff
        {
            get
            {
                return falloff;
            }
            set
            {
                falloff = value;
            }
        }

        [SerializeField]
        private List<GFoliageStampLayer> layers;
        public List<GFoliageStampLayer> Layers
        {
            get
            {
                if (layers == null)
                {
                    layers = new List<GFoliageStampLayer>();
                }
                return layers;
            }
            set
            {
                layers = value;
            }
        }

        [SerializeField]
        private int maskResolution;
        public int MaskResolution
        {
            get
            {
                return maskResolution;
            }
            set
            {
                maskResolution = Mathf.Clamp(Mathf.ClosestPowerOfTwo(value), GCommon.TEXTURE_SIZE_MIN, GCommon.TEXTURE_SIZE_MAX);
            }
        }

        public Rect Rect
        {
            get
            {
                Vector3[] quad = new Vector3[4];
                GetQuad(quad);
                Rect r = GUtilities.GetRectContainsPoints(quad);
                return r;
            }
        }

        private Texture2D falloffTexture;

        private Vector3[] worldPoints = new Vector3[4];
        private Vector2[] uvPoints = new Vector2[4];

        private Dictionary<string, RenderTexture> tempRt;
        private Dictionary<string, RenderTexture> TempRt
        {
            get
            {
                if (tempRt == null)
                {
                    tempRt = new Dictionary<string, RenderTexture>();
                }
                return tempRt;
            }
        }

        private void Reset()
        {
            position = Vector3.zero;
            rotation = Quaternion.identity;
            scale = Vector3.one * 100;
            mask = null;
            falloff = AnimationCurve.EaseInOut(0, 1, 1, 0);
            maskResolution = 1024;
        }

        private void OnDisable()
        {
            ReleaseResources();
        }

        private void OnDestroy()
        {
            ReleaseResources();
        }

        private void ReleaseResources()
        {
            foreach (RenderTexture rt in TempRt.Values)
            {
                if (rt != null)
                {
                    rt.Release();
                    GUtilities.DestroyObject(rt);
                }
            }
        }

        public void Apply()
        {
            if (falloffTexture != null)
                Object.DestroyImmediate(falloffTexture);
            Internal_UpdateFalloffTexture();
            Internal_UpdateLayerTransitionTextures();
            IEnumerator<GStylizedTerrain> terrains = GStylizedTerrain.ActiveTerrains.GetEnumerator();

            try
            {
                while (terrains.MoveNext())
                {
                    GStylizedTerrain t = terrains.Current;
                    if (groupId < 0 ||
                        (groupId >= 0 && groupId == t.GroupId))
                    {
                        Apply(t);
                    }
                }
            }
            catch (GProgressCancelledException)
            {
                Debug.Log("Stamp process canceled, result may be incorrect. Use History to clean up!");
#if UNITY_EDITOR
                GCommonGUI.ClearProgressBar();
#endif
            }
        }

        private void Apply(GStylizedTerrain t)
        {
            if (t.TerrainData == null)
                return;
            if (Layers.Count == 0)
                return;

            GetQuad(worldPoints);
            GetUvPoints(t, worldPoints, uvPoints);
            Rect dirtyRect = GUtilities.GetRectContainsPoints(uvPoints);
            if (!dirtyRect.Overlaps(new Rect(0, 0, 1, 1)))
                return;

            RenderTexture[] brushes = new RenderTexture[Layers.Count];
            for (int i = 0; i < Layers.Count; ++i)
            {
                brushes[i] = GetRenderTexture("brush" + i.ToString());
            }
            Internal_RenderBrushes(brushes, t, uvPoints);

            for (int i = 0; i < Layers.Count; ++i)
            {
                StampLayer(t, brushes[i], i);
            }

            bool willUpdateTree = Layers.Exists(l => l.StampTrees && l.TreeInstanceCount > 0);
            bool willUpdateGrass = Layers.Exists(l => l.StampGrasses && l.TreeInstanceCount > 0);

#if UNITY_EDITOR
            string title = "Stamping on " + t.name;
            string info = "Finishing up...";
            GCommonGUI.ProgressBar(title, info, 1);
#endif

            try
            {
                t.TerrainData.Foliage.SetTreeRegionDirty(dirtyRect);
                t.TerrainData.Foliage.SetGrassRegionDirty(dirtyRect);
                if (willUpdateTree)
                    t.UpdateTreesPosition();
                if (willUpdateGrass)
                    t.UpdateGrassPatches(-1, true);
                t.TerrainData.Foliage.ClearTreeDirtyRegions();
                t.TerrainData.Foliage.ClearGrassDirtyRegions();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }

#if UNITY_EDITOR
            GCommonGUI.ClearProgressBar();
#endif

            t.TerrainData.SetDirty(GTerrainData.DirtyFlags.Foliage);
        }

        private void StampLayer(GStylizedTerrain t, RenderTexture brush, int layerIndex)
        {
            GFoliageStampLayer layer = Layers[layerIndex];
            if (layer.Ignore)
                return;
            if (layer.TreeInstanceCount == 0)
                return;
            Texture2D tex = new Texture2D(brush.width, brush.height, TextureFormat.ARGB32, false, true);
            GCommon.CopyFromRT(tex, brush);
            Color[] maskData = tex.GetPixels();

            if (layer.StampTrees &&
                layer.TreeIndices.Count > 0 &&
                t.TerrainData.Foliage.Trees != null)
            {
                SpawnTreeOnTerrain(t, maskData, layerIndex);
            }

            if (layer.StampGrasses &&
                layer.GrassIndices.Count > 0 &&
                t.TerrainData.Foliage.Grasses != null)
            {
                SpawnGrassOnTerrain(t, maskData, layerIndex);
            }
        }

        private void SpawnTreeOnTerrain(GStylizedTerrain t, Color[] maskData, int layerIndex)
        {
            GFoliageStampLayer layer = Layers[layerIndex];
            Vector3 centerPos = Vector3.zero;
            Vector3 samplePos = Vector3.zero;
            Vector2 uv = Vector2.zero;
            float maskValue = 0;
            Vector3 terrainSize = new Vector3(
                t.TerrainData.Geometry.Width,
                t.TerrainData.Geometry.Height,
                t.TerrainData.Geometry.Length);
            Vector3 scale = new Vector3(
                GUtilities.InverseLerpUnclamped(0, terrainSize.x, Scale.x),
                1,
                GUtilities.InverseLerpUnclamped(0, terrainSize.z, Scale.z));
            Matrix4x4 matrix = Matrix4x4.TRS(
                t.WorldPointToNormalized(Position),
                Rotation,
                scale);

            int treeIndex = -1;
            int instanceCount = 0;
            int attempt = 0;
            int maxAttempt = layer.TreeInstanceCount * 100;

#if UNITY_EDITOR
            string title = "Stamping on " + t.name;
            string info = string.Format("Layer: {0}", !string.IsNullOrEmpty(layer.Name) ? layer.Name : layerIndex.ToString());
            int currentPercent = 0;
            int attemptPercent = 0;
            int instancePercent = 0;
            GCommonGUI.CancelableProgressBar(title, info, 0);
#endif

            int prototypeCount = t.TerrainData.Foliage.Trees.Prototypes.Count;
            List<GTreeInstance> newInstances = new List<GTreeInstance>();
            while (instanceCount < layer.TreeInstanceCount && attempt <= maxAttempt)
            {
                attempt += 1;

#if UNITY_EDITOR
                attemptPercent = (int)(attempt * 100.0f / maxAttempt);
                instancePercent = (int)(instanceCount * 100.0f / layer.TreeInstanceCount);
                if (currentPercent != Mathf.Max(attemptPercent, instancePercent))
                {
                    currentPercent = Mathf.Max(attemptPercent, instancePercent);
                    GCommonGUI.CancelableProgressBar(title, string.Format("{0} ... {1}%", info, currentPercent), currentPercent / 100.0f);
                }
#endif

                treeIndex = layer.TreeIndices[Random.Range(0, layer.TreeIndices.Count)];
                if (treeIndex < 0 || treeIndex >= prototypeCount)
                    continue;

                centerPos.Set(Random.value - 0.5f, 0, Random.value - 0.5f);
                samplePos = matrix.MultiplyPoint(centerPos);
                if (samplePos.x < 0 || samplePos.x > 1 ||
                    samplePos.z < 0 || samplePos.z > 1)
                    continue;
                uv.Set(samplePos.x, samplePos.z);
                maskValue = GUtilities.GetColorBilinear(maskData, MaskResolution, MaskResolution, uv).r;
                if (Random.value > maskValue)
                    continue;

                GTreeInstance tree = GTreeInstance.Create(treeIndex);
                tree.Position = new Vector3(samplePos.x, 0, samplePos.z);
                tree.Rotation = Quaternion.Euler(0, Random.Range(layer.MinRotation, layer.MaxRotation), 0);
                tree.Scale = Vector3.Lerp(layer.MinScale, layer.MaxScale, Random.value);

                newInstances.Add(tree);
                instanceCount += 1;
            }
            t.TerrainData.Foliage.AddTreeInstances(newInstances);

#if UNITY_EDITOR
            GCommonGUI.ClearProgressBar();
#endif
        }

        private void SpawnGrassOnTerrain(GStylizedTerrain t, Color[] maskData, int layerIndex)
        {
            GFoliageStampLayer layer = Layers[layerIndex];
            Vector3 centerPos = Vector3.zero;
            Vector3 samplePos = Vector3.zero;
            Vector2 uv = Vector2.zero;
            float maskValue = 0;
            Vector3 terrainSize = new Vector3(
                t.TerrainData.Geometry.Width,
                t.TerrainData.Geometry.Height,
                t.TerrainData.Geometry.Length);
            Vector3 scale = new Vector3(
                GUtilities.InverseLerpUnclamped(0, terrainSize.x, Scale.x),
                1,
                GUtilities.InverseLerpUnclamped(0, terrainSize.z, Scale.z));
            Matrix4x4 matrix = Matrix4x4.TRS(
                t.WorldPointToNormalized(Position),
                Rotation,
                scale);

            int grassIndex = -1;
            int instanceCount = 0;
            int attempt = 0;
            int maxAttempt = layer.GrassInstanceCount * 100;
            List<GGrassInstance> grasses = new List<GGrassInstance>();

#if UNITY_EDITOR
            string title = "Stamping on " + t.name;
            string info = string.Format("Layer: {0}", !string.IsNullOrEmpty(layer.Name) ? layer.Name : layerIndex.ToString());
            int currentPercent = 0;
            int attemptPercent = 0;
            int instancePercent = 0;
            GCommonGUI.CancelableProgressBar(title, info, 0);
#endif

            int prototypeCount = t.TerrainData.Foliage.Grasses.Prototypes.Count;
            while (instanceCount < layer.GrassInstanceCount && attempt <= maxAttempt)
            {
                attempt += 1;

#if UNITY_EDITOR
                attemptPercent = (int)(attempt * 100.0f / maxAttempt);
                instancePercent = (int)(instanceCount * 100.0f / layer.GrassInstanceCount);
                if (currentPercent != Mathf.Max(attemptPercent, instancePercent))
                {
                    currentPercent = Mathf.Max(attemptPercent, instancePercent);
                    GCommonGUI.CancelableProgressBar(title, string.Format("{0} ... {1}%", info, currentPercent), currentPercent / 100.0f);
                }
#endif

                grassIndex = layer.GrassIndices[Random.Range(0, layer.GrassIndices.Count)];
                if (grassIndex < 0 || grassIndex >= prototypeCount)
                    continue;

                centerPos.Set(Random.value - 0.5f, 0, Random.value - 0.5f);
                samplePos = matrix.MultiplyPoint(centerPos);
                if (samplePos.x < 0 || samplePos.x > 1 ||
                    samplePos.z < 0 || samplePos.z > 1)
                    continue;
                uv.Set(samplePos.x, samplePos.z);
                maskValue = GUtilities.GetColorBilinear(maskData, MaskResolution, MaskResolution, uv).r;
                if (Random.value > maskValue)
                    continue;

                GGrassInstance grass = GGrassInstance.Create(grassIndex);
                grass.Position = new Vector3(samplePos.x, 0, samplePos.z);
                grass.Rotation = Quaternion.Euler(0, Random.Range(layer.MinRotation, layer.MaxRotation), 0);
                grass.Scale = Vector3.Lerp(layer.MinScale, layer.MaxScale, Random.value);

                grasses.Add(grass);
                instanceCount += 1;
            }

            t.TerrainData.Foliage.AddGrassInstances(grasses);

#if UNITY_EDITOR
            GCommonGUI.ClearProgressBar();
#endif
        }

        public void Internal_RenderBrushes(RenderTexture[] brushes, GStylizedTerrain t, Vector2[] uvPoints)
        {
            for (int i = 0; i < brushes.Length; ++i)
            {
                GStampLayerMaskRenderer.Render(
                    brushes[i],
                    Layers[i],
                    t,
                    Matrix4x4.TRS(Position, Rotation, Scale),
                    Mask,
                    falloffTexture,
                    uvPoints,
                    EnableTerrainMask);
            }
        }

        public void Internal_UpdateFalloffTexture()
        {
            if (falloffTexture != null)
                GUtilities.DestroyObject(falloffTexture);
            falloffTexture = GCommon.CreateTextureFromCurve(Falloff, 256, 1);
        }

        public void Internal_UpdateLayerTransitionTextures()
        {
            for (int i = 0; i < Layers.Count; ++i)
            {
                Layers[i].UpdateCurveTextures();
            }
        }

        public Vector3[] GetQuad()
        {
            Matrix4x4 matrix = Matrix4x4.TRS(Position, Rotation, Scale);
            Vector3[] quad = new Vector3[4]
            {
                matrix.MultiplyPoint(new Vector3(-0.5f, 0, -0.5f)),
                matrix.MultiplyPoint(new Vector3(-0.5f, 0, 0.5f)),
                matrix.MultiplyPoint(new Vector3(0.5f, 0, 0.5f)),
                matrix.MultiplyPoint(new Vector3(0.5f, 0, -0.5f))
            };

            return quad;
        }

        public void GetQuad(Vector3[] quad)
        {
            Matrix4x4 matrix = Matrix4x4.TRS(Position, Rotation, Scale);
            quad[0] = matrix.MultiplyPoint(new Vector3(-0.5f, 0, -0.5f));
            quad[1] = matrix.MultiplyPoint(new Vector3(-0.5f, 0, 0.5f));
            quad[2] = matrix.MultiplyPoint(new Vector3(0.5f, 0, 0.5f));
            quad[3] = matrix.MultiplyPoint(new Vector3(0.5f, 0, -0.5f));
        }

        private void GetUvPoints(GStylizedTerrain t, Vector3[] worldPoint, Vector2[] uvPoint)
        {
            for (int i = 0; i < uvPoints.Length; ++i)
            {
                uvPoints[i] = t.WorldPointToUV(worldPoints[i]);
            }
        }

        public void GetBox(Vector3[] box)
        {
            Matrix4x4 matrix = Matrix4x4.TRS(Position, Rotation, Scale);
            box[0] = matrix.MultiplyPoint(new Vector3(-0.5f, 0, -0.5f));
            box[1] = matrix.MultiplyPoint(new Vector3(-0.5f, 0, 0.5f));
            box[2] = matrix.MultiplyPoint(new Vector3(0.5f, 0, 0.5f));
            box[3] = matrix.MultiplyPoint(new Vector3(0.5f, 0, -0.5f));
            box[4] = matrix.MultiplyPoint(new Vector3(-0.5f, 1, -0.5f));
            box[5] = matrix.MultiplyPoint(new Vector3(-0.5f, 1, 0.5f));
            box[6] = matrix.MultiplyPoint(new Vector3(0.5f, 1, 0.5f));
            box[7] = matrix.MultiplyPoint(new Vector3(0.5f, 1, -0.5f));
        }

        private RenderTexture GetRenderTexture(string key)
        {
            int resolution = MaskResolution;
            if (!TempRt.ContainsKey(key) ||
                TempRt[key] == null)
            {
                RenderTexture rt = new RenderTexture(resolution, resolution, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
                rt.wrapMode = TextureWrapMode.Clamp;
                TempRt[key] = rt;
            }
            else if (TempRt[key].width != resolution || TempRt[key].height != resolution)
            {
                TempRt[key].Release();
                Object.DestroyImmediate(TempRt[key]);
                RenderTexture rt = new RenderTexture(resolution, resolution, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
                rt.wrapMode = TextureWrapMode.Clamp;
                TempRt[key] = rt;
            }

            return TempRt[key];
        }

        public void ClearTrees()
        {
            IEnumerator<GStylizedTerrain> terrains = GStylizedTerrain.ActiveTerrains.GetEnumerator();
            while (terrains.MoveNext())
            {
                GStylizedTerrain t = terrains.Current;
                if (groupId < 0 ||
                    (groupId >= 0 && groupId == t.GroupId))
                {
                    ClearTrees(t);
                }
            }
        }

        private void ClearTrees(GStylizedTerrain t)
        {
            if (t.TerrainData == null)
                return;
            if (t.TerrainData.Foliage.Trees == null)
                return;
            Vector3 terrainSize = new Vector3(
                t.TerrainData.Geometry.Width,
                t.TerrainData.Geometry.Height,
                t.TerrainData.Geometry.Length);
            Vector3 scale = new Vector3(
                GUtilities.InverseLerpUnclamped(0, terrainSize.x, Scale.x),
                GUtilities.InverseLerpUnclamped(0, terrainSize.y, Scale.y),
                GUtilities.InverseLerpUnclamped(0, terrainSize.z, Scale.z));
            Matrix4x4 matrix = Matrix4x4.TRS(
                t.WorldPointToNormalized(Position),
                Rotation,
                scale);
            Matrix4x4 normalizeToStamp = matrix.inverse;

            t.TerrainData.Foliage.RemoveTreeInstances(tree =>
            {
                Vector3 stampSpacePos = normalizeToStamp.MultiplyPoint(tree.Position);
                return
                    stampSpacePos.x >= -0.5f && stampSpacePos.x <= 0.5f &&
                    stampSpacePos.y >= 0f && stampSpacePos.y <= 1f &&
                    stampSpacePos.z >= -0.5f && stampSpacePos.z <= 0.5f;
            });

            t.TerrainData.SetDirty(GTerrainData.DirtyFlags.Foliage);
        }

        public void ClearGrasses()
        {
            IEnumerator<GStylizedTerrain> terrains = GStylizedTerrain.ActiveTerrains.GetEnumerator();
            while (terrains.MoveNext())
            {
                GStylizedTerrain t = terrains.Current;
                if (groupId < 0 ||
                    (groupId >= 0 && groupId == t.GroupId))
                {
                    ClearGrasses(t);
                }
            }
        }

        private void ClearGrasses(GStylizedTerrain t)
        {
            if (t.TerrainData == null)
                return;
            if (t.TerrainData.Foliage.Grasses == null)
                return;
            Vector3 terrainSize = new Vector3(
                t.TerrainData.Geometry.Width,
                t.TerrainData.Geometry.Height,
                t.TerrainData.Geometry.Length);
            Vector3 scale = new Vector3(
                GUtilities.InverseLerpUnclamped(0, terrainSize.x, Scale.x),
                GUtilities.InverseLerpUnclamped(0, terrainSize.y, Scale.y),
                GUtilities.InverseLerpUnclamped(0, terrainSize.z, Scale.z));
            Matrix4x4 matrix = Matrix4x4.TRS(
                t.WorldPointToNormalized(Position),
                Rotation,
                scale);
            Matrix4x4 normalizeToStamp = matrix.inverse;

            GGrassPatch[] patches = t.TerrainData.Foliage.GrassPatches;
            for (int i = 0; i < patches.Length; ++i)
            {
                patches[i].RemoveInstances(grass =>
                {
                    Vector3 stampSpacePos = normalizeToStamp.MultiplyPoint(grass.Position);
                    return
                        stampSpacePos.x >= -0.5f && stampSpacePos.x <= 0.5f &&
                        stampSpacePos.y >= 0f && stampSpacePos.y <= 1f &&
                        stampSpacePos.z >= -0.5f && stampSpacePos.z <= 0.5f;
                });
            }
            t.TerrainData.SetDirty(GTerrainData.DirtyFlags.Foliage);
        }
    }
}