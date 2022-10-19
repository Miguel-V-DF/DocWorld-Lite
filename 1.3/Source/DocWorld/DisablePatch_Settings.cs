using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace DocWorldLite;

public class DisablePatch_Settings : ModSettings
{
    // expose data is the games way of saving/loading data.
    private Dictionary<string, bool> _scribeHelper;

    // keep a dictionary of patches and a enabled/disabled bool
    // we initialize all patches as not disabled, we'll load their
    // values in ExposeData.
    public Dictionary<PatchDescription, bool> PatchDisabled = PatchDisablerMod.Patches.ToDictionary(p => p, _ => false);
    //private Vector2 scrollPosition;
    //private Rect viewRect;

    // I like putting the 'render function' for settings in the settings class, vanilla would put it in the mod class. 
    public void DoWindowContents(Rect canvas)
    {
        // listing_standard is the simplest way to make an interface. It's not pretty, but it works.
        // I use it for most of my settings, as people shouldn't be spending much time there anyway.
        var options = new Listing_Standard();

        // tells the listing where it can render
        options.Begin(canvas);
        options.Label("Game has to be restarted in order for the changes to be applied!");
        options.Label("Check the github for an exact list of what each setting changes.");
        options.Label("");
        options.Label("Choose what should be enabled:");
        //Widgets.BeginScrollView(options.GetRect(400), ref scrollPosition, viewRect);

        // for each patch in the list of patches, render a checkbox.
        // this is one of the things that is super easy to do in options.
        // NOTE: if you have a lot of patches, you may want to try out 
        // options.BeginScrollView
        foreach (var patch in PatchDisablerMod.Patches)
        {
            // we can't use ref on a dictionary value, so pull it out for a sec.
            var status = PatchDisabled[patch];
            options.CheckboxLabeled(patch.label, ref status, patch.tooltip);

            PatchDisabled[patch] = status;
        }

        //Widgets.EndScrollView();

        // see also other functions on `options`, for textboxes, radio buttons, etc.
        options.End();
    }

    public override void ExposeData()
    {
        // store any values in the base ModSettings class (there aren't any, but still good practice).
        base.ExposeData();

        // save/load now becomes a bit more complicated, as we need to associate each of the patches with 
        // a specific value, while dealing with updates and such.
        // the 'proper' way to do this would be to use ILoadReferencable, but that is WAY overkill for this 
        // scenario.

        // we're going to store the filename, because that's a relatively unique identifier.
        if (Scribe.mode == LoadSaveMode.Saving)
        {
            // create the data structure we're going to save.
            _scribeHelper = PatchDisabled.ToDictionary(
                // delegate to transform a dict item into a key, we want the file property of the old key. ( PatchDescription => string )
                k => k.Key.file,

                // same for the value, which is just the value. ( bool => bool )
                v => v.Value);
        }

        // and store it. Notice that both the keys and values of our collection are strings, so simple values.
        // Note that we want this step to take place in all scribe stages, so it's not in a Scribe.mode == xxx block.
        Scribe_Collections.Look(ref _scribeHelper, "patches", LookMode.Value, LookMode.Value);

        // finally, when the scribe finishes, we need to transform this back to a data structure we understand.
        if (Scribe.mode != LoadSaveMode.PostLoadInit)
        {
            return;
        }

        // for each stored patch, update the value in our dictionary.
        foreach (var storedPatch in _scribeHelper)
        {
            var index = PatchDisablerMod.Patches.FindIndex(p => p.file == storedPatch.Key);
            if (index < 0)
            {
                continue;
            }

            var patch = PatchDisablerMod.Patches[index];
            PatchDisabled[patch] = storedPatch.Value;
        }
    }
}