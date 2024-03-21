# MiAPI CSharp Translator

This is a **given-as-is** C# Translator (Example Project) for [MiTAC Corporation](https://www.mitacmct.com/)'s [MiAPI.dll](https://download.mitacmct.com/Files/download/CBU/MiAPI_v3.1.zip).

This repo's purpose is providing a good base for C# development with any Systems involving the MiAPI.
The main value in this are my (own) C#-style translations of the inconcise official documentation and function comments.

I wanted to use it to create a modern GUI for managing a semi-embedded system when the project ultimately got scrapped.
During the development of this interface I was running into several issues with the MiAPI, mostly stemming from (from what I could tell) driver and/or hardware issues, as the system even crashed shortly after using the MiAPI included example-programs.
However, I was able to successfully interface with the `MiAPI.Start()` function multiple times using this code.

I am aware that there are better ways for C# intercompatibility with C++/C DLLs. I have stumbled upon some links (see in [MiAPI.cs](./MiAPI.cs)) on how to use them but ultimately didn't get around to applying them before the project's end.
I had also found a (seemingly) revised version of the official documentation at [icp-deutschland.de](https://files.icp-deutschland.de/produkte/KC002088/web/MiAPI-DLL-Use-manual-v3.1a.pdf)

### Things I know of that **DON'T WORK** are
 - all functions that accept or return Strings

## Key reqiurements

...are:
 - the fact that you are running as Administrator (best to also tell the VS solution that fact!)
 - You are not trying to mix-and-match x86 and x64 binariers. See the Code comments for further information.

---

## Legal
**!!For security reasons I advise not to use the binaries or DLLs included in this repo!!**
If you (or MiTAC mct) take issue with the fact that I have included the (to this date) openly available binaries, please open an issue on this repo or write me via GitHub or mail and I will gladly remove them.
THIS REPO IS PROVIDED AS IS AND SHOULD ONLY SERVE AS INSPIRATION. I DO NOT TAKE RESPONSIBILITY FOR ANY DAMAGES OR INJURIES.
