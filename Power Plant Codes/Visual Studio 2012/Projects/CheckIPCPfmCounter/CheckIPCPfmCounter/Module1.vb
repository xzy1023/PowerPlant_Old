Module Module1

    Sub Main()
        Dim proclist() As Process
        Dim strProcessName As String = "Notepad.exe"

        Using da As New dsComputerConfigTableAdapters.tblComputerConfigTableAdapter
            Using dt As New dsComputerConfig.tblComputerConfigDataTable
                da.Fill(dt)
                For Each dr As dsComputerConfig.tblComputerConfigRow In dt.Rows
                    Try
                        'If dr.ComputerName <> "MPFWIPC59" And dr.ComputerName <> "MPFWIPC64" Then
                        If My.Computer.Network.Ping(dr.ComputerName, 2000) Then
                            proclist = Process.GetProcessesByName(strProcessName, dr.ComputerName)
                            Console.WriteLine(dr.ComputerName & " - OK")
                        Else
                            Console.WriteLine(dr.ComputerName & "- Cannot ping")
                        End If
                        'End If
                    Catch ex As Net.NetworkInformation.PingException
                        Console.WriteLine(dr.ComputerName & " - " & ex.InnerException.Message)
                    Catch ex As Exception
                        Console.WriteLine(dr.ComputerName & " - " & ex.InnerException.Message)
                    End Try
                Next
            End Using
        End Using
        Console.WriteLine("It is done.")
        Console.Read()
    End Sub

End Module
