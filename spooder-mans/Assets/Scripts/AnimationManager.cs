using UnityEngine;

public class AnimationManager : MonoBehaviour
{
	
	// PRIVATE VARIABLES
	public Animator anim;
	private int stunTimer = 0;
	private int maxStun = 0;
	
	// ANIMATION TRANSITIONS
	public bool IsRunningUp() { return anim.GetBool ( AnimatorParams.runningUpBool ); }
	public void SetRunningUp() { anim.SetBool ( AnimatorParams.runningUpBool, true ); }
	public void ClearRunningUp() { anim.SetBool ( AnimatorParams.runningUpBool, false ); }
	
	public bool IsRunningDown() { return anim.GetBool ( AnimatorParams.runningDownBool ); }
	public void SetRunningDown() { anim.SetBool ( AnimatorParams.runningDownBool, true ); }
	public void ClearRunningDown() { anim.SetBool ( AnimatorParams.runningDownBool, false ); }

	public bool IsChargeDone() { return anim.GetBool ( AnimatorParams.chargeDoneBool ); }
	public void SetChargeDone() { anim.SetBool ( AnimatorParams.chargeDoneBool, true ); }
	public void ClearChargeDone() { anim.SetBool ( AnimatorParams.chargeDoneBool, false ); }

	public bool IsHitDone() { return anim.GetBool ( AnimatorParams.hitDoneBool ); }
	public void SetHitDone() { anim.SetBool ( AnimatorParams.hitDoneBool, true ); }
	public void ClearHitDone() { anim.SetBool ( AnimatorParams.hitDoneBool, false ); }

	public bool IsCooldownDone() { return anim.GetBool ( AnimatorParams.cooldownDoneBool ); }
	public void SetCooldownDone() { anim.SetBool ( AnimatorParams.cooldownDoneBool, true ); }
	public void ClearCooldownDone() { anim.SetBool ( AnimatorParams.cooldownDoneBool, false ); }

	public bool IsAttacking() { return anim.GetBool ( AnimatorParams.attackingBool ); }
	public void SetAttacking() { anim.SetBool ( AnimatorParams.attackingBool, true ); }
	public void ClearAttacking() { anim.SetBool ( AnimatorParams.attackingBool, false ); }

	public bool IsAttackStarted() { return anim.GetBool ( AnimatorParams.attackStartedBool ); }
	public void SetAttackStarted() { anim.SetBool ( AnimatorParams.attackStartedBool, true ); }
	public void ClearAttackStarted() { anim.SetBool ( AnimatorParams.attackStartedBool, false ); }

	public bool IsJumpStarted() { return anim.GetBool ( AnimatorParams.jumpStartedBool ); }
	public void SetJumpStarted() { anim.SetBool ( AnimatorParams.jumpStartedBool, true ); }
	public void ClearJumpStarted() { anim.SetBool ( AnimatorParams.jumpStartedBool, false ); }

	public int GetJumpCounter()  { return anim.GetInteger ( AnimatorParams.jumpCounterInt ); }
	public void IncrementJumpCounter() { anim.SetInteger ( AnimatorParams.jumpCounterInt, anim.GetInteger( AnimatorParams.jumpCounterInt ) + 1 ); }
	public void ClearJumpCounter() { anim.SetInteger ( AnimatorParams.jumpCounterInt, 0 ); }

	public bool IsMidair() { return anim.GetBool ( AnimatorParams.midairBool ); }
	public void SetMidair() { anim.SetBool ( AnimatorParams.midairBool, true ); }
	public void ClearMidair() { anim.SetBool ( AnimatorParams.midairBool, false ); }

	public bool IsStunned() { return anim.GetBool ( AnimatorParams.stunnedBool ); }
	public void SetStunned() { anim.SetBool ( AnimatorParams.stunnedBool, true ); }
	public void ClearStunned() { anim.SetBool ( AnimatorParams.stunnedBool, false ); }

