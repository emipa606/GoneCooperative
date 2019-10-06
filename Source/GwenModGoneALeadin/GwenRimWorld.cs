using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using RimWorld.Planet;
using Verse.AI;
using UnityEngine;

namespace GBKT_Defs
{
    public class GBKT_TraitChecker : WorldComponent
    {
        private List<BodyPartDef> GBKT_BodyPartDef = new List<BodyPartDef>();
        private TraitDef[] GBKT_TraitDef = new TraitDef[16];
        List<Map> maps;

        public GBKT_TraitChecker(World world) : base(world)
        {
            //GBKT_BodyPartDef.Add(BodyPartDefOf.Body);
            GBKT_BodyPartDef.Add(BodyPartDefOf.Brain);
            GBKT_TraitDef[0] = GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Taskmaster;
            GBKT_TraitDef[1] = GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_MeleeCommander;
            GBKT_TraitDef[2] = GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_CheerLeader;
            GBKT_TraitDef[3] = GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Bodyguard;
            GBKT_TraitDef[4] = GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Teacher;
            GBKT_TraitDef[5] = GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Gibbering;
            GBKT_TraitDef[6] = GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_ResearchAssistant;
            GBKT_TraitDef[7] = GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_BeastMaster;
            GBKT_TraitDef[8] = GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Outrider;
            GBKT_TraitDef[9] = GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_PlayLeader;
            GBKT_TraitDef[10] = GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_CreativePlanner;
            GBKT_TraitDef[11] = GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_FireBrigadier;
            GBKT_TraitDef[12] = GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_SpasticFool;
            GBKT_TraitDef[13] = GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Blabbermouth;
            GBKT_TraitDef[14] = GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Damsel;
            GBKT_TraitDef[15] = GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Sterilizer;
        }

