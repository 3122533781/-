using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityObject = UnityEngine.Object;

/// <summary>
/// Some reflection utilities that can be AOT compiled (and are therefore available at runtime).
/// </summary>
public class RuntimeReflectionUtility
{
    /// <summary>
    /// Returns all types that derive from UnityEngine.Object that are usable during runtime.
    /// </summary>
    public static IEnumerable<Type> GetUnityObjectTypes()
    {
        return from assembly in GetRuntimeAssemblies()

               // GetExportedTypes() doesn't work for dynamic modules, so we jut use GetTypes()
               // instead and manually filter for public
               from type in assembly.GetTypes()
               where type.IsVisible

               // Cannot be an open generic type
               where type.IsGenericTypeDefinition == false

               where typeof(UnityObject).IsAssignableFrom(type)

               select type;
    }

    /// <summary>
    /// Returns the equivalent of assembly.GetName().Name, which does not work on WebPlayer.
    /// </summary>
    private static string GetName(Assembly assembly)
    {
        int index = assembly.FullName.IndexOf(",");
        if (index >= 0)
        {
            return assembly.FullName.Substring(0, index);
        }
        return assembly.FullName;
    }

    /// <summary>
    /// Return a guess of all assemblies that can be used at runtime.
    /// </summary>
    public static IEnumerable<Assembly> GetRuntimeAssemblies()
    {
#if !UNITY_EDITOR && UNITY_METRO
            yield return typeof(Assembly).GetTypeInfo().Assembly;
#else

        if (_cachedRuntimeAssemblies == null)
        {
            _cachedRuntimeAssemblies =
                (from assembly in AppDomain.CurrentDomain.GetAssemblies()

                 // Unity exposes lots of assemblies that won't contain any behaviors that will
                 // contain a MonoBehaviour or UnityObject reference... so we ignore them to speed
                 // up reflection processing

                 where IsBannedAssembly(assembly) == false
                 where IsUnityEditorAssembly(assembly) == false

                 // In the editor, even Assembly-CSharp, etc contain a reference to UnityEditor,
                 // so we can't strip the assembly that way
                 where GetName(assembly).Contains("-Editor") == false

                 select assembly).ToList();
        }
        return _cachedRuntimeAssemblies;
#endif
    }
    private static List<Assembly> _cachedRuntimeAssemblies;

    /// <summary>
    /// Returns a guess of all user-defined assemblies that are available in the editor, but not
    /// necessarily in the runtime. This is a superset over GetRuntimeAssemblies().
    /// </summary>
    public static IEnumerable<Assembly> GetUserDefinedEditorAssemblies()
    {
#if !UNITY_EDITOR && UNITY_METRO
            yield return typeof(Assembly).GetTypeInfo().Assembly;
#else
        if (_cachedUserDefinedEditorAssemblies == null)
        {
            _cachedUserDefinedEditorAssemblies =
                (from assembly in AppDomain.CurrentDomain.GetAssemblies()

                 where IsBannedAssembly(assembly) == false
                 where IsUnityEditorAssembly(assembly) == false

                 // bugfix: This does not necessarily work because Assembly-CSharp will is not guaranteed
                 //         to have a reference to UnityEditor if there are no conditional references to
                 //         UnityEditor
                 //where ContainsAssemblyReference(assembly, typeof(EditorWindow).Assembly)

                 select assembly).ToList();
        }

        return _cachedUserDefinedEditorAssemblies;
#endif
    }
    private static List<Assembly> _cachedUserDefinedEditorAssemblies;

