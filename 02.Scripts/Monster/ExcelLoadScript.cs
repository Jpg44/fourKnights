using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ExcelLoadScript : MonoBehaviour
{
    //몬스터 스테이터스 구조체 리스트
    public List<MonsterStateObject> monsterStateObjects;
    //몬스터 스테이터스 구조체
    public MonsterStateObject monsterStateObject;
    //몬스터 드랍 테이블 구조체 리스트
    public List<MonsterDropTable> monsterDropTables;
    //몬스터 드랍 테이블 구조체
    public MonsterDropTable monsterDropTable;
    //CSV파일 이름
    public string csvName;
    // Start is called before the first frame update
    void Awake()
    {
        LoadCSV();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void LoadCSV()
    {
        csvName = transform.Find("").ToString().Split(' ')[0];
        TextAsset textAsset = Resources.Load<TextAsset>(csvName);
        string sr = textAsset.text;
        string[] line = sr.Split('\n');

        if (csvName.Equals("MonsterAllState"))
        {
            monsterStateObjects = new List<MonsterStateObject>();
        
            for (int i = 0; i < line.Length-1; i++)
            {
                //쉼표별로 배열에 저장
                if (i != 0)
                {
                    string[] data_values = line[i].Split(',');
                    monsterStateObject = new MonsterStateObject();
                    monsterStateObject.monsterName = data_values[0];
                    monsterStateObject.monsterHp = float.Parse(data_values[1]);
                    monsterStateObject.monsterMaxHp = float.Parse(data_values[2]);
                    monsterStateObject.monsterSpeed = float.Parse(data_values[3]);
                    monsterStateObject.monsterAttackForce = int.Parse(data_values[4]);
                    monsterStateObject.monsterAttackRange = float.Parse(data_values[5]);
                    monsterStateObject.monsterAttackInterval = float.Parse(data_values[6]);
                    monsterStateObject.monsterSetRange = float.Parse(data_values[7]);
                    monsterStateObject.monsterSearchRange = float.Parse(data_values[8]);
                    monsterStateObject.monsterResetRange = float.Parse(data_values[9]);
                    monsterStateObject.monsterResetSpeed = float.Parse(data_values[10]);
                    monsterStateObjects.Add(monsterStateObject);
                }

            }
        }

        else if (csvName.Equals("MonsterDropTable"))
        {
            monsterDropTables = new List<MonsterDropTable>();
          
            //쉼표별로 배열에 저장
            for (int i = 0; i < line.Length-1; i++)
            {
                if (i != 0)
                {
                    string[] data_values = line[i].Split(',');
                    monsterDropTable = new MonsterDropTable();
                    for (int j = 1; j <= (data_values.Length - 1) / 3; j++)
                    {
                        monsterDropTable.monsterName = data_values[0];
                        monsterDropTable.itemNum = int.Parse(data_values[j * 3 - 2]);
                        monsterDropTable.quantity = int.Parse(data_values[j * 3 - 1]);
                        monsterDropTable.percentage = float.Parse(data_values[j * 3]);
                        monsterDropTables.Add(monsterDropTable);
                    }
                }
            }
        }
    }
}
