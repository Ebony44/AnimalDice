using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
	게 	물	박	호	닭	새
게	10	12	13	14	15	16
물		20	23	24	25	26
박			30	34	35	36
호				40	45	46
닭					50	56
새						60
*/

public enum eADBetPlace
{
	_BET_NONE = 0,

	_BET_ANIMAL_GE_SAE = 16,
	_BET_ANIMAL_GE_BAK = 13,
	_BET_ANIMAL_GE_DAK = 15,
	_BET_ANIMAL_BAK_DAK = 35,

	_BET_ANIMAL_GE = 10,
	_BET_ANIMAL_GE_MUL = 12,
	_BET_ANIMAL_MUL = 20,
	_BET_ANIMAL_MUL_BAK = 23,
	_BET_ANIMAL_BAK = 30,

	_BET_ANIMAL_GE_HO = 14,
	_BET_ANIMAL_MUL_DAK = 25,
	_BET_ANIMAL_BAK_SAE = 36,

	_BET_ANIMAL_HO = 40,
	_BET_ANIMAL_HO_DAK = 45,
	_BET_ANIMAL_DAK = 50,
	_BET_ANIMAL_DAK_SAE = 56,
	_BET_ANIMAL_SAE = 60,

	_BET_ANIMAL_HO_SAE = 46,
	_BET_ANIMAL_HO_BAK = 34,
	_BET_ANIMAL_HO_MUL = 24,
	_BET_ANIMAL_SAE_MUL = 26,

	_BET_MAX = 70,
}
public enum eAD_DICE
{
	_DICE1_NONE = 0,
	_DICE1_GE = 1,
	_DICE2_MUL,
	_DICE3_BAK,
	_DICE4_HO,
	_DICE5_DAK,
	_DICE6_SAE,
	_DICE_MAX
};

enum eAD_emACTION_ERROR
{
	_ER_NONE = 0,
	_ERR_NO_MATCH_GAMEIDX,
	_ERR_STEP_NO_MATCH,
	_ERR_PLAYER_NOT_READY,
	_ERR_PLAYER_NONE,
	_ERR_ALREADY_PLAY,
	_ERR_PLAYER_SET_FAIL,
	_ERR_BETPLACE_FAIL,
	_ERR_BETBTN_FAIL,
	_ERR_BETMONEY_0,
	_ERR_BETMONEY_OVER,
	_ERR_BETMONEY_MAX,
	_ERR_ALREADY_BETTING,
	_ERR_ALREADY_BETTING_0,
	_ERR_ALREADY_BETTING_OVER,
	_ERR_BEFOREBET_NOTFIND,
	_ERR_BEFOREBET_BETTING,
	_ERR_RESULT_BETINFO_NOTFIND,
	_ERR_RESULT_BETMONEY_OVER,
	_ERR_UNKNOWN,
	_ER_MAX
};
//enum _SYS_BUTTONLIST
//{
//	_BTN_MENU = 0,  //버튼 리스트 보기
//	_BTN_JOKBO,     //족보버튼
//	_BTN_SOUND_ON,  //사운드 온
//	_BTN_SOUND_OFF, //사운드 오프
//	_BTN_HELP,
//	_BTN_MENU_END,  //버튼 리스트 끝
//	_BTN_EXIT,      //나가기		
//	_BTN_LIKE,      //유저 정보창 좋아요
//	_BTN_FRIEND,    //유저 정보창 친추
//	_BTN_FRIEND_BLOCK,  //유저 정보창 친추

//	_BTN_AUTO_SET,
//	_BTN_AUTO_CANCEL,

//	//관전인지 구분
//	_BTN_EMO,       //이모티콘 버튼
//	_BTN_CHAT,      //채팅버튼
//	_BTN_NPC_TIP,   //npc 팁버튼	
//	_BTN_RECTAB_QUEST,
//	_BTN_RECTAB_GIFT,
//	_SYS_BTN_MAX,
//	//카드 자동받기 예약
//	_BTN_NONE = _SYS_BTN_MAX,
//	_BTN_BETTING_1, //베팅 1
//	_BTN_BETTING_2, //베팅 2
//	_BTN_BETTING_3, //베팅 3
//	_BTN_BETTING_4, //베팅 4
//	_BTN_BETTING_5, //베팅 5(이전 총베팅)	
//	_BTN_MAX
//};
public enum eAD_BUTTONLIST
{

	//카드 자동받기 예약
	_BTN_NONE = _SYS_BUTTONLIST._SYS_BTN_MAX,
	_BTN_BETTING_1, //베팅 1
	_BTN_BETTING_2, //베팅 2
	_BTN_BETTING_3, //베팅 3
	_BTN_BETTING_4, //베팅 4
	_BTN_BETTING_5, //베팅 5(이전 총베팅)	
	_BTN_MAX
};

#region None-serverside enum

public enum eAD_BettingPlaceSize
{
    BIG_BOARD,
    SMALL_UPLOW_BOARD,
    GAP_VERTICAL_BOARD,
    GAP_HORIZONTAL_BOARD,
    _PLACESIZE_MAX
}

public enum eAD_AnteSetting
{
	ANTE_500,
	ANTE_1K,
	ANTE_5K,
	ANTE_10K,
	ANTE_50K,
	ANTE_100K,

}

#endregion