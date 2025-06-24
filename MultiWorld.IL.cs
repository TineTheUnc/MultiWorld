using MonoMod.Cil;
using MultiWorld.Common.Systems;
using MultiWorld.Common.Type;
using System;
using System.Reflection;
using Mono.Cecil.Cil;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework;

namespace MultiWorld
{
	public partial class MultiWorld
	{
		private void LoadIL() {
			IL_WorldGen.SaveAndQuitCallBack += Il_SaveAndQuitCallBack;
			IL_WorldGen.do_worldGenCallBack += IL_do_worldGenCallBack;
			IL_UIWorldListItem.ctor += IL_UIWorldListItem_ctor;
			IL_UIWorldListItem.DrawSelf += IL_UIWorldListItem_DrawSelf;
			IL_UIWorldListItem.SetColorsToHovered += IL_SetColorsToHovered;
			IL_UIWorldListItem.SetColorsToNotHovered += IL_SetColorsToNotHovered;
			IL_Player.Spawn += IL_Player_Spawn;
			IL_UIWorldCreation.MakeInfoMenu += IL_UIWorldCreation_MakeInfoMenu;
			IL_UIWorldCreation.MakeBackAndCreatebuttons += IL_UIWorldCreation_MakeBackAndCreatebuttons;
			IL_Player.ItemCheck_Inner += IL_Player_ItemCheck_Inner;
			IL_WorldGen.SpreadGrass += IL_WorldGen_SpreadGrass;
		}

		private void UnloadIL()
		{
			IL_WorldGen.SaveAndQuitCallBack -= Il_SaveAndQuitCallBack;
			IL_WorldGen.do_worldGenCallBack -= IL_do_worldGenCallBack;
			IL_UIWorldListItem.ctor -= IL_UIWorldListItem_ctor;
			IL_UIWorldListItem.DrawSelf -= IL_UIWorldListItem_DrawSelf;
			IL_UIWorldListItem.SetColorsToHovered -= IL_SetColorsToHovered;
			IL_UIWorldListItem.SetColorsToNotHovered -= IL_SetColorsToNotHovered;
			IL_Player.Spawn -= IL_Player_Spawn;
			IL_UIWorldCreation.MakeInfoMenu -= IL_UIWorldCreation_MakeInfoMenu;
			IL_UIWorldCreation.MakeBackAndCreatebuttons -= IL_UIWorldCreation_MakeBackAndCreatebuttons;
			IL_Player.ItemCheck_Inner -= IL_Player_ItemCheck_Inner;
			IL_WorldGen.SpreadGrass -= IL_WorldGen_SpreadGrass;

		}

		private void DebugIL(ILCursor ilCursor)
		{
			Logger.Debug("-- DebugIL --");
			Logger.Debug(ilCursor.Body.Method.Name);
			Logger.Debug(ilCursor.Body.Instructions[ilCursor.Index].OpCode.Code.ToString() + " " + ilCursor.Body.Instructions[ilCursor.Index].Operand.ToString());
		}

		private void IL_WorldGen_SpreadGrass(ILContext il)
		{
			try
			{
				var ilCursor = new ILCursor(il);
				ilCursor.GotoNext(i => i.MatchVolatile());
				ilCursor.Index+=2;
				ilCursor.EmitDelegate<Func<bool>>(() =>
				{
					return !MultiWorldFileData.IsMultiWorld(Main.ActiveWorldFileData.Path);
				});
				ilCursor.EmitAnd();
			}
			catch (Exception e)
			{
				MonoModHooks.DumpIL(ModContent.GetInstance<MultiWorld>(), il);
				throw new ILPatchFailureException(ModContent.GetInstance<MultiWorld>(), il, e);
			}
		}

