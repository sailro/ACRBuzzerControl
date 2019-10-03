# ACRBuzzerControl
Control device buzzer when an NFC card is detected. ACR122U or compatible NFC reader devices.

I bought an ACS ACR1252 NFC Dual Reader on amazon. By default the reader buzzes when it reads a card. You can use this tool to disable or enable this behavior.

The full API for the device is here:
https://www.acs.com.hk/en/download-manual/419/API-ACR122U-2.04.pdf

Kudos to Daniel Mueller for his great PC/SC wrapper for .NET (https://github.com/danm-de/pcsc-sharp).

```
Connected readers:
(0) ACS ACR1252 Dual Reader PICC 0
(1) ACS ACR1252 Dual Reader SAM 0
(2) Windows Hello for Business 1

Please select a compatible ACR122U device...0
Using ACS ACR1252 Dual Reader PICC 0

Expected state:
(0) Buzzer will NOT turn on when a card is detected
(1) Buzzer will turn on when a card is detected

Please select state...0
Setting buzzer state... Success!
```

This tool is using .NET Core 3. A ready to use, self-contained executable, running on Windows, is available [here](https://github.com/sailro/ACRBuzzerControl/releases/download/1.1/ACRBuzzerControl-selfcontained-winx86.zip).