    /// <summary>
    /// Gets all possible editor assemblies, including those defined by Unity. This is a superset over
    /// GetUserDefinedEditorAssemblies().
    /// </summary>
    public static IEnumerable<Assembly> GetAllEditorAssemblies()
    {
#if !UNITY_EDITOR && UNITY_METRO
            yield return typeof(Assembly).GetTypeInfo().Assembly;
#else

        if (_cachedAllEditorAssembles == null)
        {
            _cachedAllEditorAssembles =
                (from assembly in AppDomain.CurrentDomain.GetAssemblies()

                 where IsBannedAssembly(assembly) == false

                 // bugfix: This does not necessarily work because Assembly-CSharp will is not guaranteed
                 //         to have a reference to UnityEditor if there are no conditional references to
                 //         UnityEditor
                 //where ContainsAssemblyReference(assembly, typeof(EditorWindow).Assembly)

                 select assembly).ToList();
        }

        return _cachedAllEditorAssembles;
#endif
    }
    private static List<Assembly> _cachedAllEditorAssembles;


    private static bool IsUnityEditorAssembly(Assembly assembly)
    {
        var allowableScripts = new string[] {
                "UnityEditor",
                "UnityEditor.UI",
            };

        return allowableScripts.Contains(GetName(assembly));
    }

    /// <summary>
    /// Returns true if the given assembly is likely to contain user-scripts or it is a core
    /// runtime assembly (ie, UnityEngine).
    /// </summary>
    /// <param name="name">The unqualified name of the assembly.</param>
    private static bool IsBannedAssembly(Assembly assembly)
    {
        var bannedScripts = new string[] {
                "AssetStoreTools",
                "AssetStoreToolsExtra",

                "UnityScript",
                "UnityScript.Lang",

                "Boo.Lang.Parser",
                "Boo.Lang",
                "Boo.Lang.Compiler",

                "mscorlib",
                "System.ComponentModel.DataAnnotations",
                "System.Xml.Linq",

                "ICSharpCode.NRefactory",
                "Mono.Cecil",
                "Mono.Cecil.Mdb",

                "Unity.DataContract",
                "Unity.IvyParser",
                "Unity.Locator",
                "Unity.PackageManager",
                "Unity.SerializationLogic",

                //"UnityEngine",
                "UnityEngine.UI",

                "UnityEditor.Android.Extensions",
                "UnityEditor.BB10.Extensions",
                "UnityEditor.Metro.Extensions",
                "UnityEditor.WP8.Extensions",
                "UnityEditor.iOS.Extensions",
                "UnityEditor.iOS.Extensions.Xcode",
                "UnityEditor.WindowsStandalone.Extensions",
                "UnityEditor.LinuxStandalone.Extensions",
                "UnityEditor.OSXStandalone.Extensions",
                "UnityEditor.WebGL.Extensions",
                "UnityEditor.Graphs",

                "protobuf-net",

                "Newtonsoft.Json",

                "System",
                "System.Configuration",
                "System.Xml",
                "System.Core",

                "Mono.Security",

                "I18N",
                "I18N.West",

                "nunit.core",
                "nunit.core.interfaces",
                "nunit.framework",
                "NSubstitute",

                "UnityVS.VersionSpecific",
                "SyntaxTree.VisualStudio.Unity.Bridge",
                "SyntaxTree.VisualStudio.Unity.Messaging",
            };
        return bannedScripts.Contains(GetName(assembly));
    }

    /// <summary>
    /// Returns all types in the current AppDomain that derive from the given baseType and are a
    /// class that is not an open generic type.
    /// </summary>
    public static IEnumerable<Type> AllSimpleTypesDerivingFrom(Type baseType)
    {
        return from assembly in GetRuntimeAssemblies()
               from type in assembly.GetTypes()
               where baseType.IsAssignableFrom(type)
               where type.IsClass
               where type.IsGenericTypeDefinition == false
               select type;
    }

    /// <summary>
    /// Returns all types in the current AppDomain that derive from the given baseType, are classes,
    /// are not generic, have a default constuctor, and are not abstract.
    /// </summary>
    public static IEnumerable<Type> AllSimpleCreatableTypesDerivingFrom(Type baseType)
    {
        return from type in AllSimpleTypesDerivingFrom(baseType)
               where type.IsAbstract == false
               where type.IsGenericType == false
               select type;
    }
}
