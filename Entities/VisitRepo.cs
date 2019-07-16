
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using DotNetNuke.Entities.Tabs;

namespace Dnn.WebAnalytics
{
    public class VisitInfoRepo
    {
        private VisitorInfoRepo visitorRepo = new VisitorInfoRepo();
        
        public VisitInfo CreateItem(VisitInfo i)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<VisitInfo>();
                rep.Insert(i);
            }

            return i;
        }

        public void DeleteItem(int itemId, int tabId)
        {
            var i = GetItem(itemId, tabId);
            DeleteItem(i);
        }

        public void DeleteItem(VisitInfo i)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<VisitInfo>();
                rep.Delete(i);
            }
        }

        public void DeleteItems(IEnumerable<VisitInfo> visits)
        {
            if (visits != null && visits.Any())
            {
                foreach(var visit in visits)
                {
                    DeleteItem(visit);
                }
            }
        }

        public IEnumerable<VisitInfo> GetItems(int tabId)
        {
            IEnumerable<VisitInfo> i;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<VisitInfo>();
                i = rep.Get(tabId);
            }

            if (i != null && i.Any())
            {
                foreach (var visit in i)
                {
                    visit.Visitor = GetVisitor(visit);
                    visit.Tab = GetTab(visit);
                    visit.PortalId = GetPortalId(visit);
                }
            }

            return i;
        }

        public IEnumerable<VisitInfo> GetItemsAll ()
        {
            IEnumerable<VisitInfo> i;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<VisitInfo>();
                i = rep.Get();
            }

            if (i != null && i.Any())
            {
                foreach(var visit in i)
                {
                    visit.Visitor = GetVisitor(visit);
                    visit.Tab = GetTab(visit);
                    visit.PortalId = GetPortalId(visit);
                }
            }

            return i;
        }

        public VisitInfo GetItem(int itemId, int tabId)
        {
            VisitInfo i = null;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<VisitInfo>();
                i = rep.GetById(itemId, tabId);
            }

            i.Visitor = GetVisitor(i);
            i.Tab = GetTab(i);
            i.PortalId = GetPortalId(i);

            return i;
        }

        public VisitInfo GetItemById(int itemId)
        {
            VisitInfo i = null;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<VisitInfo>();
                i = rep.GetById(itemId);
            }

            i.Visitor = GetVisitor(i);
            i.Tab = GetTab(i);
            i.PortalId = GetPortalId(i);

            return i;
        }

        public IEnumerable<VisitInfo> GetItemsByPortalId(int portalId)
        {
            var items = GetItemsAll().Where(i => i.PortalId == portalId);
            return items;
        }

        public void UpdateItem(VisitInfo i)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<VisitInfo>();
                rep.Update(i);
            }
        }

        #region Private Helper Methods
        private VisitorInfo GetVisitor(VisitInfo visit)
        {
            if (visit == null) return null;

            if (visit.visitor_id > -1)
            {
                var visitor = visitorRepo.GetItemById(visit.visitor_id);
                if (visitor != null)
                {
                    return visitor;
                }
            }

            return null;
        }

        private TabInfo GetTab(VisitInfo visit)
        {
            if (visit == null) return null;
            if (visit.Visitor == null) return null;

            if (visit.tab_id > -1)
            {
                var tab = TabController.Instance.GetTab(visit.tab_id, visit.Visitor.portal_id, false);
                if (tab != null)
                {
                    return tab;
                }
            }

            return null;
        }

        private int GetPortalId(VisitInfo visit)
        {
            if (visit == null) return Null.NullInteger;
            if (visit.Tab == null) return Null.NullInteger;

            return visit.Tab.PortalID;
        }
        #endregion
    }
}