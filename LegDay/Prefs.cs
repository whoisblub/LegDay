using BepInEx.Configuration;
using System.Reflection;

namespace LegDay
{
    internal static class Prefs
    {
        public static ConfigEntry<bool> buffsEnabled;

        public static Buff[] buffs;
        //public static Buff jumpBuff;
        //public static Buff sprintBuff;

        public static void Bind(ConfigFile config)
        {
            buffsEnabled = config.Bind("Buffs", "ToggleBuffs", true);
            buffs = new Buff[]
            {
                new Buff(config.Bind("Buffs", "JumpBuff", 0f, "Change Jump Strength Buff"), "StrengthBuffJumpHeightInc"),
                new Buff(config.Bind("Buffs", "SprintBuff", 0f, "Change Sprint Speed Buff"), "StrengthBuffSprintSpeedInc"),
            };
        }

        public static void FindBuffReferences(SkillsClass skills)
        {
            // @Todo: put all buffs in an array so we can just loop over them
            //jumpBuff.FindReference(skills);
            //sprintBuff.FindReference(skills);

            for (int i = 0; i < buffs.Length; i++)
            {
                buffs[i].FindReference(skills);
            }
        }

        public static void ApplyAllBuffs()
        {
            for (int i = 0; i < buffs.Length; i++)
            {
                buffs[i].Apply();
            }
        }

        public class Buff
        {
            public ConfigEntry<float> entry;
            public SkillsClass.GClass1563 reference;

            private string referenceName;

            public Buff(ConfigEntry<float> entry, string referenceName)
            {
                this.entry = entry;
                this.referenceName = referenceName;

                entry.SettingChanged += Entry_SettingChanged;
            }

            public void Apply()
            {
                if (reference == null)
                {
                    Plugin.Log.LogWarning($"Buff reference ({referenceName}) not found! The mod only works in raid.");
                    return;
                }

                reference.Value = entry.Value;
            }

            public void FindReference(SkillsClass skills)
            {
                var info = typeof(SkillsClass).GetField(referenceName, BindingFlags.Public | BindingFlags.Instance);
                reference = (SkillsClass.GClass1563)info.GetValue(skills);
            }

            private void Entry_SettingChanged(object sender, System.EventArgs e)
            {
                if (!Prefs.buffsEnabled.Value)
                    return;

                Apply();
            }
        }
    }
}
