using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GameDatabase
{
    public List<PlayerTable> players = new List<PlayerTable>();
    public List<RecordTable> records = new List<RecordTable>();
    public List<DetailTable> details = new List<DetailTable>();
}
[System.Serializable]
public class PlayerTable
{
    public int playerID;
    public string name;
}
[System.Serializable]
public class RecordTable
{
    public int recordID;
    public int playerID;
    public long playDate;
}
[System.Serializable]
public class DetailTable
{
    public int detailID;
    public int recordID;
    public int round;
    public int correct;
    public int miss;
    public int score;
    public float avgSpeed;
    public int maxCombo;
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public string filePath;
    public GameDatabase db = new GameDatabase();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        filePath = Path.Combine(Application.persistentDataPath, "ranking_db.json");
        LoadDatabase();
    }

    public void SaveDatabase()
    {
        try
        {
            string json = JsonUtility.ToJson(db, true);
            File.WriteAllText(filePath, json);
            Debug.Log($"保存完了: {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"JSON保存エラー: {e.Message}");
        }
    }

    public void LoadDatabase()
    {
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                db = JsonUtility.FromJson<GameDatabase>(json);

                if (db == null) db = new GameDatabase();
                Debug.Log("JSON読み込み完了");
            }
            else
            {
                db = new GameDatabase();
                SaveDatabase();
                Debug.Log("新規データベースを作成しました");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"JSON読み込みエラー: {e.Message}");
            db = new GameDatabase();
        }
    }

    public PlayerTable RegisterPlayer(string playerName)
    {
        if (string.IsNullOrWhiteSpace(playerName)) playerName = "NO NAME";
        if (playerName.Length > 8) playerName = playerName.Substring(0, 8);

        // すでに同じ名前が存在するかチェック
        PlayerTable existingPlayer = db.players.FirstOrDefault(p => p.name == playerName);
        if (existingPlayer != null)
        {
            Debug.Log($"既存プレイヤーが見つかりました: ID={existingPlayer.playerID}, Name={existingPlayer.name}");
            return existingPlayer;
        }

        // 新規プレイヤーとして登録
        int nextPlayerId = db.players.Count > 0 ? db.players.Max(p => p.playerID) + 1 : 1;
        PlayerTable newPlayer = new PlayerTable
        {
            playerID = nextPlayerId,
            name = playerName
        };
        db.players.Add(newPlayer);

        // プレイヤー登録時点で即座にJSON保存
        SaveDatabase();
        Debug.Log($"新規プレイヤーを登録しました: ID={newPlayer.playerID}, Name={newPlayer.name}");

        return newPlayer;
    }

    public void RegisterResult(string playerName, int round, int correct, int miss, int score, float avgSpeed, int maxCombo)
    {
        // --- 1. 文字列・数値の桁数フォーマット調整 ---
        if (string.IsNullOrWhiteSpace(playerName)) playerName = "NO NAME";
        if (playerName.Length > 8) playerName = playerName.Substring(0, 8); // 8文字制限

        round = Mathf.Clamp(round, 0, 99);
        correct = Mathf.Clamp(correct, 0, 99);
        miss = Mathf.Clamp(miss, 0, 99);
        score = Mathf.Clamp(score, 0, 999999);
        maxCombo = Mathf.Clamp(maxCombo, 0, 999);
        // 小数点第2位までに丸める
        avgSpeed = (float)Math.Round(avgSpeed, 2);

        // --- 2. PlayerTable の登録または検索 ---
        // すでに同じ名前のプレイヤーがいればそのIDを使い、いなければ新規IDを発行
        PlayerTable player = db.players.FirstOrDefault(p => p.name == playerName);
        if (player == null)
        {
            int nextPlayerId = db.players.Count > 0 ? db.players.Max(p => p.playerID) + 1 : 1;
            player = new PlayerTable
            {
                playerID = nextPlayerId,
                name = playerName
            };
            db.players.Add(player);
        }

        // --- 3. RecordTable の登録 ---
        int playCount = db.records.Count(r => r.playerID == player.playerID) + 1;

        int nextRecordId = (player.playerID * 100) + playCount;
        long currentDate = long.Parse(DateTime.Now.ToString("yyyyMMddHHmm")); // 12桁の現在日時

        RecordTable record = new RecordTable
        {
            recordID = nextRecordId,
            playerID = player.playerID,
            playDate = currentDate
        };
        db.records.Add(record);

        int nextDetailId = db.details.Count > 0 ? db.details.Max(d => d.detailID) + 1 : 1;

        // --- 4. DetailTable の登録 ---
        DetailTable detail = new DetailTable
        {
            detailID = nextDetailId,
            recordID = nextRecordId,
            round = round,
            correct = correct,
            miss = miss,
            score = score,
            avgSpeed = avgSpeed,
            maxCombo = maxCombo
        };
        db.details.Add(detail);

        // --- 5. JSONファイルへ保存 ---
        SaveDatabase();
    }
}