		private void IL_Player_ItemCheck_Inner(ILContext il)
		{
			try
			{
				var ilCursor = new ILCursor(il);
				ilCursor.GotoNext(i => i.MatchCall(typeof(Player).GetMethod("Spawn", BindingFlags.Instance | BindingFlags.Public)));
				ilCursor.Index--;
				ilCursor.RemoveRange(28);
				ilCursor.EmitDelegate<Action<Player>>((self) =>
				{
					if (MultiWorldFileData.IsMultiWorld(Main.ActiveWorldFileData.Path))
					{
						var System = ModContent.GetInstance<WorldManageSystem>();
						var data = MultiWorldFileData.LoadMeta(Path.Combine(Path.GetDirectoryName(Main.ActiveWorldFileData.Path), "meta.world"));
						var entityIdInfo = self.GetType().GetField("entityId", BindingFlags.Instance | BindingFlags.NonPublic);
						var entityId = (long)entityIdInfo.GetValue(self);
						System.NextWorldIndex = 0;
						if (data.spawnPoint != null)
						{
							if (data.spawnPoint.TryGetValue(entityId, out int value)) {
								System.NextWorldIndex = value;
							}
						}
						if (System.NextWorldIndex.ToString() == Path.GetFileNameWithoutExtension(Main.ActiveWorldFileData.Path))
						{
							self.Spawn(PlayerSpawnContext.RecallFromItem);
							for (int m = 0; m < 70; m++)
							{
								Dust.NewDust(self.position, self.width, self.height, DustID.MagicMirror, 0f, 0f, 150, default(Color), 1.5f);
							}
						}
						else
						{
							System.CurrentWorldIndex = int.Parse(Path.GetFileNameWithoutExtension(Main.ActiveWorldFileData.Path));
							System.do_respawn = true;
							System.do_recall = true;
							System.ChangeWorld();
						}
					}
					else
					{
						self.Spawn(PlayerSpawnContext.RecallFromItem);
						for (int m = 0; m < 70; m++)
						{
							Dust.NewDust(self.position, self.width, self.height, DustID.MagicMirror, 0f, 0f, 150, default(Color), 1.5f);
						}
					}

				});
				ilCursor.GotoNext(i => i.MatchCall(typeof(Player).GetMethod("Shellphone_Spawn", BindingFlags.Instance | BindingFlags.Public)));
				ilCursor.Remove();
				ilCursor.EmitDelegate<Action<Player>>((self) =>
				{
					if (MultiWorldFileData.IsMultiWorld(Main.ActiveWorldFileData.Path))
					{
						var System = ModContent.GetInstance<WorldManageSystem>();
						if ("0" == Path.GetFileNameWithoutExtension(Main.ActiveWorldFileData.Path))
						{
							self.Shellphone_Spawn();
						}
						else
						{
							System.CurrentWorldIndex = int.Parse(Path.GetFileNameWithoutExtension(Main.ActiveWorldFileData.Path));
							System.NextWorldIndex = 0;
							System.do_respawn = true;
							System.do_phone = true;
							System.ChangeWorld();
						}
					}
					else
					{
						self.Shellphone_Spawn();
					}
				});
			}
			catch (Exception e)
			{
				MonoModHooks.DumpIL(ModContent.GetInstance<MultiWorld>(), il);
				throw new ILPatchFailureException(ModContent.GetInstance<MultiWorld>(), il, e);
			}
		}



		private void IL_UIWorldCreation_MakeBackAndCreatebuttons(ILContext il)
		{
			try
			{
				var ilCursor = new ILCursor(il);
				ilCursor.GotoNext(i => i.MatchLdcR4(-45));
				ilCursor.Remove();
				ilCursor.EmitLdcR4(15);
				ilCursor.GotoNext(i => i.MatchLdcR4(-45));
				ilCursor.Remove();
				ilCursor.EmitLdcR4(15);
			}
			catch (Exception e)
			{
				MonoModHooks.DumpIL(ModContent.GetInstance<MultiWorld>(), il);
				throw new ILPatchFailureException(ModContent.GetInstance<MultiWorld>(), il, e);
			}
		}

