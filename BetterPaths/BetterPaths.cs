using BepInEx;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

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
			MinimapManager.OnVanillaMapDataLoaded += TestMapOverlay;
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.F8))
			{
				TestMapOverlay();
			}
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
				CraftingStation = CraftingStations.None,
			};
			path.AddRequirement(new RequirementConfig("Stone", 1));
			CustomPiece stonePathen = new CustomPiece("Stone Pathen", "paved_road_v2", path);
			stonePathen.Piece.m_craftingStation = null;
			Jotunn.Logger.LogDebug($"Crafting station: {stonePathen.Piece.m_craftingStation}");


			PieceManager.Instance.AddPiece(stonePathen);

			PrefabManager.OnVanillaPrefabsAvailable -= AddClonedItems;
		}

		private void TestMapOverlay()
		{
			MinimapManager.MapOverlay overlay = MinimapManager.Instance.GetMapOverlay("Paths");
			int mapSize = overlay.TextureSize * overlay.TextureSize;

			List<TerrainComp> tcs = TerrainComp.s_instances;
			foreach (TerrainComp tc in tcs)
			{
				
			}

			Color[] mainPixels = new Color[mapSize];
			
			foreach (Piece p in Piece.s_allPieces)
			{
				//Vector2 coords = MinimapManager.Instance.WorldToOverlayCoords(p.GetCenter(), overlay.TextureSize);
				//overlay.OverlayTex.SetPixel((int)coords.x, (int)coords.y, Color.red);
				//Jotunn.Logger.LogInfo($"Marking piece {p.m_name} at {p.GetCenter()}, or {p.transform.position}. This is at {coords} on the minimap.");

				if (p.m_groundPiece)
				{
					Vector2 coords = MinimapManager.Instance.WorldToOverlayCoords(p.transform.position, overlay.TextureSize);
					overlay.OverlayTex.SetPixel((int)coords.x, (int)coords.y, Color.red);
					Jotunn.Logger.LogInfo($"Marking piece {p.m_name} at {p.transform.position}. This is at {coords} on the minimap.");

					//int index = (int)(coords.x * coords.y);
					//mainPixels[index] = Color.red;
				}
			}

			//overlay.OverlayTex.SetPixels(mainPixels);
			overlay.OverlayTex.Apply();
		}
	}
}


