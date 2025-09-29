using UnityEngine;

public static class AnimatorHash
{
    public static readonly int Player_MoveDirectionX = Animator.StringToHash("MoveDirectionX");
    public static readonly int Player_MoveDirectionY = Animator.StringToHash("MoveDirectionY");
    public static readonly int Player_Dodge = Animator.StringToHash("Dodge");
    public static readonly int Player_GetHit = Animator.StringToHash("GetHit");
    public static readonly int Player_ShieldForming = Animator.StringToHash("ShieldForming");
    public static readonly int Player_ShieldPopping = Animator.StringToHash("ShieldPopping");

    public static readonly int Boss_Move = Animator.StringToHash("BossMove");
    public static readonly int Boss_Attack1Anticipation = Animator.StringToHash("BossAttack1_Anticipation");
    public static readonly int Boss_Attack1Charge = Animator.StringToHash("BossAttack1_Charge");
    public static readonly int Boss_Attack1End = Animator.StringToHash("BossAttack1_End");
    public static readonly int Boss_Attack2Anticipation = Animator.StringToHash("BoosAttack2_Anticipation");
    public static readonly int Boss_BossAttack2Beam = Animator.StringToHash("BossAttack2_Beam");

    public static readonly int Level_IronGateIsOpen= Animator.StringToHash("IsOpen");
}
