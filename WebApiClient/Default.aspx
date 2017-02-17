<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="WebApiClient._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>ASP.NET</h1>
        <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS and JavaScript.</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Request</h2>
            <p>
                <asp:Button ID="btnGet" runat="server" Text="Get" OnClick="btnGet_Click"/>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Result</h2>
            <p>
               <asp:Label ID="lblResult" runat="server"></asp:Label>
            </p>
        </div>
    </div>

</asp:Content>
