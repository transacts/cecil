//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// Copyright (c) 2008 - 2015 Jb Evain
// Copyright (c) 2008 - 2011 Novell, Inc.
//
// Licensed under the MIT/X11 license.
//

using System;

namespace Mono.Cecil {

	public abstract class MemberReference : IMetadataTokenProvider {

		string name;
		TypeReference declaring_type;

		internal MetadataToken token;
		internal object projection;

		public virtual string Name {
			get { return name; }
			set {
				if (IsWindowsRuntimeProjection && value != name)
					throw new InvalidOperationException ();

				name = value;
			}
		}

		public abstract string FullName {
			get;
		}

		public virtual TypeReference DeclaringType {
			get { return declaring_type; }
			set { declaring_type = value; }
		}

		public MetadataToken MetadataToken {
			get { return token; }
			set { token = value; }
		}

		public bool IsWindowsRuntimeProjection => projection != null;

		internal MemberReferenceProjection WindowsRuntimeProjection {
			get { return (MemberReferenceProjection) projection; }
			set { projection = value; }
		}

		internal bool HasImage {
			get {
				var module = Module;
				return module != null && module.HasImage;
			}
		}

		public virtual ModuleDefinition Module => declaring_type?.Module;

		public virtual bool IsDefinition => false;

		public virtual bool ContainsGenericParameter => declaring_type != null && declaring_type.ContainsGenericParameter;

		internal MemberReference ()
		{
		}

		internal MemberReference (string name)
		{
			this.name = name ?? string.Empty;
		}

		internal string MemberFullName () => declaring_type == null
			? name
			: declaring_type.FullName + "::" + name;

		public IMemberDefinition Resolve () => ResolveDefinition ();

		protected abstract IMemberDefinition ResolveDefinition ();

		public override string ToString ()
		{
			return FullName;
		}
	}
}
