﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Native;

namespace ChaosIV {
    public class ChaosMain : Script {
		static Random R = new Random();

		Timer EffectTimer;
		List<Action> Loops = new List<Action>();
		List<Effect> Effects = new List<Effect>();
		List<Effect> RecentEffects = new List<Effect>(3);
		List<Vector3> Safehouses = new List<Vector3>() {
			new Vector3(900, -500, 0),  // broker
			new Vector3(595, 1400, 0),  // bohan
			new Vector3(115, 845, 0),   // algonquin - middle park east
			new Vector3(-420, 1490, 0), // algonquin - northwood
			new Vector3(-963, 897, 0),  // alderney
		};

		// the fact that i have to make this instead of using a maxspeed function is very stupid. oh well
		Dictionary<string, float> Speeds = new Dictionary<string, float>() { //nyoom
			{"ADMIRAL", 140}, {"AIRTUG", 140},
			{"AKUMA", 150}, {"ALBANY", 140},
			{"AMBULAN", 140}, {"ANGEL", 125}, 
			{"ANNHIL", 160}, {"APC", 80},
			{"AVAN", 100}, {"BANSHEE", 160}, 
			{"BATI", 150}, {"BATI2", 150},
			{"BENSON", 115}, {"BIFF", 110}, {"BLADE", 83},
			{"BLISTA", 150}, {"BOBBER", 125}, 
			{"BOBCAT", 125}, {"BOXVLE", 110}, 
			{"BUCCNEER", 140}, {"BUFFALO", 155},
			{"BULLET", 163}, {"BURRITO", 130}, 
			{"BURRITO2", 130}, {"BUS", 135},
			{"BUZZARD", 160}, {"CABBY", 135},
			{"CABLECAR", 80}, {"CADDY", 40},
			{"CAVCADE", 135}, {"CHAV", 145}, 
			{"COGNONTI", 150}, {"COMET", 160}, 
			{"CONTENDE", 135}, {"COQUETTE", 160},
			{"DAEMON", 125}, {"DF8", 150},
			{"DIABO", 125}, {"DILANTE", 130}, 
			{"DINGHY", 60}, {"DOUBLE", 150},
			{"DOUBLE2", 160}, {"DUKES", 135}, 
			{"EMPEROR", 130}, {"EMPEROR2", 130}, 
			{"ESPERNTO", 120}, {"F620", 155}, {"FACTION", 140}, 
			{"FAGGIO", 80}, {"FBI", 150}, 
			{"FELTZER", 145}, {"FEROCI", 140}, 
			{"FEROCI2", 140}, {"FIRETRUK", 140}, 
			{"FLATBED", 115}, {"FLOATER", 80}, {"FORTUNE", 143}, 
			{"FORK", 50}, {"FUTO", 140}, 
			{"FXT", 130}, {"GBURRITO", 130},
			{"HABANRO", 130}, {"HAKUCH", 150},
			{"HAKUCH2", 160}, {"HAKUMAI", 135},
			{"HELLFURY", 125}, {"HEXER", 125},
			{"HUNT", 145}, {"INFERNUS", 160}, 
			{"INGOT", 130}, {"INNOV", 125},
			{"INTRUD", 135}, {"JETMAX", 75},
			{"LANSTALK", 135}, {"LIMO2", 140},
			{"LOKUS", 135}, {"LYCAN", 125}, {"MANANA", 130}, 
			{"MARBELLA", 130}, {"MARQUI", 30}, 
			{"MAVERICK", 160}, {"MERIT", 140}, 
			{"MINVAN", 130}, {"MOONB", 130}, 
			{"MRTASTY", 110}, {"MULE", 100}, 
			{"NOOSE", 150}, {"NRG900", 150}, 
			{"NSTOCK", 120}, {"ORACLE", 143}, 
			{"PACKER", 105}, {"PATRIOT", 130}, {"PBUS", 140},
			{"PCJ", 140}, {"PEREN", 130}, 
			{"PEREN2", 130}, {"PEYOTE", 135}, 
			{"PHANTOM", 135}, {"PINCLE", 140}, 
			{"PMP600", 140}, {"POLICE", 150}, 
			{"POLICE2", 150}, {"POLICE3", 150},
			{"POLICE4", 160}, {"POLICEB", 130}, {"POLMAV", 160}, 
			{"POLPAT", 140}, {"PONY", 115}, 
			{"PREDTOR", 70}, {"PREMIER", 138}, 
			{"PRES", 145}, {"PRIMO", 140}, 
			{"RANCHER", 130}, {"REBLA", 134}, 
			{"REEFER", 40}, {"REGINA", 130},
			{"REVENANT", 130}, {"RHAPSODY", 130}, {"RIPLEY", 70}, 
			{"ROMERO", 120}, {"RUINER", 150},
			{"RUSTBOAT", 40}, {"SABRE", 140},
			{"SABRE2", 140}, {"SABREGT", 145},
			{"SANCHEZ", 130}, {"SCHAFTER", 141},
			{"SCHAFTE2", 141}, {"SENTINEL", 150},
			{"SERRANO", 145}, {"SKYLIFT", 160},
			{"SLAMVAN", 125}, {"SMUGGLER", 85}, {"SOLAIR", 130},
			{"SPEEDO", 125}, {"SQUALO", 70},
			{"STALION", 140}, {"STEED", 100},
			{"STRATUM", 135}, {"STRETCH", 140},
			{"SUBWAY", 80}, {"SULTAN", 145},
			{"SULTANRS", 150}, {"SUPER", 155},
			{"SUPERD", 160}, {"SUPERD2", 160},
			{"SWIFT", 160}, {"TAMPA", 145},
			{"TAXI", 135}, {"TAXI2", 135},
			{"TOURMAV", 160}, {"TOWTRUCK", 125}, {"TROPIC", 75},
			{"TRUSH", 100}, {"TURISMO", 160},
			{"URANUS", 130}, {"VADER", 154}, {"VIGERO", 135},
			{"VIGERO2", 135}, {"VINCENT", 130},
			{"VIRGO", 125}, {"VOODOO", 120},
			{"WASHINGT", 135}, {"WAYFAR", 125},
			{"WILARD", 130}, {"WOLFS", 125},
			{"YANKEE", 105}, {"YANKEE2", 130}, {"ZOMB", 125}
		};
		List<string> Vehicles = new List<string>() { 
			"ADMIRAL", "AIRTUG", "AMBULANCE", "ANNIHILATOR", "BANSHEE", "BENSON",
			"BIFF", "BLISTA", "BOBBER", "BOBCAT", "BOXVILLE", "BUCCANEER",
			"BURRITO", "BURRITO2", "BUS", "CABBY", "CAVALCADE", "CHAVOS",
			"COGNOSCENTI", "COMET", "COQUETTE", "DF8", "DILETTANTE", "DINGHY",
			"DUKES", "E109", "EMPEROR", "EMPEROR2", "ESPERANT", "FACTION",
			"FAGGIO", "FBI", "FELTZER", "FEROCI", "FEROCI2", "FIRETRUK",
			"FLATBED", "FORTUNE", "FORKLIFT", "FUTO", "FXT", "HABANERO",
			"HAKUMAI", "HELLFURY", "HUNTLEY", "INFERNUS", "INGOT", "INTRUDER",
			"JETMAX", "LANDSTALKER", "LOKUS", "MANANA", "MARBELLA", "MARQUIS",
			"MAVERICK", "MERIT", "MINIVAN", "MOONBEAM", "MRTASTY", "MULE",
			"NOOSE", "NRG900", "ORACLE", "PACKER", "PATRIOT", "PCJ",
			"PERENNIAL", "PERENNIAL2", "PEYOTE", "PHANTOM", "PINNACLE", 
			"PMP600", "POLICE", "POLICE2", "POLMAV", "POLPATRIOT", "PONY",
			"PREDATOR", "PREMIER", "PRES", "PRIMO", "RANCHER", "REBLA", 
			"REEFER", "RIPLEY", "ROM", "ROMERO", "RUINER", "SABRE",
			"SABRE2", "SABREGT", "SANCHEZ", "SCHAFTER", "SENTINEL", "SOLAIR",
			"SPEEDO", "SQUALO", "STALION", "STEED", "STOCKADE", "STRATUM",
			"STRETCH", "SULTAN", "SULTANRS", "SUPERGT", "TAXI", "TAXI2", 
			"TOURMAV", "TRASH", "TROPIC", "TUGA", "TURISMO", "URANUS",
			"VIGERO", "VIGERO2", "VINCENT", "VIRGO", "VOODOO", "WASHINGTON",
			"WILLARD", "YANKEE", "ZOMBIEB"
		};

