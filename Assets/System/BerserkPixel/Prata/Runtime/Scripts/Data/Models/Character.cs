using System;
using System.Collections.Generic;
using UnityEngine;

namespace BerserkPixel.Prata
{
    [CreateAssetMenu(fileName = "Character", menuName = "Prata/New Character", order = 1)]
    public class Character : ScriptableObject
    {
        public string id = Guid.NewGuid().ToString();

        public string characterName;

        public List<Faces> faces;

        public Sprite GetFaceForEmotion(ActorsEmotions emotion) =>
            faces.Find(face => face.emotion == emotion)?.face;
    }
}