        public override void WorldComponentTick()
        {
            maps = Find.Maps;
            foreach (Map map in maps)
            {
                List<Pawn> pawns = map.mapPawns.AllPawnsSpawned;
                foreach (Pawn pawn in pawns)
                {
                    //Log.Error("this runs");

                    for (int i = 0; i < GBKT_TraitDef.Length; i++)
                    {
                        bool? hasTrait = pawn.story?.traits?.HasTrait(GBKT_TraitDef[i]);
                        if (hasTrait == true)
                        {
                            if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Gibbering)
                            {
                                if(pawn.health.capacities.GetLevel(GBKT_DefinitionTypes.GBTK_DefinitionTypes_CapacityDeff.Talking) > 0f)
                                {
                                    bool tryThisHediff = HediffGiverUtility.TryApply(pawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_GibberingBase, GBKT_BodyPartDef);
                                }
                            }
                            if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_CreativePlanner)
                            {
                                int randomNumber;
                                randomNumber = UnityEngine.Random.Range(1, 1000);
                                if (randomNumber == 1 && !pawn.Awake())
                                {
                                    pawn.Awake();
                                }
                            }
                            if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Sterilizer)
                            {
                                
                                string PawnsCurrentJob = pawn.CurJobDef.ToString();
                                Room room = pawn.GetRoom(RegionType.Set_Passable);
                                if (PawnsCurrentJob == "Clean" && room.Role != RoomRoleDefOf.Hospital)
                                {
                                    bool tryThisHediff = HediffGiverUtility.TryApply(pawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_SterilizerCleaning, GBKT_BodyPartDef);
                                }
                                if (PawnsCurrentJob == "Clean" && room.Role == RoomRoleDefOf.Hospital)
                                {
                                    bool tryThisHediff = HediffGiverUtility.TryApply(pawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_SterilizerCleaning2, GBKT_BodyPartDef);
                                }
                            }
                            foreach (Pawn possibleFacPawn in pawns)
                            {
                                //ALLIED COLONISTS RANGE 25
                                if (possibleFacPawn.Position.DistanceTo(pawn.Position) < 26 && !pawn.InMentalState && !possibleFacPawn.InMentalState && possibleFacPawn.Awake() && pawn.Awake() && pawn.Faction == possibleFacPawn.Faction && !possibleFacPawn.RaceProps.Animal)
                                {
                                    //CREATIVE PLANNER
                                    if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_CreativePlanner)
                                    {
                                        if(possibleFacPawn != pawn)
                                        {
                                            bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_CreativePlannerNear, GBKT_BodyPartDef);
                                        }
                                    }
                                    //Fire Brigadier   
                                    if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_FireBrigadier)
                                    {
                                        string PawnsCurrentJob = "Null";
                                        if (possibleFacPawn.CurJobDef != null)
                                        {
                                            PawnsCurrentJob = possibleFacPawn.CurJobDef.ToString();
                                        }
                                        if (PawnsCurrentJob == "TriggerFirefoamPopper" || PawnsCurrentJob == "BeatFire" || PawnsCurrentJob == "ExtinguishSelf")
                                        {
                                            bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_FireBrigadierNear, GBKT_BodyPartDef);
                                        }
                                        string PawnsCurrentJob2 = "Null";
                                        if (pawn.CurJobDef != null)
                                        {
                                            PawnsCurrentJob2 = possibleFacPawn.CurJobDef.ToString();
                                        }
                                        if (PawnsCurrentJob2 == "TriggerFirefoamPopper" || PawnsCurrentJob2 == "BeatFire" || PawnsCurrentJob2 == "ExtinguishSelf")
                                        {
                                            bool tryThisHediff = HediffGiverUtility.TryApply(pawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_FireBrigadierNear, GBKT_BodyPartDef);
                                        }
                                    }
                                }
                                //ALLIED COLONISTS RANGE 25 damsel
                                if (possibleFacPawn.Position.DistanceTo(pawn.Position) < 26 && !pawn.InMentalState && !possibleFacPawn.InMentalState && possibleFacPawn.Awake() && pawn.Faction == possibleFacPawn.Faction)
                                {
                                    //DAMSEL
                                    if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Damsel && pawn.Downed)
                                    {
                                       bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_DamselInTrouble, GBKT_BodyPartDef);
                                    }
                                    if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Damsel)
                                    {
                                        string PawnsCurrentJob = "Null";
                                        if (possibleFacPawn.CurJobDef != null)
                                        {
                                            PawnsCurrentJob = possibleFacPawn.CurJobDef.ToString();
                                        }
                                        if (PawnsCurrentJob == "TendPatient" && possibleFacPawn.CurJob.targetA == pawn)
                                        {
                                            bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_DamselBeingTended, GBKT_BodyPartDef);
                                        }
                                    }
                                }
                                //ALLIED COLONISTS RANGE 10
                                if (possibleFacPawn != pawn && possibleFacPawn.Position.DistanceTo(pawn.Position) < 11 && pawn.Faction == possibleFacPawn.Faction && !possibleFacPawn.RaceProps.Animal && !pawn.InMentalState && possibleFacPawn.Awake() && !possibleFacPawn.InMentalState && pawn.Awake())
                                {
                                    //TASKMASTER
                                    if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Taskmaster)
                                    {
                                        if (pawn.health.capacities.GetLevel(GBKT_DefinitionTypes.GBTK_DefinitionTypes_CapacityDeff.Talking) > 0f && possibleFacPawn.health.capacities.GetLevel(GBKT_DefinitionTypes.GBTK_DefinitionTypes_CapacityDeff.Hearing) > 0f)
                                        {
                                            bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_TaskmasterNear, GBKT_BodyPartDef);
                                        }
                                    }
                                    //MELEE COMMANDER
                                    if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_MeleeCommander)
                                    {
                                        if (possibleFacPawn.health.capacities.GetLevel(GBKT_DefinitionTypes.GBTK_DefinitionTypes_CapacityDeff.Sight) > 0f || possibleFacPawn.health.capacities.GetLevel(GBKT_DefinitionTypes.GBTK_DefinitionTypes_CapacityDeff.Hearing) > 0f)
                                        {
                                            bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_MeleeCommanderNear, GBKT_BodyPartDef);
                                        }
                                    }
                                    //CHEER LEADER
                                    if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_CheerLeader)
                                    {
                                        if (pawn.health.capacities.GetLevel(GBKT_DefinitionTypes.GBTK_DefinitionTypes_CapacityDeff.Talking) > 0f && possibleFacPawn.health.capacities.GetLevel(GBKT_DefinitionTypes.GBTK_DefinitionTypes_CapacityDeff.Hearing) > 0f)
                                        {
                                            bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_CheerLeaderNear, GBKT_BodyPartDef);
                                        }
                                    }
                                    //PLAY LEADER 
                                    if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_PlayLeader)
                                    {
                                        string PawnsCurrentJoyKind = "Null";
                                        if (possibleFacPawn.CurJobDef.joyKind != null)
                                        {
                                            PawnsCurrentJoyKind = possibleFacPawn.CurJobDef.joyKind.ToString();
                                        }
                                        if (PawnsCurrentJoyKind != "Null")
                                        {
                                            bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_PlayLeaderNear, GBKT_BodyPartDef);
                                        }
                                    }
                                }
                                //ALLIED COLONISTS RANGE 5
                                if (possibleFacPawn != pawn && possibleFacPawn.Position.DistanceTo(pawn.Position) < 6 && !pawn.InMentalState && !possibleFacPawn.InMentalState && possibleFacPawn.Awake() && pawn.Awake() && pawn.Faction == possibleFacPawn.Faction && !possibleFacPawn.RaceProps.Animal)
                                {
                                    //TEACHER
                                    if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Teacher)
                                    {
                                        if (possibleFacPawn.health.capacities.GetLevel(GBKT_DefinitionTypes.GBTK_DefinitionTypes_CapacityDeff.Sight) > 0f && possibleFacPawn.health.capacities.GetLevel(GBKT_DefinitionTypes.GBTK_DefinitionTypes_CapacityDeff.Hearing) > 0f)
                                        {
                                            bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_TeacherNear, GBKT_BodyPartDef);
                                        }
                                        if (possibleFacPawn.story.adulthood.baseDesc == null && possibleFacPawn.health.capacities.GetLevel(GBKT_DefinitionTypes.GBTK_DefinitionTypes_CapacityDeff.Sight) > 0f && possibleFacPawn.health.capacities.GetLevel(GBKT_DefinitionTypes.GBTK_DefinitionTypes_CapacityDeff.Hearing) > 0f)
                                        {
                                            bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_TeacherNear2, GBKT_BodyPartDef);
                                        }
                                    }
                                    //RESEARCH ASSISTANT 
                                    if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_ResearchAssistant)
                                    {
                                        bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_ResearchAssistantNear, GBKT_BodyPartDef);
                                    }
                                    //Sterilizer
                                    if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Sterilizer)
                                    {
                                        string PawnsCurrentJob = possibleFacPawn.CurJobDef.ToString(); ;
                                        string PawnsCurrentJob2 = pawn.CurJobDef.ToString();
                                        Room room = pawn.GetRoom(RegionType.Set_Passable);
                                        if (possibleFacPawn.CurJobDef != null)
                                        {
                                            PawnsCurrentJob = possibleFacPawn.CurJobDef.ToString();
                                        }
                                        if (!pawn.Downed && !pawn.InBed() && PawnsCurrentJob == "TendPatient")
                                        {
                                            bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_SterilizerNearby, GBKT_BodyPartDef);
                                        }
                                        if (!pawn.Downed && !pawn.InBed() && possibleFacPawn.InBed() && room.Role == RoomRoleDefOf.Hospital)
                                        {
                                            bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_SterilizerNearby2, GBKT_BodyPartDef);
                                        }
                                    }
                                }

                                //ALL PAWNS RANGE 2
                                if (possibleFacPawn != pawn && possibleFacPawn.Position.DistanceTo(pawn.Position) < 3 && possibleFacPawn.Awake() && pawn.Awake())
                                {
                                    //GIBBERING
                                    if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_SpasticFool)
                                    {
                                        if (pawn.Drafted)
                                        {
                                            bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_SpasticFoolNear, GBKT_BodyPartDef);
                                        }
                                    }
                                }

                                //ALL PAWNS RANGE 10
                                if (possibleFacPawn != pawn && possibleFacPawn.Position.DistanceTo(pawn.Position) < 11 && !possibleFacPawn.RaceProps.Animal && possibleFacPawn.Awake() && pawn.Awake())
                                {
                                    //GIBBERING
                                    if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Gibbering)
                                    {
                                        if (pawn.health.capacities.GetLevel(GBKT_DefinitionTypes.GBTK_DefinitionTypes_CapacityDeff.Talking) > 0f && possibleFacPawn.health.capacities.GetLevel(GBKT_DefinitionTypes.GBTK_DefinitionTypes_CapacityDeff.Hearing) > 0f)
                                        {
                                            bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_GibberingNear, GBKT_BodyPartDef);
                                        }
                                    }
                                }

                                //ALL PAWNS RANGE 100
                                if (possibleFacPawn != pawn && possibleFacPawn.Position.DistanceTo(pawn.Position) < 100 && !possibleFacPawn.RaceProps.Animal && possibleFacPawn.Awake() && pawn.Awake())
                                {
                                    //GIBBERING
                                    if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Blabbermouth)
                                    {
                                        if (pawn.health.capacities.GetLevel(GBKT_DefinitionTypes.GBTK_DefinitionTypes_CapacityDeff.Talking) > 0f && possibleFacPawn.health.capacities.GetLevel(GBKT_DefinitionTypes.GBTK_DefinitionTypes_CapacityDeff.Hearing) > 0f)
                                        {
                                            bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_BlabbermouthNear, GBKT_BodyPartDef);
                                        }
                                    }
                                }
                                //ALLIED ANIMALS RANGE 10
                                if (possibleFacPawn != pawn && possibleFacPawn.Position.DistanceTo(pawn.Position) < 11 && pawn.Faction == possibleFacPawn.Faction && possibleFacPawn.RaceProps.Animal && !pawn.InMentalState && possibleFacPawn.Awake() && !possibleFacPawn.InMentalState && pawn.Awake())
                                {
                                    //CALVARY MASTER
                                    if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Outrider)
                                    { 
                                      bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_OutriderNear, GBKT_BodyPartDef);
                                    }
                                }
                                //ALLIED ANIMALS RANGE 1
                                if (possibleFacPawn != pawn && possibleFacPawn.Position.DistanceTo(pawn.Position) < 1 && pawn.Faction == possibleFacPawn.Faction && possibleFacPawn.RaceProps.Animal && !pawn.InMentalState && possibleFacPawn.Awake() && !possibleFacPawn.InMentalState && pawn.Awake())
                                {
                                    //CALVARY MASTER
                                    if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Outrider)
                                    {
                                        bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_OutriderAdjacent, GBKT_BodyPartDef);
                                    }
                                }
                                //ALLIED ANIMALS RANGE 100
                                if (possibleFacPawn != pawn && possibleFacPawn.Position.DistanceTo(pawn.Position) < 100 && possibleFacPawn.RaceProps.Animal && pawn.Faction == possibleFacPawn.Faction)
                                {
                                    //BEAST MASTER  
                                    if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_BeastMaster)
                                    {
                                        List<DirectPawnRelation> directRelations = possibleFacPawn.relations.DirectRelations;
                                        for (int u = 0; u < directRelations.Count; u++)
                                        {
                                            DirectPawnRelation directPawnRelation = directRelations[u];
                                            Pawn otherPawn = directPawnRelation.otherPawn;
                                            if (directPawnRelation.def == PawnRelationDefOf.Bond)
                                            {
                                                bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_BeastMasterNear, GBKT_BodyPartDef);
                                            }
                                            if (directPawnRelation.def == PawnRelationDefOf.Bond && pawn.CurJob.targetA.Equals(possibleFacPawn))
                                            {
                                                bool tryThisHediff = HediffGiverUtility.TryApply(pawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_BeastMasterWorkingWithBondeds, GBKT_BodyPartDef);
                                            }
                                        }
                                    }
                                }
                                //ALLIED PAWNS RANGE 1
                                if (possibleFacPawn != pawn && possibleFacPawn.Position.DistanceTo(pawn.Position) < 2 && !pawn.InMentalState && !possibleFacPawn.InMentalState && possibleFacPawn.Awake() && pawn.Awake() && !possibleFacPawn.HostileTo(pawn))
                                {
                                    //BODYGUARD
                                    if (GBKT_TraitDef[i] == GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_Bodyguard)
                                    {
                                        if (pawn.health.capacities.GetLevel(GBKT_DefinitionTypes.GBTK_DefinitionTypes_CapacityDeff.Talking) > 0f && possibleFacPawn.health.capacities.GetLevel(GBKT_DefinitionTypes.GBTK_DefinitionTypes_CapacityDeff.Hearing) > 0f)
                                        {
                                            bool tryThisHediff = HediffGiverUtility.TryApply(possibleFacPawn, GBKT_DefinitionTypes.GBKT_DefinitionTypes_Hediff.GBKT_BodyguardNear, GBKT_BodyPartDef);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}