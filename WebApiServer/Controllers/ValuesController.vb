Imports System.Security.Cryptography.X509Certificates
Imports System.Web.Http

Public Class ValuesController
    Inherits ApiController

    ' GET api/values
    ' <Authorize(Users:="Alice,Bob")>
    <RequireHttps()>
    Public Function GetValues() As IHttpActionResult
        Dim cert As X509Certificate2 = RequestContext.ClientCertificate
        If cert Is Nothing Then
            Dim authenticationHeaderValue As New Net.Http.Headers.AuthenticationHeaderValue("ClientCert")
            Return Unauthorized(authenticationHeaderValue)
        End If
        'Dim jsonResponseContent = New String("{""Model"": {""Prop1"": """"}")
        'Dim jsonResponsePath = HttpContext.Current.Server.MapPath("~/App_Data/Response.json")
        'Dim jsonResponseContent = IO.File.ReadAllText(jsonResponsePath)
        Dim models As New List(Of Model) From {
            New Model With {.Prop1 = cert.Issuer},
            New Model With {.Prop1 = cert.Subject}
        }
        Return Ok(models)
    End Function

    ' GET api/values/5
    Public Function GetValue(ByVal id As Integer) As String
        Return "value"
    End Function

    ' POST api/values
    Public Sub PostValue(<FromBody()> ByVal value As String)

    End Sub

    ' PUT api/values/5
    Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

    End Sub

    ' DELETE api/values/5
    Public Sub DeleteValue(ByVal id As Integer)

    End Sub
End Class
