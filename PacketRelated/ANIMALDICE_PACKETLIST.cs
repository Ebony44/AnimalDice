using		UnityEngine;
using		System.Collections;
using		System.Collections.Generic;
using		System.Net.Sockets;
using		System.Net;
using		System.Text;


public enum ANIMALDICE_PK{
		R_09_GAMESTART           =90201, R_09_HISTORY             =90202, R_09_BETLIST             =90203, R_09_SHUFFLE             =90204, R_09_BETSTART            =90205, 
		R_09_SEC                 =90206, R_09_BET                 =90208, R_09_RESULT_DICE         =90209, R_09_RESULT_LOSE         =90210, R_09_RESULT_WIN          =90211, 
		R_09_RESULT              =90212, R_09_GAMESTATE           =90213, R_09_ERROR               =90214, R_09_BETBTN              =90215, 

		S_09_REQ_BET             =90207, 
		PACKET_MAX };


	public class st09_TABLE_PART
	{
		public int		nTABLEPOS;
		public stINT64	stTOTALBETMONEY;
		public stINT64	stRESULTMONEY;
		public stINT64	stMYBETMONEY;

		public st09_TABLE_PART()
		{
			nTABLEPOS      	= 0;
			stTOTALBETMONEY	= new stINT64();
			stRESULTMONEY  	= new stINT64();
			stMYBETMONEY   	= new stINT64();
		}

		~st09_TABLE_PART()
		{
		}

		public void set( ByteBuffer[] bBuffer )
		{
			nTABLEPOS      	= PKMAKER.get_Byte( bBuffer );
			stTOTALBETMONEY.set( bBuffer );
			stRESULTMONEY.set( bBuffer );
			stMYBETMONEY.set( bBuffer );
		}

		public void get( ByteBuffer bBuffer )
		{
			PKMAKER.make_Byte( bBuffer, nTABLEPOS );
			stTOTALBETMONEY.get( bBuffer );
			stRESULTMONEY.get( bBuffer );
			stMYBETMONEY.get( bBuffer );
		}

		public void clear()
		{
			nTABLEPOS      	= 0;
			stTOTALBETMONEY.clear();
			stRESULTMONEY.clear();
			stMYBETMONEY.clear();
		}
	};

	public class st09_WINTABLE_INFO
	{
		public int		nTABLEPOS;
		public int		nMULTI;
		public List<stUSER_BASE> 	lUSER;

		public st09_WINTABLE_INFO()
		{
			nTABLEPOS      	= 0;
			nMULTI         	= 0;
			lUSER          	= new List<stUSER_BASE>();
		}

		~st09_WINTABLE_INFO()
		{
		}

		public void set( ByteBuffer[] bBuffer )
		{
			nTABLEPOS      	= PKMAKER.get_Byte( bBuffer );
			nMULTI         	= PKMAKER.get_Long( bBuffer );
			int nCnt = 0;
			nCnt = PKMAKER.get_Long( bBuffer );
			for( int n = 0; n < nCnt;++n )
			{
				stUSER_BASE pSt = new stUSER_BASE();
				pSt.set( bBuffer );
				lUSER.Add( pSt );
			}
		}

		public void get( ByteBuffer bBuffer )
		{
			PKMAKER.make_Byte( bBuffer, nTABLEPOS );
			PKMAKER.make_Long( bBuffer, nMULTI );
			int nCnt = (int)lUSER.Count;
			PKMAKER.make_Long( bBuffer, nCnt );
			for( int n = 0; n < nCnt;++n )
			{
				stUSER_BASE pSt = lUSER[n];
				pSt.get( bBuffer );
			}
		}

		public void clear()
		{
			nTABLEPOS      	= 0;
			nMULTI         	= 0;
			for( int z = 0; z < lUSER.Count; ++z )
			{
				stUSER_BASE pD = lUSER[z];
				pD.clear();
			}
			lUSER.Clear();
		}
	};

	public class st09_HISTORY
	{
		public stINT64	stGAME_IDX;
		public int		nDICE1;
		public int		nDICE2;
		public int		nDICE3;
		public stINT64	stRESULTMONEY;

		public st09_HISTORY()
		{
			stGAME_IDX     	= new stINT64();
			nDICE1         	= 0;
			nDICE2         	= 0;
			nDICE3         	= 0;
			stRESULTMONEY  	= new stINT64();
		}

		~st09_HISTORY()
		{
		}

		public void set( ByteBuffer[] bBuffer )
		{
			stGAME_IDX.set( bBuffer );
			nDICE1         	= PKMAKER.get_Byte( bBuffer );
			nDICE2         	= PKMAKER.get_Byte( bBuffer );
			nDICE3         	= PKMAKER.get_Byte( bBuffer );
			stRESULTMONEY.set( bBuffer );
		}

		public void get( ByteBuffer bBuffer )
		{
			stGAME_IDX.get( bBuffer );
			PKMAKER.make_Byte( bBuffer, nDICE1 );
			PKMAKER.make_Byte( bBuffer, nDICE2 );
			PKMAKER.make_Byte( bBuffer, nDICE3 );
			stRESULTMONEY.get( bBuffer );
		}

		public void clear()
		{
			stGAME_IDX.clear();
			nDICE1         	= 0;
			nDICE2         	= 0;
			nDICE3         	= 0;
			stRESULTMONEY.clear();
		}
	};

	public class st09_DICE_PER
	{
		public int		nTABLEPOS;
		public int		nPER;

		public st09_DICE_PER()
		{
			nTABLEPOS      	= 0;
			nPER           	= 0;
		}

		~st09_DICE_PER()
		{
		}

		public void set( ByteBuffer[] bBuffer )
		{
			nTABLEPOS      	= PKMAKER.get_Byte( bBuffer );
			nPER           	= PKMAKER.get_Byte( bBuffer );
		}

		public void get( ByteBuffer bBuffer )
		{
			PKMAKER.make_Byte( bBuffer, nTABLEPOS );
			PKMAKER.make_Byte( bBuffer, nPER );
		}

		public void clear()
		{
			nTABLEPOS      	= 0;
			nPER           	= 0;
		}
	};

	public class st09_BET_DATA
	{
		public stUSER_BASE	stUSER;
		public int		nTABLEPOS;
		public int		nBTNIDX;
		public stINT64	stBETMONEY;
		public stUSERMONEY	stHAVEMONEY;
		public st09_TABLE_PART	stPARTINFO;

		public st09_BET_DATA()
		{
			stUSER         	= new stUSER_BASE();
			nTABLEPOS      	= 0;
			nBTNIDX        	= 0;
			stBETMONEY     	= new stINT64();
			stHAVEMONEY    	= new stUSERMONEY();
			stPARTINFO     	= new st09_TABLE_PART();
		}

		~st09_BET_DATA()
		{
		}

		public void set( ByteBuffer[] bBuffer )
		{
			stUSER.set( bBuffer );
			nTABLEPOS      	= PKMAKER.get_Byte( bBuffer );
			nBTNIDX        	= PKMAKER.get_Byte( bBuffer );
			stBETMONEY.set( bBuffer );
			stHAVEMONEY.set( bBuffer );
			stPARTINFO.set( bBuffer );
		}

		public void get( ByteBuffer bBuffer )
		{
			stUSER.get( bBuffer );
			PKMAKER.make_Byte( bBuffer, nTABLEPOS );
			PKMAKER.make_Byte( bBuffer, nBTNIDX );
			stBETMONEY.get( bBuffer );
			stHAVEMONEY.get( bBuffer );
			stPARTINFO.get( bBuffer );
		}

		public void clear()
		{
			stUSER.clear();
			nTABLEPOS      	= 0;
			nBTNIDX        	= 0;
			stBETMONEY.clear();
			stHAVEMONEY.clear();
			stPARTINFO.clear();
		}
	};

	public class st09_BETLIST
	{
		public int		nORDER;
		public int		nTABLEPOS;
		public stUSER_BASE	stUSER;
		public stINT64	stBETMONEY;

		public st09_BETLIST()
		{
			nORDER         	= 0;
			nTABLEPOS      	= 0;
			stUSER         	= new stUSER_BASE();
			stBETMONEY     	= new stINT64();
		}

		~st09_BETLIST()
		{
		}

		public void set( ByteBuffer[] bBuffer )
		{
			nORDER         	= PKMAKER.get_Long( bBuffer );
			nTABLEPOS      	= PKMAKER.get_Byte( bBuffer );
			stUSER.set( bBuffer );
			stBETMONEY.set( bBuffer );
		}

		public void get( ByteBuffer bBuffer )
		{
			PKMAKER.make_Long( bBuffer, nORDER );
			PKMAKER.make_Byte( bBuffer, nTABLEPOS );
			stUSER.get( bBuffer );
			stBETMONEY.get( bBuffer );
		}

		public void clear()
		{
			nORDER         	= 0;
			nTABLEPOS      	= 0;
			stUSER.clear();
			stBETMONEY.clear();
		}
	};

	public class st09_WINLOSE
	{
		public stUSER_BASE	stUSER;
		public int		nTABLEPOS;
		public int		nWINLOSE;
		public stINT64	stBETMONEY;
		public stINT64	stRESULTMONEY;
		public stUSERMONEY	stHAVEMONEY;

		public st09_WINLOSE()
		{
			stUSER         	= new stUSER_BASE();
			nTABLEPOS      	= 0;
			nWINLOSE       	= 0;
			stBETMONEY     	= new stINT64();
			stRESULTMONEY  	= new stINT64();
			stHAVEMONEY    	= new stUSERMONEY();
		}

		~st09_WINLOSE()
		{
		}

		public void set( ByteBuffer[] bBuffer )
		{
			stUSER.set( bBuffer );
			nTABLEPOS      	= PKMAKER.get_Byte( bBuffer );
			nWINLOSE       	= PKMAKER.get_Byte( bBuffer );
			stBETMONEY.set( bBuffer );
			stRESULTMONEY.set( bBuffer );
			stHAVEMONEY.set( bBuffer );
		}

		public void get( ByteBuffer bBuffer )
		{
			stUSER.get( bBuffer );
			PKMAKER.make_Byte( bBuffer, nTABLEPOS );
			PKMAKER.make_Byte( bBuffer, nWINLOSE );
			stBETMONEY.get( bBuffer );
			stRESULTMONEY.get( bBuffer );
			stHAVEMONEY.get( bBuffer );
		}

		public void clear()
		{
			stUSER.clear();
			nTABLEPOS      	= 0;
			nWINLOSE       	= 0;
			stBETMONEY.clear();
			stRESULTMONEY.clear();
			stHAVEMONEY.clear();
		}
	};

	public class st09_STATE_PLAYER
	{
		public stUSER_BASE	stUSER;
		public stUSERMONEY	stHAVEMONEY;
		public int		nISPLAYER;
		public int		nDIS;
		public int		nWINLOSE;
		public stINT64	stBETMONEY;

		public st09_STATE_PLAYER()
		{
			stUSER         	= new stUSER_BASE();
			stHAVEMONEY    	= new stUSERMONEY();
			nISPLAYER      	= 0;
			nDIS           	= 0;
			nWINLOSE       	= 0;
			stBETMONEY     	= new stINT64();
		}

		~st09_STATE_PLAYER()
		{
		}

		public void set( ByteBuffer[] bBuffer )
		{
			stUSER.set( bBuffer );
			stHAVEMONEY.set( bBuffer );
			nISPLAYER      	= PKMAKER.get_Byte( bBuffer );
			nDIS           	= PKMAKER.get_Byte( bBuffer );
			nWINLOSE       	= PKMAKER.get_Byte( bBuffer );
			stBETMONEY.set( bBuffer );
		}

		public void get( ByteBuffer bBuffer )
		{
			stUSER.get( bBuffer );
			stHAVEMONEY.get( bBuffer );
			PKMAKER.make_Byte( bBuffer, nISPLAYER );
			PKMAKER.make_Byte( bBuffer, nDIS );
			PKMAKER.make_Byte( bBuffer, nWINLOSE );
			stBETMONEY.get( bBuffer );
		}

		public void clear()
		{
			stUSER.clear();
			stHAVEMONEY.clear();
			nISPLAYER      	= 0;
			nDIS           	= 0;
			nWINLOSE       	= 0;
			stBETMONEY.clear();
		}
	};

	public class R_09_GAMESTART
	{
		public int		nSTEP;
		public stINT64	stGAME_IDX;

		public R_09_GAMESTART( ByteBuffer[] bBuffer )
		{
			nSTEP          	= PKMAKER.get_Byte( bBuffer );
			stGAME_IDX     	= new stINT64();
			stGAME_IDX.set( bBuffer );
		}

		public R_09_GAMESTART()
		{
			nSTEP          	= 0;
			stGAME_IDX     	= new stINT64();
		}

		~R_09_GAMESTART()
		{
		}

		public void clear()
		{
			nSTEP          	= 0;
			stGAME_IDX.clear();
		}
	};

	public class R_09_HISTORY
	{
		public List<st09_DICE_PER> 	lPER;
		public List<st09_HISTORY> 	lHISTORYS;

		public R_09_HISTORY( ByteBuffer[] bBuffer )
		{
			int nCnt = 0;
			lPER = new List<st09_DICE_PER>();
			nCnt = PKMAKER.get_Long( bBuffer );
			for( int n = 0; n < nCnt;++n )
			{
				st09_DICE_PER pSt = new st09_DICE_PER();
				pSt.set( bBuffer );
				lPER.Add( pSt );
			}
			nCnt = 0;
			lHISTORYS = new List<st09_HISTORY>();
			nCnt = PKMAKER.get_Long( bBuffer );
			for( int n = 0; n < nCnt;++n )
			{
				st09_HISTORY pSt = new st09_HISTORY();
				pSt.set( bBuffer );
				lHISTORYS.Add( pSt );
			}
		}

		public R_09_HISTORY()
		{
			lPER           	= new List<st09_DICE_PER>();
			lHISTORYS      	= new List<st09_HISTORY>();
		}

		~R_09_HISTORY()
		{
		}

		public void clear()
		{
			for( int z = 0; z < lPER.Count; ++z )
			{
				st09_DICE_PER pD = lPER[z];
				pD.clear();
			}
			lPER.Clear();
			for( int z = 0; z < lHISTORYS.Count; ++z )
			{
				st09_HISTORY pD = lHISTORYS[z];
				pD.clear();
			}
			lHISTORYS.Clear();
		}
	};

	public class R_09_BETLIST
	{
		public List<st09_BETLIST> 	lLIST;

		public R_09_BETLIST( ByteBuffer[] bBuffer )
		{
			int nCnt = 0;
			lLIST = new List<st09_BETLIST>();
			nCnt = PKMAKER.get_Long( bBuffer );
			for( int n = 0; n < nCnt;++n )
			{
				st09_BETLIST pSt = new st09_BETLIST();
				pSt.set( bBuffer );
				lLIST.Add( pSt );
			}
		}

		public R_09_BETLIST()
		{
			lLIST          	= new List<st09_BETLIST>();
		}

		~R_09_BETLIST()
		{
		}

		public void clear()
		{
			for( int z = 0; z < lLIST.Count; ++z )
			{
				st09_BETLIST pD = lLIST[z];
				pD.clear();
			}
			lLIST.Clear();
		}
	};

	public class R_09_SHUFFLE
	{
		public int		nSTEP;
		public stINT64	stGAME_IDX;
		public int		nANITIME;

		public R_09_SHUFFLE( ByteBuffer[] bBuffer )
		{
			nSTEP          	= PKMAKER.get_Byte( bBuffer );
			stGAME_IDX     	= new stINT64();
			stGAME_IDX.set( bBuffer );
			nANITIME       	= PKMAKER.get_Long( bBuffer );
		}

		public R_09_SHUFFLE()
		{
			nSTEP          	= 0;
			stGAME_IDX     	= new stINT64();
			nANITIME       	= 0;
		}

		~R_09_SHUFFLE()
		{
		}

		public void clear()
		{
			nSTEP          	= 0;
			stGAME_IDX.clear();
			nANITIME       	= 0;
		}
	};

	public class R_09_BETSTART
	{
		public int		nSTEP;
		public stINT64	stGAME_IDX;
		public List<st09_TABLE_PART> 	lPART;
		public int		nSEC;

		public R_09_BETSTART( ByteBuffer[] bBuffer )
		{
			nSTEP          	= PKMAKER.get_Byte( bBuffer );
			stGAME_IDX     	= new stINT64();
			stGAME_IDX.set( bBuffer );
			int nCnt = 0;
			lPART = new List<st09_TABLE_PART>();
			nCnt = PKMAKER.get_Long( bBuffer );
			for( int n = 0; n < nCnt;++n )
			{
				st09_TABLE_PART pSt = new st09_TABLE_PART();
				pSt.set( bBuffer );
				lPART.Add( pSt );
			}
			nSEC           	= PKMAKER.get_Long( bBuffer );
		}

		public R_09_BETSTART()
		{
			nSTEP          	= 0;
			stGAME_IDX     	= new stINT64();
			lPART          	= new List<st09_TABLE_PART>();
			nSEC           	= 0;
		}

		~R_09_BETSTART()
		{
		}

		public void clear()
		{
			nSTEP          	= 0;
			stGAME_IDX.clear();
			for( int z = 0; z < lPART.Count; ++z )
			{
				st09_TABLE_PART pD = lPART[z];
				pD.clear();
			}
			lPART.Clear();
			nSEC           	= 0;
		}
	};

	public class R_09_SEC
	{
		public int		nSEC;

		public R_09_SEC( ByteBuffer[] bBuffer )
		{
			nSEC           	= PKMAKER.get_Long( bBuffer );
		}

		public R_09_SEC()
		{
			nSEC           	= 0;
		}

		~R_09_SEC()
		{
		}

		public void clear()
		{
			nSEC           	= 0;
		}
	};

	public class S_09_REQ_BET
	{
		public int		nIDX;
		public stINT64	stGAME_IDX;
		public int		nBETPOS;
		public int		nBTNIDX;

		public S_09_REQ_BET()
		{
			nIDX           	= 0;
			stGAME_IDX     	= new stINT64();
			nBETPOS        	= 0;
			nBTNIDX        	= 0;
		}

		public void send()
		{
			nIDX = SubGameSocket.__IDX++;
			ByteBuffer TempBuffer = new ByteBuffer();

			PKMAKER.make_Long( TempBuffer, nIDX );
			stGAME_IDX.get( TempBuffer );
			PKMAKER.make_Byte( TempBuffer, nBETPOS );
			PKMAKER.make_Byte( TempBuffer, nBTNIDX );
			SubGameSocket.WritePacket( (int)ANIMALDICE_PK.S_09_REQ_BET, TempBuffer.array(),TempBuffer._nAddPos );
		}

		~S_09_REQ_BET()
		{
		}

		public void clear()
		{
			nIDX           	= 0;
			stGAME_IDX.clear();
			nBETPOS        	= 0;
			nBTNIDX        	= 0;
		}
	};

	public class R_09_BET
	{
		public string	szNICK;
		public List<st09_BET_DATA> 	lBET;

		public R_09_BET( ByteBuffer[] bBuffer )
		{
			szNICK         	= PKMAKER.get_String( bBuffer );
			int nCnt = 0;
			lBET = new List<st09_BET_DATA>();
			nCnt = PKMAKER.get_Long( bBuffer );
			for( int n = 0; n < nCnt;++n )
			{
				st09_BET_DATA pSt = new st09_BET_DATA();
				pSt.set( bBuffer );
				lBET.Add( pSt );
			}
		}

		public R_09_BET()
		{
			szNICK         	= "";
			lBET           	= new List<st09_BET_DATA>();
		}

		~R_09_BET()
		{
		}

		public void clear()
		{
			szNICK         	= "";
			for( int z = 0; z < lBET.Count; ++z )
			{
				st09_BET_DATA pD = lBET[z];
				pD.clear();
			}
			lBET.Clear();
		}
	};

	public class R_09_RESULT_DICE
	{
		public int		nSTEP;
		public stINT64	stGAME_IDX;
		public int		nTIME;
		public int		nOPENTIME;
		public int		nVIEWTIME;
		public int		nTABLETIME;
		public int		nDICE1;
		public int		nDICE2;
		public int		nDICE3;
		public List<st09_WINTABLE_INFO> 	lWINPART;

		public R_09_RESULT_DICE( ByteBuffer[] bBuffer )
		{
			nSTEP          	= PKMAKER.get_Byte( bBuffer );
			stGAME_IDX     	= new stINT64();
			stGAME_IDX.set( bBuffer );
			nTIME          	= PKMAKER.get_Long( bBuffer );
			nOPENTIME      	= PKMAKER.get_Long( bBuffer );
			nVIEWTIME      	= PKMAKER.get_Long( bBuffer );
			nTABLETIME     	= PKMAKER.get_Long( bBuffer );
			nDICE1         	= PKMAKER.get_Byte( bBuffer );
			nDICE2         	= PKMAKER.get_Byte( bBuffer );
			nDICE3         	= PKMAKER.get_Byte( bBuffer );
			int nCnt = 0;
			lWINPART = new List<st09_WINTABLE_INFO>();
			nCnt = PKMAKER.get_Long( bBuffer );
			for( int n = 0; n < nCnt;++n )
			{
				st09_WINTABLE_INFO pSt = new st09_WINTABLE_INFO();
				pSt.set( bBuffer );
				lWINPART.Add( pSt );
			}
		}

		public R_09_RESULT_DICE()
		{
			nSTEP          	= 0;
			stGAME_IDX     	= new stINT64();
			nTIME          	= 0;
			nOPENTIME      	= 0;
			nVIEWTIME      	= 0;
			nTABLETIME     	= 0;
			nDICE1         	= 0;
			nDICE2         	= 0;
			nDICE3         	= 0;
			lWINPART       	= new List<st09_WINTABLE_INFO>();
		}

		~R_09_RESULT_DICE()
		{
		}

		public void clear()
		{
			nSTEP          	= 0;
			stGAME_IDX.clear();
			nTIME          	= 0;
			nOPENTIME      	= 0;
			nVIEWTIME      	= 0;
			nTABLETIME     	= 0;
			nDICE1         	= 0;
			nDICE2         	= 0;
			nDICE3         	= 0;
			for( int z = 0; z < lWINPART.Count; ++z )
			{
				st09_WINTABLE_INFO pD = lWINPART[z];
				pD.clear();
			}
			lWINPART.Clear();
		}
	};

	public class R_09_RESULT_LOSE
	{
		public int		nSTEP;
		public int		nTIME;
		public int		nMOVETIME;
		public List<st09_TABLE_PART> 	lLOSEPART;

		public R_09_RESULT_LOSE( ByteBuffer[] bBuffer )
		{
			nSTEP          	= PKMAKER.get_Byte( bBuffer );
			nTIME          	= PKMAKER.get_Long( bBuffer );
			nMOVETIME      	= PKMAKER.get_Long( bBuffer );
			int nCnt = 0;
			lLOSEPART = new List<st09_TABLE_PART>();
			nCnt = PKMAKER.get_Long( bBuffer );
			for( int n = 0; n < nCnt;++n )
			{
				st09_TABLE_PART pSt = new st09_TABLE_PART();
				pSt.set( bBuffer );
				lLOSEPART.Add( pSt );
			}
		}

		public R_09_RESULT_LOSE()
		{
			nSTEP          	= 0;
			nTIME          	= 0;
			nMOVETIME      	= 0;
			lLOSEPART      	= new List<st09_TABLE_PART>();
		}

		~R_09_RESULT_LOSE()
		{
		}

		public void clear()
		{
			nSTEP          	= 0;
			nTIME          	= 0;
			nMOVETIME      	= 0;
			for( int z = 0; z < lLOSEPART.Count; ++z )
			{
				st09_TABLE_PART pD = lLOSEPART[z];
				pD.clear();
			}
			lLOSEPART.Clear();
		}
	};

	public class R_09_RESULT_WIN
	{
		public int		nSTEP;
		public int		nTIME;
		public int		nMOVETIME;
		public List<st09_TABLE_PART> 	lWINPART;
		public List<st09_WINLOSE> 	lWINNER;

		public R_09_RESULT_WIN( ByteBuffer[] bBuffer )
		{
			nSTEP          	= PKMAKER.get_Byte( bBuffer );
			nTIME          	= PKMAKER.get_Long( bBuffer );
			nMOVETIME      	= PKMAKER.get_Long( bBuffer );
			int nCnt = 0;
			lWINPART = new List<st09_TABLE_PART>();
			nCnt = PKMAKER.get_Long( bBuffer );
			for( int n = 0; n < nCnt;++n )
			{
				st09_TABLE_PART pSt = new st09_TABLE_PART();
				pSt.set( bBuffer );
				lWINPART.Add( pSt );
			}
			nCnt = 0;
			lWINNER = new List<st09_WINLOSE>();
			nCnt = PKMAKER.get_Long( bBuffer );
			for( int n = 0; n < nCnt;++n )
			{
				st09_WINLOSE pSt = new st09_WINLOSE();
				pSt.set( bBuffer );
				lWINNER.Add( pSt );
			}
		}

		public R_09_RESULT_WIN()
		{
			nSTEP          	= 0;
			nTIME          	= 0;
			nMOVETIME      	= 0;
			lWINPART       	= new List<st09_TABLE_PART>();
			lWINNER        	= new List<st09_WINLOSE>();
		}

		~R_09_RESULT_WIN()
		{
		}

		public void clear()
		{
			nSTEP          	= 0;
			nTIME          	= 0;
			nMOVETIME      	= 0;
			for( int z = 0; z < lWINPART.Count; ++z )
			{
				st09_TABLE_PART pD = lWINPART[z];
				pD.clear();
			}
			lWINPART.Clear();
			for( int z = 0; z < lWINNER.Count; ++z )
			{
				st09_WINLOSE pD = lWINNER[z];
				pD.clear();
			}
			lWINNER.Clear();
		}
	};

	public class R_09_RESULT
	{
		public int		nSTEP;
		public int		nTIME;
		public int		nVIEWTIME;
		public List<st09_WINLOSE> 	lWINLOSE;

		public R_09_RESULT( ByteBuffer[] bBuffer )
		{
			nSTEP          	= PKMAKER.get_Byte( bBuffer );
			nTIME          	= PKMAKER.get_Long( bBuffer );
			nVIEWTIME      	= PKMAKER.get_Long( bBuffer );
			int nCnt = 0;
			lWINLOSE = new List<st09_WINLOSE>();
			nCnt = PKMAKER.get_Long( bBuffer );
			for( int n = 0; n < nCnt;++n )
			{
				st09_WINLOSE pSt = new st09_WINLOSE();
				pSt.set( bBuffer );
				lWINLOSE.Add( pSt );
			}
		}

		public R_09_RESULT()
		{
			nSTEP          	= 0;
			nTIME          	= 0;
			nVIEWTIME      	= 0;
			lWINLOSE       	= new List<st09_WINLOSE>();
		}

		~R_09_RESULT()
		{
		}

		public void clear()
		{
			nSTEP          	= 0;
			nTIME          	= 0;
			nVIEWTIME      	= 0;
			for( int z = 0; z < lWINLOSE.Count; ++z )
			{
				st09_WINLOSE pD = lWINLOSE[z];
				pD.clear();
			}
			lWINLOSE.Clear();
		}
	};

	public class R_09_GAMESTATE
	{
		public int		nSTEP;
		public int		nTIME;
		public int		nRESTTIME;
		public stINT64	stGAME_IDX;
		public List<st09_STATE_PLAYER> 	lUSERSTATE;
		public List<st09_TABLE_PART> 	lPART;

		public R_09_GAMESTATE( ByteBuffer[] bBuffer )
		{
			nSTEP          	= PKMAKER.get_Byte( bBuffer );
			nTIME          	= PKMAKER.get_Long( bBuffer );
			nRESTTIME      	= PKMAKER.get_Long( bBuffer );
			stGAME_IDX     	= new stINT64();
			stGAME_IDX.set( bBuffer );
			int nCnt = 0;
			lUSERSTATE = new List<st09_STATE_PLAYER>();
			nCnt = PKMAKER.get_Long( bBuffer );
			for( int n = 0; n < nCnt;++n )
			{
				st09_STATE_PLAYER pSt = new st09_STATE_PLAYER();
				pSt.set( bBuffer );
				lUSERSTATE.Add( pSt );
			}
			nCnt = 0;
			lPART = new List<st09_TABLE_PART>();
			nCnt = PKMAKER.get_Long( bBuffer );
			for( int n = 0; n < nCnt;++n )
			{
				st09_TABLE_PART pSt = new st09_TABLE_PART();
				pSt.set( bBuffer );
				lPART.Add( pSt );
			}
		}

		public R_09_GAMESTATE()
		{
			nSTEP          	= 0;
			nTIME          	= 0;
			nRESTTIME      	= 0;
			stGAME_IDX     	= new stINT64();
			lUSERSTATE     	= new List<st09_STATE_PLAYER>();
			lPART          	= new List<st09_TABLE_PART>();
		}

		~R_09_GAMESTATE()
		{
		}

		public void clear()
		{
			nSTEP          	= 0;
			nTIME          	= 0;
			nRESTTIME      	= 0;
			stGAME_IDX.clear();
			for( int z = 0; z < lUSERSTATE.Count; ++z )
			{
				st09_STATE_PLAYER pD = lUSERSTATE[z];
				pD.clear();
			}
			lUSERSTATE.Clear();
			for( int z = 0; z < lPART.Count; ++z )
			{
				st09_TABLE_PART pD = lPART[z];
				pD.clear();
			}
			lPART.Clear();
		}
	};

	public class R_09_ERROR
	{
		public int		nERROR_TYPE;
		public string	szMSG;

		public R_09_ERROR( ByteBuffer[] bBuffer )
		{
			nERROR_TYPE    	= PKMAKER.get_Long( bBuffer );
			szMSG          	= PKMAKER.get_String( bBuffer );
		}

		public R_09_ERROR()
		{
			nERROR_TYPE    	= 0;
			szMSG          	= "";
		}

		~R_09_ERROR()
		{
		}

		public void clear()
		{
			nERROR_TYPE    	= 0;
			szMSG          	= "";
		}
	};

	public class R_09_BETBTN
	{
		public List<stBUTTONSET> 	lBTNS;

		public R_09_BETBTN( ByteBuffer[] bBuffer )
		{
			int nCnt = 0;
			lBTNS = new List<stBUTTONSET>();
			nCnt = PKMAKER.get_Long( bBuffer );
			for( int n = 0; n < nCnt;++n )
			{
				stBUTTONSET pSt = new stBUTTONSET();
				pSt.set( bBuffer );
				lBTNS.Add( pSt );
			}
		}

		public R_09_BETBTN()
		{
			lBTNS          	= new List<stBUTTONSET>();
		}

		~R_09_BETBTN()
		{
		}

		public void clear()
		{
			for( int z = 0; z < lBTNS.Count; ++z )
			{
				stBUTTONSET pD = lBTNS[z];
				pD.clear();
			}
			lBTNS.Clear();
		}
	};
