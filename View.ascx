<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Dnn.WebAnalytics.View" %>

<div ng-app="DNN_WebAnalytics" ng-cloak>
    <div view></div>
</div>

<script>
    var module_id = <%= ModuleId %>;
    var portal_id = <%= PortalId %>;
    var sf = $.ServicesFramework(module_id);
</script>
