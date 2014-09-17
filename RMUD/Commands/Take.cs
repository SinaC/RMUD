﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD.Commands
{
	internal class Take : CommandFactory
	{
		public override void Create(CommandParser Parser)
		{
			Parser.AddCommand(
				new Sequence(
					new Or(
						new KeyWord("GET", false),
						new KeyWord("TAKE", false)),
					new ObjectMatcher("TARGET"))
				, new TakeProcessor());
		}
	}

	public interface ITakeRules
	{
		bool CanTake(Actor Actor);
	}

	internal class TakeProcessor : ICommandProcessor
	{
		public void Perform(PossibleMatch Match, Actor Actor)
		{
			var target = Match.Arguments["TARGET"] as Thing;
			if (target == null)
			{
				if (Actor.ConnectedClient != null) Actor.ConnectedClient.Send("Take what again?\r\n");
			}
			else
			{
				var takeRules = target as ITakeRules;
				if (takeRules != null && !takeRules.CanTake(Actor))
				{
					Actor.ConnectedClient.Send("You can't take that.\r\n");
					return;
				}

				Mud.SendEventMessage(Actor, EventMessageScope.Locality, "{0} takes {1}.\r\n", target);
				MudObject.Move(target, Actor);
			}
		}
	}
}
