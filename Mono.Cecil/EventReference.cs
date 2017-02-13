//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// Copyright (c) 2008 - 2015 Jb Evain
// Copyright (c) 2008 - 2011 Novell, Inc.
//
// Licensed under the MIT/X11 license.
//

using static Mono.Cecil.Mixin;

namespace Mono.Cecil {

	public abstract class EventReference : MemberReference {

		TypeReference event_type;

		public TypeReference EventType {
			get { return event_type; }
			set { event_type = value; }
		}

		public override string FullName => event_type.FullName + " " + MemberFullName ();

		protected EventReference (string name, TypeReference eventType)
			: base (name)
		{
			CheckType (eventType, Argument.eventType);
			event_type = eventType;
		}

		protected override IMemberDefinition ResolveDefinition () => this.Resolve ();

		public new abstract EventDefinition Resolve ();
	}
}
