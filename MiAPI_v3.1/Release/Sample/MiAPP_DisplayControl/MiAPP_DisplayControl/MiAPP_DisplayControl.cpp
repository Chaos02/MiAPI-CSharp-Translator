// MiAPP_DisplayControl.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <Windows.h>
#include "MiAPI.h"

#pragma comment(lib, "MiAPI.lib")
#pragma warning(disable:4996)


#define APP_NAME     "MiAPP Display Control Demo"
#define APP_VERSION  "0.01"   

int _tmain(int argc, _TCHAR* argv[])
{
	int errorcode = MiAPI_OK;

	int Major,Minor;
	char BIOSVersion[80];
	char ProductName[80];
	DWORD size;
		
	
	int nDisplays;
	MIAPI_MONITOR_INFO disp;

	//-- Start the MPAI APIs libary
	if( MiAPI_Start() != MiAPI_OK )
	{
		printf("Error: Failed to initialize MAPI library.\n");
		return MiAPI_INIT_FAIL;
	}

	//--- Show Product name ---
	MiAPI_GetProductName(ProductName, &size);
	printf("Product name : %s\n", ProductName);
	
	MiAPI_GetBIOSVersion(BIOSVersion, &size);
	printf("BIOS version : %s\n", BIOSVersion);
	
	errorcode = MiAPI_GetMiAPIVersion(&Major, &Minor);
	printf("MiAPI DLL version : %d.%d \n",Major,Minor);
	if(errorcode == MiAPI_OLD_VERSION ) printf("[Note] This MiAPI DLL only supports less legacy features for this mother board!\n");

	//--------------------
	 errorcode = MiAPI_Display_GetAmountOfMonitors(&nDisplays);
	 printf("\nThere is %d Display(s) detected.(Errorcode = %X)\n", nDisplays, errorcode);
	 
	 //Test the brightness control on each display
	 for(int i = 0; i < nDisplays; i++)
	 {
		 MIAPI_BRIGHTNESS brg;
		 DWORD stepsize;

		 errorcode = MiAPI_Display_GetMonitorInfo(&disp, i);
		 wprintf(L"\nDisplay %d : %ls  With total brightness Levels = %d (ErrorCode = %X)\n", disp.DeviceIndex, disp.FriendlyDeviceName, disp.WMITotalBrightnessLevel, errorcode);
		 errorcode = MiAPI_Display_GetBrightness(&brg, i);
		 if(errorcode == MiAPI_OK) 
		 {
			 printf(" Brightness : Max = %d; Min = %d; Cur = %d\n", brg.MaximumBrightness, brg.MinimumBrightness, brg.CurrentBrightness );
		     stepsize = (brg.MaximumBrightness - brg.MinimumBrightness) / (disp.WMITotalBrightnessLevel - 1);
			 printf(" Brightness control from the darkest to the brightest ...\n ");
		     for(int j = 0; j< disp.WMITotalBrightnessLevel; j++)
		     { 
			     Sleep(200);
			     errorcode = MiAPI_Display_SetBrightness(brg.MinimumBrightness + j * stepsize, i);
		     }
			 // after 500 ms , restore to orignal brightness
		     Sleep(500);
		     errorcode = MiAPI_Display_SetBrightness(brg.CurrentBrightness, i);
		 }
	 }

	 printf("\nThe following test is to turn off/on all display...\n");
	 printf(" It will dim the display for 2 seconds or light it up again by key pressed or mouse moved.\n");	
	 Sleep(500);
	 system("pause");
	
	 // Turn off monitor
	 errorcode = MiAPI_Display_Off();
	 
	 Sleep(2000);	 
	 // Turn on monitor
	 errorcode = MiAPI_Display_On();

    //--------------
    MiAPI_Exit();

	return errorcode;
}

