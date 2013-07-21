using System;
using System.IO;
using System.Reflection;

namespace PulseMates.Models
{
    public static class ApplicationInfo
    {
        #region Constructor Members

        static ApplicationInfo()
        {
            var asm = Assembly.GetExecutingAssembly();

            var titleAttr = GetAttribute<AssemblyTitleAttribute>(asm);
            Title = titleAttr != null ? titleAttr.Title : Path.GetFileNameWithoutExtension(asm.CodeBase);

            var copyAttr = GetAttribute<AssemblyCopyrightAttribute>(asm);
            Copyright = copyAttr != null ? copyAttr.Copyright : "ctrl+c";

            var companyAttr = GetAttribute<AssemblyCompanyAttribute>(asm);
            Company = companyAttr != null ? companyAttr.Company : "Zoomcube LLT";

            var descAttr = GetAttribute<AssemblyDescriptionAttribute>(asm);
            Description = descAttr != null ? descAttr.Description : "muuha";

            var versionAttr = GetAttribute<AssemblyVersionAttribute>(asm);
            Version = versionAttr != null ? versionAttr.Version : "0.0.0.1b";
        }

        #endregion

        #region Property Members

        public static string Title { get; private set; }
        public static string Copyright { get; private set; }
        public static string Company { get; private set; }
        public static string Description { get; private set; }
        public static string Version { get; private set; }

        #endregion

        #region Method Members

        private static T GetAttribute<T>(Assembly asm) where T : Attribute
        {
            var attributes = asm.GetCustomAttributes(typeof(T), false);
            
            if (attributes.Length > 0)
                return (T)attributes[0];

            return null;
        }

        #endregion
    }
}