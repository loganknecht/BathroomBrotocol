﻿using UnityEngine;
using UnityEditor;

namespace FullInspector.Internal {
    public static class fiEditorWindowUtility {
        public const float DefaultWindowWidth = 600;
        public const float DefaultWindowHeight = 300;

        public static T Show<T>(string title) where T : EditorWindow {
            return Show<T>(title, DefaultWindowWidth, DefaultWindowHeight);
        }

        public static T Show<T>(string title, float windowWidth, float windowHeight) where T : EditorWindow {
            var window = EditorWindow.GetWindow<T>();
            InitializeWindow(window, title, windowWidth, windowHeight);
            return window;
        }

        public static T ShowUtility<T>(string title) where T : EditorWindow {
            var window = EditorWindow.GetWindow<T>(/*utility:*/true);
            InitializeWindow(window, title, DefaultWindowWidth, DefaultWindowHeight);
            return window;
        }

        public static T ShowUtility<T>(string title, float windowWidth, float windowHeight) where T : EditorWindow {
            var window = EditorWindow.GetWindow<T>(/*utility:*/true);
            InitializeWindow(window, title, windowWidth, windowHeight);
            return window;
        }

        public static T ShowFixedSizeUtility<T>(string title, float windowWidth, float windowHeight) where T : EditorWindow {
            var window = EditorWindow.GetWindow<T>(/*utility:*/true);
            InitializeWindow(window, title, windowWidth, windowHeight);
            window.minSize = new Vector2(windowWidth, windowHeight);
            window.maxSize = new Vector2(windowWidth, windowHeight);
            return window;
        }

        private static void InitializeWindow(EditorWindow window, string title, float windowWidth, float windowHeight) {
            window.title = title;
            float x = (Screen.currentResolution.width - windowWidth) / 2f;
            float y = (Screen.currentResolution.height - windowHeight) / 2f;
            window.position = new Rect(x, y, windowWidth, windowHeight);
        }
    }
}