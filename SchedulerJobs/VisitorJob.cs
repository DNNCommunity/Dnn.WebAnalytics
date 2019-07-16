using DotNetNuke.Instrumentation;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Scheduling;
using System;

namespace Dnn.WebAnalytics
{
    public class VisitorJob : SchedulerClient
	{
        private static readonly ILog Logger = LoggerSource.Instance.GetLogger(typeof(VisitorJob));

        public VisitorJob(ScheduleHistoryItem objScheduleHistoryItem) : base()
		{
			ScheduleHistoryItem = objScheduleHistoryItem;
		}

		public override void DoWork()
		{
			try
            {
				string strMessage = Processing();
				ScheduleHistoryItem.Succeeded = true;
                ScheduleHistoryItem.AddLogNote("Successful. " + strMessage);
			}
            catch (Exception exc)
            {
                Logger.Error(exc.Message, exc);
				ScheduleHistoryItem.Succeeded = false;
				ScheduleHistoryItem.AddLogNote("Failed. " + exc.Message);
				Errored(ref exc);
                Exceptions.LogException(exc);
            }
        }

		public string Processing()
		{
            VisitController visitController = new VisitController();
            visitController.PurgeVisits();

            return string.Empty;
		}

	}

}