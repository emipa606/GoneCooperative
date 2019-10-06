using RimWorld;
using Verse;
using RimWorld.Planet;
using System.Collections.Generic;

namespace GBKT_Gone_Cooperative_ThoughtWorkers
{
    public class ThoughtWorker_PlayLEaderNotInARecRoom : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn pawn)
        {
            Room room = pawn.GetRoom(RegionType.Set_Passable);
            if (!pawn.Spawned)
                return ThoughtState.Inactive;
            if (!pawn.RaceProps.Humanlike)
                return ThoughtState.Inactive;
            if (!pawn.story.traits.HasTrait(GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_PlayLeader))
                return ThoughtState.Inactive;
            if (room.Role == RoomRoleDefOf.RecRoom)
                return ThoughtState.Inactive;
            if (room == null)
                return ThoughtState.Inactive;
            return ThoughtState.ActiveAtStage(0);
        }
    }
    public class ThoughtWorker_FireBrigadierFightingAFire : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn pawn)
        {
            string PawnsCurrentJob = pawn.CurJobDef.ToString();
            if (!pawn.Spawned)
                return ThoughtState.Inactive;
            if (!pawn.RaceProps.Humanlike)
                return ThoughtState.Inactive;
            if (!pawn.story.traits.HasTrait(GBKT_DefinitionTypes.GBKT_DefinitionTypes_Traits.GBKT_PlayLeader))
                return ThoughtState.Inactive;
            if (PawnsCurrentJob == "TriggerFirefoamPopper" || PawnsCurrentJob == "BeatFire" || PawnsCurrentJob == "ExtinguishSelf")
                return ThoughtState.ActiveAtStage(0);
            return ThoughtState.Inactive;
        }
    }
    
}
