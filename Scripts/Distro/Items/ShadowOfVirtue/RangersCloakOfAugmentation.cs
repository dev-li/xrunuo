﻿using System;
using Server;

namespace Server.Items
{
	public class RangersCloakOfAugmentation : Cloak
	{
		public override int LabelNumber { get { return 1115514; } } // Ranger's Cloak Of Augmentation

		[Constructable]
		public RangersCloakOfAugmentation()
		{
			Hue = 0x54A;

			AbsorptionAttributes.KineticEater = 5;
			Attributes.SpellDamage = 3;
			Attributes.LowerManaCost = 1;
			Attributes.WeaponSpeed = 5;
		}

		public RangersCloakOfAugmentation( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
