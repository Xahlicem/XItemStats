using Terraria.ModLoader;

namespace XItemDamage
{
	class XItemDamage : Mod
	{
		public XItemDamage()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}
	}
}