		private void IL_UIWorldCreation_MakeInfoMenu(ILContext il)
		{
			try
			{
				var ilCursor = new ILCursor(il);
				ilCursor.GotoNext(i => i.MatchLdstr("desc"));
				ilCursor.Index -= 3;
				ilCursor.EmitLdloc0();
				ilCursor.EmitLdloc1();
				ilCursor.EmitCall(typeof(MultiWorld).GetMethod(nameof(AddMultiWorldMenu), BindingFlags.Static | BindingFlags.NonPublic));
				ilCursor.EmitLdloc1();
				ilCursor.EmitLdcR4(54);
				ilCursor.EmitAdd();
				ilCursor.EmitStloc1();
				ilCursor.EmitLdloc0();
				ilCursor.EmitLdloc1();
				ilCursor.EmitCall(typeof(UIWorldCreation).GetMethod("AddHorizontalSeparator", BindingFlags.Static | BindingFlags.NonPublic));
			}
			catch (Exception e)
			{
				MonoModHooks.DumpIL(ModContent.GetInstance<MultiWorld>(), il);
				throw new ILPatchFailureException(ModContent.GetInstance<MultiWorld>(), il, e);
			}
		}

		
		private void IL_Player_Spawn(ILContext il)
		{
			try
			{
				var ilCursor = new ILCursor(il);
				ilCursor.GotoNext(i => i.MatchStloc1());
				ilCursor.GotoNext(i => i.MatchStfld(typeof(Entity).GetField("wet", BindingFlags.Public | BindingFlags.Instance)));
				ilCursor.Index++;
				ilCursor.EmitLdarg0();
				ilCursor.EmitDelegate<Action<Player>>((player) =>
				{
					if (MultiWorldFileData.IsMultiWorld(Main.ActiveWorldFileData.Path))
					{
						var System = ModContent.GetInstance<WorldManageSystem>();
						if (System.do_chaneWorld && !System.do_respawn)
						{
							var index = int.Parse(Path.GetFileNameWithoutExtension(Main.ActiveWorldFileData.Path));
							int i = (System.playerY / 100) * 100;
							if (i <= 0)
							{
								i = 10;
							}
							else if (i >= Main.maxTilesX)
							{
								i = Main.maxTilesX - 10;
							}
							if (Math.Abs(System.NextWorldIndex) == Math.Abs(System.Radius) && System.Radius > 0 && System.CurrentWorldIndex != 0)
							{
								if (System.NextWorldIndex < 0)
								{
									player.position.X = (float)(50 * 16 + 8 - player.width / 2);
									while (!CanSpawn(50, i)) { i++; }
									player.position.Y = (float)(i * 16 - player.height);
								}
								else if (System.NextWorldIndex > 0)
								{
									player.position.X = (float)((Main.maxTilesX - 50) * 16 + 8 - player.width / 2);
									while (!CanSpawn(Main.maxTilesX - 50, i)) { i++; }
									player.position.Y = (float)(i * 16 - player.height);
								}
							}
							else if (System.CurrentWorldIndex > System.NextWorldIndex)
							{
								player.position.X = (float)((Main.maxTilesX - 50) * 16 + 8 - player.width / 2);
								while (!CanSpawn(Main.maxTilesX - 50, i)) { i++; }
								player.position.Y = (float)(i * 16 - player.height);
							}
							else if (System.CurrentWorldIndex < System.NextWorldIndex)
							{
								player.position.X = (float)(50 * 16 + 8 - player.width / 2);
								while (!CanSpawn(50, i)) { i++; }
								player.position.Y = (float)(i * 16 - player.height);
							}
						}
						else if (System.do_recall)
						{
							for (int m = 0; m < 70; m++)
							{
								Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, 0f, 0f, 150, default(Color), 1.5f);
							}
							System.do_recall = false;
							System.do_respawn = false;
						}
						else if (System.do_phone)
						{
							player.Shellphone_Spawn();
							System.do_phone = false;
							System.do_respawn = false;
						}
					}
				});

			}
			catch (Exception e)
			{
				MonoModHooks.DumpIL(ModContent.GetInstance<MultiWorld>(), il);
				throw new ILPatchFailureException(ModContent.GetInstance<MultiWorld>(), il, e);
			}
		}

