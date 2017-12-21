using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace etp
{
    class ETPDatabase
    {
        public static string DatabaseFilePath
        {
            get
            {
                var filename = "ETPDatabase.db3";

#if __ANDROID__
                string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
#else
#if __IOS__
                // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
                // (they don't want non-user-generated data in Documents)
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
                string libraryPath = Path.Combine(documentsPath, "..", "Library");

#endif
#endif
                var path = Path.Combine(libraryPath, filename);

                return path;
            }

        }


    }
}
