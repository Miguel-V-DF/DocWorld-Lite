using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace DocWorldLite;

// Made by Fluffy and ISOREX for Dr_Zhivago's DocWorld
public class PatchDisablerMod : Mod
{
    public static DisablePatch_Settings Settings;

    // create a list of all the patches we want to make controllable.
    public static List<PatchDescription> Patches = new List<PatchDescription>
    {
        //new PatchDescription("Coolers.xml", "DocWorld Coolers", "Should new coolers be added?"),
        //new PatchDescription("Rock_Furniture.xml", "DocWorld Rock Furniture", "Should new rock furniture be added?"),
        //new PatchDescription("Table_Lamps.xml", "DocWorld Table Lamps", "Should new table lamps be added?"),
        //new PatchDescription("Underground_Conduits.xml", "DocWorld Underground Conduits","Should underground conduits be added?"),
        //new PatchDescription("Vents.xml", "DocWorld Vents", "Should new vents be added?"),
        //new PatchDescription("Vitals_Monitors.xml", "DocWorld Vitals Monitors", "Should new vitals monitors be added?"),
        //new PatchDescription("Blank.xml","=============================Mod Features Below this Line===================================","This is just a line. Stop reading this tooltip."),
        //new PatchDescription(".Load1_DesignationCategories.xml", "Custom Categories","This is the base file for all Architect Menu changes. If disabled, you must disable the other Category options as well."),
        //new PatchDescription(".Load2_DesignationCategory_Combined.xml", "Custom Categories Combined", "Should modded architect tabs be combined into DocWorld's custom ones? Note: Depends on 'Custom Categories'"),
        //new PatchDescription(".Load3_DesignationCategory_Fences.xml", "Custom Category Fences", "Should fences be given their own architect tab?"),
        //new PatchDescription(".Load4_DesignationCategory_Removal.xml", "Modded Category Removals", "Should modded architect tabs be removed now that they're combined? Note: Depends on 'Custom Categories'"),
        new PatchDescription("Biome_Foraging.xml", "Biome Foraging",
            "Should biomes give different foraged food? Note: Requires 'Vanilla Plants Expanded' or `VGP Vegetable Garden`"),
        new PatchDescription("Cloth_Beds.xml", "Cloth Beds",
            "Should beds require cloth? Note: Mods like 'Soft Warm Beds' will automatically nullify this content."),
        //new PatchDescription("Dropdown_Designator.xml", "Dropdown Menus", "Should custom dropdown menus be enabled?"),
        //new PatchDescription("Efficient_Power.xml", "Efficient Power", "Should utilities recieve power consumption changes?"),
        new PatchDescription("Enhanced_Miniaturisation.xml", "Enhanced Miniaturisation",
            "Should hand selected items become minifiable?"),
        new PatchDescription("Glass_Requisites.xml", "Glass Requisites",
            "Should modded lights require glass to be built? Note: Must use 'Glass+Lights' mod."),
        new PatchDescription("Linkables.xml", "Linkables", "Should more furniture link to modded linkables?"),
        new PatchDescription("Misc.xml", "Misc",
            "Should a variety of miscellaneous changes be done? Note: check Wiki for further info"),
        new PatchDescription("Multi_Mod_Support.xml", "Multi Mod Support",
            "Should changes be applied when certain mods are loaded together?"),
        new PatchDescription("Research_Projects.xml", "Research Projects",
            "Should research projects be combined or removed where appropriate?"),
        new PatchDescription("Stuffed_Items.xml", "Stuffed Items", "Should more items become stuffable?")
        //new PatchDescription("Textures.xml", "Textures", "Should custom textures be applied?"),
        //new PatchDescription("VFE_Usable_Props.xml", "VFE Usable Props", "Should certain props be usable? Note: Requires `VFE-Props and Decor` and `LWM Deep Storage`")
    };


    public PatchDisablerMod(ModContentPack content) : base(content)
    {
        // make the settings available for other code. 
        // Note that GetSettings also loads any previously set settings so that we don't have to.
        Settings = GetSettings<DisablePatch_Settings>();

        // this does several things;
        // 1: getting Patches causes the game to load the files.
        // 2: content.Patches gets passed to us as an IEnumerable<PatchOperation> (which is essentially read-only),
        //  but it is actually a List<PatchOperation> (which we can alter), so we cast it back. Note that this works because
        // List<T> implements IEnumerable<T>, and we know the underlying type. We can't just do this to things that aren't 
        // lists.
        var allPatches = content.Patches as List<PatchOperation>;
        foreach (var patch in Patches)
        {
            if (Settings.PatchDisabled[patch] == false)
            {
                // find our target patch, null if not found.
                // note that `sourceFile` is the FULL file path, so we only check that it ends in the correct file name.
                // make sure that there aren't any false positives here, include some of the folder structure if you need.
                // and finally, actually remove the patch.
                allPatches?.RemoveAll(p => p.sourceFile.EndsWith(patch.file));
            }
        }
        // profit!
    }

    // the game expects a render function for the settings here, forwarding it to settings (this is personal preference)
    public override void DoSettingsWindowContents(Rect canvas)
    {
        Settings.DoWindowContents(canvas);
    }

    // title of the settings tab for our mod
    public override string SettingsCategory()
    {
        return "DocWorld Lite";
    }
}

// define a simple struct as a data container for our patches.