
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Data;
using DotNetNuke.Entities.Users;

namespace Dnn.WebAnalytics
{
    public class VisitorInfoRepo
    {
        public VisitorInfo CreateItem(VisitorInfo i)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<VisitorInfo>();
                rep.Insert(i);
            }

            return i;
        }

        public void DeleteItem(int itemId, int portalId)
        {
            var i = GetItem(itemId, portalId);
            DeleteItem(i);
        }

        public void DeleteItem(VisitorInfo i)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<VisitorInfo>();
                rep.Delete(i);
            }
        }

        public void DeleteItems(IEnumerable<VisitorInfo> visitors)
        {
            if (visitors != null && visitors.Any())
            {
                foreach (var visitor in visitors)
                {
                    DeleteItem(visitor);
                }
            }
        }

        public IEnumerable<VisitorInfo> GetItems(int portalId)
        {
            IEnumerable<VisitorInfo> i;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<VisitorInfo>();
                i = rep.Get(portalId);
            }

            if (i != null && i.Any())
            {
                foreach (var visitor in i)
                {
                    visitor.User = GetUser(visitor);
                }
            }

            return i;
        }

        public IEnumerable<VisitorInfo> GetItemsAll()
        {
            IEnumerable<VisitorInfo> i;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<VisitorInfo>();
                i = rep.Get();
            }

            if (i != null && i.Any())
            {
                foreach(var visitor in i)
                {
                    visitor.User = GetUser(visitor);
                }
            }

            return i;
        }

        public VisitorInfo GetItem(int itemId, int portalId)
        {
            VisitorInfo i = null;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<VisitorInfo>();
                i = rep.GetById(itemId, portalId);
            }

            i.User = GetUser(i);

            return i;
        }

        public VisitorInfo GetItemById(int itemId)
        {
            VisitorInfo i = null;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<VisitorInfo>();
                i = rep.GetById(itemId);
            }

            i.User = GetUser(i);

            return i;
        }

        public void UpdateItem(VisitorInfo i)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<VisitorInfo>();
                rep.Update(i);
            }
        }

        #region Private Helper Classes
        private UserInfo GetUser(VisitorInfo visitor)
        {
            if (visitor == null) return null;
            if (!visitor.user_id.HasValue) return null;

            var user = UserController.GetUserById(visitor.portal_id, visitor.user_id.Value);
            if (user != null)
            {
                return user;
            }

            return null;
        }
        #endregion
    }
}