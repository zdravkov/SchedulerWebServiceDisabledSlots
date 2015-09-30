<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RadSchedulerWebForm.aspx.cs"
    Inherits="RadSchedulerWebForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />
    <style type="text/css">
        .Disabled {
            background: silver !important;
            cursor: not-allowed;
        }

            .Disabled.rsAptCreate {
                background: silver !important;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <%--Needed for JavaScript IntelliSense in VS2010--%>
                <%--For VS2008 replace RadScriptManager with ScriptManager--%>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
            </Scripts>
        </telerik:RadScriptManager>
        <script type="text/javascript">
            function OnClientAppointmentsPopulating(sender, eventArgs) {
                eventArgs.get_schedulerInfo().VisibleRangeStart = sender.get_activeModel().get_visibleRangeStart();
                eventArgs.get_schedulerInfo().VisibleRangeEnd = sender.get_activeModel().get_visibleRangeEnd();
            }
            function OnClientAppointmentsPopulated(sender, args) {
                var appCount = sender.get_appointments().get_count();
            }
            var someDates;
            function requestSuccess(sender, args) {
                someDates = args.get_result().SomeDates;

            }

            function OnClientDataBound(sender, args) {
                var $ = $telerik.$;

                $(".rsAllDayTable:visible td, .rsContentTable:visible td").removeClass("Disabled");

                for (var i = 0; i < someDates.length; i++) {
                    var someDate = someDates[i];
                    console.log(someDate);
                    $(".rsAllDayTable:visible td, .rsContentTable:visible td", sender.get_element()).each(function(i) {
                        var currentTimeSlot = sender.get_activeModel().getTimeSlotFromDomElement($(this).get(0));

                        if (currentTimeSlot.get_startTime() < someDate && currentTimeSlot.get_endTime() > someDate)
                            $(this).addClass("Disabled");
                    });
                }
            }

            function OnClientAppointmentInserting(sender, args) {
                var slotElement = $telerik.$(args.get_targetSlot().get_domElement());
                if (slotElement.is(".Disabled") || slotElement.parent().is(".Disabled")) {
                    args.set_cancel(true);
                }
            }

            function OnClientAppointmentMoveEnd(sender, args) {
                var slotElement = $telerik.$(args.get_targetSlot().get_domElement());
                if (slotElement.is(".Disabled") || slotElement.parent().is(".Disabled")) {
                    args.set_cancel(true);
                }
            }

        </script>
        <telerik:RadScheduler ID="RadScheduler1" runat="server"
            OnClientDataBound="OnClientDataBound"
            OnClientAppointmentMoveEnd="OnClientAppointmentMoveEnd"
            OnClientAppointmentsPopulating="OnClientAppointmentsPopulating" OnClientRequestSuccess="requestSuccess"
            AppointmentStyleMode="Default" SelectedView="WeekView"
            OnClientAppointmentsPopulated="OnClientAppointmentsPopulated"
            OnClientAppointmentInserting="OnClientAppointmentInserting"
            Width="600px">
            <WebServiceSettings Path="SchedulerWebService.asmx" ResourcePopulationMode="ServerSide" />
            <ResourceStyles>
                <telerik:ResourceStyleMapping Type="Teacher" Key="1" BackColor="Orange" />
                <telerik:ResourceStyleMapping Type="Teacher" Key="2" BackColor="Aqua" />
            </ResourceStyles>
        </telerik:RadScheduler>
    </form>
</body>
</html>