		private void IL_SetColorsToNotHovered(ILContext il)
		{
			try
			{
				var ilCursor = new ILCursor(il);
				var label1 = il.DefineLabel();
				var label2 = il.DefineLabel();
				ilCursor.GotoNext(i => i.MatchLdarg0());
				ilCursor.GotoNext(i => i.MatchLdarg0());
				ilCursor.GotoNext(i => i.MatchLdarg0());
				ilCursor.EmitLdarg0();
				ilCursor.EmitLdfld(typeof(AWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic));
				ilCursor.EmitCallvirt(typeof(FileData).GetMethod("get_Path", BindingFlags.Instance | BindingFlags.Public));
				ilCursor.EmitCall(typeof(MultiWorldFileData).GetMethod("IsMultiWorld", BindingFlags.Public | BindingFlags.Static));
				ilCursor.Emit(OpCodes.Brfalse, label1);
				ilCursor.EmitLdarg0();
				ilCursor.EmitLdcI4(63);
				ilCursor.EmitLdcI4(151);
				ilCursor.EmitLdcI4(82);
				ilCursor.EmitNewobj(typeof(Color).GetConstructor([typeof(int), typeof(int), typeof(int)]));
				ilCursor.EmitStfld(typeof(UIPanel).GetField("BackgroundColor", BindingFlags.Instance | BindingFlags.Public));
				ilCursor.EmitLdarg0();
				ilCursor.EmitLdcI4(89);
				ilCursor.EmitLdcI4(213);
				ilCursor.EmitLdcI4(116);
				ilCursor.EmitNewobj(typeof(Color).GetConstructor([typeof(int), typeof(int), typeof(int)]));
				ilCursor.EmitStfld(typeof(UIPanel).GetField("BorderColor", BindingFlags.Instance | BindingFlags.Public));
				ilCursor.GotoNext(i => i.MatchLdarg0());
				ilCursor.MarkLabel(label1);
				ilCursor.GotoNext(i => i.MatchRet());
				ilCursor.EmitLdarg0();
				ilCursor.EmitLdfld(typeof(AWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic));
				ilCursor.EmitCallvirt(typeof(FileData).GetMethod("get_Path", BindingFlags.Instance | BindingFlags.Public));
				ilCursor.EmitCall(typeof(MultiWorldFileData).GetMethod("IsMultiWorld", BindingFlags.Public | BindingFlags.Static));
				ilCursor.Emit(OpCodes.Brfalse, label2);
				ilCursor.EmitLdarg0();
				ilCursor.EmitLdcI4(63);
				ilCursor.EmitLdcI4(151);
				ilCursor.EmitLdcI4(82);
				ilCursor.EmitNewobj(typeof(Color).GetConstructor([typeof(int), typeof(int), typeof(int)]));
				ilCursor.EmitLdcI4(80);
				ilCursor.EmitLdcI4(80);
				ilCursor.EmitLdcI4(80);
				ilCursor.EmitNewobj(typeof(Color).GetConstructor([typeof(int), typeof(int), typeof(int)]));
				ilCursor.EmitLdcR4(0.5f);
				ilCursor.EmitCall(typeof(Color).GetMethod("Lerp", BindingFlags.Static | BindingFlags.Public, null, [typeof(Color), typeof(Color), typeof(float)], null));
				ilCursor.EmitLdcR4(0.7f);
				ilCursor.EmitCall(typeof(Color).GetMethod("op_Multiply", BindingFlags.Static | BindingFlags.Public, null, [typeof(Color), typeof(float)], null));
				ilCursor.EmitStfld(typeof(UIPanel).GetField("BackgroundColor", BindingFlags.Instance | BindingFlags.Public));
				ilCursor.GotoNext(i => i.MatchRet());
				ilCursor.MarkLabel(label2);
			}
			catch (Exception e)
			{
				MonoModHooks.DumpIL(ModContent.GetInstance<MultiWorld>(), il);
				throw new ILPatchFailureException(ModContent.GetInstance<MultiWorld>(), il, e);
			}
		}

		private void IL_SetColorsToHovered(ILContext il)
		{
			try
			{
				var ilCursor = new ILCursor(il);
				var label1 = il.DefineLabel();
				ilCursor.GotoNext(i => i.MatchLdarg0());
				ilCursor.GotoNext(i => i.MatchLdarg0());
				ilCursor.GotoNext(i => i.MatchLdarg0());
				ilCursor.EmitLdarg0();
				ilCursor.EmitLdfld(typeof(AWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic));
				ilCursor.EmitCallvirt(typeof(FileData).GetMethod("get_Path", BindingFlags.Instance | BindingFlags.Public));
				ilCursor.EmitCall(typeof(MultiWorldFileData).GetMethod("IsMultiWorld", BindingFlags.Public | BindingFlags.Static));
				ilCursor.Emit(OpCodes.Brfalse, label1);
				ilCursor.EmitLdarg0();
				ilCursor.EmitLdcI4(72);
				ilCursor.EmitLdcI4(171);
				ilCursor.EmitLdcI4(94);
				ilCursor.EmitNewobj(typeof(Color).GetConstructor([typeof(int), typeof(int), typeof(int)]));
				ilCursor.EmitStfld(typeof(UIPanel).GetField("BackgroundColor", BindingFlags.Instance | BindingFlags.Public));
				ilCursor.EmitLdarg0();
				ilCursor.EmitLdcI4(89);
				ilCursor.EmitLdcI4(213);
				ilCursor.EmitLdcI4(116);
				ilCursor.EmitNewobj(typeof(Color).GetConstructor([typeof(int), typeof(int), typeof(int)]));
				ilCursor.EmitStfld(typeof(UIPanel).GetField("BorderColor", BindingFlags.Instance | BindingFlags.Public));
				ilCursor.GotoNext(i => i.MatchLdarg0());
				ilCursor.MarkLabel(label1);
			}
			catch (Exception e)
			{
				MonoModHooks.DumpIL(ModContent.GetInstance<MultiWorld>(), il);
				throw new ILPatchFailureException(ModContent.GetInstance<MultiWorld>(), il, e);
			}
		}

