
namespace ProERP.Web.Framework.UI
{
    class PageHeadBuilder : IPageHeadBuilder
    {
        private string _activeAdminMenuSystemName;
        /// <summary>
        /// Specify system name of admin menu item that should be selected (expanded)
        /// </summary>
        /// <param name="systemName">System name</param>
        public virtual void SetActiveMenuItemSystemName(string systemName)
        {
            _activeAdminMenuSystemName = systemName;
        }
        /// <summary>
        /// Get system name of admin menu item that should be selected (expanded)
        /// </summary>
        /// <returns>System name</returns>
        public virtual string GetActiveMenuItemSystemName()
        {
            return _activeAdminMenuSystemName;
        }
    }
}
