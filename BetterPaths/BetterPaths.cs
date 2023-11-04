using BepInEx;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;

namespace BetterPaths
{
	[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
	[BepInDependency(Jotunn.Main.ModGuid)]
	//[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
	internal class BetterPaths : BaseUnityPlugin
	{
		public const string PluginGUID = "com.foxthorne.BetterPaths";
		public const string PluginName = "BetterPaths";
		public const string PluginVersion = "0.0.1";
		
		// Use this class to add your own localization to the game
		// https://valheim-modding.github.io/Jotunn/tutorials/localization.html
		public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

		private void Awake()
		{
			PrefabManager.OnVanillaPrefabsAvailable += AddClonedItems;
		}

		private void AddClonedItems()
		{
			// Pathing Stone
			ItemConfig pathingStoneConfig = new ItemConfig
			{
				Name = "$item_pathingStone",
				Description = "$item_pathingStone_desc",
				CraftingStation = CraftingStations.Stonecutter
			};
			pathingStoneConfig.AddRequirement(new RequirementConfig("Stone", 1));

			CustomItem pathingStone = new CustomItem("PathingStone", "Stone", pathingStoneConfig);
			ItemManager.Instance.AddItem(pathingStone);

			// Stone Pathing
			PieceConfig path = new PieceConfig
			{
				Name = "piece_stonePathen",
				PieceTable = PieceTables.Hoe,
				Category = PieceCategories.Misc,
				CraftingStation = CraftingStations.None
			};
			path.AddRequirement(new RequirementConfig("PathingStone", 1));

			PieceManager.Instance.AddPiece(new CustomPiece("Stone Pathen", "paved_road_v2", path));

			PrefabManager.OnVanillaPrefabsAvailable -= AddClonedItems;
		}
	}
}


