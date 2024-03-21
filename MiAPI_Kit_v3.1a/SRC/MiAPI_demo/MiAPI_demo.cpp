//-----------------------------------------------------------------------------
//     Author : York Huang
//       Mail : york.huang@mic.com.tw        
//
//    Copyright 2015-2020 MiTAC Computing Technokogy Corp. All rights reserved.
//-----------------------------------------------------------------------------
// MiAPI_demo.cpp : This the console application will demonstrate MiAPI 5 major
// features : GPIO, Hardware monitoring, Display Control, SMBUS, Watchdog. 

#include "stdafx.h"
#include <Windows.h>
#include <process.h>
#include <stdio.h>
#include <conio.h>
#include <iostream>
#include <time.h> 
#include "../LIB/MiAPI.h"

// Alternatively add the following pragma comment, instead of setting up referrence dependence 
// in compiler environment setting. Be aware to put the same bits version MiAPI.lib in the source
// folder
#pragma comment(lib, "../LIB/MiAPI.lib")

//--Global varibles 
bool isRefresh = false;     //It indicates if WDT timer has refreshed or not.
int TimeToCrash = 0;        //It set a time to simulate application crash for WDT demo.


//-- Funtions start ----

int Do_MiAPI_Version(void)
// This feature demonstrates the BIOS version information provided by motherboard SMBIOS. 
// It also fetches the current MiAPI DLL version. Note: If the error code of MiAPI_GetMiAPIVersion
// returns MiAPI_OLD_VERSION (0x05), it might mean the MB's BIOS implement limited features only,
// and might have few compatibility issues on watchdog,SMBUS and GPIO. Please contact vendor for 
// issue report.     
{
	int Major,Minor;
	char BIOSVersion[80];
	char ProductName[80];
	DWORD size;
	int ret = MiAPI_OK;	
	
	printf("*** Show BIOS & MiAPI version ***\n");

	ret = MiAPI_GetProductName(ProductName, &size); 
	printf("Product name : %s\n", ProductName);
	
	
	ret = MiAPI_GetBIOSVersion(BIOSVersion, &size);
	printf("BIOS version : %s\n", BIOSVersion);
	
	
	ret = MiAPI_GetMiAPIVersion(&Major, &Minor);
	printf("MAPI DLL version : %d.%d \n",Major,Minor);
	
	return ret;
}

int Do_MiAPI_GPIO(void)
// This feature demonstrates the GPIO configuration. It uses a C struct (MIAPI_GPIO_STATUS) to 
// hold the GPIO setting(Input/Output) and value(High/Low). 
{
	int ret = MiAPI_OK;
	MIAPI_GPIO_STATUS gpio[10];
	
		
	printf("*** GPIO demo ***\n");
	printf("This demo will set the GPIO 1~5 as input \n  and GPIO 6~10 as output low.");
	
	//--Set GPIO 1~5 to input
	for(int i = 1; i<=5; i++)
	{
       gpio[i-1].Direction = 0x01;     //input = 0x01; output = 0x00 for MiAPI v3.1;
	   gpio[i-1].VoltageLevel = 0x00;  // for input mode, this voltage level is dummy value as no use.
	   ret = MiAPI_GPIO_SetStatus(i, gpio[i-1]);	   
	}	
		
	//--Set GPIO 6~10 to output low
	for(int i = 6; i<=10; i++)
	{
       gpio[i-1].Direction = 0x00;     //input = 0x01; output = 0x00 for MiAPI v3.1;
	   gpio[i-1].VoltageLevel = 0x00;  // Low = 0x00; High = 0x01
	   ret = MiAPI_GPIO_SetStatus(i, gpio[i-1]);	   
	}	
		
	//-- Show  all GPIO tatus
	for (int i =1; i <= 10; i++ )
	{
    //--Get GPIO status from each GPIO
	    ret = MiAPI_GPIO_GetStatus((BYTE)i, &(gpio[i-1]));	
		printf("  GPIO %02d  :  DIR=%d  LEVEL=%d (Err=%02Xh)\n", i, gpio[i-1].Direction, gpio[i-1].VoltageLevel, ret);
	}

    return ret;	
}

