﻿using System.IO;
using UnityEditor;

namespace RPG.Dialogue.Editor
{
    public class DialogueModificationProcessor : AssetModificationProcessor
    {
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            var dialogue = AssetDatabase.LoadMainAssetAtPath(sourcePath) as Dialogue;

            if (!dialogue)
                return AssetMoveResult.DidNotMove;

            if (Path.GetDirectoryName(sourcePath) != Path.GetDirectoryName(destinationPath))
                return AssetMoveResult.DidNotMove;

            dialogue.name = Path.GetFileNameWithoutExtension(destinationPath);

            return AssetMoveResult.DidNotMove;
        }
    }
}
