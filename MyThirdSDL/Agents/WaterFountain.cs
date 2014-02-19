using SharpDL.Graphics;
using System;

namespace MyThirdSDL.Agents
{
	public class WaterFountain : Equipment
	{
		public WaterFountain(TimeSpan birthTime, Texture texture, Vector startingPosition, string name, int price, string description, string iconTextureKey,
			NecessityEffect necessityEffect, SkillEffect skillEffect)
			: base(birthTime, name, texture, startingPosition, price, description, iconTextureKey)
		{
			NecessityEffect = necessityEffect;
			SkillEffect = skillEffect;
		}
	}
}