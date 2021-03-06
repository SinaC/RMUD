﻿class wolf : RMUD.NPC
{
    public bool IsFed = false;

    public override void Initialize()
    {
        Perform<RMUD.MudObject, RMUD.MudObject, RMUD.MudObject>("topic response")
            .Do((actor, npc, topic) =>
            {
                RMUD.MudObject.SendLocaleMessage(actor, "The wolf snarls and howls, showing its large sharp teeth.");
                return RMUD.PerformResult.Stop;
            });

        Short = "wolf";
        Long = "This is a snarling grey beast with a long snout and upright ears. It has a tail that does not wag even a little.";

        Nouns.Add("wolf");

        Perform<RMUD.MudObject, RMUD.MudObject>("handle-entrail-drop").Do((wolf, entrails) =>
            {
                RMUD.MudObject.SendLocaleMessage(this, "The wolf snatches up the entrails.");
                IsFed = true;
                RMUD.MudObject.Move(entrails, RMUD.MudObject.GetObject("palantine/soranus"), RMUD.RelativeLocations.Worn);
                return RMUD.PerformResult.Stop;
            });

        Value<RMUD.MudObject, RMUD.MudObject, string, string>("printed name").First.Do((viewer, item, article) => article + " wolf");

        Value<RMUD.MudObject, bool>("entrail-quest-is-fed").Do(wolf => IsFed);

        Perform<RMUD.MudObject, RMUD.MudObject>("quest reset")
            .When((quest, item) => quest.Path == "palantine/entrail_quest")
            .Do((quest, item) => { IsFed = false; return RMUD.PerformResult.Stop; });

        RMUD.Core.GlobalRules.Perform("heartbeat").Do(() =>
        {
            if (!IsFed)
                RMUD.MudObject.SendLocaleMessage(this, "The wolf whines for food.");
            return RMUD.PerformResult.Continue;
        });
    }

}