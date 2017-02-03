using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using System.IO;

public class NoteCreator2 : MonoBehaviour {
    public GameObject[] Note = new GameObject[7]; //生成するノーツのプレハブをインスペクタで指定
    private TextAsset csvFile; // 読み込むCSVファイル
    private List<string[]> csvDatas = new List<string[]>(); // 読み込んだCSVデータのリスト
    private int[,] NumDatas = new int[10000, 8];
    private int[] Bar = new int[10000];

    float[] posY = new float[8] { 2.0f, 0.0f, -2.0f, -4.0f, 2.0f, 0.0f, -2.0f, -4.0f};//流すレーンのY座標

    public float highspeed;//ハイスピ
    public float BGMTimelag;
    //public float waitTime;

    int NoteType; //ノーツの種類
    int num; //流すレーンに対応するタッチバーの指定
    int num2 = 0;
    int split;
    int temple = 0; //tmpの間違いな気がする
    int count = 0;
    int k = 0;
    int i = 0;
    int last = 0; //赤い線の行
    private bool isDoubleCreate = false; // 同時押しをすでに生成したかどうか

    private float createInterval; // 生成間隔
    private float tmp;
    private float timer;
    private float[] notestime = new float[20000]; // ノーツのあるべき時間
    private bool[] notesbool = new bool[20000]; // ノーツがあるかどうか
    private int array;
    private bool tmpbool;

    private int n;

    private GameObject NewNote; // 生成するノーツ用のオブジェクト

    private AudioSource audioSource;

    private int csvHeight = 0;
    private int measureLine = 0;

    void Start() {
        readCSV();
        analyzeCSV();
        
        array = 0;
        n = 0;

        audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.Play();
        //UnityEngine.Debug.Log(notestime[0]);
        //UnityEngine.Debug.Log(notestime[1]);
        //UnityEngine.Debug.Log(notestime[2]);
        //UnityEngine.Debug.Log(notestime[3]);
        //UnityEngine.Debug.Log(notestime[4]);
    }

    private void readCSV(){
        csvFile = Resources.Load("testcsv") as TextAsset; // Load内はCSVファイルのパス
        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() > -1) {
            string line = reader.ReadLine();
            csvDatas.Add(line.Split(',')); // リストに入れる
            csvHeight++;
        }
    }

    private void analyzeCSV(){
        notestime[0] = 0; // 最初のノーツのある時間(曲の開始時刻によってずらす)
        array = 1;
        last = 0;
        n = 0;
        int i = 0;
        int countNote = 0;

        while(true){
            if(i == measureLine){

            }else if(i != measureLine){
                
            }
        }

        while (true) { //読み込んだCSVを順に見ていく
            //UnityEngine.Debug.Log(i); // 譜面エラー検出用(何行目がおかしいか検出)
            if (i == last) { //赤い行なら
                Bar[count] = i; //n番目の赤い行が何行目かをBar[n]に記録
                count++;
                NumDatas[i, 0] = int.Parse(csvDatas[i][0]);
                if(csvDatas[i][7] == "-1") NumDatas[i, 7] = -1; // 赤い行のときは-1を入れておく
                //UnityEngine.Debug.Log(csvDatas[i][2]);
                if (NumDatas[i, 0] == 1000) break; //最終行の1000に当たればwhile文を抜ける
                NumDatas[i, 1] = int.Parse(csvDatas[i][1]);
                createInterval = (float)60f / NumDatas[i, 1] * 4 / NumDatas[i, 0] * 1000; // 「60 / bpm * 4 / 小節の分割数」でノーツの生成間隔を計算
                for (int p = 1; p <= NumDatas[i, 0]; p++) { //赤い行から次の赤い行までのノーツ生成時間の計算
                    notestime[array] = createInterval + notestime[array - 1]; //array番目のノーツ生成時間
                    //UnityEngine.Debug.Log(highSpeedTime[array]);
                    array++;
                }
                last += NumDatas[i, 0] + 1; //次の赤い行 = 赤い行＋小節の分割数＋1

            } else if (i != last) { //赤い線の行以外なら
                if (NumDatas[i, 0] == 1000) break; //最終行の1000に当たればwhile文を抜ける

                for (int j = 0; j < 8; j++) {
                    NumDatas[i, j] = int.Parse(csvDatas[i][j]); //csvの情報をそのまま入れる
                    if (NumDatas[i, j] > 0) {
                        tmpbool = true; //ノーツがひとつでもあればtrue
                        countNote ++;
                    }
                }
                if (tmpbool) { //tmpboolがtrueならi行目にはノーツがある
                    notesbool[i] = true;
                    tmpbool = false;
                }
            }
            if (NumDatas[i, 0] == 1000) break; //最終行の1000に当たればwhile文を抜ける
            i++;
        }
    }


	/*
    void Update() {
        NoteCreate();
    }

    private void NoteCreate() { // ノーツ生成関数

        //UnityEngine.Debug.Log(i);
        //UnityEngine.Debug.Log(array);
        //UnityEngine.Debug.Log(notestime[array]);
        //UnityEngine.Debug.Log(atomSourceBGM.time);

        if (atomSourceBGM.time > notestime[array]) { //BGMの時刻がノーツの生成時刻になったら
            if (temple == 0) i = 0; // iの初期化
            if (i == Bar[temple]) { // i行目が赤い行だったら
                temple++;
                i++; //赤い行を飛ばす
            }
            for (int j = 0; j < 8; j++) {
                if (NumDatas[i, j] != 0) { //ノーツ番号が0以外ならノーツを生成
                    //num=Random.Range(0,4);    //流すレーンをランダムで決める
                    //流すレーンを場合分け
                    num = j;

                    //ノーツの種類をランダムで決めて生成
                    //NoteType=Random.Range (0,1);
                    switch (NumDatas[i, j]) {
                        case 1: // シングル
                            NoteType = 0;
                            break;

                        case 2: // スワイプ
                            NoteType = 1;
                            break;

                        case 3: // ロング始点
                            NoteType = 2;
                            break;

                        case 4: // ロング終点
                            NoteType = 3;
                            break;

                        case 5: // 同時押し
                            NoteType = 4;
                            break;

                        case 6: // スライド
                            NoteType = 5;
                            break;

                        case 7: // ロング中間
                            NoteType = 6;
                            break;
                        default:
                            break;
                    }
                    if(NoteType != 3){
                        NewNote = Instantiate(this.Note[NoteType], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    }
                    //UnityEngine.Debug.Log(NoteType);

                    switch (NoteType) {
                        case 0:    //シングルの場合
                            noteobjS = NewNote.GetComponent<NoteDesSingle>();
                            noteobjS.BarNum = num; // レーン番号
                            if(DebugManager.isDebugNoteReverse == true){
                                noteobjS.BarNum = reverseNotePosition(num);
                            }
                            break;

                        default:
                            break;
                    }
                    if (num2 == k) continue; //←よくわかんない←多分、同時押しを1つすでに読み取ったら2つ目はスキップかと(なんかうまく機能してないっぽい)

                }
            }
            isDoubleCreate = false; // 列(横方向)を探索するfor文が終わったら、同時押し生成フラグを初期化
            i++;
            array++;
        }
    }
	*/

}