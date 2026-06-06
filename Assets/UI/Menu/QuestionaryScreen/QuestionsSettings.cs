using System.Collections.Generic;

public static class QuestionSettings
{
    public static Dictionary<string, string[]> QuestionBank = new Dictionary<string, string[]>
    {
        { "Map_Green", new string[] { 
            "A cor traz sensação de estabilidade?", 
            "A cor favorece a meditação?" 
        }},
        { "Fase_Vermelha", new string[] { 
            "A cor afeta seu humor positivamente?", 
            "Como a cor influencia seu estado de alerta?" 
        }}
    };

    public static string[] GetQuestions(string levelName)
    {
        if (QuestionBank.ContainsKey(levelName))
            return QuestionBank[levelName];
        
        return new string[] { "Questão 1", "Questão 2" };
    }
}