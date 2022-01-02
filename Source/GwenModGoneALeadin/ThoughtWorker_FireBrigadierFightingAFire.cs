using GBKT_DefinitionTypes;
using RimWorld;
using Verse;

namespace GBKT_Gone_Cooperative_ThoughtWorkers;

public class ThoughtWorker_FireBrigadierFightingAFire : ThoughtWorker
{
    protected override ThoughtState CurrentStateInternal(Pawn pawn)
    {
        var PawnsCurrentJob = pawn.CurJobDef.ToString();
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

        if (PawnsCurrentJob == "TriggerFirefoamPopper" || PawnsCurrentJob == "BeatFire" ||
            PawnsCurrentJob == "ExtinguishSelf")
        {
            return ThoughtState.ActiveAtStage(0);
        }

        return ThoughtState.Inactive;
    }
}