	public bool IsHooking() { return anim.GetBool ( AnimatorParams.hookingBool ); }
	public void SetHooking() { anim.SetBool ( AnimatorParams.hookingBool, true ); }
	public void ClearHooking() { anim.SetBool ( AnimatorParams.hookingBool, false ); }

	public bool IsAttackLifted() { return anim.GetBool ( AnimatorParams.attackLiftedBool ); }
	public void SetAttackLifted() { anim.SetBool ( AnimatorParams.attackLiftedBool, true ); }
	public void ClearAttackLifted() { anim.SetBool ( AnimatorParams.attackLiftedBool, false ); }
	
	public bool IsJumpLifted() { return anim.GetBool ( AnimatorParams.jumpLiftedBool ); }
	public void SetJumpLifted() { anim.SetBool ( AnimatorParams.jumpLiftedBool, true ); }
	public void ClearJumpLifted() { anim.SetBool ( AnimatorParams.jumpLiftedBool, false ); }

	public bool IsHitStarted() { return anim.GetBool ( AnimatorParams.hitStartedBool ); }
	public void SetHitStarted() { anim.SetBool ( AnimatorParams.hitStartedBool, true ); }
	public void ClearHitStarted() { anim.SetBool ( AnimatorParams.hitStartedBool, false ); }

	public bool IsGettingPulled() { return anim.GetBool ( AnimatorParams.getPulledBool ); }
	public void SetGettingPulled() { anim.SetBool ( AnimatorParams.getPulledBool, true ); }
	public void ClearGettingPulled() { anim.SetBool ( AnimatorParams.getPulledBool, false ); }

	public bool IsLagged() { return anim.GetBool ( AnimatorParams.laggedBool ); }
	public void SetLagged() { anim.SetBool ( AnimatorParams.laggedBool, true ); }
	public void ClearLagged() { anim.SetBool ( AnimatorParams.laggedBool, false ); }

	public bool IsCharging() { return anim.GetBool ( AnimatorParams.chargingBool ); }
	public void SetCharging() { anim.SetBool ( AnimatorParams.chargingBool, true ); }
	public void ClearCharging() { anim.SetBool ( AnimatorParams.chargingBool, false ); }

	void FixedUpdate ()
	{
		if ( stunTimer < maxStun )
		{
			stunTimer++;
		}
		if ( stunTimer >= maxStun )
		{
			ClearStunned ();
			stunTimer = 0;
			maxStun = 0;
		}
	}

	public void SetStunned ( int frames )
	{
		StunAndSetStunStatuses ();
		maxStun = frames;
        stunTimer = 0;
	}

	public void StunAndSetStunStatuses ()
	{
		SetStunned ();
		ClearAttackStatuses ();
		ClearJumpCounter ();
		ClearRunningUp ();
		ClearRunningDown ();
		ClearAllHooking ();
		ClearAttackStarted ();
		ClearJumpStarted ();
	}

	public void SetWallStatuses ()
	{
		ClearMidair ();
		ClearJumpCounter ();
		ClearAllHooking ();
	}

	public void SetMidairStatuses()
	{
		SetMidair ();
		ClearRunningUp ();
		ClearRunningDown ();
	}

	public void JumpAndSetJumpStatuses ()
	{
		SetJumpStarted ();
		ClearRunningUp ();
		ClearRunningDown ();
		ClearAttackStatuses ();
		ClearAllHooking ();
	}

	public void ClearAttackStatuses ()
	{
		ClearChargeDone ();
		ClearHitDone();
		ClearCooldownDone ();
		ClearAttacking ();
	}

	public void SetSpawnStatuses ()
	{
		SetMidair ();
		ClearJumpCounter ();
		ClearAllHooking();
		ClearAttackStatuses ();
		ClearRunningDown ();
		ClearRunningUp ();
		ClearStunned ();
		ClearTriggers ();
	}

	public void ClearTriggers()
	{
		ClearJumpStarted ();
		ClearAttackStarted ();
	}

	public void ClearAllHooking()
	{
		ClearHooking();
		ClearGettingPulled();
	}
}
