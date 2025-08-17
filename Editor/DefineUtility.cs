/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;

#if UNITY_2021_2_OR_NEWER
using UnityEditor.Build; 
#endif

namespace OmiLAXR.Editor
{
    /// <summary>
    /// Utility class for managing Unity scripting define symbols across multiple build targets.
    /// Provides version-agnostic methods for checking, setting, and removing preprocessor defines
    /// with support for Unity 2021.2+ NamedBuildTarget system and legacy BuildTargetGroup approach.
    /// </summary>
    public static class DefineUtility
    {
        /// <summary>
        /// Checks if a scripting define symbol is currently set for the active build target.
        /// Uses Unity version-appropriate APIs to query the current define symbols.
        /// </summary>
        /// <param name="define">Define symbol to check for existence</param>
        /// <returns>True if the define symbol is currently set, false otherwise</returns>
        public static bool IsDefined(string define)
        {
            string[] defines;

#if UNITY_2021_2_OR_NEWER
            // Use the new NamedBuildTarget API for Unity 2021.2 and later
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            defines = PlayerSettings
                .GetScriptingDefineSymbols(namedBuildTarget)
                .Split(';');
#else
            // Use legacy BuildTargetGroup API for older Unity versions
            defines = PlayerSettings
                .GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup)
                .Split(';');
#endif

            // Check if the define exists in the current defines list
            return defines.Contains(define);
        }
        
        /// <summary>
        /// Removes a scripting define symbol from all major build targets.
        /// Ensures the define is removed from Standalone, Android, iOS, and WebGL platforms
        /// using Unity version-appropriate APIs for maximum compatibility.
        /// </summary>
        /// <param name="define">Define symbol to remove from all build targets</param>
        public static void Unset(string define)
        {
#if UNITY_2021_2_OR_NEWER
            // Define target platforms using new NamedBuildTarget system
            var groups = new[]
            {
                NamedBuildTarget.Standalone,
                NamedBuildTarget.Android,
                NamedBuildTarget.iOS,
                NamedBuildTarget.WebGL
            };
            
            // Remove define from each target platform
            foreach (var namedBuildTarget in groups)
            {
                var defines = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);
                var defineList = new System.Collections.Generic.List<string>(defines.Split(';'));
                
                // Remove the define if it exists and update the target
                if (defineList.Remove(define))
                {
                    PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, string.Join(";", defineList));
                }
            }
#else
            // Define target platforms using legacy BuildTargetGroup system
            var groups = new[]
            {
                BuildTargetGroup.Standalone,
                BuildTargetGroup.Android,
                BuildTargetGroup.iOS,
                BuildTargetGroup.WebGL
            };
            
            // Remove define from each target platform
            foreach (var group in groups)
            {
                var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
                var defineList = new System.Collections.Generic.List<string>(defines.Split(';'));
                
                // Remove the define if it exists and update the target
                if (defineList.Remove(define))
                {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", defineList));
                }
            }
#endif
        }
        
        /// <summary>
        /// Adds a scripting define symbol to all major build targets if not already present.
        /// Ensures the define is set for Standalone, Android, iOS, and WebGL platforms
        /// using Unity version-appropriate APIs for maximum compatibility.
        /// </summary>
        /// <param name="define">Define symbol to add to all build targets</param>
        public static void Set(string define)
        {
#if UNITY_2021_2_OR_NEWER
            // Define target platforms using new NamedBuildTarget system
            var groups = new[]
            {
                NamedBuildTarget.Standalone,
                NamedBuildTarget.Android,
                NamedBuildTarget.iOS,
                NamedBuildTarget.WebGL
            };
            
            // Add define to each target platform if not already present
            foreach (var namedBuildTarget in groups)
            {
                var defines = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);
                
                // Only add if the define doesn't already exist
                if (!defines.Contains(define))
                {
                    // Add semicolon separator if defines already exist
                    if (!string.IsNullOrEmpty(defines))
                        defines += ";";
                    defines += define;
                    
                    PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, defines);
                }
            }
#else
            // Define target platforms using legacy BuildTargetGroup system
            var groups = new[]
            {
                BuildTargetGroup.Standalone,
                BuildTargetGroup.Android,
                BuildTargetGroup.iOS,
                BuildTargetGroup.WebGL
            };
            
            // Add define to each target platform if not already present
            foreach (var group in groups)
            {
                var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
                
                // Only add if the define doesn't already exist
                if (!defines.Contains(define))
                {
                    // Add semicolon separator if defines already exist
                    if (!string.IsNullOrEmpty(defines))
                        defines += ";";
                    defines += define;
                    
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(group, defines);
                }
            }
#endif
        }
    }
}
#endif