		Font smol = new Font("Arial", 0.02f, FontScaling.ScreenUnits);
		AnimationSet hood = new AnimationSet("amb@dance_maleidl_a");
		Color barcolor = Color.Yellow;

		bool isBlind = false;
		bool isHUDless = false;
		int lagTicks = 0;

		public ChaosMain() {
			Interval = 33;
			Tick += new EventHandler(ChaosLoop);
			PerFrameDrawing += new GraphicsEventHandler(ChaosDraw);

			if (Game.CurrentEpisode != GameEpisode.GTAIV) {
				Vehicles.AddRange(new string[] {"BATI2", "DOUBLE", "HAKUCHOU", "HEXER", "SLAMVAN", "TAMPA"});

				if (Game.CurrentEpisode == GameEpisode.TLAD) {
					barcolor = Color.Red;

					Vehicles.AddRange(new string[] {"ANGEL", "BATI", "DAEMON", "DIABOLUS", "DOUBLE2", "GBURRITO",
						"HAKUCHOU2", "INNOVATION", "LYCAN", "PBUS", "REGINA", "REVENANT", "RHAPSODY", "TOWTRUCK",
						"WAYFARER", "WOLFSBANE", "YANKEE2"
					});
				} else {
					barcolor = Color.Magenta;

					Vehicles.AddRange(new string[] {"AKUMA", "APC", "AVAN", "BLADE", "BUFFALO", "BULLET",
						"BUZZARD", "CADDY", "F620", "FLOATER", "LIMO2", "POLICE3", "POLICE4", "POLICEB",
						"SCHAFTER2", "SERRANO", "SKYLIFT", "SMUGGLER", "SUPERD", "SUPERD2", "SWIFT", "VADER"
					});
				}
			}

			Effects.Add(new Effect("Invert Current Velocity", EffectMiscInvertVelocity));
			//Effects.Add(new Effect("Zero Gravity", EffectMiscNoGravity, new Timer(28000), null, EffectMiscNormalGravity)); //this one doesn't affect vehicles :/
			Effects.Add(new Effect("No HUD", EffectMiscNoHUD, new Timer(88000), null, EffectMiscShowHUD));
			Effects.Add(new Effect("Nothing", EffectMiscNothing));
			Effects.Add(new Effect("Spawn Jet", EffectMiscSpawnJet));
			Effects.Add(new Effect("SPEEN", EffectMiscSPEEN, new Timer(28000), EffectMiscSPEEN, EffectMiscSPEEN));

			Effects.Add(new Effect("All Nearby Peds Are Wanted", EffectPedsAllNearbyWanted));
			Effects.Add(new Effect("Peds Don't See Very Well", EffectPedsBlindLoop, new Timer(88000), EffectPedsBlindLoop, EffectPedsBlindStop));
			Effects.Add(new Effect("In The Hood", EffectPedsDance, new Timer(88000), EffectPedsDance, EffectPedsWander));
			Effects.Add(new Effect("Explosive Peds", EffectPedsExplosive, new Timer(88000), EffectPedsExplosive, EffectPedsExplosive));
			Effects.Add(new Effect("All Nearby Peds Flee", EffectPedsFlee));
			Effects.Add(new Effect("You Are Famous", EffectPedsFollow));
			Effects.Add(new Effect("Give Everyone A Rocket Launcher", EffectPedsGiveAllRocket));
			Effects.Add(new Effect("Everyone Is Invincible", EffectPedsInvincibleLoop, new Timer(88000), EffectPedsInvincibleLoop, EffectPedsInvincibleStop));
			Effects.Add(new Effect("Everyone Is A Ghost", EffectPedsInvisibleLoop, new Timer(88000), EffectPedsInvisibleLoop, EffectPedsInvisibleStop));
			Effects.Add(new Effect("Ignite All Nearby Peds", EffectPedsIgniteNearby));
			Effects.Add(new Effect("Launch All Nearby Peds Up", EffectPedsLaunch));
			Effects.Add(new Effect("No Headshots", EffectPedsNoHeadshotsLoop, new Timer(88000), EffectPedsNoHeadshotsLoop, EffectPedsNoHeadshotsStop));
			Effects.Add(new Effect("No Ragdoll", EffectPedsNoRagdollLoop, new Timer(88000), EffectPedsNoRagdollLoop, EffectPedsNoRagdollStop));
			Effects.Add(new Effect("Obliterate All Nearby Peds", EffectPedsObliterateNearby));
			Effects.Add(new Effect("One Hit KO", EffectPedsOHKO, new Timer(28000), EffectPedsOHKO, EffectPedsOHKOStop));
			Effects.Add(new Effect("Remove Weapons From Everyone", EffectPedsRemoveWeapons));
			Effects.Add(new Effect("Revive Dead Peds", EffectPedsReviveDead));
			Effects.Add(new Effect("Spawn An Angry Doctor Who Wants To Knife You", EffectPedsSpawnAngryDoctor));

			Effects.Add(new Effect("Give Armor", EffectPlayerArmor));
			Effects.Add(new Effect("Bankrupt", EffectPlayerBankrupt));
			Effects.Add(new Effect("Blind", EffectPlayerBlindStart, new Timer(28000), null, EffectPlayerBlindStop));
			Effects.Add(new Effect("It's Time For A Break", EffectPlayerBreakTimeStart, new Timer(28000), null, EffectPlayerBreakTimeStop));
			Effects.Add(new Effect("Exit Current Vehicle", EffectPlayerExitCurrentVehicle));
			Effects.Add(new Effect("Give All Weapons", EffectPlayerGiveAll));
			Effects.Add(new Effect("Give Grenades", EffectPlayerGiveGrenades));
			Effects.Add(new Effect("Give Molotov Cocktails", EffectPlayerGiveGrenades));
			Effects.Add(new Effect("Give Rocket Launcher", EffectPlayerGiveRocket));
			Effects.Add(new Effect("Heal Player", EffectPlayerHeal));
			Effects.Add(new Effect("Ignite Player", EffectPlayerIgnite));
			Effects.Add(new Effect("Invincibility", EffectPlayerInvincible, new Timer(88000), EffectPlayerInvincible, EffectPlayerInvincibleStop));
			Effects.Add(new Effect("Launch Player Up", EffectPlayerLaunchUp));
			Effects.Add(new Effect("Millionaire", EffectPlayerMillionaire));
			Effects.Add(new Effect("Ragdoll", EffectPlayerRagdoll));
			Effects.Add(new Effect("Randomize Player Outfit", EffectPlayerRandomClothes));
			Effects.Add(new Effect("Remove All Weapons", EffectPlayerRemoveWeapons));
			Effects.Add(new Effect("Set Player Into Random Unoccupied Seat", EffectPlayerSetRandomSeat));
			Effects.Add(new Effect("Set Player Into Closest Vehicle", EffectPlayerSetVehicleClosest));
			Effects.Add(new Effect("Set Player Into Random Vehicle", EffectPlayerSetVehicleRandom));
			Effects.Add(new Effect("Suicide", EffectPlayerSuicide));
			Effects.Add(new Effect("Teleport To Alderney Prison", EffectPlayerTeleportAlderneyPrison));
			Effects.Add(new Effect("Teleport To GetALife Building", EffectPlayerTeleportGetALife));
			Effects.Add(new Effect("Teleport To The Heart Of Liberty City", EffectPlayerTeleportHeart));
			Effects.Add(new Effect("Teleport To Nearest Safehouse", EffectPlayerTeleportNearestSafehouse));
			Effects.Add(new Effect("Teleport To Swingset", EffectPlayerTeleportSwingset));
			Effects.Add(new Effect("Teleport To Waypoint", EffectPlayerTeleportWaypoint));
			Effects.Add(new Effect("+2 Wanted Stars", EffectPlayerWantedAddTwo));
			Effects.Add(new Effect("Clear Wanted Level", EffectPlayerWantedClear));
			Effects.Add(new Effect("Five Wanted Stars", EffectPlayerWantedFiveStars));
			Effects.Add(new Effect("Never Wanted", EffectPlayerWantedClear, new Timer(88000), EffectPlayerWantedClear, EffectPlayerWantedClear));

			Effects.Add(new Effect("Advance One Day", EffectTimeAdvanceOneDay));
			Effects.Add(new Effect("x0.2 Gamespeed", EffectTimeGameSpeedFifth, new Timer(28000), null, EffectTimeGameSpeedNormal, new[] { "x0.5 Gamespeed", "Lag" }));
			Effects.Add(new Effect("x0.5 Gamespeed", EffectTimeGameSpeedHalf, new Timer(28000), null, EffectTimeGameSpeedNormal, new[] { "x0.2 Gamespeed", "Lag" }));
			Effects.Add(new Effect("Lag", EffectTimeLagStart, new Timer(28000), EffectTimeLagLoop, EffectTimeGameSpeedNormal, new[] { "x0.2 Gamespeed", "x0.5 Gamespeed" }));
			Effects.Add(new Effect("Set Time To Evening", EffectTimeSetEvening));
			Effects.Add(new Effect("Set Time To Midnight", EffectTimeSetMidnight));
			Effects.Add(new Effect("Set Time To Morning", EffectTimeSetMorning));
			Effects.Add(new Effect("Set Time To Noon", EffectTimeSetNoon));
			Effects.Add(new Effect("Timelapse", EffectTimeLapse, new Timer(88000), EffectTimeLapse, EffectTimeLapse));

			Effects.Add(new Effect("Break All Doors of Current Vehicle", EffectVehicleBreakDoorsPlayer));
			Effects.Add(new Effect("Clean Current Vehicle", EffectVehicleCleanPlayer));
			Effects.Add(new Effect("Explode Nearby Vehicles", EffectVehicleExplodeNearby));
			Effects.Add(new Effect("Explode Current Vehicle", EffectVehicleExplodePlayer));
			Effects.Add(new Effect("Full Acceleration", EffectVehicleFullAccel, new Timer(28000), EffectVehicleFullAccel, EffectVehicleFullAccel));
			Effects.Add(new Effect("Invisible Vehicles", EffectVehicleInvisibleLoop, new Timer(88000), EffectVehicleInvisibleLoop, EffectVehicleInvisibleStop, new[] { "Black Traffic", "Blue Traffic", "Red Traffic", "White Traffic" }));
			Effects.Add(new Effect("Jumpy Vehicles", EffectVehicleJumpy, new Timer(28000), EffectVehicleJumpy, EffectVehicleJumpy));
			Effects.Add(new Effect("Kill Engine Of Current Vehicle", EffectVehicleKillEnginePlayer));
			Effects.Add(new Effect("Launch All Vehicles Up", EffectVehicleLaunchAllUp));
			Effects.Add(new Effect("Lock All Vehicles", EffectVehicleLockAll));
			Effects.Add(new Effect("Lock Vehicle Player Is In", EffectVehicleLockPlayer));
			Effects.Add(new Effect("Mass Breakdown", EffectVehicleMassBreakdown));
			Effects.Add(new Effect("No Traffic", EffectVehicleTrafficNone, new Timer(88000), EffectVehicleTrafficNone, null));
			Effects.Add(new Effect("Pop Tires Of Current Vehicle", EffectVehiclePopTiresPlayer));
			Effects.Add(new Effect("Repair Current Vehicle", EffectVehicleRepairPlayer));
			Effects.Add(new Effect("Spammy Vehicle Doors", EffectVehicleSpamDoors, new Timer(88000), EffectVehicleSpamDoors, null));
			Effects.Add(new Effect("Spawn Bus", EffectVehicleSpawnBus));
			Effects.Add(new Effect("Spawn Infernus", EffectVehicleSpawnInfernus));
			Effects.Add(new Effect("Spawn Maverick", EffectVehicleSpawnMaverick));
			Effects.Add(new Effect("Spawn Police Cruiser", EffectVehicleSpawnPolice));
			Effects.Add(new Effect("Spawn Random Vehicle", EffectVehicleSpawnRandom));
			Effects.Add(new Effect("Need A Cab?", EffectVehicleSpawnTaxis));
			Effects.Add(new Effect("Spawn Tug", EffectVehicleSpawnTug));
			Effects.Add(new Effect("Black Traffic", EffectVehicleTrafficBlack, new Timer(88000), EffectVehicleTrafficBlack, EffectVehicleTrafficBlack, new[] { "Invisible Vehicles", "Blue Traffic", "Red Traffic", "White Traffic" }));
			Effects.Add(new Effect("Blue Traffic", EffectVehicleTrafficBlue, new Timer(88000), EffectVehicleTrafficBlue, EffectVehicleTrafficBlue, new[] { "Invisible Vehicles", "Black Traffic", "Red Traffic", "White Traffic" }));
			Effects.Add(new Effect("Red Traffic", EffectVehicleTrafficRed, new Timer(88000), EffectVehicleTrafficRed, EffectVehicleTrafficRed, new[] { "Invisible Vehicles", "Black Traffic", "Blue Traffic", "White Traffic" }));
			Effects.Add(new Effect("White Traffic", EffectVehicleTrafficWhite, new Timer(88000), EffectVehicleTrafficWhite, EffectVehicleTrafficWhite, new[] { "Invisible Vehicles", "Black Traffic", "Blue Traffic", "Red Traffic" }));

			Effects.Add(new Effect("Cloudy Weather", EffectWeatherCloudy));
			Effects.Add(new Effect("Foggy Weather", EffectWeatherFoggy));
			Effects.Add(new Effect("Sunny Weather", EffectWeatherSunny));
			Effects.Add(new Effect("Stormy Weather", EffectWeatherThunder));

			EffectTimer = new Timer();
			EffectTimer.Tick += new EventHandler(DeployEffect);
			EffectTimer.Interval = 30000;
			EffectTimer.Start();
		}

