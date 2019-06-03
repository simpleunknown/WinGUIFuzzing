
;
; Example automation script to drive Notepad.exe
;
; What does this script do?
;
;  - Wait for notepad to load
;  - Switch to notepad window
;  - Open the file "text.txt"
;  - Exit application
;
; Q: Does this script require a 3rd party program?
; A: Yes, this is an Auto Hot Key script.  This is a
;    free program. https://autohotkey.com
;
; Q: What operating systems does auto hot key support?
; A: Windows.  Other solutions exist for OS X and Linux
;
; Q: How do I test this script?
; A: Launch notepad.exe, then run script (double click)
;
; Q: Why doesn't it launch notepad?
; A: With Peach, notepad would be launched by a debugger
;    like Windows Debug Engine
;
; Q: How to I use this script with Peach?
; A: Follow these steps:
;    
;    a) Add a RunCommand monitor *after* your Debugger monitor
;    b) Command: C:\Program Files\AutoHotkey\AutoHotkey.exe
;    c) Arguments: c:\assets\script.ahk
;    d) When: OnCall
;    e) OnCall: OnIterationExitEvent
;
;    Note: steps d & e should match your debugger config
;

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

; Switch to notepad window
; "Untitled - Notepad" is the window title
WinActivate, Untitled - Notepad
WinWaitActive, Untitled - Notepad

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

; Open file
Send, ^o

WinWaitActive, Open
Send, test.txt
Send, {enter}

; NOTE: Here you should handle any error dialogs
;       that might open.

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

; Wait for open to finish, then exit

WinWaitActive, test.txt - Notepad

; Exit notepad
;Send, !f
;Send, x

; done
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
