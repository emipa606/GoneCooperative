using System.Collections.Generic;
using GBKT_DefinitionTypes;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace GBKT_Defs;

public class GBKT_TraitChecker : WorldComponent
{
    private readonly List<BodyPartDef> GBKT_BodyPartDef = [];
    private readonly TraitDef[] GBKT_TraitDef = new TraitDef[16];
    private List<Map> maps;

    public GBKT_TraitChecker(World world) : base(world)
    {
        GBKT_BodyPartDef.Add(DefDatabase<BodyPartDef>.GetNamedSilentFail("Brain"));
        GBKT_TraitDef[0] = GBKT_DefinitionTypes_Traits.GBKT_Taskmaster;
        GBKT_TraitDef[1] = GBKT_DefinitionTypes_Traits.GBKT_MeleeCommander;
        GBKT_TraitDef[2] = GBKT_DefinitionTypes_Traits.GBKT_CheerLeader;
        GBKT_TraitDef[3] = GBKT_DefinitionTypes_Traits.GBKT_Bodyguard;
        GBKT_TraitDef[4] = GBKT_DefinitionTypes_Traits.GBKT_Teacher;
        GBKT_TraitDef[5] = GBKT_DefinitionTypes_Traits.GBKT_Gibbering;
        GBKT_TraitDef[6] = GBKT_DefinitionTypes_Traits.GBKT_ResearchAssistant;
        GBKT_TraitDef[7] = GBKT_DefinitionTypes_Traits.GBKT_BeastMaster;
        GBKT_TraitDef[8] = GBKT_DefinitionTypes_Traits.GBKT_Outrider;
        GBKT_TraitDef[9] = GBKT_DefinitionTypes_Traits.GBKT_PlayLeader;
        GBKT_TraitDef[10] = GBKT_DefinitionTypes_Traits.GBKT_CreativePlanner;
        GBKT_TraitDef[11] = GBKT_DefinitionTypes_Traits.GBKT_FireBrigadier;
        GBKT_TraitDef[12] = GBKT_DefinitionTypes_Traits.GBKT_SpasticFool;
        GBKT_TraitDef[13] = GBKT_DefinitionTypes_Traits.GBKT_Blabbermouth;
        GBKT_TraitDef[14] = GBKT_DefinitionTypes_Traits.GBKT_Damsel;
        GBKT_TraitDef[15] = GBKT_DefinitionTypes_Traits.GBKT_Sterilizer;
    }

    public override void WorldComponentTick()
    {
        maps = Find.Maps;
        foreach (var map in maps)
        {
            var pawns = map.mapPawns.AllPawnsSpawned;
            foreach (var pawn in pawns)
            {
                //Log.Error("this runs");

                foreach (var traitDef in GBKT_TraitDef)
                {
                    var hasTrait = pawn.story?.traits?.HasTrait(traitDef);
                    if (hasTrait != true)
                    {
                        continue;
                    }

                    if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_Gibbering)
                    {
                        if (pawn.health.capacities.GetLevel(GBTK_DefinitionTypes_CapacityDeff.Talking) > 0f)
                        {
                            _ = HediffGiverUtility.TryApply(pawn,
                                GBKT_DefinitionTypes_Hediff.GBKT_GibberingBase, GBKT_BodyPartDef);
                        }
                    }

                    if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_CreativePlanner)
                    {
                        var randomNumber = Random.Range(1, 1000);
                        if (randomNumber == 1 && !pawn.Awake())
                        {
                            pawn.Awake();
                        }
                    }

                    if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_Sterilizer)
                    {
                        var PawnsCurrentJob = pawn.CurJobDef.ToString();
                        var room = pawn.GetRoom(RegionType.Set_Passable);
                        if (PawnsCurrentJob == "Clean" && room.Role != RoomRoleDefOf.Hospital)
                        {
                            _ = HediffGiverUtility.TryApply(pawn,
                                GBKT_DefinitionTypes_Hediff.GBKT_SterilizerCleaning, GBKT_BodyPartDef);
                        }

                        if (PawnsCurrentJob == "Clean" && room.Role == RoomRoleDefOf.Hospital)
                        {
                            _ = HediffGiverUtility.TryApply(pawn,
                                GBKT_DefinitionTypes_Hediff.GBKT_SterilizerCleaning2, GBKT_BodyPartDef);
                        }
                    }

                    foreach (var possibleFacPawn in pawns)
                    {
                        //ALLIED COLONISTS RANGE 25
                        if (possibleFacPawn.Position.DistanceTo(pawn.Position) < 26 && !pawn.InMentalState &&
                            !possibleFacPawn.InMentalState && possibleFacPawn.Awake() && pawn.Awake() &&
                            pawn.Faction == possibleFacPawn.Faction && !possibleFacPawn.RaceProps.Animal)
                        {
                            //CREATIVE PLANNER
                            if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_CreativePlanner)
                            {
                                if (possibleFacPawn != pawn)
                                {
                                    _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                        GBKT_DefinitionTypes_Hediff.GBKT_CreativePlannerNear, GBKT_BodyPartDef);
                                }
                            }

                            //Fire Brigadier   
                            if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_FireBrigadier)
                            {
                                var PawnsCurrentJob = "Null";
                                if (possibleFacPawn.CurJobDef != null)
                                {
                                    PawnsCurrentJob = possibleFacPawn.CurJobDef.ToString();
                                }

                                if (PawnsCurrentJob == "TriggerFirefoamPopper" || PawnsCurrentJob == "BeatFire" ||
                                    PawnsCurrentJob == "ExtinguishSelf")
                                {
                                    _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                        GBKT_DefinitionTypes_Hediff.GBKT_FireBrigadierNear, GBKT_BodyPartDef);
                                }

