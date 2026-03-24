Imports Microsoft.VisualBasic
'WO#17432 ADD Start
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Public Class Cryptography
    Public Function Encrypt(ByVal strPlainText As String) As String
        Dim strResult As String
        Dim memoryStream As MemoryStream
        Dim enc As UTF8Encoding = New System.Text.UTF8Encoding
        Dim cryptoStream As CryptoStream
        Try
            memoryStream = New MemoryStream()
            cryptoStream = New CryptoStream(memoryStream, GetCryptoTransform("encrypt"), CryptoStreamMode.Write)
            cryptoStream.Write(enc.GetBytes(strPlainText), 0, strPlainText.Length)
            cryptoStream.FlushFinalBlock()
            strResult = Convert.ToBase64String(memoryStream.ToArray())
            memoryStream.Close()
            cryptoStream.Close()
            Return strResult
        Catch ex As Exception
            Throw New Exception("Error in Encrypt." & vbCrLf & ex.Message)
        End Try
    End Function

    Public Function Decrypt(ByVal strPlainText As String) As String
        Dim strResult As String
        Dim cryptoStream As CryptoStream
        Dim cypherTextBytes As Byte()
        Dim memoryStream As MemoryStream
        Dim enc As UTF8Encoding = New System.Text.UTF8Encoding
        Dim intDecryptedByteCount As Integer
        Try
            cypherTextBytes = Convert.FromBase64String(strPlainText)
            memoryStream = New MemoryStream(cypherTextBytes)
            cryptoStream = New CryptoStream(memoryStream, GetCryptoTransform("decrypt"), CryptoStreamMode.Read)
            Dim plainTextBytes(cypherTextBytes.Length) As Byte
            intDecryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length)
            memoryStream.Close()
            cryptoStream.Close()
            strResult = enc.GetString(plainTextBytes, 0, intDecryptedByteCount)
            Return strResult
        Catch ex As Exception
            Throw New Exception("Error in Decrypt." & vbCrLf & ex.Message)
        End Try
    End Function

    Public Function GetCryptoTransform(strAction As String) As ICryptoTransform
        Dim KEY_128 As Byte() = {42, 1, 52, 67, 231, 13, 94, 101, 123, 6, 0, 12, 32, 91, 4, 111, 31, 70, 21, 141, 123, 142, 234, 82, 95, 129, 187, 162, 12, 55, 98, 23}
        Dim IV_128 As Byte() = {234, 12, 52, 44, 214, 222, 200, 109, 2, 98, 45, 76, 88, 53, 23, 77}
        Dim symmetricKey As RijndaelManaged = New RijndaelManaged()
        Try
            symmetricKey.Mode = CipherMode.CBC
            If strAction = "encrypt" Then
                Return symmetricKey.CreateEncryptor(KEY_128, IV_128)
            Else
                Return symmetricKey.CreateDecryptor(KEY_128, IV_128)
            End If
        Catch ex As Exception
            Throw New Exception("Error in GetCryptoTransform." & vbCrLf & ex.Message)
        End Try
    End Function

End Class
