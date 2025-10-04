using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace Utility.Unity.Editor
{
    [InitializeOnLoad]
    public static class RunToolbarButton
    {
        #region FIELDS PRIVATE
        private const string BUTTON_ID = "RunBootstrapScene";
        private const string SCENE_PATH = "Assets/_Project/_SCENES/Build/B00-Bootstrap.unity";
        #endregion

        #region CONSTRUCTOR
        static RunToolbarButton()
        {
            EditorApplication.update += OnUpdate;
        }
        #endregion

        #region METHODS PRIVATE
        private static void OnUpdate()
        {
            if (EditorApplication.isPlaying) return;

            var toolbarType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.Toolbar");
            var toolbars = Resources.FindObjectsOfTypeAll(toolbarType);
            if (toolbars.Length == 0) return;

            var toolbar = toolbars[0];
            var rootField = toolbarType.GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
            if (rootField == null) return;

            var root = rootField.GetValue(toolbar);
            if (root is not VisualElement visualElement) return;
            if (visualElement.Q(BUTTON_ID) != null) return;

            var button = new Button(PlayFromScene)
            {
                name = BUTTON_ID,
                text = "🚧",
                tooltip = "Play from Bootstrap Scene",
                style =
                {
                    overflow = Overflow.Visible,
                    minWidth = 32,
                    height = 20,
                    paddingLeft = 4,
                    paddingRight = 4,
                    paddingTop = 2,
                    paddingBottom = 2,
                    marginTop = 0,
                    marginRight = 1,
                    fontSize = 14,
                }
            };

            visualElement.Q("PlayMode")?.Insert(0, button);
        }

        private static void PlayFromScene()
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.ExitPlaymode();
                return;
            }

            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            EditorSceneManager.OpenScene(SCENE_PATH);
            EditorApplication.EnterPlaymode();
        }
        #endregion
    }
}
