using GBKT_DefinitionTypes;
using RimWorld;
using Verse;

namespace GBKT_Gone_Cooperative_ThoughtWorkers;

public class ThoughtWorker_FireBrigadierFightingAFire : ThoughtWorker
{
    protected override ThoughtState CurrentStateInternal(Pawn pawn)
    {
        var pawnsCurrentJob = pawn.CurJobDef.ToString();
        if (!pawn.Spawned || !pawn.RaceProps.Humanlike ||
            !pawn.story.traits.HasTrait(GBKT_DefinitionTypes_Traits.GBKT_PlayLeader))
        {
            return ThoughtState.Inactive;
        }

        return pawnsCurrentJob is "TriggerFirefoamPopper" or "BeatFire" or "ExtinguishSelf"
            ? ThoughtState.ActiveAtStage(0)
            : ThoughtState.Inactive;
    }
}