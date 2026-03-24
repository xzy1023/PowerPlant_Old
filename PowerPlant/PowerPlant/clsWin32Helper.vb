Public NotInheritable Class Win32Helper
    <System.Runtime.InteropServices.DllImport("user32.dll", _
    EntryPoint:="SetForegroundWindow", _
    CallingConvention:=Runtime.InteropServices.CallingConvention.StdCall, _
    CharSet:=Runtime.InteropServices.CharSet.Unicode, SetLastError:=True)> _
    Public Shared Function _
         SetForegroundWindow(ByVal handle As IntPtr) As Boolean
        ' Leave function empty
    End Function

    <System.Runtime.InteropServices.DllImport("user32.dll", _
    EntryPoint:="ShowWindow", _
    CallingConvention:=Runtime.InteropServices.CallingConvention.StdCall, _
    CharSet:=Runtime.InteropServices.CharSet.Unicode, SetLastError:=True)> _
    Public Shared Function ShowWindow(ByVal handle As IntPtr, _
                                 ByVal nCmd As Int32) As Boolean
        ' Leave function empty 
    End Function

End Class ' End Win32Helper 
