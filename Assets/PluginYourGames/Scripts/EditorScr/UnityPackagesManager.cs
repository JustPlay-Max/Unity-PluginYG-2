#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace YG.EditorScr
{
    public class UnityPackagesManager
    {
        public static bool IsPackageImported(string packageName)
        {
            string packagePath = "Packages/" + packageName;
            PackageInfo packageInfo = PackageInfo.FindForAssetPath(packagePath);
            return packageInfo != null;
        }

        public static bool ImportPackage(string packageName, bool dialog = false)
        {
            if (!IsPackageImported(packageName))
            {
                if (dialog)
                {
                    if (!DialogImportPackage(packageName))
                        return false;
                }

                Client.Add(packageName);
            }
            return false;
        }

        public static bool DeletePackage(string packageName, bool dialog = false)
        {
            if (!IsPackageImported(packageName))
            {
                if (dialog)
                {
                    if (!DialogDeletePackage(packageName))
                        return false;
                }

                Client.Remove(packageName);
            }
            return false;
        }


        private static bool DialogImportPackage(string packageName)
        {
            int option = EditorUtility.DisplayDialogComplex("Download package", $"To continue, you need to install the package: {packageName}\nInstall it?", "Yes", "No", "");

            if (option == 0)
                return true;
            else
                return false;
        }

        private static bool DialogDeletePackage(string packageName)
        {
            int option = EditorUtility.DisplayDialogComplex("Delete package", $"To continue, you need to delete the package: {packageName}\nDelete it?", "Yes", "No", "");

            if (option == 0)
                return true;
            else
                return false;
        }
    }
}
#endif