using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using LostintheGarden.DebugUtils;
using System.Reflection;
using System;
using System.Linq;

public class DebugSymbolsEditor : EditorWindow
{
	private class ActiveFlag
	{
		public bool isActive = false;
	}
	private Dictionary<string, ActiveFlag> activeFlagDict = new Dictionary<string, ActiveFlag>();
	private Dictionary<string, string> flagDict = new Dictionary<string, string>();

	private void OnGUI()
	{
		UpdateFlagDict();
		var activeFlagsList = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Split(';');
		GUILayout.Label("Active Flags:");
		var activeFlags = "";
		foreach(var flag in activeFlagsList)
		{
			activeFlags += " " + flag + "\n";
		}
		GUILayout.Label(activeFlags);
		DisplayCompilerToggles();
		if(GUILayout.Button("Set Flags")) {
			var flagsStrb = BuildCompilerFlags();
			PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, flagsStrb.ToString());
			PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.PS4, flagsStrb.ToString());
			PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.XboxOne, flagsStrb.ToString());
		}
	}

	private void DisplayCompilerToggles()
	{
		foreach(var flag in activeFlagDict.Keys)
		{
			activeFlagDict[flag].isActive = EditorGUILayout.ToggleLeft(flag, activeFlagDict[flag].isActive);
		}
	}

	private StringBuilder BuildCompilerFlags()
	{
		var strb = new StringBuilder();
		foreach(var flag in activeFlagDict.Keys)
		{
			if(activeFlagDict[flag].isActive)
			{
				strb.Append(flag + ";");
			}
		}
		return strb;
	}

	private void UpdateFlagDict()
	{
		flagDict.Clear();
		Assembly mscorlib = typeof(Log).Assembly;
		foreach (Type type in mscorlib.GetTypes())
		{
			var props = type.GetFields().Where(
				prop => Attribute.IsDefined(prop, typeof(CompilerFlagAttribute)));
			foreach(var prop in props)
			{
				Debug.Log(type.FullName + "-" + prop.Name + prop.GetValue(null));
				flagDict[type.FullName + prop.Name] = prop.GetValue(null) as string;
			}
		}

		foreach(var flag in flagDict.Keys)
		{
			if(!activeFlagDict.ContainsKey(flagDict[flag]))
			{
				activeFlagDict[flagDict[flag]] = new ActiveFlag();
			}
		}
		var toRemove = activeFlagDict.Where(pair => !flagDict.ContainsValue(pair.Key)).Select(pair => pair.Key)
						 .ToList();

		foreach (var key in toRemove)
		{
			activeFlagDict.Remove(key);
		}
	}


	[MenuItem("Window/LITG/Debug Symbols Editor")]
	static void ShowWindow()
	{
		DebugSymbolsEditor window = (DebugSymbolsEditor)EditorWindow.GetWindow(typeof(DebugSymbolsEditor));
		window.titleContent = new GUIContent("Debug Symbols Editor");
		window.Show();
	}
}