int DO_MiAPI_HWMonitoring(void)
// This feature demonstrates the Hardware Monitoring reading. It might show 0 RPM if no fan is installed.
{
	int ret = MiAPI_OK;
	WORD dummy = 0;
    WORD T_CPU = 0 , T_SYS;
	WORD RPM_CPU = 0, RPM_SYS = 0;
	WORD Volt_CPU = 0;
	
	printf("*** Hardware Monitoring demo ***\n");
	printf("\nShow Hardware moniter information per second for 5 times...");
	
	for( int i = 0; i < 5; i++)
	{
		ret = MiAPI_GetTemperature(1, &T_CPU, &dummy);
		ret = MiAPI_GetTemperature(2, &T_SYS, &dummy);
		printf("\n  Temperature : CPU = %d C, System = %d C\n", T_CPU, T_SYS );
		
		ret = MiAPI_GetFanSpeed(1, &RPM_CPU, &dummy);
		ret = MiAPI_GetFanSpeed(2, &RPM_SYS, &dummy);
		printf("  Fan Speed   : CPU = %d RPM, System = %d RPM\n", RPM_CPU, RPM_SYS);
		
		ret = MiAPI_GetVoltage(1, &Volt_CPU, &dummy);        
        printf("  Voltage     : CPU = %3.3f V\n", (float)Volt_CPU/1000.0);
		
        Sleep(1000);   		
	}
	
	return ret;
}

int Do_MiAPI_DisplayControl(void)
// This feature demonstrates the display control including brightness control , constrast, 
// display on/off, orientation. It will work on most of modern displays/panels that MS Windows
// has supported. 
{
	int ret = MiAPI_OK;
	
	int nDisplays;
	MIAPI_MONITOR_INFO disp;
	MIAPI_BRIGHTNESS brg;
	
	printf("*** Display Control demo ***\n");

	ret = MiAPI_Display_GetAmountOfMonitors(&nDisplays);
	printf("\nThere is %d Display(s) detected.(Errorcode = %X)\n", nDisplays, ret);
	
	for(int i = 0; i < nDisplays; i++)
	{
		//-- Get Display infomation
		ret = MiAPI_Display_GetMonitorInfo(&disp, i);		
		if(ret == MiAPI_OK) 
		{			
			wprintf(L"\nDisplay #%02d : %ls\n", disp.DeviceIndex, disp.FriendlyDeviceName);
		//-- Get Brightness info 
		    ret = MiAPI_Display_GetBrightness(&brg, i);
			printf("Brightness: Min = %d, Max = %d, Current = %d\n",brg.MinimumBrightness, brg.MaximumBrightness, brg.CurrentBrightness );
		
        //-- Set brightness to minimum.  
            printf("\nSet brightness to minimum.\n");
            ret = MiAPI_Display_SetBrightness(brg.MinimumBrightness, i);		    
			Sleep(1000);
			
		//-- Set brightness to maximum. 
            printf("Set brightness to maximum.\n");
            ret = MiAPI_Display_SetBrightness(brg.MaximumBrightness, i);		    
			Sleep(1000);
		
		//-- Restore to the original brightness and contrast 
            printf("Restore to orignal brightnesst\n");
            ret = MiAPI_Display_SetBrightness(brg.CurrentBrightness, i);		    
			Sleep(1500);
			
		//-- Reverse orientation & return after 2 seconds 
            printf("\nReverse the display by 180 degree\n"); 	
            MiAPI_Display_SetOrientation(180, i);
			Sleep(1500);
			printf("Return to normal orientation\n"); 	
            MiAPI_Display_SetOrientation(0, i);			
		}
	}
     //-- Display On & OFF
     printf("\nThe following test is to turn off/on all display...\n");
	 printf(" It will dim the display for 2 seconds , then wake it up by key pressed or mouse moved.\n");	 
	 system("pause");
	
	 // Turn off monitor
	 ret = MiAPI_Display_Off();
	 
	 Sleep(2000);
	 
	 // Turn on monitor
	 ret = MiAPI_Display_On();	 
			
	return ret;
}

