<%@ Control Language="C#" AutoEventWireup="true" Inherits="Dnn.WebAnalytics.Map" CodeBehind="Map.ascx.cs" %>

<div ng-app="DNN_WebAnalytics" ng-cloak>
    <div map></div>
</div>

<script>
    var module_id = <%= ModuleId %>;
    var portal_id = <%= PortalId %>;
    var sf = $.ServicesFramework(module_id);
    var apiUrlBase = "<%= ApiUrlBase %>";
</script>