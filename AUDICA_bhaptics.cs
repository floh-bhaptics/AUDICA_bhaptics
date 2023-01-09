using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using HarmonyLib;
using UnityEngine;
using MyBhapticsTactsuit;
using Il2Cpp;

[assembly: MelonInfo(typeof(AUDICA_bhaptics.AUDICA_bhaptics), "AUDICA_bhaptics", "1.1.0", "Florian Fahrenberger")]
[assembly: MelonGame("Harmonix Music Systems, Inc.", "Audica")]

namespace AUDICA_bhaptics
{
    public class AUDICA_bhaptics : MelonMod
    {
        public static TactsuitVR tactsuitVr = null!;

        public override void OnInitializeMelon()
        {
            tactsuitVr = new TactsuitVR();
            tactsuitVr.PlaybackHaptics("HeartBeat");
        }

        [HarmonyPatch(typeof(Gun), "Fire", new Type[] { typeof(Target), typeof(Vector3), typeof(int), typeof(bool), typeof(bool), typeof(ShootableToy) })]
        public class bhaptics_GunFire
        {
            [HarmonyPostfix]
            public static void Postfix(Gun __instance)
            {
                bool isRight = (__instance.hand == Target.TargetHandType.Right);
                tactsuitVr.Recoil("Pistol", isRight);
            }
        }

        [HarmonyPatch(typeof(MeleeWeapon), "OnMeleeAttackSuccess", new Type[] { typeof(Target), typeof(Vector3) })]
        public class bhaptics_MeleeSuccess
        {
            [HarmonyPostfix]
            public static void Postfix(MeleeWeapon __instance)
            {
                tactsuitVr.PlaybackHaptics("MeleeCrash");
            }
        }

        [HarmonyPatch(typeof(Gun), "OnMeleeAttackSuccess", new Type[] { typeof(Target), typeof(Vector3) })]
        public class bhaptics_GunMelee
        {
            [HarmonyPostfix]
            public static void Postfix(Gun __instance)
            {
                bool isRight = (__instance.hand == Target.TargetHandType.Right);
                tactsuitVr.Recoil("Melee", isRight);
            }
        }

        [HarmonyPatch(typeof(Gun), "UpdateSustainFX", new Type[] { })]
        public class bhaptics_GunSustainFX
        {
            [HarmonyPostfix]
            public static void Postfix(Gun __instance)
            {
                bool isRight = (__instance.hand == Target.TargetHandType.Right);
                if (!__instance.mSustainFlarePlaying) { tactsuitVr.StopTelekinesis(isRight); return; }
                tactsuitVr.StartTelekinesis(isRight);
            }
        }

        [HarmonyPatch(typeof(FinishLineEffect), "Go", new Type[] { typeof(Vector3), typeof(bool) })]
        public class bhaptics_FinishLine
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.PlaybackHaptics("Healing");
            }
        }
    }
}
