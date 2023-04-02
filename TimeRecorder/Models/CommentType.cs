namespace TimeRecorder.Models;

public enum CommentType
{
    /// <summary>
    /// アプリ起動時のコメント
    /// </summary>
    RunApp,

    /// <summary>
    /// アプリ終了時のコメント
    /// </summary>
    CloseApp,

    /// <summary>
    /// アプリウィンドウがアクティブになった時のコメント
    /// </summary>
    Activated,

    /// <summary>
    /// アプリのウィンドウがアクティブ状態でなくなった時のコメント
    /// </summary>
    Deactivated,

    /// <summary>
    /// グループ作成時のコメント
    /// </summary>
    CreateGroup,

    /// <summary>
    /// ユーザーがコメント未入力でつけたコメント
    /// </summary>
    Default,
}