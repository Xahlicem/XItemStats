using Terraria.ModLoader;

namespace XItemStats
{
	class XItemStats : Mod
	{
		public XItemStats()
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
