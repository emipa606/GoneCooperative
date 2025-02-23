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
                var pawnAwake = pawn.Awake();

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
                        if (randomNumber == 1 && !pawnAwake)
                        {
                            RestUtility.WakeUp(pawn);
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
                        if (possibleFacPawn != pawn && possibleFacPawn.story?.traits?.HasTrait(traitDef) == true)
                        {
                            // Ignore other pawns that has the same trait as they will give themselves the effect
                            continue;
                        }

                        var range = possibleFacPawn.Position.DistanceTo(pawn.Position);
                        var possiblePawnAwake = possibleFacPawn.Awake();

                        if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_CreativePlanner ||
                            traitDef == GBKT_DefinitionTypes_Traits.GBKT_FireBrigadier)
                            //ALLIED COLONISTS RANGE 25
                        {
                            if (range < 26 && !pawn.InMentalState && !possibleFacPawn.InMentalState &&
                                possiblePawnAwake && pawnAwake && pawn.Faction == possibleFacPawn.Faction &&
                                !possibleFacPawn.RaceProps.Animal)
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

                                    if (PawnsCurrentJob is "TriggerFirefoamPopper" or "BeatFire" or "ExtinguishSelf")
                                    {
                                        _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                            GBKT_DefinitionTypes_Hediff.GBKT_FireBrigadierNear, GBKT_BodyPartDef);
                                    }

                                    var PawnsCurrentJob2 = "Null";
                                    if (pawn.CurJobDef != null)
                                    {
                                        PawnsCurrentJob2 = possibleFacPawn.CurJobDef.ToString();
                                    }

                                    if (PawnsCurrentJob2 is "TriggerFirefoamPopper" or "BeatFire" or "ExtinguishSelf")
                                    {
                                        _ = HediffGiverUtility.TryApply(pawn,
                                            GBKT_DefinitionTypes_Hediff.GBKT_FireBrigadierNear, GBKT_BodyPartDef);
                                    }
                                }
                            }
                        }

                        if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_Damsel)
                        {
                            //ALLIED COLONISTS RANGE 25 damsel
                            if (range < 26 && !pawn.InMentalState && !possibleFacPawn.InMentalState &&
                                possiblePawnAwake && pawn.Faction == possibleFacPawn.Faction)
                            {
                                //DAMSEL
                                if (pawn.Downed)
                                {
                                    _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                        GBKT_DefinitionTypes_Hediff.GBKT_DamselInTrouble, GBKT_BodyPartDef);
                                }

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

                        if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_Taskmaster ||
                            traitDef == GBKT_DefinitionTypes_Traits.GBKT_MeleeCommander ||
                            traitDef == GBKT_DefinitionTypes_Traits.GBKT_CheerLeader ||
                            traitDef == GBKT_DefinitionTypes_Traits.GBKT_PlayLeader)
                            //ALLIED COLONISTS RANGE 10
                        {
                            if (possibleFacPawn != pawn && range < 11 && pawn.Faction == possibleFacPawn.Faction &&
                                !possibleFacPawn.RaceProps.Animal && !pawn.InMentalState && possiblePawnAwake &&
                                !possibleFacPawn.InMentalState && pawnAwake)
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
                        }

                        if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_Teacher ||
                            traitDef == GBKT_DefinitionTypes_Traits.GBKT_ResearchAssistant ||
                            traitDef == GBKT_DefinitionTypes_Traits.GBKT_Sterilizer)

                            //ALLIED COLONISTS RANGE 5
                        {
                            if (possibleFacPawn != pawn && range < 6 &&
                                !pawn.InMentalState && !possibleFacPawn.InMentalState && possiblePawnAwake &&
                                pawnAwake && pawn.Faction == possibleFacPawn.Faction &&
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
                        }

                        //GBKT_SpasticFool
                        if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_SpasticFool)
                        {
                            //ALL PAWNS RANGE 2
                            if (possibleFacPawn != pawn && range < 3 &&
                                possiblePawnAwake && pawnAwake)
                            {
                                if (pawn.Drafted)
                                {
                                    _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                        GBKT_DefinitionTypes_Hediff.GBKT_SpasticFoolNear, GBKT_BodyPartDef);
                                }
                            }
                        }

                        //GIBBERING
                        if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_Gibbering)
                        {
                            //ALL PAWNS RANGE 10
                            if (possibleFacPawn != pawn && range < 11 &&
                                !possibleFacPawn.RaceProps.Animal && possiblePawnAwake && pawnAwake)
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

                        //GBKT_Blabbermouth
                        if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_Blabbermouth)
                        {
                            //ALL PAWNS RANGE 100
                            if (possibleFacPawn != pawn && range < 100 &&
                                !possibleFacPawn.RaceProps.Animal && possiblePawnAwake && pawnAwake)
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

                        //CALVARY MASTER
                        if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_Outrider)
                        {
                            //ALLIED ANIMALS RANGE 10
                            if (possibleFacPawn != pawn && range < 11 &&
                                pawn.Faction == possibleFacPawn.Faction && possibleFacPawn.RaceProps.Animal &&
                                !pawn.InMentalState && possiblePawnAwake && !possibleFacPawn.InMentalState &&
                                pawnAwake)
                            {
                                _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                    GBKT_DefinitionTypes_Hediff.GBKT_OutriderNear, GBKT_BodyPartDef);
                            }
                        }

                        //CALVARY MASTER
                        if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_Outrider)
                        {
                            //ALLIED ANIMALS RANGE 1
                            if (possibleFacPawn != pawn && range < 1 &&
                                pawn.Faction == possibleFacPawn.Faction && possibleFacPawn.RaceProps.Animal &&
                                !pawn.InMentalState && possiblePawnAwake && !possibleFacPawn.InMentalState &&
                                pawnAwake)
                            {
                                _ = HediffGiverUtility.TryApply(possibleFacPawn,
                                    GBKT_DefinitionTypes_Hediff.GBKT_OutriderAdjacent, GBKT_BodyPartDef);
                            }
                        }

                        //BEAST MASTER  
                        if (traitDef == GBKT_DefinitionTypes_Traits.GBKT_BeastMaster)
                        {
                            //ALLIED ANIMALS RANGE 100
                            if (possibleFacPawn != pawn && range < 100 &&
                                possibleFacPawn.RaceProps.Animal && pawn.Faction == possibleFacPawn.Faction)
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

                        //BODYGUARD
                        if (traitDef != GBKT_DefinitionTypes_Traits.GBKT_Bodyguard)
                        {
                            continue;
                        }

                        //ALLIED PAWNS RANGE 1
                        if (possibleFacPawn == pawn || !(range < 2) ||
                            pawn.InMentalState || possibleFacPawn.InMentalState || !possiblePawnAwake ||
                            !pawnAwake || possibleFacPawn.HostileTo(pawn))
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