int Do_MiAPI_SMBUS_SCAN(void)
// This feature demonstrates the capbility to read DRAM DIMM SPD or EEPROM mounted on SMBUS interface.
// Note: There could be Write protection bit set on specific MB for security purpose, and result in 
// malfunction of these SMBUS protocol. 
{
	int ret = MiAPI_OK;
	BYTE saddr, cmd, data, dat[0x40]; 
	
	printf("*** SMBUS demo ***\n");

	printf("\nThis demo will scan all EEPROM devices(Slave addr 0xA0~0xAF) via SMBUS.\n(It dumps first 64 bytes of Memory DIMM SPD in this case.)\n");	

	//-- Scan all EEPROM from Slave address 0xA0 ~ 0XAF
	for(saddr = 0xA0; saddr <= 0xAF; saddr++)
	{	
       DWORD len =1;	   
	   WORD offs = 0;	   
	   
	   //-- cmd hold MSB of EEPROM offset(WORD), and data will hold LSB
       //  The demo read from offset 0x0000. 	   
       data = (BYTE)(offs& 0x00ff);
	   cmd = (BYTE)(offs >> 8);
	 	 
	   //-- Send a ReadByte command 
	   ret = MiAPI_SMBusReadByte(saddr,cmd, &dat[0]);	

	   if(ret == 0)
	   {
	      for(int i= 1; i< 64; i++ )
	      {		        
			  MiAPI_SMBusReceiveByte(saddr,&dat[i]);	         
	      }

		  printf("\nSlave address %02X :\n", saddr);
		  
		  printf("ADDR  00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F\n");
	      printf("====  == == == == == == == == == == == == == == == ==\n");
		  for( int i = 0; i< 64; i++)
		  {
			 if((i %16) == 0) printf("[%02X]:", i);
			 printf(" %02X", dat[i]);
			 if((i %16) == 15) printf("\n");
		  }
	   }
	}

	
	return ret;
}


void loading(void)
// A simulated loading routine to display timestamp & watchdog countdown.
// It uses a global varible "TimeToCrash" to simulate app crash after a 
// specific time; otherwise set it to '0' for normal operation.    
{
	static int TimeBeenRun = 0;	
	int loading =5;

	//--Simulate a 5 seconds time-consume loading 
        for( int i = 0; i < loading; i++ )	
        {
			SYSTEMTIME time;
			DWORD min, max, cur;

			GetLocalTime(&time);
			MiAPI_Watchdog_GetRange(&min, &max, &cur);
			printf( "%02d/%02d/%02d %02d:%02d:%02d - WDT = %d ", 					 
					time.wMonth, time.wDay,    time.wYear,
					time.wHour,  time.wMinute, time.wSecond,
					cur );
			if(isRefresh) printf("(Refreshed !)\n ");
			isRefresh =false;

			printf("\r");
			fflush(stdout);
            Sleep(1000);

			if(TimeToCrash)
			{
				if( ++TimeBeenRun > TimeToCrash)
				{
					printf("\n**** Application crash! ****\n");
					printf("Wait for WDT timeout to reboot...\n");
				
                   //-- Infinite loop to simulate application crash.				
					while(true) { Sleep(1000); }; 
				}
			}
		}

}

