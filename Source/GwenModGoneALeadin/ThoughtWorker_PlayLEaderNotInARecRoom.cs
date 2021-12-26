using GBKT_DefinitionTypes;
using RimWorld;
using Verse;

namespace GBKT_Gone_Cooperative_ThoughtWorkers;

public class ThoughtWorker_PlayLEaderNotInARecRoom : ThoughtWorker
{
    protected override ThoughtState CurrentStateInternal(Pawn pawn)
    {
        var room = pawn.GetRoom(RegionType.Set_Passable);
        if (!pawn.Spawned)
        {
            return ThoughtState.Inactive;
        }

        if (!pawn.RaceProps.Humanlike)
        {
            return ThoughtState.Inactive;
        }

        if (!pawn.story.traits.HasTrait(GBKT_DefinitionTypes_Traits.GBKT_PlayLeader))
        {
            return ThoughtState.Inactive;
        }

        if (room.Role == RoomRoleDefOf.RecRoom)
        {
            return ThoughtState.Inactive;
        }

        return ThoughtState.ActiveAtStage(0);
    }
}