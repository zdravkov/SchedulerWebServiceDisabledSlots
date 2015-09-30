using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using Telerik.Web.UI;
using System.Data.Common;

public class SchedulerResult : SchedulerOperationResult<AppointmentData>
{
    private List<DateTime> _someDates;
    public List<DateTime> SomeDates
    {
        get { return _someDates; }
        set { _someDates = value; }
    }
}
    /// <summary>
    /// Summary description for SchedulerWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class SchedulerWebService : System.Web.Services.WebService
    {

        private WebServiceAppointmentController _controller;
        //private MySchedulerInfo localSchedulerInfo;
        private MyDbSchedulerProvider _provider;
        private MyDbSchedulerProvider Provider
        {
            get
            {
                if (_provider == null)
                {
                    var connString = ConfigurationManager.ConnectionStrings["TelerikVSXConnectionString"].ConnectionString;
                    var factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
                    _provider = new MyDbSchedulerProvider() { ConnectionString = connString, DbFactory = factory, PersistChanges = true };
                }
                return _provider;
            }
        }

        /// <summary>
        /// The WebServiceAppointmentController class is used as a facade to the SchedulerProvider.
        /// </summary>
        private WebServiceAppointmentController Controller
        {
            get
            {
                if (_controller == null)
                {
                    _controller = new WebServiceAppointmentController(Provider);
                }

                return _controller;
            }
        }
         [WebMethod]
        public SchedulerResult GetAppointments(MySchedulerInfo schedulerInfo)
        {
            SchedulerResult result = new SchedulerResult();
            result.Appointments = Controller.GetAppointments<AppointmentData>(schedulerInfo);
            result.SomeDates = Provider.GetTimeSlots();
            return result;
        }

         [WebMethod]
         public SchedulerResult InsertAppointment(MySchedulerInfo schedulerInfo, AppointmentData appointmentData)
         {
             SchedulerResult result = new SchedulerResult();
             result.Appointments = Controller.InsertAppointment(schedulerInfo, appointmentData);
             result.SomeDates = Provider.GetTimeSlots();
             return result;
         }


        public interface ISchedulerOperationResult<T> where T : IAppointmentData
        {
            IEnumerable<T> Appointments { get; set; }
        }

        //[WebMethod]
        //public IEnumerable<AppointmentData> GetAppointments(MySchedulerInfo schedulerInfo)
        //{
        //    return Controller.GetAppointments(schedulerInfo);
        //}

        //[WebMethod]
        //public IEnumerable<AppointmentData> InsertAppointment(MySchedulerInfo schedulerInfo, AppointmentData appointmentData)
        //{
        //    return Controller.InsertAppointment(schedulerInfo, appointmentData);
        //}

		[WebMethod]
		public IEnumerable<AppointmentData> UpdateAppointment(MySchedulerInfo schedulerInfo, AppointmentData appointmentData)
		{
			return Controller.UpdateAppointment(schedulerInfo, appointmentData);
		}

		[WebMethod]
		public IEnumerable<AppointmentData> CreateRecurrenceException(MySchedulerInfo schedulerInfo, AppointmentData recurrenceExceptionData)
		{
			return Controller.CreateRecurrenceException(schedulerInfo, recurrenceExceptionData);
		}

		[WebMethod]
		public IEnumerable<AppointmentData> RemoveRecurrenceExceptions(MySchedulerInfo schedulerInfo, AppointmentData masterAppointmentData)
		{
			return Controller.RemoveRecurrenceExceptions(schedulerInfo, masterAppointmentData);
		}

		[WebMethod]
		public IEnumerable<AppointmentData> DeleteAppointment(MySchedulerInfo schedulerInfo, AppointmentData appointmentData, bool deleteSeries)
		{
			return Controller.DeleteAppointment(schedulerInfo, appointmentData, deleteSeries);
		}

		[WebMethod]
		public IEnumerable<ResourceData> GetResources(MySchedulerInfo schedulerInfo)
		{
			return Controller.GetResources(schedulerInfo);
		}
    }

