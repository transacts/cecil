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
using System.Text;
using Mono.Collections.Generic;
using MD = Mono.Cecil.Metadata;
using static Mono.Cecil.Mixin;

namespace Mono.Cecil {

	public struct ArrayDimension {

		int? lower_bound;
		int? upper_bound;

		public int? LowerBound {
			get { return lower_bound; }
			set { lower_bound = value; }
		}

		public int? UpperBound {
			get { return upper_bound; }
			set { upper_bound = value; }
		}

		public bool IsSized => lower_bound.HasValue || upper_bound.HasValue;

		public ArrayDimension (int? lowerBound, int? upperBound)
		{
			this.lower_bound = lowerBound;
			this.upper_bound = upperBound;
		}

		public override string ToString ()
		{
			return !IsSized
				? string.Empty
				: lower_bound + "..." + upper_bound;
		}
	}

	public sealed class ArrayType : TypeSpecification {

		Collection<ArrayDimension> dimensions;

		public Collection<ArrayDimension> Dimensions {
			get {
				if (dimensions != null)
					return dimensions;

				dimensions = new Collection<ArrayDimension> ();
				dimensions.Add (new ArrayDimension ());
				return dimensions;
			}
		}

		public int Rank => dimensions?.Count ?? 1;

		public bool IsVector {
			get {
				if (dimensions == null)
					return true;

				if (dimensions.Count > 1)
					return false;

				var dimension = dimensions [0];

				return !dimension.IsSized;
			}
		}

		public override bool IsValueType {
			get { return false; }
			set { throw new InvalidOperationException (); }
		}

		public override string Name => base.Name + Suffix;

		public override string FullName => base.FullName + Suffix;

		string Suffix {
			get {
				if (IsVector)
					return "[]";

				var suffix = new StringBuilder ();
				suffix.Append ("[");
				for (int i = 0; i < dimensions.Count; i++) {
					if (i > 0)
						suffix.Append (",");

					suffix.Append (dimensions [i].ToString ());
				}
				suffix.Append ("]");

				return suffix.ToString ();
			}
		}

		public override bool IsArray => true;

		public ArrayType (TypeReference type)
			: base (type)
		{
			CheckType (type);
			this.etype = MD.ElementType.Array;
		}

		public ArrayType (TypeReference type, int rank)
			: this (type)
		{
			CheckType (type);

			if (rank == 1)
				return;

			dimensions = new Collection<ArrayDimension> (rank);
			for (int i = 0; i < rank; i++)
				dimensions.Add (new ArrayDimension ());
			this.etype = MD.ElementType.Array;
		}
	}
}
