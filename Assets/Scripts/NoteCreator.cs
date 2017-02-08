using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine;

public class NoteCreator : MonoBehaviour {
    public GameObject[] noteObj = new GameObject[4]; //生成するノーツのプレハブをインスペクタで指定
    private TextAsset csvFile; // 読み込むCSVファイル
    private List<string[]> csvDatas = new List<string[]>(); // 読み込んだCSVデータのリスト
    int count = 0;
    int k = 0;
    int i = 0,j;
    private float createInterval; // 生成間隔

    private int n,p,height = 0,lane;

    private GameObject newNote; // 生成するノーツ用のオブジェクト

    private AudioSource audioSource;
    private AudioClip audioClip;
    private int csvHeight = 0;
    private int nextMeasure = 0;
    private int[] measureLine = new int[5000];
    private int[,] notesData = new int[10000, 9];
    private float[] notesTime = new float[10000];
    private bool[] notesExist = new bool[10000];
    private int noteType;
    public static int[] laneNoteCount = new int[9];
	public static int[] nextNoteValue = new int[9];
    public static float gameTime = 0;
    public string csvName;
    private float longStartTime, longEndTime;

    void Start() {
        readCSV();
        analyzeCSV();
        for(i=0;i<9;i++){
			laneNoteCount[i] = 0;
			nextNoteValue[i] = 0;
		}
        
        n = 0;
        p = 0;
        i = 0;
        //UnityEngine.Debug.Log(notestime[0]);
        //UnityEngine.Debug.Log(notestime[1]);
        //UnityEngine.Debug.Log(notestime[2]);
        //UnityEngine.Debug.Log(notestime[3]);
        //UnityEngine.Debug.Log(notestime[4]);
        Invoke("playAudio",1f);
    }

    void Update(){
        notesCreate();
    }

    private void readCSV(){
        csvFile = Resources.Load(csvName) as TextAsset; // Load内はCSVファイルのパス
        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() > -1) {
            string line = reader.ReadLine();
            csvDatas.Add(line.Split(',')); // リストに入れる
            csvHeight++;
        }
    }

    private void analyzeCSV(){
        notesTime[0] = 0;
        i = 0;
        p = 1;
        int countNote = 0;
        while(true){
            if(i == nextMeasure){
                if(i == csvHeight) break;
                measureLine[count] = i;
                count++;
                notesData[i,0] = int.Parse(csvDatas[i][0]);
                if(csvDatas[i][7] == "-1") notesData[i,7] = -1;
                notesData[i,1] = int.Parse(csvDatas[i][1]);
                createInterval =  (float)60f / notesData[i, 1] * 4 / notesData[i, 0];
                for(k=1;k<=notesData[i,0];k++){
                    notesTime[p] = createInterval + notesTime[p-1];
                    p++;
                }
                nextMeasure += notesData[i,0] + 1;
            }else if(i != nextMeasure){
                if(i == csvHeight) break;
                for(j=0;j<9;j++){
                    notesData[i,j] = int.Parse(csvDatas[i][j]);
                    if(notesData[i,j] > 0){
                        notesExist[i] = true;
                        countNote++;
                    }
                }
                i++;
            }else if(i == csvHeight){
                break;
            }
        }

    }

    private void playAudio(){
        audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.Play();
        audioClip = audioSource.clip;
        StatusManager.audioLength = audioClip.length;
    }

    private void notesCreate(){
        gameTime += Time.deltaTime;
        if(gameTime >= notesTime[p]){
            if(i == measureLine[height]){
                height++;
                i++;
            }
            for(j=0;j<9;j++){
                if(notesData[i,j] != 0){
                    lane = j;
                    switch(notesData[i,j]){
                        case 1:
                            noteType = 0;
                            break;
                        case 2:
                            noteType = 1;
                            break;
                        case 3:
                            noteType = 2;
                            break;
                        case 4:
                            noteType = 3;
                            break;
                        default:
                            break;
                    }
                    if(noteType != 3){ // ロング終点以外のときは生成
                        newNote = Instantiate(noteObj[noteType], new Vector3(0, 3.6f, 0), Quaternion.identity) as GameObject;
                        newNote.transform.parent = GameObject.Find("NoteParent").transform;
                    }
                    switch(noteType){
                        case 0:
                            newNote.GetComponent<SingleNote>().idealTime = notesTime[p];
                            newNote.GetComponent<SingleNote>().laneIndex = laneNoteCount[lane];
                            newNote.GetComponent<NoteMove>().idealTime = notesTime[p];
                            newNote.GetComponent<NoteMove>().laneValue = lane;
                            laneNoteCount[lane]++;
                            break;
                        case 2: // ロングノーツ始点の場合
                            longStartTime = notesTime[p]; // 始点の時間                        
                            int tmp = i;
                            int c = 0; // 赤い行が何個あるかカウント
                            while(true){ // 始点から終点を見つける
                                if(notesData[tmp,j] == 4){ // 終点が見つかったら
                                    longEndTime = notesTime[p + (tmp-i-c)]; // 終点の時刻を取得(notestime[]はCSV1行ごとの時間が保存、つまり始点と終点の間の行数から時刻取得)
                                    break;
                                }
                                if(notesData[tmp+1,8] < 0){ // 赤い行のときは
                                    tmp+=2; // それを飛ばして
                                    c++; // 赤い行が見つかったのでカウント
                                }else{ // それ以外なら
                                    tmp++; // 次の行へ
                                }
                            }
                            newNote.GetComponent<NoteMove>().laneValue = lane;
                            newNote.GetComponent<LongNote>().laneIndex = laneNoteCount[lane];
                            newNote.GetComponent<LongNote>().startTime = longStartTime;
                            newNote.GetComponent<LongNote>().endTime = longEndTime;

                            /*
                            noteobjL = newNote.GetComponent<NoteDesLong>(); // ロングノーツ用のアレ(日本語障害)を取得
                            noteobjL.BarNum = num; // バー番号
                            noteobjL.startTime = longStartTime; // 始点の時刻
                            noteobjL.endTime = longEndTime; // 終点の時刻
                            */
                            break;
                        case 3:
                            break;
                        default:
                            break;
                    }
                }
            }
            i++;
            p++;
        }
    }

}