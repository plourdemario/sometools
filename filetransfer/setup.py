import sys
from cx_Freeze import setup, Executable

# Dependencies are automatically detected, but it might need fine tuning.
includes = ["csv", "tarfile", "Tkinter", "tkMessageBox", "copy", "_winreg", "win32file", "win32con", "win32api", "subprocess", "time", "logging"]
build_exe_options = {"packages": ["os"], "includes": includes}

# GUI applications require a different base on Windows (the default is for a
# console application).
base = None
if sys.platform == "win32":
    base = "Win32GUI"

setup(  name = "guifoo",
        version = "0.1",
        description = "My GUI application!",
        options = {"build_exe": build_exe_options},
        executables = [Executable("datatransfer11 - No size.py", base=base)])
