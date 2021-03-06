using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Mobiles
{
	public class Gregorio : BaseCreature
	{						
		public override bool InitialInnocent{ get{ return true; } }
		
		[Constructable]
		public Gregorio() : base( AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{			
			Race = Race.Human;
			Name = "Gregorio";
			Title = "the brigand";
			
			InitBody();
			InitOutfit();
			
			SetStr( 86, 100 );
			SetDex( 81, 95 );
			SetInt( 61, 75 );

			SetDamage( 15, 27 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 10, 15 );
			SetResistance( ResistanceType.Fire, 10, 15 );
			SetResistance( ResistanceType.Poison, 10, 15 );
			SetResistance( ResistanceType.Energy, 10, 15 );

			SetSkill( SkillName.MagicResist, 25.0, 50.0 );
			SetSkill( SkillName.Tactics, 80.0, 100.0 );
			SetSkill( SkillName.Wrestling, 80.0, 100.0 );	
			
			PackGold( 50, 150 );
		}
		
		public Gregorio( Serial serial ) : base( serial )
		{
		}
		
		public virtual void InitBody()
		{
			InitStats( 100, 100, 25 );
				
			Hue = 0x8412;
			Female = false;		
			
			HairItemID = 0x203C;
			HairHue = 0x47A;
			FacialHairItemID = 0x204D;
			FacialHairHue = 0x47A;
		}
		
		public virtual void InitOutfit()
		{						
			AddItem( new Sandals( 0x75E ) );
			AddItem( new Shirt() );
			AddItem( new ShortPants( 0x66C ) );
			AddItem( new SkullCap( 0x649 ) );
			AddItem( new Pitchfork() );
		}
		
		public override void Damage( int amount, Mobile from )	
		{
			if ( from != null && from.Player )
			{
				if ( IsMurderer( from as PlayerMobile ) )
					base.Damage( amount, from );		
				else
					from.SendLocalizedMessage( 1075456 ); // You are not allowed to damage this NPC unless your on the Guilty Quest
			}
		}
		
		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( !IsMurderer( to as PlayerMobile ) )
				damage = 1000;
		}
		
		public bool IsMurderer( PlayerMobile from )
		{			
			if ( from != null )
			{
				BaseQuest quest = QuestHelper.GetQuest<GuiltyQuest>( from );
				
				if ( quest != null )
					return !quest.Completed;
			}
				
			return false;
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
	
			writer.Write( (int) 0 ); // version
		}
	
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
	
			int version = reader.ReadInt();
		}
	}
}