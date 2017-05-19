
using UnityEngine;
using UnityEngine.Internal;
using System.Diagnostics;
using System.Collections;
using System;


public static class Debugger {
	/// <summary>
	/// Prints a message to the unity console.
	/// </summary>
	public static void printLog(object message, UnityEngine.Object context = null) {
		UnityEngine.Debug.Log (message, context);
	}
	/// <summary>
	/// Prints a message to the unity console.
	/// </summary>
//	public static void printLog(params object[] args) {
//		UnityEngine.Debug.Log (string.Concat(args));
//	}
	/// <summary>
	/// Prints a message to the unity console.
	/// </summary>
	public static void printErrorLog(object message, UnityEngine.Object context = null) {
		UnityEngine.Debug.LogError (message, context);
	}
	/// <summary>
	/// Prints a message to the unity console.
	/// </summary>
//	public static void printErrorLog(params object[] args) {
//		UnityEngine.Debug.LogError (string.Concat(args));
//	}
}
