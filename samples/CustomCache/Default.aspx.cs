using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FastReflectionLib;

namespace CustomCache
{
    public class PropertyAccessorCache
    {
        private object m_mutex = new object();
        private Dictionary<Type, Dictionary<string, IPropertyAccessor>> m_cache =
            new Dictionary<Type, Dictionary<string, IPropertyAccessor>>();

        public IPropertyAccessor GetAccessor(Type type, string propertyName)
        {
            IPropertyAccessor accessor;
            Dictionary<string, IPropertyAccessor> typeCache;

            if (this.m_cache.TryGetValue(type, out typeCache))
            {
                if (typeCache.TryGetValue(propertyName, out accessor))
                {
                    return accessor;
                }
            }

            lock (m_mutex)
            {
                if (!this.m_cache.ContainsKey(type))
                {
                    this.m_cache[type] = new Dictionary<string, IPropertyAccessor>();
                }

                var propertyInfo = type.GetProperty(propertyName);
                accessor = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo);
                this.m_cache[type][propertyName] = accessor;

                return accessor;
            }
        }
    }

    public static class FastEvalExtensions
    {
        public static object FastEval(this Control control, object o, string propertyName)
        {
            var cache = (control.Page as PageBase).PropertyAccessorCache;
            return cache.GetAccessor(o.GetType(), propertyName).GetValue(o);
        }

        public static object FastEval(this TemplateControl control, string propertyName)
        {
            return control.FastEval(control.Page.GetDataItem(), propertyName);
        }
    }

    public class PageBase : Page
    {
        private static PropertyAccessorCache s_propertyAccessorCache = new PropertyAccessorCache();

        public virtual PropertyAccessorCache PropertyAccessorCache
        {
            get
            {
                return s_propertyAccessorCache;
            }
        }
    }

    public partial class _Default : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Users = new List<object>();
            for (int i = 0; i < 5; i++)
            {
                this.Users.Add(new
                {
                    UserID = i,
                    UserName = "Jeffrey"
                });
            }

            this.rptUsers.DataSource = this.Users;
            this.rptUsers.DataBind();
        }

        protected List<object> Users { get; private set; }

        private static PropertyAccessorCache s_propertyAccessorCache = new PropertyAccessorCache();

        public override PropertyAccessorCache PropertyAccessorCache
        {
            get
            {
                return s_propertyAccessorCache;
            }
        }
    }
}