		public void ChaosDraw(object s, GraphicsEventArgs e) {
			e.Graphics.Scaling = FontScaling.ScreenUnits;

			// Blind
			if (isBlind) e.Graphics.DrawRectangle(new RectangleF(0f, 0f, 1f, 1f), Color.Black);

			// No HUD
			if (isHUDless) Function.Call("HIDE_HUD_AND_RADAR_THIS_FRAME");

			// Draw Timer Bar
			e.Graphics.DrawRectangle(new RectangleF(0f, 0f, 1f, 0.02f), Color.FromArgb(10, 10, 10));
			e.Graphics.DrawText("ChaosIV", new RectangleF(0f, 0f, 1f, 0.02f), TextAlignment.Center, Color.FromArgb(40, 40, 40), smol);
			e.Graphics.DrawRectangle(new RectangleF(0f, 0f, (float)((double)EffectTimer.ElapsedTime / 30000), 0.02f), barcolor);

			// Draw Recent Effects
			if (RecentEffects.Count >= 1) {
				if (RecentEffects[0].Timer != null) e.Graphics.DrawRectangle(0.5f, 0.05f, (float)(((double)(RecentEffects[0].Timer.Interval - RecentEffects[0].Timer.ElapsedTime) / RecentEffects[0].Timer.Interval) * 0.4), 0.02f, Color.FromArgb(128, 80, 80, 80));
				e.Graphics.DrawText(RecentEffects[0].Name, new RectangleF(0f, 0.04f, 1f, 0.02f), TextAlignment.Center, smol);
			}

			if (RecentEffects.Count >= 2) {
				if (RecentEffects[1].Timer != null) e.Graphics.DrawRectangle(0.5f, 0.08f, (float)(((double)(RecentEffects[1].Timer.Interval - RecentEffects[1].Timer.ElapsedTime) / RecentEffects[1].Timer.Interval) * 0.4), 0.02f, Color.FromArgb(128, 80, 80, 80));
				e.Graphics.DrawText(RecentEffects[1].Name, new RectangleF(0f, 0.07f, 1f, 0.02f), TextAlignment.Center, smol);
			}

			if (RecentEffects.Count == 3) {
				if (RecentEffects[2].Timer != null) e.Graphics.DrawRectangle(0.5f, 0.11f, (float)(((double)(RecentEffects[2].Timer.Interval - RecentEffects[2].Timer.ElapsedTime) / RecentEffects[2].Timer.Interval) * 0.4), 0.02f, Color.FromArgb(128, 80, 80, 80));
				e.Graphics.DrawText(RecentEffects[2].Name, new RectangleF(0f, 0.1f, 1f, 0.02f), TextAlignment.Center, smol);
			}
		}

