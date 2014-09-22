﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
	public class LockedDoor : Thing, IOpenableRules, ITakeRules, ILockableRules
	{
		public String Key;

		public LockedDoor()
		{
			this.Nouns.Add("DOOR");
			Open = false;
			Locked = true;
		}

		#region IOpenable

		public bool Open { get; set; }

		bool IOpenableRules.CanOpen(Actor Actor)
		{
			if (Locked) return false;
			return !Open;
		}

		bool IOpenableRules.CanClose(Actor Actor)
		{
			return Open;
		}

		void IOpenableRules.HandleOpen(Actor Actor)
		{
			Open = true;
		}

		void IOpenableRules.HandleClose(Actor Actor)
		{
			Open = false;
			Locked = false;
		}

		#endregion

		bool ITakeRules.CanTake(Actor Actor)
		{
			return false;
		}

		#region ILockableRules

		public bool Locked { get; set; }

		bool ILockableRules.CanLock(Actor Actor, Thing Key)
		{
			if (Open) return false;
			if (Locked) return false;
			return Key.Is(this.Key);
		}

		bool ILockableRules.CanUnlock(Actor Actor, Thing Key)
		{
			if (Open) return false;
			if (!Locked) return false;
			return Key.Is(this.Key);
		}

		void ILockableRules.HandleLock(Actor Actor, Thing Key)
		{
			Locked = true;
		}

		void ILockableRules.HandleUnlock(Actor Actor, Thing Key)
		{
			Locked = false;
		}

		#endregion
	}
}