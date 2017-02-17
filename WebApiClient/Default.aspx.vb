Imports System.IO
Imports System.Net
Imports System.Runtime.Serialization.Json
Imports System.Security.Cryptography.X509Certificates

Public Class _Default
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnGet_Click(sender As Object, e As EventArgs)
        Dim cert = GetClientCertificate()
        lblResult.Text = GetUsingHttpWebRequest(My.Settings.WebApiServerURISecured, cert, "Alice", "pwd1234")

    End Sub

    Private Function GetClientCertificate() As X509Certificate2
        ' Get certificates from user store
        Dim store As New X509Store(StoreName.My, StoreLocation.CurrentUser)
        Try
            store.Open(OpenFlags.ReadOnly)
            Dim collection As X509Certificate2Collection = store.Certificates
            Dim fcollection As X509Certificate2Collection = collection.Find(X509FindType.FindBySubjectName, "localhostClient", True)
            'Dim scollection As X509Certificate2Collection = X509Certificate2UI.SelectFromCollection(fcollection, "Client Certificate Select", "Select a certificate", X509SelectionFlag.SingleSelection)
            Dim cert As X509Certificate2 = fcollection.Item(0)
            Return cert
        Finally
            store.Close()
        End Try
    End Function

    Private Function GetUsingHttpWebRequest(ByVal uri As String, ByVal certificate As X509Certificate2, ByVal userName As String, ByVal pwd As String) As String
        Try
            Dim request As HttpWebRequest = CType(WebRequest.Create(uri), HttpWebRequest)
            request.ClientCertificates.Add(certificate)
            request.ContentType = "application/json"
            request.Headers.Add(HttpRequestHeader.Authorization, $"{userName}:{pwd}")
            request.Method = "GET"

            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            Dim receiveStream As Stream = response.GetResponseStream()
            Dim readStream As New StreamReader(receiveStream, Encoding.UTF8)
            Dim json As String = readStream.ReadToEnd()

            Dim serializer As New DataContractJsonSerializer(GetType(List(Of Model)), New DataContractJsonSerializerSettings With {.UseSimpleDictionaryFormat = True})
            Dim memStream As New MemoryStream(Encoding.Unicode.GetBytes(json))
            Dim models As List(Of Model) = CType(serializer.ReadObject(memStream), List(Of Model))

            response.Close()
            receiveStream.Close()
            readStream.Close()
            memStream.Close()

            Return models.First.Prop1
        Catch ex As Exception
            Throw New Exception("Error while executing the request", ex)
        End Try
    End Function

    Private Function GetUsingWebClient(ByVal uri As String, ByVal certificate As X509Certificate2, ByVal userName As String, ByVal pwd As String) As String
        Dim client As New WebClient()
        'client.Credentials = New NetworkCredential(userName, pwd)
        client.Headers.Add(HttpRequestHeader.Authorization, $"{userName}:{pwd}")
        Dim reponseContent As String = client.DownloadString(uri)
        Return reponseContent
    End Function

End Class