using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class CreateAnimationFromSprites
{
    private static EditorCurveBinding SpriteBinding = new EditorCurveBinding()
    {
        type = typeof(SpriteRenderer),
        path = "",
        propertyName = "m_Sprite"
    };

    private static EditorCurveBinding ImageBinding = new EditorCurveBinding()
    {
        type = typeof(Image),
        path = "",
        propertyName = "m_Sprite"
    };

    [MenuItem("Assets/Convert selected sprite to animation file", isValidateFunction: true)]
    private static bool SpriteValidation()
    {
        return ValidateInternal();
    }

    [MenuItem("Assets/Convert selected sprite to UI animation file", isValidateFunction: true)]
    private static bool UIValidation()
    {
        return ValidateInternal();
    }

    private static bool ValidateInternal()
    {
        return Selection.objects.Any() && Selection.objects.All(x => x is Sprite || x is Texture2D);
    }

    [MenuItem("Assets/Convert selected sprite to animation file", isValidateFunction: false)]
    private static void ConvertSelectedSpriteToAnimation()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
        var parentPath = Path.GetDirectoryName(path);
        var selectedSprites = Selection.objects
            .Select(AssetDatabase.GetAssetPath)
            .SelectMany(AssetDatabase.LoadAllAssetsAtPath)
            .OfType<Sprite>()
            .ToArray();

        Create(spriteMode: true, parentPath, selectedSprites, selectedSprites[0]);
    }



    [MenuItem("Assets/Convert selected sprite to UI animation file", isValidateFunction: false)]
    private static void ConvertSelectedUIToAnimation()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
        var parentPath = Path.GetDirectoryName(path);
        var selectedSprites = Selection.objects
            .Select(AssetDatabase.GetAssetPath)
            .SelectMany(AssetDatabase.LoadAllAssetsAtPath)
            .OfType<Sprite>()
            .ToArray();

        Create(spriteMode: false, parentPath, selectedSprites, selectedSprites[0]);
    }

    private static void ConvertSelectedInternal(bool spriteMode)
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
        var parentPath = Path.GetDirectoryName(path);
        var selectedSprites = Selection.objects
            .Select(AssetDatabase.GetAssetPath)
            .SelectMany(AssetDatabase.LoadAllAssetsAtPath)
            .OfType<Sprite>()
            .ToArray();

        Create(spriteMode, parentPath, selectedSprites, selectedSprites[0]);
    }

    private static void Create(bool spriteMode, string parentFolder, Sprite[] sprites, Sprite defaultSprite)
    {
        SetupAnimationClip(spriteMode, sprites.First().name.Split(' ')[0], 60, 0.05f, sprites, parentFolder);
    }

    private static void SetupAnimationClip(bool spriteMode, string name, int frameRate, float timeCoefficient, Sprite[] sprites, string parentFolder)
    {
        var clip = new AnimationClip();
        clip.frameRate = frameRate;
        clip.wrapMode = WrapMode.Loop;
        clip.name = name;

        var settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(clip, settings);

        var spriteKeyFrames = sprites
            .Select((x, i) => new ObjectReferenceKeyframe() { time = i * timeCoefficient, value = x })
            .ToArray();

        AnimationUtility.SetObjectReferenceCurve(clip, spriteMode ? SpriteBinding : ImageBinding, spriteKeyFrames);
        AssetDatabase.CreateAsset(clip, $"{parentFolder}/{name}.anim");
        AssetDatabase.SaveAssets();
    }
}