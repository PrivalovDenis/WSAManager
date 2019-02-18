using System.Reflection;

namespace WSAManager.Web.References
{
    public static class ReferencedAssemblies
    {
        public static Assembly Services
        {
            get { return Assembly.Load("WSAManager.Services"); }
        }

        public static Assembly Repositories
        {
            get { return Assembly.Load("WSAManager.Data"); }
        }

        public static Assembly Dto
        {
            get
            {
                return Assembly.Load("WSAManager.Dto");
            }
        }

        public static Assembly Domain
        {
            get
            {
                return Assembly.Load("WSAManager.Core");
            }
        }
    }
}
