﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public class StatusEffect
    {
        public virtual void Apply(Actor To) { }
        public virtual void Remove(Actor From) { }
        public virtual void Heartbeat(ulong HeartbeatID, Actor AppliedTo) { }

        public virtual Tuple<bool, String> OverrideName(Actor For, String PreviousName) { return new Tuple<bool,string>(false, PreviousName); }
    }
}
