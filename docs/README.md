SwordStudio.NET
======

***SwordStudio.NET*** is a full-featured game MOD development platform dedicated to making mods for the classic Chinese RPG game known as *PAL*.


LICENSE
=======

SwordStudio.NET was originally created by [liuzhier](https://github.com/liuzhier/) from 2024-02-18 02:23 PM.
Please see [AUTHORS](AUTHORS) for full author list.

SwordStudio.NET is distributed under the terms of **GNU General Public License, version 3** as published by the [Free Software Foundation](http://www.fsf.org/). See [LICENSE](LICENSE) for details.

Many of the ideas of this program are based on documents from [PAL Research Project](https://github.com/palxex/palresearch).

SwordStudio.NET was written using "C #. NET8.0", And it is a reference to the open source program ***SDLPAL*** to achieve.
This program does **NOT** include any code or data files of the original game, which are proprietary and copyrighted by [SoftStar](http://www.softstar.com.tw/) Inc.


Building the game tool
=================

Currently, SwordStudio.NET only supports Windows platforms.

Windows
-------

### Visual Studio

To build SwordStudio.NET as a Windows **desktop** app, you can use ***Microsoft Visual Studio 2022*** to open the solution file *`SwordStudio.NET.sln`*.


Running the game tool
================

The data files required for running the game are not included with the source package due to copyright issues.  You need to obtain them from a licensed copy of the game before you can run the game.

To run the game, copy all the files in the original game CD to a directory, then copy the built SwordStudio.NET executable to the same directory, and run the executable.


Configuring the game tool
====================

Manually
--------
To set the configuration options manually, create a file named as *`SwordStudio.NET.ini`* (make sure to use lower-case file name in case-sensitive filesystems) in the game directory created by the above step. Please refer to the [example file](SwordStudio.NET.ini.example) for format specfication.


Reporting issues
================

If you find any issues of SwordStudio.NET, please feel free to report them to the development team through GitHub's issue tracking system using either English or Chinese.


Contributing to the game
========================

Any original code & documentation contributions are welcomed as long as the contributed code & documentation is licensed under GPL. You can use GitHub's pull request system to submit your changes to the main repository here. 
