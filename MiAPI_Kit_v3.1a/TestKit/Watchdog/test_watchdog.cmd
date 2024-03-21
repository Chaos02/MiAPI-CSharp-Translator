@echo off
prompt $s
cecho {aqua} 1. Check current watchdog timeout count...{#}{\n}
MiAPP_WDT -timeout=10
echo on
MiAPP_WDT -status
@echo off
echo.

cecho {aqua} 2. Let Watchdog go for 2 second...{#}{\n}
echo on
MiAPP_WDT -go
@echo off
timeout /T 2 /nobreak
echo on
MiAPP_WDT -status
@echo off
cecho {yellow} Check if current countdown decreses or not?{#}{\n}
echo.

cecho {aqua} 3. After 2 seconds, Refresh the watchdog....{#}{\n}
timeout /T 2 /nobreak
echo on
MiAPP_WDT -refresh
MiAPP_WDT -status 
@echo off
echo.

cecho {aqua} 4. Halt the watchdog and set the timeout to 5 seconds.{#}{\n}
echo on
MiAPP_WDT -halt
MiAPP_WDT -timeout=5 -reboot=1
MiAPP_WDT -refresh
MiAPP_WDT -status 
@echo off
echo.

cecho {aqua} 5. *** Make sure you have saved the log above... ***{#}{\n}
cecho {red} Next, hit any key to continue and system will ***REBOOT*** after 5 second...!{#}{\n}
pause
echo on
MiAPP_WDT -go