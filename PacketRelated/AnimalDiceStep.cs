using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalDiceStep : StepListBase
{
	[Step((int)eAD_STEP.IS_START)]
	public void ADStart(float time)
	{
		ADGameStepHandle.STEP = eAD_STEP.IS_START;
		Debug.Log("[GAMESTEP] " + eAD_STEP.IS_START);
		Debug.LogError("[GAMESTEP] " + eAD_STEP.IS_START);
		
		ResourceContainer.Get<ADGameMain>().HandleBackgroundMusicWithDelay(0.5f, 1f);
		#region large hack...
		ResourceContainer.Get<ADGameMain>().bJoinMiddleOfGame = false;
		
        ResourceContainer.Get<ADChipBettingManager>().bDestroyChipImmediately = true;
		CoroutineChain.Start
			.Wait(0.6f)
			.Call(() => ResourceContainer.Get<ADChipBettingManager>().bDestroyChipImmediately = false);
        #endregion

    }

    #region don't be used at serverside...
    //[Step((int)eAD_STEP._IS_AD_STAY)]
    //public void ADStay(float time)
    //{
    //	Debug.Log("[GAMESTEP] " + eAD_STEP._IS_AD_STAY);
    //}

    //[Step((int)eAD_STEP._IS_AD_STOP)]
    //public void ADStop(float time)
    //{
    //	Debug.Log("[GAMESTEP] " + eAD_STEP._IS_AD_STOP);

    //	// BooG.ClearAll();

    //	// 1. 
    //}
    //[Step((int)eAD_STEP._IS_AD_READY)]
    //public void ADReady(float time)
    //{
    //	Debug.Log("[GAMESTEP] " + eAD_STEP._IS_AD_READY);
    //}
    #endregion



    [Step((int)eAD_STEP._IS_AD_SHUFFLE)]
	public void ADShuffle(float time)
	{
		ADGameStepHandle.STEP = eAD_STEP._IS_AD_SHUFFLE;
		Debug.Log("[GAMESTEP] " + eAD_STEP._IS_AD_SHUFFLE);
	}

	[Step((int)eAD_STEP._IS_AD_SHUFFLE_END)]
	public void ADShuffleEnd(float time)
	{
		ADGameStepHandle.STEP = eAD_STEP._IS_AD_SHUFFLE_END;
		ResourceContainer.Get<ADGameMain>().ClearAll(); // should have been in start
		Debug.Log("[GAMESTEP] " + eAD_STEP._IS_AD_SHUFFLE_END);
	}


	[Step((int)eAD_STEP._IS_AD_BET)]
	public void BetStart(float time)
	{
		ADGameStepHandle.STEP = eAD_STEP._IS_AD_BET;
		Debug.Log("[GAMESTEP] " + eAD_STEP._IS_AD_BET);
		
		// ResourceContainer.Get<ADBettingTimeCounter>().TurnOnWithAlpha();
		ResourceContainer.Get<ADChipBettingManager>().DisableDestroyingChipSystem();

		ResourceContainer.Get<ADChipBettingManager>().SetEnableBettingBoards(true);
		var tempUsers = ResourcePool.GetAll<GamePlayer>();
		// ResourceContainer.Get<ADChipBettingManager>().bHasBoardEnabled = true;
	}

	[Step((int)eAD_STEP._IS_AD_DISPLAY)]
	public void DisplayOnGoing(float time)
	{
		ADGameStepHandle.STEP = eAD_STEP._IS_AD_DISPLAY;
		Debug.Log("[GAMESTEP] " + eAD_STEP._IS_AD_DISPLAY);
		// _IS_AD_DISPLAY,

	}
	[Step((int)eAD_STEP._IS_AD_DISPLAY_WAIT)]
	public void DisplayOnWait(float time)
	{
		ADGameStepHandle.STEP = eAD_STEP._IS_AD_DISPLAY_WAIT;
		Debug.Log("[GAMESTEP] " + eAD_STEP._IS_AD_DISPLAY_WAIT);
		// _IS_AD_DISPLAY,

	}


	[Step((int)eAD_STEP._IS_AD_OPEN_DICE)]
	public void DiceOpen(float time)
	{
		ADGameStepHandle.STEP = eAD_STEP._IS_AD_OPEN_DICE;
		Debug.Log("[GAMESTEP] " + eAD_STEP._IS_AD_OPEN_DICE);
		ResourceContainer.Get<ADChipBettingManager>().SetEnableBettingBoards(false);
		ResourceContainer.Get<ADChipBettingManager>().bIsBettingTime = false;
		ResourceContainer.Get<ADChipBettingManager>().bIsBettingRoutineOff = true;


	}


	[Step((int)eAD_STEP._IS_AD_OPEN_DICE_END)]
	public void DiceOpenEnd(float time)
	{
		ADGameStepHandle.STEP = eAD_STEP._IS_AD_OPEN_DICE_END;
		Debug.Log("[GAMESTEP] " + eAD_STEP._IS_AD_OPEN_DICE_END);
		// TimeContainer t1 = new TimeContainer("NPCTimerBalloonAlphaTime", 0.5f);
		// ResourceContainer.Get<ADBettingTimeCounter>().timerObject.GetComponent<Graphic>().AlphaTween(0f, t1, true);
		ResourceContainer.Get<ADBettingTimeCounter>().TurnOffWithAlpha(0.5f);
	}

	[Step((int)eAD_STEP._IS_AD_RESULT_END)]
	public void ResultEnd(float time)
	{
		ADGameStepHandle.STEP = eAD_STEP._IS_AD_RESULT_END;
		Debug.Log("[GAMESTEP] " + eAD_STEP._IS_AD_RESULT_END);
		
	}
	[Step((int)eAD_STEP._IS_AD_CLEAR)]
	public void ADClear(float time)
	{
		ADGameStepHandle.STEP = eAD_STEP._IS_AD_CLEAR;
		Debug.Log("[GAMESTEP] " + eAD_STEP._IS_AD_CLEAR);
		ResourceContainer.Get<ADGameMain>().ClearAll();

	}
	[Step((int)eAD_STEP.IS_GAME_END)]
	public void GameEnd(float time)
	{
		ADGameStepHandle.STEP = eAD_STEP.IS_GAME_END;
		Debug.Log("[GAMESTEP] " + eAD_STEP.IS_GAME_END);

	}
	[Step((int)eAD_STEP.IS_GAME_END_END)]
	public void GameEndEnd(float time)
	{
		ADGameStepHandle.STEP = eAD_STEP.IS_GAME_END_END;
		Debug.Log("[GAMESTEP] " + eAD_STEP.IS_GAME_END_END);

	}

}

