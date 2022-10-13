using UnityEngine;
using RoR2;
using MysticsRisky2Utils.BaseAssetTypes;
using UnityEngine.AddressableAssets;
using MysticsRisky2Utils;

namespace AspectAbilities.Buffs
{
    public class HauntedDodge : BaseBuff
    {
        public static ConfigOptions.ConfigurableValue<float> dodgeChance = ConfigOptions.ConfigurableValue.CreateFloat(
            AspectAbilitiesPlugin.PluginGUID,
            AspectAbilitiesPlugin.PluginName,
            AspectAbilitiesPlugin.config,
            "Celestine",
            "Dodge Chance",
            10f,
            0f,
            100f,
            "How much dodge chance should the buff give to allies (in %)",
            useDefaultValueConfigEntry: AspectAbilitiesPlugin.ignoreBalanceChanges.bepinexConfigEntry
        );

        public override void OnLoad()
        {
            buffDef.name = "AspectAbilities_HauntedDodge";
            buffDef.buffColor = new Color32(150, 215, 215, 255);
            buffDef.canStack = false;
            buffDef.isDebuff = false;
            buffDef.iconSprite = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdArmorBoost.asset").WaitForCompletion().iconSprite;

            MysticsRisky2Utils.GenericGameEvents.BeforeTakeDamage += GenericGameEvents_BeforeTakeDamage;
        }

        private void GenericGameEvents_BeforeTakeDamage(DamageInfo damageInfo, MysticsRisky2Utils.MysticsRisky2UtilsPlugin.GenericCharacterInfo attackerInfo, MysticsRisky2Utils.MysticsRisky2UtilsPlugin.GenericCharacterInfo victimInfo)
        {
            if (!damageInfo.rejected && victimInfo.body && victimInfo.body.HasBuff(buffDef) && (Util.CheckRoll(dodgeChance, victimInfo.master) || Util.CheckRoll(2f, victimInfo.master)))
            {
                EffectManager.SpawnEffect(HealthComponent.AssetReferences.damageRejectedPrefab, new EffectData
                {
                    origin = damageInfo.position + Vector3.up * 2f
                }, true);
                damageInfo.rejected = true;
                Debug.Log("HauntedDodge triggered by " + victimInfo.body.name);
            }
        }
    }
}
