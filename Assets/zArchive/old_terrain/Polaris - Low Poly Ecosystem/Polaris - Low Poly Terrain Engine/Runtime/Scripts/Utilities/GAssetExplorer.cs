using UnityEngine;

namespace Pinwheel.Griffin
{
    public class GAssetExplorer : MonoBehaviour
    {
        public static string PINWHEEL_ASSETS_LINK = "https://assetstore.unity.com/publishers/17305?aid=1100l3QbW&pubref=p2-editor";
        public static string VEGETATION_LINK = "https://assetstore.unity.com/lists/stylized-vegetation-120082?aid=1100l3QbW&pubref=p2-editor";
        public static string ROCK_PROPS_LINK = "https://assetstore.unity.com/lists/stylized-rock-props-120083?aid=1100l3QbW&pubref=p2-editor";
        public static string WATER_LINK = "https://assetstore.unity.com/lists/stylized-water-120085?aid=1100l3QbW&pubref=p2-editor";
        public static string SKY_AMBIENT_LINK = "https://assetstore.unity.com/lists/stylized-sky-and-ambient-120088?aid=1100l3QbW&pubref=p2-editor";
        public static string CHARACTER_LINK = "https://assetstore.unity.com/lists/stylized-character-120084?aid=1100l3QbW&pubref=p2-editor";

        public static string POLARIS_LINK = "https://assetstore.unity.com/packages/slug/170400?aid=1100l3QbW&pubref=p2-editor";
        public static string POSEIDON_LINK = "https://assetstore.unity.com/packages/vfx/shaders/substances/poseidon-low-poly-water-system-builtin-lwrp-153826?aid=1100l3QbW&pubref=p2-editor";
        public static string JUPITER_LINK = "https://assetstore.unity.com/packages/slug/159992?aid=1100l3QbW&pubref=p2-editor";
        public static string CSHARP_WIZARD_LINK = "https://assetstore.unity.com/packages/slug/104887";
        public static string MESH_TO_FILE_LINK = "https://assetstore.unity.com/packages/slug/135071";

        public static void ShowPolarisLink()
        {
            Application.OpenURL(POLARIS_LINK);
        }

        public static void ShowPoseidonLink()
        {
            Application.OpenURL(POSEIDON_LINK);
        }

        public static void ShowJupiterLink()
        {
            Application.OpenURL(JUPITER_LINK);
        }

        public static void ShowCSharpWizardLink()
        {
            Application.OpenURL(CSHARP_WIZARD_LINK);
        }

        public static void ShowMeshToFileLink()
        {
            Application.OpenURL(MESH_TO_FILE_LINK);
        }

        public static void ShowPinwheelAssets()
        {
            Application.OpenURL(PINWHEEL_ASSETS_LINK);
        }

        public static void ShowVegetationLink()
        {
            Application.OpenURL(VEGETATION_LINK);
        }

        public static void ShowRockPropsLink()
        {
            Application.OpenURL(ROCK_PROPS_LINK);
        }

        public static void ShowWaterLink()
        {
            Application.OpenURL(WATER_LINK);
        }

        public static void ShowSkyAmbientLink()
        {
            Application.OpenURL(SKY_AMBIENT_LINK);
        }

        public static void ShowCharacterLink()
        {
            Application.OpenURL(CHARACTER_LINK);
        }
    }
}