		public void ChaosLoop(object s, EventArgs e) {
			for (int i = RecentEffects.Count - 1; i >= 0; i--) {
				if (RecentEffects[i].Timer != null) {
					if (RecentEffects[i].Timer.ElapsedTime > RecentEffects[i].Timer.Interval) {
						RecentEffects[i].Stop?.Invoke();
						RecentEffects[i].Timer.Stop();
						Loops.Remove(RecentEffects[i].Loop);
						RecentEffects.RemoveAt(i);
					}
				}
			}

			if (Loops.Count > 0) {
				foreach (Action a in Loops) a();
			}
		}

		public void DeployEffect(object s, EventArgs e) {
			Effect next = Effects[R.Next(Effects.Count)];

			if (next.Timer != null && RecentEffects.Contains(next)) {
				RecentEffects.Find(x => x.Name == next.Name).Start();
			} else {
				if (next.Conflicts != null) {
					foreach(string c in next.Conflicts) {
						if (RecentEffects.Find(x => x.Name == c).Name != null) {
							RecentEffects.Find(x => x.Name == c).Stop?.Invoke();
							RecentEffects.Find(x => x.Name == c).Timer.Stop();
							Loops.Remove(RecentEffects.Find(x => x.Name == c).Loop);
							RecentEffects.Remove(RecentEffects.Find(x => x.Name == c));
						}
					}
				}

				next.Start();
				if (next.Timer != null) next.Timer.Start();
				if (next.Loop != null) Loops.Add(next.Loop);

				if (RecentEffects.Count != 3) {
					RecentEffects.Add(next);
				} else {
					foreach (Effect f in RecentEffects) {
						if (f.Timer != null) {
							if (f.Timer.ElapsedTime > f.Timer.Interval) {
								RecentEffects.Remove(f);
								break;
							}
						} else {
							RecentEffects.Remove(f);
							break;
						}
					}
					RecentEffects.Add(next);
				}
			}

			EffectTimer.Start();
		}