		private void IL_UIWorldListItem_DrawSelf(ILContext il)
		{
			try
			{
				var ilCursor = new ILCursor(il);
				var label1 = il.DefineLabel();
				ilCursor.GotoNext(i => i.MatchStloc(4));
				ilCursor.Index++;
				ilCursor.EmitLdarg0();
				ilCursor.EmitLdfld(typeof(AWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic));
				ilCursor.EmitCallvirt(typeof(FileData).GetMethod("get_Path", BindingFlags.Instance | BindingFlags.Public));
				ilCursor.EmitCall(typeof(MultiWorldFileData).GetMethod("IsMultiWorld", BindingFlags.Public | BindingFlags.Static));
				ilCursor.Emit(OpCodes.Brfalse, label1);
				ilCursor.EmitLdloc(4);
				ilCursor.EmitLdstr(" [Multi World] ");
				ilCursor.EmitCall(typeof(string).GetMethod("Concat", [typeof(string), typeof(string)]));
				ilCursor.EmitStloc(4);
				ilCursor.GotoNext(i => i.MatchLdarg1());
				ilCursor.MarkLabel(label1);
			}
			catch (Exception e)
			{
				MonoModHooks.DumpIL(ModContent.GetInstance<MultiWorld>(), il);
				throw new ILPatchFailureException(ModContent.GetInstance<MultiWorld>(), il, e);
			}
		}

		private void IL_UIWorldListItem_ctor(ILContext il)
		{
			try
			{
				var ilCursor = new ILCursor(il);
				var label1 = il.DefineLabel();
				ilCursor.GotoNext(i => i.MatchLdcR4(24));
				ilCursor.GotoNext(i => i.MatchLdcR4(24));
				ilCursor.Index += 3;
				ilCursor.EmitLdarg1();
				ilCursor.EmitCallvirt(typeof(FileData).GetMethod("get_Path", BindingFlags.Instance | BindingFlags.Public));
				ilCursor.EmitCall(typeof(MultiWorldFileData).GetMethod("IsMultiWorld", BindingFlags.Public | BindingFlags.Static));
				ilCursor.Emit(OpCodes.Brtrue, label1);
				ilCursor.GotoNext(i => i.MatchStloc0());
				ilCursor.Index++;
				ilCursor.MarkLabel(label1);
			}
			catch (Exception e)
			{
				MonoModHooks.DumpIL(ModContent.GetInstance<MultiWorld>(), il);
				throw new ILPatchFailureException(ModContent.GetInstance<MultiWorld>(), il, e);
			}
		}

		private void IL_do_worldGenCallBack(ILContext il)
		{
			try
			{
				var ilCursor = new ILCursor(il);
				ilCursor.GotoNext(i => i.MatchLdcI4(888));
				ilCursor.Index++;
				ilCursor.Index++;
				ilCursor.Remove();
				ilCursor.Remove();
				ilCursor.EmitDelegate<Action>(() =>
				{
					var worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
					if (!worldManageSystem.do_chaneWorld)
					{
						Main.menuMode = 6;
					}
				});
				ilCursor.GotoNext(i => i.MatchRet());
				ilCursor.Index--;
				ilCursor.EmitDelegate<Action>(() =>
				{
					var worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
					if (worldManageSystem.do_chaneWorld)
					{
						WorldGen.playWorld();
					}
				});
			}
			catch (Exception e)
			{
				MonoModHooks.DumpIL(ModContent.GetInstance<MultiWorld>(), il);
				throw new ILPatchFailureException(ModContent.GetInstance<MultiWorld>(), il, e);
			}
		}

		private void Il_SaveAndQuitCallBack(ILContext il)
		{
			try
			{
				var ilCursor = new ILCursor(il);
				ilCursor.GotoNext(i => i.MatchLdarg0());
				ilCursor.Index--;
				ilCursor.Index--;
				ilCursor.Remove();
				ilCursor.Remove();
				ilCursor.EmitDelegate<Action>(() =>
				{
					var worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
					if (!worldManageSystem.do_chaneWorld)
					{
						Main.menuMode = 0;
					}
				});
			}
			catch (Exception e)
			{
				MonoModHooks.DumpIL(ModContent.GetInstance<MultiWorld>(), il);
				throw new ILPatchFailureException(ModContent.GetInstance<MultiWorld>(), il, e);
			}
		}
	}
}
