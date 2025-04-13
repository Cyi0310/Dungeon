# Dungeon

## 簡介
目前這款遊戲是在下班中開發中的遊戲，是一個探索地下城的遊戲，玩家將進入隨機生成的地下城，挑戰各種怪物並尋找寶藏。


## 功能(Feature)
- 隨機生成的地圖/怪物
- 藉由動態掌握的戰鬥系統
- 能夠動態更換雙手武器/防具


## 目前進度

![](https://raw.githubusercontent.com/Cyi0310/Dungeon/main/ReadMeUse/Dev/Dev_Dungeon.gif)

[影片連結](https://youtu.be/cWJmQqwcewQ)

## 程式架構
主要程式碼都位於 `Unity_Dungeon/Assets`。
程式進入點位於 `GameMain.cs`，由GameMain呼叫，在傳到內部各個Mgr。
而內部元件大部分是使用被擁有(如:`HealthComponents`)來取代繼承的方式來提高類別的重複使用性。

- Character：主要玩家本身的程式碼都位於這裡，有把資料層(`Character`)拆分出來，讓後續資料同步較為方便。

- Common：大多放置共用功能的腳本
  - BaseEntityView：實體(Entity)是位於場上所有能動的物體的概念(如:怪物、玩家)，使用約束泛型來約束物體要使用繼承介面來指定資料層的類別。
  - 碰撞Issue：在設計上不想要把碰撞(Collider)綁定在Root上，所以建構了`ColliderController`、`OnHitDelegater`來抽離關聯，讓角色在層級(Hierarchy)更為彈性。
  - HealthComponents：將血量本身不綁並在角色身上，另外單獨以Model的方式存在，再由各自需要的類別擁有來抽離耦合性以及模組化，再利用註冊委派來通知外部當前發生甚麼事(`OnDieHandler`、`OnTakeDamage`、`OnTakeHeal`)
  
- Level：負責處理關卡所有相關
  - TileMgr：負責生成場景上所有東西以及再生成時做綁定的行為(除了玩家本身)。
