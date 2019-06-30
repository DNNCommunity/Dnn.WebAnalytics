using System;

namespace Dnn.WebAnalytics
{
    public class VisitorJob : DotNetNuke.Services.Scheduling.SchedulerClient
	{
        public VisitorJob(DotNetNuke.Services.Scheduling.ScheduleHistoryItem objScheduleHistoryItem) : base()
		{
			ScheduleHistoryItem = objScheduleHistoryItem;
		}

		public override void DoWork()
		{
			try {
				string strMessage = Processing();
				ScheduleHistoryItem.Succeeded = true;
                ScheduleHistoryItem.AddLogNote("Successful. " + strMessage);
			} catch (Exception exc) {
				ScheduleHistoryItem.Succeeded = false;
				ScheduleHistoryItem.AddLogNote("Failed. " + exc.Message);
				Errored(ref exc);
                DotNetNuke.Services.Exceptions.Exceptions.LogException(exc);
            }
        }

		public string Processing()
		{
			string Message = "";

            VisitController visitController = new VisitController();
            //visitController.WriteVisits();
            visitController.PurgeVisits();

            return Message;
		}

	}

}