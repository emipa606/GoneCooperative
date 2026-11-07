using GBKT_DefinitionTypes;
using RimWorld;
using Verse;

namespace GBKT_Gone_Cooperative_ThoughtWorkers;

public class ThoughtWorker_FireBrigadierFightingAFire : ThoughtWorker
{
    protected override ThoughtState CurrentStateInternal(Pawn p)
    {
        var pawnsCurrentJob = p.CurJobDef.ToString();
        if (!p.Spawned || !p.RaceProps.Humanlike ||
            !p.story.traits.HasTrait(GBKT_DefinitionTypes_Traits.GBKT_PlayLeader))
        {
            return ThoughtState.Inactive;
        }

        return pawnsCurrentJob is "TriggerFirefoamPopper" or "BeatFire" or "ExtinguishSelf"
            ? ThoughtState.ActiveAtStage(0)
            : ThoughtState.Inactive;
    }
}