unsigned int __stdcall thread_WDT(void* data) 
// This is application worker thread to demonstrate the WDT refresh and disable feature.
// Main thread pass a running time into loading while loop for WDT refresh. After it exits,
// disable the WDT timer to stop the countdown.   
{		 
	int& running = *((int*)data);

	//-- Loop running times for WDT refresh before WDT countdown to 0.
	while(running-- > 0 )
	{
        MiAPI_Watchdog_Refresh();
		isRefresh = true;

		//--Simulate a time-consume loading 
        loading();
	}

	//--Stop the watchdog for safety when it exits the while loop.
	MiAPI_Watchdog_Disable();

	return 0;
}


int Do_MiAPI_WDT(void)
// This routine demonstrates two WDT cases. One for general operation for normal application life cycles.
// The other to simulate app crash and system force reboot due to WDT timeout. There is risk to damage HDD
// if the HDD is doing read/write operation simultaneously. Be cautious about the case 2.
{
	int ret = MiAPI_OK;
	char choosed = 0;
	int running = 3;
	
	HANDLE handleWDT;
	
	printf("*** Watchdog demo ***\n");
    //-- Handle WDT cases
	do {
		printf("\n=== WDT Demo cases ===\n");
		printf("  0 : Exit\n");
		printf("  1 : App thread exits and halt WDT --- NORMAL \n");
		printf("  2 : APP thread crash and system reboot --- DANGER\n");		
	    printf("Choose (0 ~ 2) : ");
		choosed = _getche();
		printf("\n\n");

		switch(choosed)
		{
		   case '0' :  //Exit			  
			   break;

		   case '1' :
			   TimeToCrash = 0;
			   running = 3;
			   ret = MiAPI_Watchdog_SetConfig(10, true); 	
               MiAPI_Watchdog_Start();	
               handleWDT = (HANDLE)_beginthreadex(0, 0, &thread_WDT, &running, 0, 0);					   
			   break;

           case '2' :
			   TimeToCrash = 13; 
			   running = 5;
			   ret = MiAPI_Watchdog_SetConfig(10, true); 
               MiAPI_Watchdog_Start();	
               handleWDT = (HANDLE)_beginthreadex(0, 0, &thread_WDT, &running, 0, 0);					   
			   break;
		}
		
	    WaitForSingleObject(handleWDT, INFINITE);
		CloseHandle(handleWDT);

	} while (choosed != '0');
	
	
	MiAPI_Watchdog_Disable();
		
	return ret;
}

int _tmain(int argc, _TCHAR* argv[])
{
	int ret ;
	char choosed = 0;
	
	//-- Start the MiAPI libary
	if( MiAPI_Start() != MiAPI_OK )

	{
		printf("Error: Failed to initialize MAPI library.\n");
		return MiAPI_INIT_FAIL;
	}

	//-- Handle MiAPI　functions
	do {
		printf("\n=== Demo Menu ===\n");
		printf("  0 : Exit\n");
		printf("  1 : Show BIOS & MiAPI version\n");
		printf("  2 : HW monitoring\n");
		printf("  3 : GPIO setting\n");
		printf("  4 : SMBUS scan\n");
		printf("  5 : Display Control\n");
		printf("  6 : Watchdog\n");
		printf("Choose (0 ~ 6) : ");
		choosed = _getche();
		printf("\n\n");
		
	 //--  Call MiAPI functions 
		switch(choosed)
		{
		   case '0' :  //Exit
			   printf(" Exit...\n");
			   break;

		   case '1' :
			   ret = Do_MiAPI_Version();
			   break;

           case '2' :
			   ret = DO_MiAPI_HWMonitoring();
			   break;

           case '3' :
			   ret = Do_MiAPI_GPIO();
			   break;

           case '4' :
			   ret = Do_MiAPI_SMBUS_SCAN();
			   break;

		   case '5' :
			   ret = Do_MiAPI_DisplayControl();
			   break;

           case '6' :
			   ret = Do_MiAPI_WDT();
			   break;		   
		}

	} while (choosed != '0');
			

	//-- It must call MiAPI_Exit() to free the resource when MiAPI exits. 	
	MiAPI_Exit();	

	return 0;
}

