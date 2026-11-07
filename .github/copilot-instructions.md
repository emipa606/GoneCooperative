# GitHub Copilot Instructions for Gone Cooperative (Continued) RimWorld Mod

## Mod Overview and Purpose
Gone Cooperative (Continued) is a RimWorld mod that adds 16 new traits to the game. These traits alter the stats of pawns that are near pawns with the traits. This mod is an update to the original "Gwen Blanket Specters" mod and is currently in an early phase of development. The mod is open to translations and contributions from the community.

## Key Features and Systems
- **16 New Traits**: Each trait has unique effects on nearby pawns' stats.
- **XML Definitions**: Traits, thoughts, and hediffs are defined in XML files.
- **C# Logic**: Handles trait checks and capacity adjustments.

## Coding Patterns and Conventions
- **Class Naming**: Classes are named in PascalCase, e.g., `GBKT_DefinitionTypes_Hediff`.
- **Method Naming**: Methods are named in camelCase, e.g., `WorldComponentTick`.
- **Constants and Enums**: Use PascalCase for constants and enums.
- **Comments**: Use XML documentation comments for classes and methods.

## XML Integration
XML files are used to define various aspects of the mod, including traits, thoughts, and hediffs. Each XML file should be well-structured and adhere to the RimWorld XML schema.

### Example XML File: GBKT_Gone_Cooperative_Traits.xml
xml
<?xml version="1.0" encoding="utf-8"?>
<TraitDefs>
  <TraitDef>
    <defName>GBKT_CooperativeTrait</defName>
    <label>Cooperative</label>
    <description>Improves teamwork and cooperation among nearby pawns.</description>
    <degreeDatas>
      <li>
        <label>Minor</label>
        <description>Minorly boosts teamwork.</description>
        <statOffsets>
          <li>
            <stat>GBKT_Teamwork</stat>
            <value>1</value>
          </li>
        </statOffsets>
      </li>
      <li>
        <label>Moderate</label>
        <description>Moderately boosts teamwork.</description>
        <statOffsets>
          <li>
            <stat>GBKT_Teamwork</stat>
            <value>2</value>
          </li>
        </statOffsets>
      </li>
    </degreeDatas>
  </TraitDef>
</TraitDefs>


## Harmony Patching
Harmony patching is used to modify the behavior of RimWorld's existing code. Patches should be applied carefully to avoid breaking the game's functionality.

### Example Harmony Patch: GBKT_TraitChecker.cs
csharp
using HarmonyLib;

namespace GwenModGoneALeadin
{
    [HarmonyPatch(typeof(Pawn), "GetStatValue")]
    public static class GBKT_TraitChecker
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn __instance, StatDef stat, ref float __result)
        {
            if (stat == GBKT_TraitChecker.GBKT_Teamwork)
            {
                var cooperativeTrait = __instance.Traits.FirstOrDefault(t => t.def.defName == "GBKT_CooperativeTrait");
                if (cooperativeTrait != null)
                {
                    __result += cooperativeTrait.Degree;
                }
            }
        }
    }
}


## Suggestions for Copilot
1. **Trait Definitions**: Provide examples of XML files for new traits, thoughts, and hediffs.
2. **Harmony Patches**: Offer code snippets for common harmony patches, such as modifying pawn stats or adding new behaviors.
3. **C# Logic**: Suggest best practices for handling trait checks, capacity adjustments, and other mod-specific logic.

By following these guidelines and suggestions, you can contribute effectively to the Gone Cooperative (Continued) mod and help make it even better!

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

