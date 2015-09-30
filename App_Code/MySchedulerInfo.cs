
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Web.UI;

public class MySchedulerInfo : SchedulerInfo
{
   
    public DateTime VisibleRangeStart { get; set; }
    public DateTime VisibleRangeEnd { get; set; }
    public MySchedulerInfo(ISchedulerInfo baseInfo, DateTime visibleRangeStart, DateTime visibleRangeEnd)
        : base(baseInfo)
    {      
        VisibleRangeStart = visibleRangeStart;
        VisibleRangeEnd = visibleRangeEnd;
    }
    public MySchedulerInfo()
    {

    }
}

