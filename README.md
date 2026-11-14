# BSPlayerDataBackup

[![Build and Release](https://github.com/denpadokei/BSPlayerDataBackup/actions/workflows/build-and-release.yml/badge.svg)](https://github.com/denpadokei/BSPlayerDataBackup/actions/workflows/build-and-release.yml)

ゲーム起動時にプレイヤーデータを zip へまとめて保存する MOD です。スコアや設定が消えたときの復元用バックアップとして使えます。

## 機能

- ゲーム起動時 (プラグイン有効化時) に自動でバックアップを作成する
- `Application.persistentDataPath` 配下のファイルとフォルダを一時フォルダへコピーしてから zip 化する
- バックアップファイル名は `<ゲームバージョン>_<yyyyMMdd>.zip` (例: `1.11.1_20261114.zip`)
- バックアップ先フォルダは設定で変更できる (既定はドキュメントフォルダの `BSPlayerDataBackup`)
- 同日に再度起動した場合は同じ名前の zip を削除して作り直す
- バックアップ先フォルダ自身 (`BSPlayerDataBackup`) はコピー対象から除外する
- 失敗しても例外をログに出すだけでゲームの起動は継続する

## 動作環境

| 項目 | 内容 |
| --- | --- |
| Beat Saber | 1.11.1 |
| 依存 MOD | BSIPA 4.0.0 以降 |

## インストール

1. Beat Saber を 1.11.1 に合わせる (BSManager などのバージョン管理ツールを推奨)
2. BSIPA と上記の依存 MOD をインストールする
3. [Releases](https://github.com/denpadokei/BSPlayerDataBackup/releases) から `BSPlayerDataBackup-0.0.1-bs1.11.1.zip` をダウンロードし、`Plugins` フォルダをゲームフォルダへ上書きコピーする

## 使い方

インストール後は操作不要です。ゲームを起動するたびにバックアップが作成されます。

- 既定の保存先: `ドキュメント\BSPlayerDataBackup\<ゲームバージョン>_<日付>.zip`
- 復元するときは zip を展開し、中身を `%USERPROFILE%\AppData\LocalLow\Beat Games\Beat Saber` へ戻します (ゲームを終了した状態で行ってください)。

## 設定

| 項目 | 型 | 既定値 | 説明 |
| --- | --- | --- | --- |
| `BackupPath` | string | `ドキュメント\BSPlayerDataBackup` | バックアップ zip を出力するフォルダ |

設定は `UserData/BSPlayerDataBackup.json` に保存されます。

## ビルド

```powershell
git clone https://github.com/denpadokei/BSPlayerDataBackup.git
cd BSPlayerDataBackup
dotnet build .\BSPlayerDataBackup\BSPlayerDataBackup.csproj -c Release
```

`BeatSaberDir` (または `GameDirectory`) に Beat Saber のインストール先を設定すると、ゲームの参照アセンブリを解決できます。

GitHub Actions の `build-and-release.yml` が push ごとにビルドし、`X.X.X` 形式のタグを push するとリリースを作成します。

## ライセンス

ライセンスファイルはありません。
