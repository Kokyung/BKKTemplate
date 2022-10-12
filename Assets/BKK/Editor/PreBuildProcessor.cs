using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class PreBuildProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildReport report)
    {
        // 빌드 전에 처리하고 싶은 코드 작성
    }
}