public static class ADGameStepHandle
{
	public static eAD_STEP STEP = eAD_STEP._STEP_MAX;
}


public enum eAD_STEP
{
	IS_STOP = 0,            //게임중 아님
	IS_START,
	IS_START_WAIT,          //게임이 시작됬다.(클라용)

	_IS_AD_STAY,
	_IS_AD_STOP,
	_IS_AD_READY,
	_IS_AD_SHUFFLE,
	_IS_AD_SHUFFLE_WAIT,
	_IS_AD_SHUFFLE_END,

	_IS_AD_BET,
	_IS_AD_BET_WAIT,
	_IS_AD_BET_END,

	_IS_AD_DISPLAY,
	_IS_AD_DISPLAY_WAIT,
	_IS_AD_DISPLAY_END,

	_IS_AD_OPEN_DICE,
	_IS_AD_OPEN_DICE_WAIT,
	_IS_AD_OPEN_DICE_END,  //결과계산한다.

	_IS_AD_RESULT_LOSE,
	_IS_AD_RESULT_LOSE_WAIT,
	_IS_AD_RESULT_LOSE_END,

	_IS_AD_RESULT_WIN,
	_IS_AD_RESULT_WIN_WAIT,
	_IS_AD_RESULT_WIN_END,

	_IS_AD_RESULT,
	_IS_AD_RESULT_WAIT,
	_IS_AD_RESULT_END,

	_IS_AD_CLEAR,
	_IS_AD_STOP_WAIT,

	IS_RESULT_CHOICE_J_SVR,
	IS_RESULT_JACKPOT,              //더블윈애니						클라
	IS_RESULT_JACKPOT_WAIT,         //더블윈애니 진행 기다림			서버
	IS_RESULT_JACKPOT_END,
	IS_GAME_END,                    //모든 결과를 팝업으로 보여준다.( 다음게임시작시까지 유지)
	IS_GAME_END_WAIT,
	IS_GAME_END_END,

	IS_STOP_WAIT,
	_STEP_MAX
};