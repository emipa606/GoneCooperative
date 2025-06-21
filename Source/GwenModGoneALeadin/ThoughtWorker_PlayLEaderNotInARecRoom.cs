using GBKT_DefinitionTypes;
using RimWorld;
using Verse;

namespace GBKT_Gone_Cooperative_ThoughtWorkers;

public class ThoughtWorker_PlayLEaderNotInARecRoom : ThoughtWorker
{
    private static readonly RoomRoleDef recRoom = DefDatabase<RoomRoleDef>.GetNamed("RecRoom");

    protected override ThoughtState CurrentStateInternal(Pawn pawn)
    {
        var room = pawn.GetRoom(RegionType.Set_Passable);
        if (!pawn.Spawned || !pawn.RaceProps.Humanlike ||
            !pawn.story.traits.HasTrait(GBKT_DefinitionTypes_Traits.GBKT_PlayLeader))
        {
            return ThoughtState.Inactive;
        }

        return room.Role == recRoom ? ThoughtState.Inactive : ThoughtState.ActiveAtStage(0);
    }
}