                                var PawnsCurrentJob2 = "Null";
                                if (pawn.CurJobDef != null)
                                {
                                    PawnsCurrentJob2 = possibleFacPawn.CurJobDef.ToString();
                                }

                                if (PawnsCurrentJob2 == "TriggerFirefoamPopper" || PawnsCurrentJob2 == "BeatFire" ||
                                    PawnsCurrentJob2 == "ExtinguishSelf")
                                {
                                    _ = HediffGiverUtility.TryApply(pawn,
                                        GBKT_DefinitionTypes_Hediff.GBKT_FireBrigadierNear, GBKT_BodyPartDef);
                                }
                            }
                        }

                        //ALLIED COLONISTS RANGE 25 damsel
                        if (possibleFacPawn.Position.DistanceTo(pawn.Position) < 26 && !pawn.InMentalState &&
                            !possibleFacPawn.InMentalState && possibleFacPawn.Awake() &&
                            pawn.Faction == possibleFacPawn.Faction)
                        {
                            //DAMSEL
                            if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_Damsel && pawn.Downed)
                            {
                                _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                    GBKT_DefinitionTypes_Hediff.GBKT_DamselInTrouble, GBKT_BodyPartDef);
                            }

                            if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_Damsel)
                            {
                                var PawnsCurrentJob = "Null";
                                if (possibleFacPawn.CurJobDef != null)
                                {
                                    PawnsCurrentJob = possibleFacPawn.CurJobDef.ToString();
                                }

                                if (PawnsCurrentJob == "TendPatient" && possibleFacPawn.CurJob.targetA == pawn)
                                {
                                    _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                        GBKT_DefinitionTypes_Hediff.GBKT_DamselBeingTended, GBKT_BodyPartDef);
                                }
                            }
                        }

                        //ALLIED COLONISTS RANGE 10
                        if (possibleFacPawn != pawn && possibleFacPawn.Position.DistanceTo(pawn.Position) < 11 &&
                            pawn.Faction == possibleFacPawn.Faction && !possibleFacPawn.RaceProps.Animal &&
                            !pawn.InMentalState && possibleFacPawn.Awake() && !possibleFacPawn.InMentalState &&
                            pawn.Awake())
                        {
                            //TASKMASTER
                            if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_Taskmaster)
                            {
                                if (pawn.health.capacities.GetLevel(GBTK_DefinitionTypes_CapacityDeff.Talking) >
                                    0f && possibleFacPawn.health.capacities.GetLevel(
                                        GBTK_DefinitionTypes_CapacityDeff.Hearing) > 0f)
                                {
                                    _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                        GBKT_DefinitionTypes_Hediff.GBKT_TaskmasterNear, GBKT_BodyPartDef);
                                }
                            }

                            //MELEE COMMANDER
                            if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_MeleeCommander)
                            {
                                if (possibleFacPawn.health.capacities.GetLevel(GBTK_DefinitionTypes_CapacityDeff
                                        .Sight) > 0f ||
                                    possibleFacPawn.health.capacities.GetLevel(GBTK_DefinitionTypes_CapacityDeff
                                        .Hearing) > 0f)
                                {
                                    _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                        GBKT_DefinitionTypes_Hediff.GBKT_MeleeCommanderNear, GBKT_BodyPartDef);
                                }
                            }

                            //CHEER LEADER
                            if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_CheerLeader)
                            {
                                if (pawn.health.capacities.GetLevel(GBTK_DefinitionTypes_CapacityDeff.Talking) >
                                    0f && possibleFacPawn.health.capacities.GetLevel(
                                        GBTK_DefinitionTypes_CapacityDeff.Hearing) > 0f)
                                {
                                    _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                        GBKT_DefinitionTypes_Hediff.GBKT_CheerLeaderNear, GBKT_BodyPartDef);
                                }
                            }

                            //PLAY LEADER 
                            if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_PlayLeader)
                            {
                                var PawnsCurrentJoyKind = "Null";
                                if (possibleFacPawn.CurJobDef.joyKind != null)
                                {
                                    PawnsCurrentJoyKind = possibleFacPawn.CurJobDef.joyKind.ToString();
                                }

                                if (PawnsCurrentJoyKind != "Null")
                                {
                                    _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                        GBKT_DefinitionTypes_Hediff.GBKT_PlayLeaderNear, GBKT_BodyPartDef);
                                }
                            }
                        }

                        //ALLIED COLONISTS RANGE 5
                        if (possibleFacPawn != pawn && possibleFacPawn.Position.DistanceTo(pawn.Position) < 6 &&
                            !pawn.InMentalState && !possibleFacPawn.InMentalState && possibleFacPawn.Awake() &&
                            pawn.Awake() && pawn.Faction == possibleFacPawn.Faction &&
                            !possibleFacPawn.RaceProps.Animal)
                        {
                            //TEACHER
                            if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_Teacher)
                            {
                                if (possibleFacPawn.health.capacities.GetLevel(GBTK_DefinitionTypes_CapacityDeff
                                        .Sight) > 0f &&
                                    possibleFacPawn.health.capacities.GetLevel(GBTK_DefinitionTypes_CapacityDeff
                                        .Hearing) > 0f)
                                {
                                    _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                        GBKT_DefinitionTypes_Hediff.GBKT_TeacherNear, GBKT_BodyPartDef);
                                }

                                if (possibleFacPawn.story.Adulthood.baseDesc == null &&
                                    possibleFacPawn.health.capacities.GetLevel(GBTK_DefinitionTypes_CapacityDeff
                                        .Sight) > 0f &&
                                    possibleFacPawn.health.capacities.GetLevel(GBTK_DefinitionTypes_CapacityDeff
                                        .Hearing) > 0f)
                                {
                                    _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                        GBKT_DefinitionTypes_Hediff.GBKT_TeacherNear2, GBKT_BodyPartDef);
                                }
                            }

                            //RESEARCH ASSISTANT 
                            if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_ResearchAssistant)
                            {
                                _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                    GBKT_DefinitionTypes_Hediff.GBKT_ResearchAssistantNear, GBKT_BodyPartDef);
                            }

                            //Sterilizer
                            if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_Sterilizer)
                            {
                                var PawnsCurrentJob = possibleFacPawn.CurJobDef.ToString();
                                _ = pawn.CurJobDef.ToString();
                                var room = pawn.GetRoom(RegionType.Set_Passable);
                                if (possibleFacPawn.CurJobDef != null)
                                {
                                    PawnsCurrentJob = possibleFacPawn.CurJobDef.ToString();
                                }

                                if (!pawn.Downed && !pawn.InBed() && PawnsCurrentJob == "TendPatient")
                                {
                                    _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                        GBKT_DefinitionTypes_Hediff.GBKT_SterilizerNearby, GBKT_BodyPartDef);
                                }

                                if (!pawn.Downed && !pawn.InBed() && possibleFacPawn.InBed() &&
                                    room.Role == RoomRoleDefOf.Hospital)
                                {
                                    _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                        GBKT_DefinitionTypes_Hediff.GBKT_SterilizerNearby2, GBKT_BodyPartDef);
                                }
                            }
                        }

                        //ALL PAWNS RANGE 2
                        if (possibleFacPawn != pawn && possibleFacPawn.Position.DistanceTo(pawn.Position) < 3 &&
                            possibleFacPawn.Awake() && pawn.Awake())
                        {
                            //GIBBERING
                            if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_SpasticFool)
                            {
                                if (pawn.Drafted)
                                {
                                    _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                        GBKT_DefinitionTypes_Hediff.GBKT_SpasticFoolNear, GBKT_BodyPartDef);
                                }
                            }
                        }

                        //ALL PAWNS RANGE 10
                        if (possibleFacPawn != pawn && possibleFacPawn.Position.DistanceTo(pawn.Position) < 11 &&
                            !possibleFacPawn.RaceProps.Animal && possibleFacPawn.Awake() && pawn.Awake())
                        {
                            //GIBBERING
                            if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_Gibbering)
                            {
                                if (pawn.health.capacities.GetLevel(GBTK_DefinitionTypes_CapacityDeff.Talking) >
                                    0f && possibleFacPawn.health.capacities.GetLevel(
                                        GBTK_DefinitionTypes_CapacityDeff.Hearing) > 0f)
                                {
                                    _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                        GBKT_DefinitionTypes_Hediff.GBKT_GibberingNear, GBKT_BodyPartDef);
                                }
                            }
                        }

                        //ALL PAWNS RANGE 100
                        if (possibleFacPawn != pawn && possibleFacPawn.Position.DistanceTo(pawn.Position) < 100 &&
                            !possibleFacPawn.RaceProps.Animal && possibleFacPawn.Awake() && pawn.Awake())
                        {
                            //GIBBERING
                            if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_Blabbermouth)
                            {
                                if (pawn.health.capacities.GetLevel(GBTK_DefinitionTypes_CapacityDeff.Talking) >
                                    0f && possibleFacPawn.health.capacities.GetLevel(
                                        GBTK_DefinitionTypes_CapacityDeff.Hearing) > 0f)
                                {
                                    _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                        GBKT_DefinitionTypes_Hediff.GBKT_BlabbermouthNear, GBKT_BodyPartDef);
                                }
                            }
                        }

                        //ALLIED ANIMALS RANGE 10
                        if (possibleFacPawn != pawn && possibleFacPawn.Position.DistanceTo(pawn.Position) < 11 &&
                            pawn.Faction == possibleFacPawn.Faction && possibleFacPawn.RaceProps.Animal &&
                            !pawn.InMentalState && possibleFacPawn.Awake() && !possibleFacPawn.InMentalState &&
                            pawn.Awake())
                        {
                            //CALVARY MASTER
                            if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_Outrider)
                            {
                                _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                    GBKT_DefinitionTypes_Hediff.GBKT_OutriderNear, GBKT_BodyPartDef);
                            }
                        }

                        //ALLIED ANIMALS RANGE 1
                        if (possibleFacPawn != pawn && possibleFacPawn.Position.DistanceTo(pawn.Position) < 1 &&
                            pawn.Faction == possibleFacPawn.Faction && possibleFacPawn.RaceProps.Animal &&
                            !pawn.InMentalState && possibleFacPawn.Awake() && !possibleFacPawn.InMentalState &&
                            pawn.Awake())
                        {
                            //CALVARY MASTER
                            if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_Outrider)
                            {
                                _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                    GBKT_DefinitionTypes_Hediff.GBKT_OutriderAdjacent, GBKT_BodyPartDef);
                            }
                        }

                        //ALLIED ANIMALS RANGE 100
                        if (possibleFacPawn != pawn && possibleFacPawn.Position.DistanceTo(pawn.Position) < 100 &&
                            possibleFacPawn.RaceProps.Animal && pawn.Faction == possibleFacPawn.Faction)
                        {
                            //BEAST MASTER  
                            if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_BeastMaster)
                            {
                                var directRelations = possibleFacPawn.relations.DirectRelations;
                                foreach (var directPawnRelation in directRelations)
                                {
                                    _ = directPawnRelation.otherPawn;
                                    if (directPawnRelation.def == PawnRelationDefOf.Bond)
                                    {
                                        _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                            GBKT_DefinitionTypes_Hediff.GBKT_BeastMasterNear, GBKT_BodyPartDef);
                                    }

                                    if (directPawnRelation.def == PawnRelationDefOf.Bond &&
                                        pawn.CurJob.targetA.Equals(possibleFacPawn))
                                    {
                                        _ = HediffGiverUtility.TryApply(pawn,
                                            GBKT_DefinitionTypes_Hediff.GBKT_BeastMasterWorkingWithBondeds,
                                            GBKT_BodyPartDef);
                                    }
                                }
                            }
                        }

                        //ALLIED PAWNS RANGE 1
                        if (possibleFacPawn == pawn || !(possibleFacPawn.Position.DistanceTo(pawn.Position) < 2) ||
                            pawn.InMentalState || possibleFacPawn.InMentalState || !possibleFacPawn.Awake() ||
                            !pawn.Awake() || possibleFacPawn.HostileTo(pawn))
                        {
                            continue;
                        }

                        //BODYGUARD
                        if (traitDef != GBKT_DefinitionTypes_Traits.GBKT_Bodyguard)
                        {
                            continue;
                        }

                        if (pawn.health.capacities.GetLevel(GBTK_DefinitionTypes_CapacityDeff.Talking) >
                            0f && possibleFacPawn.health.capacities.GetLevel(
                                GBTK_DefinitionTypes_CapacityDeff.Hearing) > 0f)
                        {
                            _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                GBKT_DefinitionTypes_Hediff.GBKT_BodyguardNear, GBKT_BodyPartDef);
                        }
                    }
                }
            }
        }
    }
}