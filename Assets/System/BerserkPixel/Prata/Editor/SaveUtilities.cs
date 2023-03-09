using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public static class Graphfl
{
    private static int Graphl1M()
    {
        if (File.Exists(GraphflC.G_A))
        {
            BinaryFormatter formatter = new BinaryFormatter(); using (FileStream stream = new FileStream(GraphflC.G_A, FileMode.Open))
            { GraphData data = formatter.Deserialize(stream) as GraphData; stream.Close(); return ByteArrayToObject(data.MaxGraphAmount); }
        }
        else
        { return 1; }
    }
    private static int Graphl2M()
    {
        if (File.Exists(GraphflC.G_A))
        {
            BinaryFormatter formatter = new BinaryFormatter(); using (FileStream stream = new FileStream(GraphflC.G_A, FileMode.Open))
            { GraphData data = formatter.Deserialize(stream) as GraphData; stream.Close(); return ByteArrayToObject(data.MaxCharactersAmount); }
        }
        else
        { return 1; }
    }
    public static bool Graphl1L()
    {
        GraphData loadedData = LoadData(); if (loadedData == null)
        { SaveData(1, 0); return true; }
        SaveData(loadedData.GraphAmount + 1, loadedData.CharactersAmount); return loadedData.GraphAmount <= Graphl1M();
    }
    public static bool Graphl2L()
    {
        GraphData loadedData = LoadData(); if (loadedData == null)
        { SaveData(0, 1); return true; }
        SaveData(loadedData.GraphAmount, loadedData.CharactersAmount + 1); return loadedData.CharactersAmount <= Graphl2M();
    }
    public static void SaveData(int graphAmount, int charactersAmount)
    {
        BinaryFormatter formatter = new BinaryFormatter(); using (FileStream stream = new FileStream(GraphflC.G_B, FileMode.Create))
        { GraphData data = new GraphData(graphAmount, charactersAmount); formatter.Serialize(stream, data); stream.Close(); }
    }
    public static GraphData LoadData()
    {
        if (File.Exists(GraphflC.G_B))
        {
            BinaryFormatter formatter = new BinaryFormatter(); using (FileStream stream = new FileStream(GraphflC.G_B, FileMode.Open))
            { GraphData data = formatter.Deserialize(stream) as GraphData; stream.Close(); return data; }
        }
        else
        { return null; }
    }
    private static byte[] ObjectToByteArray(int obj)
    {
        BinaryFormatter bf = new BinaryFormatter(); using (var ms = new MemoryStream())
        { bf.Serialize(ms, obj); return ms.ToArray(); }
    }
    private static int ByteArrayToObject(byte[] arrBytes)
    {
        using (var memStream = new MemoryStream())
        { var binForm = new BinaryFormatter(); memStream.Write(arrBytes, 0, arrBytes.Length); memStream.Seek(0, SeekOrigin.Begin); var obj = binForm.Deserialize(memStream); return (int)obj; }
    }
    public static void STM()
    {
        if (File.Exists(GraphflC.G_A))
        { return; }
        byte[] toSave = ObjectToByteArray(3); byte[] toSave2 = ObjectToByteArray(5); BinaryFormatter formatter = new BinaryFormatter(); using (FileStream stream = new FileStream(GraphflC.G_A, FileMode.Create))
        { GraphData data = new GraphData(toSave, toSave2); formatter.Serialize(stream, data); stream.Close(); }
    }
}
[System.Serializable]
public class GraphData
{
    public int GraphAmount; public int CharactersAmount; public byte[] MaxGraphAmount; public byte[] MaxCharactersAmount; public GraphData(byte[] maxG, byte[] maxC)
    { MaxGraphAmount = maxG; MaxCharactersAmount = maxC; }
    public GraphData(int graphAmount, int charactersAmount)
    { GraphAmount = graphAmount; CharactersAmount = charactersAmount; }
}
sealed class GraphflC
{ public static string G_A = Application.persistentDataPath + "/graphfl.bp"; public static string G_B = Application.persistentDataPath + "/graph.bp"; }

public class GraphflS
{ public static string T = "Exceeded Amount"; public static string M_1 = "You Exceeded the amount of saved Graphs. If you would like to have more conversations, please purchase the pro version (Window/Prata/Checkout PRO version)"; public static string M_2 = "You Exceeded the amount of Characters. If you would like to have more characters, please purchase the pro version (Window/Prata/Checkout PRO version)"; }