		// !!! EFFECT METHODS BEGIN BELOW !!! //

		#region Misc Effects
		public void EffectMiscInvertVelocity() {
			if (Player.Character.isInVehicle()) {
				Player.Character.CurrentVehicle.Velocity = new Vector3(Player.Character.CurrentVehicle.Velocity.X * -1, Player.Character.CurrentVehicle.Velocity.Y * -1, Player.Character.CurrentVehicle.Velocity.Z * -1);
			} else {
				Player.Character.Velocity = new Vector3(Player.Character.Velocity.X * -1, Player.Character.Velocity.Y * -1, Player.Character.Velocity.Z * -1);
			}
		}

		public void EffectMiscNoGravity() {
			// floating free...
			// one, two, three...
			// eternity....
			World.GravityEnabled = false;
		}

		public void EffectMiscNoHUD() {
			isHUDless = true;
		}

		public void EffectMiscNormalGravity() {
			World.GravityEnabled = true;
		}

		public void EffectMiscNothing() {
			Game.Console.Print("ok maybe not quite \"nothing\" but eh who cares");
		}

		public void EffectMiscShowHUD() {
			isHUDless = false;
		}

		public void EffectMiscSpawnJet() {
			World.CreateObject("ec_jet", Player.Character.Position.Around(2f));
		}

