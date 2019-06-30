<%@ Control Language="C#" AutoEventWireup="true" Inherits="Dnn.WebAnalytics.MapSettings" CodeBehind="Settings.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<div class="dnnForm dnnSettings dnnClear" id="dnnSettings">
    <fieldset>
        <div class="dnnFormItem">
            <dnn:label id="plGoogle" runat="server" text="Google API Key" helptext="Your Google API Key" controlname="txtGoogle" />
            <asp:textbox id="txtGoogle" runat="server" maxlength="50" />
        </div>       
   </fieldset>
</div>


