@echo off

:GPIO_TEST
ECHO --- This is for 20 pin header 
ECHO --- Ensure the following jumper connection
ECHO     GPIO1 - GPIO2
ECHO     GPIO3 - GPIO4
ECHO     GPIO6 - GPIO7
ECHO     GPIO8 - GPIO9
ECHO     GPIO5 - GPIO10  (Plug off WD_TIME pin) 
ECHO.

ECHO 0. Show all GPIOs' initial status
echo on
MiAPP_GPIO -all
echo off
ECHO. 

ECHO --- Set GPIO 1,3,6,8,5 as Output
ECHO --- Set GPIO 2,4,7,9,10 as Input
ECHO.
ECHO 1. Set all Output GPIO levels to low.
ECHO    Expect: all Input GPIO to be low, too.
echo on
MiAPP_GPIO -io=0101001011 -hl=0101001011
MiAPP_GPIO -cp=0000000000
echo off
ECHO.
if errorlevel 4 goto GPIO_FAIL

ECHO 2. Set all Output GPIO levels to high.
ECHO    Expect: all Input GPIO to be high, too.
echo on
MiAPP_GPIO -hl=1010110100
MiAPP_GPIO -cp=1111111111
echo off
ECHO.
if errorlevel 4 goto GPIO_FAIL

ECHO Switch Input and Output GPIOs
ECHO --- Set GPIO 1,3,6,8,5 as Input
ECHO --- Set GPIO 2,4,7,9,10 as Output
ECHO.
ECHO 3. Set all Output GPIO levels to low.
ECHO    Expect: all Input GPIO to be low, too.
echo on
MiAPP_GPIO -io=1010110100 -hl=1010110100
MiAPP_GPIO -cp=0000000000
echo off
ECHO.
if errorlevel 4 goto GPIO_FAIL

ECHO 4. Set all Output GPIO levels to high.
ECHO    Expect: all Input GPIO to be high, too.
echo on
MiAPP_GPIO -hl=0101001011
MiAPP_GPIO -cp=1111111111
echo off
ECHO.
if errorlevel 4 goto GPIO_FAIL

ECHO ***********************
ECHO *   GPIO test: PASS   *
ECHO ***********************
goto EXIT_TEST

:GPIO_FAIL
ECHO ***********************
ECHO *   GPIO test: FAIL   *
ECHO ***********************

:EXIT_TEST