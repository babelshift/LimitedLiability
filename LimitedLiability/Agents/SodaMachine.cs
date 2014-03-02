using SharpDL.Graphics;
using System;

namespace LimitedLiability.Agents
{
	public class SodaMachine : Equipment
	{
		public SodaMachine(TimeSpan birthTime, Texture texture, Vector startingPosition, string name, int price, string description, string iconTextureKey,
			NecessityEffect necessityEffect, SkillEffect skillEffect)
			: base(birthTime, name, texture, startingPosition, price, description, iconTextureKey)
		{
			NecessityEffect = necessityEffect;
			SkillEffect = skillEffect;
		}

		public NecessityEffect DispenseDrink()
		{
			return NecessityEffect;
		}
	}
}