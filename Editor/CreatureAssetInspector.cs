﻿// ------------------------------------------------------------------------------
//  Created by Kestrel Moon Studios. C# Engine. 
//  Copyright (c) 2015 Kestrel Moon Studios. All rights reserved.
// ------------------------------------------------------------------------------

using UnityEngine;
using UnityEditor;
using CreatureModule;
using System.Collections;
using System.Collections.Generic;


[CustomEditor(typeof(CreatureAsset))]
public class CreatureAssetInspector : Editor {
	private SerializedProperty creatureJSON;
	private List<string> animation_names;

	CreatureAssetInspector()
	{
		animation_names = new List<string> ();
	}

	void OnEnable () 
	{
		creatureJSON = serializedObject.FindProperty("creatureJSON");
	}

	void UpdateData()
	{
		CreatureAsset creature_asset = (CreatureAsset)target;
		TextAsset text_asset = (TextAsset)creatureJSON.objectReferenceValue;
		if (text_asset.text.Length > 0) {
			creature_asset.ResetState();
			creature_asset.creatureJSON = text_asset;
			FillAnimationNames();
		}
	}

	void FillAnimationNames()
	{
		CreatureAsset creature_asset = (CreatureAsset)target;
		CreatureManager creature_manager = creature_asset.GetCreatureManager ();
		Dictionary<string, CreatureModule.CreatureAnimation> all_animations
			= creature_manager.animations;
		
		animation_names.Clear();
		foreach (string cur_name in all_animations.Keys) {
			animation_names.Add(cur_name);
		}
	}

	override public void OnInspectorGUI () 
	{
		serializedObject.Update();

		EditorGUI.BeginChangeCheck();

		EditorGUILayout.PropertyField (creatureJSON);

		bool did_change = EditorGUI.EndChangeCheck();

		if ((creatureJSON.objectReferenceValue != null) && did_change)
		{
			UpdateData();
		}

		if (creatureJSON.objectReferenceValue != null) {
			FillAnimationNames();

			int i = 1;
			foreach (string cur_name in animation_names) {
				EditorGUILayout.LabelField ("Animation Clip #" + i.ToString () + ":", 
				                            cur_name);
				i++;
			}
		}

	}
}