		public void EffectMiscSPEEN() {
			foreach (GTA.Object o in World.GetAllObjects()) {
				if (o.Exists()) o.Heading += 5f;
			}
			foreach (Vehicle v in World.GetAllVehicles()) {
				if (v.Exists()) {
					v.Heading += 5f;
					if (v.Heading >= 355f) v.Heading = 0f;
					v.ApplyForce(new Vector3(0f, 0f, 0.1f));
				}
			}
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists() & (p != Player.Character)) {
					p.Heading += 5f;
				}
			}
		}
		#endregion

		#region Ped Effects
		public void EffectPedsAllNearbyWanted() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists() & (p != Player.Character) & (p.PedType != PedType.Cop)) {
					p.WantedByPolice = true;
				}
			}
		}

		public void EffectPedsBlindLoop() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists() & (p != Player.Character)) {
					p.SenseRange = 0f;
				}
			}
		}

		public void EffectPedsBlindStop() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists() & (p != Player.Character)) {
					p.SenseRange = 50f;
				}
			}
		}

		public void EffectPedsDance() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists() & (p != Player.Character)) {
					if (!p.Animation.isPlaying(hood, "loop_a")) p.Task.PlayAnimation(hood, "loop_a", 1f);
					p.Task.AlwaysKeepTask = true;
				}
			}
		}

		public void EffectPedsExplosive() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists() & (p != Player.Character)) {
					if (p.isRagdoll & !p.isDead) World.AddExplosion(p.Position);
				}
			}
		}

		public void EffectPedsFlee() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists() & (p != Player.Character)) {
					p.Task.FleeFromChar(Player.Character);
					p.Task.AlwaysKeepTask = true;
				}
			}
		}

		public void EffectPedsFollow() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists() & (p != Player.Character)) {
					p.LeaveVehicle();
					p.Task.GoTo(Player.Character);
					p.Task.AlwaysKeepTask = true;
				}
			}
		}

		public void EffectPedsGiveAllRocket() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists() & (p != Player.Character)) {
					Function.Call("GIVE_WEAPON_TO_CHAR", p, 18, 9999);
					p.Weapons.Select(Weapon.Heavy_RocketLauncher);
				}
			}
			Function.Call("GIVE_WEAPON_TO_CHAR", Player.Character, 18, 9999);
		}

		public void EffectPedsInvincibleLoop() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists()) p.Invincible = true;
			}
		}

		public void EffectPedsInvincibleStop() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists()) p.Invincible = false;
			}
		}

		public void EffectPedsInvisibleLoop() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists()) p.Visible = false;
			}
		}

		public void EffectPedsInvisibleStop() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists()) p.Visible = true;
			}
		}

		public void EffectPedsIgniteNearby() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists() & (p != Player.Character)) {
					p.isOnFire = true;
				}
			}
		}

		public void EffectPedsLaunch() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists() & (p != Player.Character)) {
					p.ApplyForce(new Vector3(0f, 0f, 250f));
				}
			}
		}

		public void EffectPedsNoHeadshotsLoop() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists()) Function.Call("SET_CHAR_SUFFERS_CRITICAL_HITS", p, false);
			}
		}

		public void EffectPedsNoHeadshotsStop() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists()) Function.Call("SET_CHAR_SUFFERS_CRITICAL_HITS", p, true);
			}
		}

		public void EffectPedsNoRagdollLoop() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists()) p.PreventRagdoll = true;
			}
		}

		public void EffectPedsNoRagdollStop() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists()) p.PreventRagdoll = false;
			}
		}

		public void EffectPedsObliterateNearby() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists() & (p != Player.Character)) {
					World.AddExplosion(p.Position);
				}
			}
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists() & (p != Player.Character) & !p.isDead) {
					World.AddExplosion(p.Position);
				}
			}
		}

		public void EffectPedsOHKO() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists() & !p.isDead & p.Health > 5) {
					p.Health = 5;
					p.Armor = 0;
				}
			}
		}

		public void EffectPedsOHKOStop() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists() & !p.isDead) {
					p.Health = 100;
				}
			}
		}

		public void EffectPedsRemoveWeapons() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists()) p.Weapons.RemoveAll();
			}
		}

		public void EffectPedsReviveDead() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists() & p.isDead) {
					World.CreatePed(p.Model, p.Position, p.RelationshipGroup).NoLongerNeeded();
					p.Delete();
				}
			}
		}

		public void EffectPedsSpawnAngryDoctor() {
			var doc = World.CreatePed(new Model("m_m_dodgydoc"), Player.Character.Position.Around(10f), RelationshipGroup.Criminal);
			doc.Task.FightAgainst(Player.Character);
			doc.Task.AlwaysKeepTask = true;
			Function.Call("GIVE_WEAPON_TO_CHAR", doc, 3, 9999);
			doc.Weapons.Select(Weapon.Melee_Knife);
		}

		public void EffectPedsWander() {
			foreach (Ped p in World.GetAllPeds()) {
				if (p.Exists() & (p != Player.Character)) {
					if (p.isInVehicle()) p.Task.CruiseWithVehicle(p.CurrentVehicle, 30f, true);
					else p.Task.WanderAround();
					p.Task.AlwaysKeepTask = false;
				}
			}
		}
		#endregion

		#region Player Effects
		public void EffectPlayerArmor() {
			Player.Character.Armor = 100;
		}

		public void EffectPlayerBankrupt() {
			Player.Money = 0;
		}

		public void EffectPlayerBlindStart() {
			isBlind = true;
		}

		public void EffectPlayerBlindStop() {
			isBlind = false;
		}

		public void EffectPlayerBreakTimeStart() {
			Player.CanControlCharacter = false;
		}

		public void EffectPlayerBreakTimeStop() {
			Player.CanControlCharacter = true;
		}

		public void EffectPlayerExitCurrentVehicle() {
			if (Player.Character.isInVehicle()) Player.Character.LeaveVehicle();
		}

		public void EffectPlayerGiveAll() {
			Function.Call("GIVE_WEAPON_TO_CHAR", Player.Character, 3, 9999); // knife
			Function.Call("GIVE_WEAPON_TO_CHAR", Player.Character, 7, 9999); // pistol
			Function.Call("GIVE_WEAPON_TO_CHAR", Player.Character, 13, 9999); // smg
			Function.Call("GIVE_WEAPON_TO_CHAR", Player.Character, 11, 9999); // shotgun
			Function.Call("GIVE_WEAPON_TO_CHAR", Player.Character, 14, 9999); // rifle
			Function.Call("GIVE_WEAPON_TO_CHAR", Player.Character, 17, 9999); // sniper
			Function.Call("GIVE_WEAPON_TO_CHAR", Player.Character, 18, 9999); // rocket launcher
			Function.Call("GIVE_WEAPON_TO_CHAR", Player.Character, R.Next(4,5), 9999); // grenades or molotov
		}

		public void EffectPlayerGiveGrenades() {
			Function.Call("GIVE_WEAPON_TO_CHAR", Player.Character, 4, 9999);
		}

		public void EffectPlayerGiveMolotovs() {
			Function.Call("GIVE_WEAPON_TO_CHAR", Player.Character, 5, 9999);
		}

		public void EffectPlayerGiveRocket() {
			Function.Call("GIVE_WEAPON_TO_CHAR", Player.Character, 18, 9999);
		}

		public void EffectPlayerHeal() {
			Player.Character.Health = 100;
		}

		public void EffectPlayerIgnite() {
			Player.Character.isOnFire = true;
		}

		public void EffectPlayerInvincible() {
			Player.Character.Invincible = true;
		}

		public void EffectPlayerInvincibleStop() {
			Player.Character.Invincible = false;
		}

		public void EffectPlayerLaunchUp() {
			if (Player.Character.isInVehicle()) Player.Character.CurrentVehicle.ApplyForce(new Vector3(0f, 0f, 50f));
			else Player.Character.ApplyForce(new Vector3(0f, 0f, 250f));
		}

		public void EffectPlayerMillionaire() {
			Player.Money += 1000000;
		}

		public void EffectPlayerRagdoll() {
			Player.Character.isRagdoll = true;
		}

		public void EffectPlayerRandomClothes() {
			Player.Character.RandomizeOutfit();
		}

		public void EffectPlayerRemoveWeapons() {
			Player.Character.Weapons.RemoveAll();
		}

		public void EffectPlayerSetRandomSeat() {
			if (Player.Character.isInVehicle()) {
				Player.Character.WarpIntoVehicle(Player.Character.CurrentVehicle, Player.Character.CurrentVehicle.GetFreeSeat());
			}
		}

		public void EffectPlayerSetVehicleClosest() {
			Vehicle cv = World.GetClosestVehicle(Player.Character.Position, 50f);
			if (cv.Exists()) Player.Character.WarpIntoVehicle(cv, VehicleSeat.Driver);
		}

		public void EffectPlayerSetVehicleRandom() {
			Player.Character.WarpIntoVehicle(World.GetAllVehicles()[R.Next(World.GetAllVehicles().Length)], VehicleSeat.Driver);
		}

		public void EffectPlayerSuicide() {
			Player.Character.Die();
		}

		public void EffectPlayerTeleportAlderneyPrison() {
			Player.TeleportTo(-1000, -400);
		}

		public void EffectPlayerTeleportGetALife() {
			Player.TeleportTo(16, 65);
		}

		public void EffectPlayerTeleportHeart() {
			Player.TeleportTo(new Vector3(-608, -755, 66));
		}

		public void EffectPlayerTeleportNearestSafehouse() {
			float d = 9999;
			Vector3 s = Player.Character.Position;

			foreach (Vector3 v in Safehouses) {
				if (Player.Character.Position.DistanceTo(v) < d) {
					d = Player.Character.Position.DistanceTo(v);
					s = v;
				}
			}

			Player.TeleportTo(s);
		}

		public void EffectPlayerTeleportSwingset() {
			Player.TeleportTo(new Vector3(1354, -256, 22));
		}

		public void EffectPlayerTeleportWaypoint() {
			if (Game.GetWaypoint() != null) Player.TeleportTo(Game.GetWaypoint().Position);
			else {
				foreach (Blip b in Blip.GetAllBlipsOfType(BlipType.Contact).Concat(Blip.GetAllBlipsOfType(BlipType.Coordinate)).Concat(Blip.GetAllBlipsOfType(BlipType.Ped))) {
					if (b.Icon == BlipIcon.Misc_Objective) {
						Player.TeleportTo(b.Position);
						break;
					}
				}
			}
		}

		public void EffectPlayerWantedAddTwo() {
			Player.WantedLevel += 2;
		}

		public void EffectPlayerWantedClear() {
			Player.WantedLevel = 0;
		}

		public void EffectPlayerWantedFiveStars() {
			Player.WantedLevel = 5;
		}
		#endregion

		#region Time Effects
		public void EffectTimeAdvanceOneDay() {
			World.OneDayForward();
		}

		public void EffectTimeGameSpeedFifth() {
			Game.TimeScale = 0.2f;
		}

		public void EffectTimeGameSpeedHalf() {
			Game.TimeScale = 0.5f;
		}

		public void EffectTimeGameSpeedNormal() {
			Game.TimeScale = 1f;
		}

		public void EffectTimeLagStart() {
			lagTicks = 0;
		}

		public void EffectTimeLagLoop() {
			lagTicks++;

			if (lagTicks < 50) Game.TimeScale = 1f;
			else Game.TimeScale = 0.05f;

			if (lagTicks >= 60) lagTicks = 0;
		}

		public void EffectTimeLapse() {
			World.UnlockDayTime();
			World.CurrentDayTime = World.CurrentDayTime.Add(new TimeSpan(0, 10, 0));
		}

		public void EffectTimeSetEvening() {
			World.CurrentDayTime = new TimeSpan(World.CurrentDayTime.Days, 18, 0, 0);
		}

		public void EffectTimeSetMidnight() {
			World.CurrentDayTime = new TimeSpan(World.CurrentDayTime.Days, 0, 0, 0);
		}

		public void EffectTimeSetMorning() {
			World.CurrentDayTime = new TimeSpan(World.CurrentDayTime.Days, 6, 0, 0);
		}

		public void EffectTimeSetNoon() {
			World.CurrentDayTime = new TimeSpan(World.CurrentDayTime.Days, 12, 0, 0);
		}
		#endregion

		#region Vehicle Effects
		public void EffectVehicleBreakDoorsPlayer() {
			if (Player.Character.isInVehicle()) {
				for (int d = 0; d < 6; d++) Function.Call("BREAK_CAR_DOOR", Player.Character.CurrentVehicle, d, false);
			}
		}

		public void EffectVehicleCleanPlayer() {
			if (Player.Character.isInVehicle()) Player.Character.CurrentVehicle.Wash();
		}

		public void EffectVehicleExplodeNearby() {
			foreach (Vehicle v in World.GetAllVehicles()) {
				if (v.Exists() & v != Player.Character.CurrentVehicle) v.Explode();
			}
		}

		public void EffectVehicleExplodePlayer() {
			if (Player.Character.isInVehicle()) Player.Character.CurrentVehicle.Explode();
		}

		public void EffectVehicleFullAccel() {
			foreach (Vehicle v in World.GetAllVehicles()) {
				if (v.Exists() & v.isOnAllWheels) {
					Game.Console.Print(v.Name);
					v.Speed = Speeds[v.Name];
				}
			}
		}

		public void EffectVehicleInvisibleLoop() {
			foreach (Vehicle v in World.GetAllVehicles()) {
				if (v.Exists()) v.Visible = false;
			}
		}

		public void EffectVehicleInvisibleStop() {
			foreach (Vehicle v in World.GetAllVehicles()) {
				if (v.Exists()) v.Visible = true;
			}
		}

		public void EffectVehicleJumpy() {
			foreach (Vehicle v in World.GetAllVehicles()) {
				if (v.Exists() & v.isOnAllWheels) {
					v.ApplyForce(new Vector3(0f, 0f, 1f));
				}
			}
		}

		public void EffectVehicleKillEnginePlayer() {
			if (Player.Character.isInVehicle()) Player.Character.CurrentVehicle.EngineHealth = 0;
		}

		public void EffectVehicleLaunchAllUp() {
			foreach (Vehicle v in World.GetAllVehicles()) {
				if (v.Exists()) v.ApplyForce(new Vector3(0f, 0f, 50f));
			}
		}

		public void EffectVehicleLockAll() {
			foreach (Vehicle v in World.GetAllVehicles()) {
				if (v.Exists()) v.DoorLock = DoorLock.ImpossibleToOpen;
			}
		}

		public void EffectVehicleLockPlayer() {
			if (Player.Character.isInVehicle()) Player.Character.CurrentVehicle.DoorLock = DoorLock.ImpossibleToOpen;
		}

		public void EffectVehicleMassBreakdown() {
			foreach (Vehicle v in World.GetAllVehicles()) {
				if (v.Exists()) v.EngineHealth = 0;
			}
		}

		public void EffectVehiclePopTiresPlayer() {
			if (Player.Character.isInVehicle()) {
				if (!Player.Character.CurrentVehicle.Model.isBoat | !Player.Character.CurrentVehicle.Model.isHelicopter) {
					Player.Character.CurrentVehicle.BurstTire(VehicleWheel.FrontLeft);
					Player.Character.CurrentVehicle.BurstTire(VehicleWheel.RearLeft);
					if (!Player.Character.CurrentVehicle.Model.isBike) {
						Player.Character.CurrentVehicle.BurstTire(VehicleWheel.FrontRight);
						Player.Character.CurrentVehicle.BurstTire(VehicleWheel.RearRight);
					}
				}
			}
		}

		public void EffectVehicleRepairPlayer() {
			if (Player.Character.isInVehicle()) Player.Character.CurrentVehicle.Repair();
		}

		public void EffectVehicleSpamDoors() {
			if (Math.Round(EffectTimer.ElapsedTime / 100d, 0) % 10 == 0) foreach (Vehicle v in World.GetAllVehicles()) if (v.Exists()) for (int i = 0; i < 6; i++) Function.Call("SHUT_CAR_DOOR", v, i);
			if (Math.Round((EffectTimer.ElapsedTime + 500) / 100d, 0) % 10 == 0) foreach (Vehicle v in World.GetAllVehicles()) if (v.Exists()) for (int i = 0; i < 6; i++) Function.Call("OPEN_CAR_DOOR", v, i);
		}

		public void EffectVehicleSpawnBus() {
			World.CreateVehicle(new Model("BUS"), Player.Character.Position.Around(2f));
		}

		public void EffectVehicleSpawnInfernus() {
			World.CreateVehicle(new Model("INFERNUS"), Player.Character.Position.Around(2f));
		}

		public void EffectVehicleSpawnMaverick() {
			World.CreateVehicle(new Model("MAVERICK"), Player.Character.Position.Around(2f));
		}

		public void EffectVehicleSpawnPolice() {
			World.CreateVehicle(new Model("POLICE"), Player.Character.Position.Around(2f));
		}

		public void EffectVehicleSpawnRandom() {
			World.CreateVehicle(new Model(Vehicles[R.Next(Vehicles.Count)]), Player.Character.Position.Around(2f));
		}

		public void EffectVehicleSpawnTaxis() { // this one's for the speedrunners out there :^)
			for (int i = 0; i < 10; i++) {
				World.CreateVehicle(new Model("TAXI"), Player.Character.Position.Around((float)R.NextDouble() * 8f)).CreatePedOnSeat(VehicleSeat.Driver, new Model("m_m_taxidriver"));
			}
		}

		public void EffectVehicleSpawnTug() {
			World.CreateVehicle(new Model("TUGA"), Player.Character.Position.Around(2f));
		}

		public void EffectVehicleTrafficBlack() {
			foreach (Vehicle v in World.GetAllVehicles()) {
				if (v.Exists()) v.Color = ColorIndex.Black;
			}
		}

		public void EffectVehicleTrafficBlue() {
			foreach (Vehicle v in World.GetAllVehicles()) {
				if (v.Exists()) v.Color = ColorIndex.BrightBluePoly3;
			}
		}

		public void EffectVehicleTrafficNone() {
			foreach (Vehicle v in World.GetAllVehicles()) {
				if (v.Exists() & !v.isRequiredForMission & v != Player.Character.CurrentVehicle) v.Delete();
			}
		}

		public void EffectVehicleTrafficRed() {
			foreach (Vehicle v in World.GetAllVehicles()) {
				if (v.Exists()) v.Color = ColorIndex.VeryRed;
			}
		}

		public void EffectVehicleTrafficWhite() {
			foreach (Vehicle v in World.GetAllVehicles()) {
				if (v.Exists()) v.Color = ColorIndex.FrostWhite;
			}
		}
		#endregion

		#region Weather Effects
		public void EffectWeatherCloudy() {
			Function.Call("FORCE_WEATHER_NOW", 3);
		}

		public void EffectWeatherFoggy() {
			Function.Call("FORCE_WEATHER_NOW", 6);
		}

		public void EffectWeatherSunny() {
			Function.Call("FORCE_WEATHER_NOW", 0);
		}

		public void EffectWeatherThunder() {
			Function.Call("FORCE_WEATHER_NOW", 7);
		}
		#endregion
	}

	public struct Effect {
		public Effect(string name, Action start, string[] conflicts = null) {
			Name = name;
			Start = start;
			Timer = null;
			Loop = null;
			Stop = null;
			Conflicts = conflicts;
		}

		public Effect(string name, Action start, Timer timer, Action loop, Action stop, string[] conflicts = null) {
			Name = name;
			Timer = timer;
			Start = start;
			Loop = loop;
			Stop = stop;
			Conflicts = conflicts;
		}

		public string Name { get; }
		public Timer Timer { get; }
		public Action Start { get; }
		public Action Loop { get; }
		public Action Stop { get; }
		public string[] Conflicts { get; }
	}
}
