Imports System.Net.Sockets
Imports System.Text

Class PPTCPClient

    Shared Sub Main()
        Dim strCmd As String = String.Empty
        Dim tcpClient As New System.Net.Sockets.TcpClient()

        strCmd = Console.ReadLine()

        tcpClient.Connect("127.0.0.1", 8000)
        Dim networkStream As NetworkStream = tcpClient.GetStream()
        If networkStream.CanWrite And networkStream.CanRead Then
            ' Do a simple write.
            'Dim sendBytes As [Byte]() = Encoding.ASCII.GetBytes("logon_noplc 1111111")
            Dim sendBytes As [Byte]() = Encoding.ASCII.GetBytes(strCmd)
            networkStream.Write(sendBytes, 0, sendBytes.Length)
            ' Read the NetworkStream into a byte buffer.
            Dim bytes(tcpClient.ReceiveBufferSize) As Byte
            networkStream.Read(bytes, 0, CInt(tcpClient.ReceiveBufferSize))
            ' Output the data received from the host to the console.
            Dim returndata As String = Encoding.ASCII.GetString(bytes)
            Console.WriteLine(("Host returned: " + returndata))
        Else
            If Not networkStream.CanRead Then
                Console.WriteLine("cannot not write data to this stream")
                tcpClient.Close()
            Else
                If Not networkStream.CanWrite Then
                    Console.WriteLine("cannot read data from this stream")
                    tcpClient.Close()
                End If
            End If
        End If
        ' pause so user can view the console output
        Console.ReadLine()
    End Sub
End Class
