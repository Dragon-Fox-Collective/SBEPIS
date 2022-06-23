using System;
using UnityEngine;
using System.Linq;
using System.Collections.ObjectModel;

namespace SBEPIS.Bits
{
	[Serializable]
	public class MemberedBitSet
	{
		public static readonly MemberedBitSet NOTHING = new(BitSet.NOTHING, null);

		[SerializeField]
		private BitSet _bits;
		public BitSet bits => _bits;

		[SerializeField]
		[SerializeReference]
		private Member[] _members;
		private ReadOnlyCollection<Member> readOnlyMembers;
		public ReadOnlyCollection<Member> members => readOnlyMembers ??= new(_members);


		public MemberedBitSet(BitSet bits, Member[] members)
		{
			_bits = bits;
			_members = members ?? new Member[0];
		}

		public override string ToString() => $"Membered{_bits}{_members.ToDelimString()}";
		public override int GetHashCode() => (_bits, _members).GetHashCode();
		public override bool Equals(object obj) => obj is MemberedBitSet set && this == set;

		public static MemberedBitSet operator |(MemberedBitSet a, MemberedBitSet b) => (a.bits | b.bits).With(a._members, b._members);
		public static MemberedBitSet operator &(MemberedBitSet a, MemberedBitSet b) => (a.bits & b.bits).With(a._members, b._members);
		public static MemberedBitSet operator ^(MemberedBitSet a, MemberedBitSet b) => (a.bits ^ b.bits).With(a._members, b._members);
		public static MemberedBitSet operator ~(MemberedBitSet a) => (~a.bits).With(a._members);
		public static bool operator ==(MemberedBitSet a, MemberedBitSet b) => a?.bits == b?.bits && MemberAppender.MembersEqual(a?._members, b?._members);
		public static bool operator !=(MemberedBitSet a, MemberedBitSet b) => a?.bits != b?.bits || !MemberAppender.MembersEqual(a?._members, b?._members);

		public static MemberedBitSet operator |(MemberedBitSet a, BitSet b) => (a.bits | b).With(a._members);
		public static MemberedBitSet operator |(BitSet a, MemberedBitSet b) => (a | b.bits).With(b._members);
		public static MemberedBitSet operator &(MemberedBitSet a, BitSet b) => (a.bits & b).With(a._members);
		public static MemberedBitSet operator &(BitSet a, MemberedBitSet b) => (a & b.bits).With(b._members);
		public static MemberedBitSet operator ^(MemberedBitSet a, BitSet b) => (a.bits ^ b).With(a._members);
		public static MemberedBitSet operator ^(BitSet a, MemberedBitSet b) => (a ^ b.bits).With(b._members);

		public static MemberedBitSet operator +(MemberedBitSet a, Member b) => a.With(new Member[] { b });
		public static MemberedBitSet operator +(MemberedBitSet a, Member[] b) => a.With(b);
		public static MemberedBitSet operator -(MemberedBitSet a, Member b) => a.Without(new Member[] { b });
		public static MemberedBitSet operator -(MemberedBitSet a, Member[] b) => a.Without(b);

		public bool Has(BitSet other) => _bits.Has(other);

		public MemberedBitSet With(Member[] other) => _bits.With(MemberAppender.Append(_members, other));
		public MemberedBitSet Without(Member[] other) => _bits.With(_members.Where(member => !other.Contains(member)).ToArray());
	}
}
