using Stateless;

/// <summary>
/// Состояния жизненного цикла дефекта по workflow (тестировщик / продукт / разработка).
/// </summary>
public enum BugState
{
    NewDefect,
    Triage,
    Fixing,
    NotReproducible,
    /// <summary> Промежуточная проверка после фикса (расширение к диаграмме). </summary>
    InReview,
    Closing,
    Closed,
    NotABug,
    WontFix,
    Duplicate,
    Reopening
}

/// <summary>
/// События перехода автомата Stateless.
/// </summary>
public enum BugTrigger
{
    Report,
    AssignToDeveloper,
    MarkNotABug,
    MarkWontFix,
    MarkDuplicate,
    DeferNoTime,
    DeferNeedInfo,
    DeferSeparateSolution,
    DeferOtherProduct,
    CannotReproduce,
    SubmitFixResolved,
    SubmitFixNotResolved,
    ConfirmNotReproOk,
    ConfirmNotReproReject,
    ApproveReview,
    RejectReview,
    FinalClose,
    Reopen,
    CompleteReopen
}

/// <summary>
/// Автомат работы с багом на базе библиотеки Stateless.
/// </summary>
public class Bug
{
    private readonly StateMachine<BugState, BugTrigger> _machine;

    public BugState State => _machine.State;

    public Bug()
    {
        _machine = new StateMachine<BugState, BugTrigger>(BugState.NewDefect);
        ConfigureMachine();
    }

    private void ConfigureMachine()
    {
        _machine.Configure(BugState.NewDefect)
            .Permit(BugTrigger.Report, BugState.Triage);

        _machine.Configure(BugState.Triage)
            .Permit(BugTrigger.AssignToDeveloper, BugState.Fixing)
            .Permit(BugTrigger.MarkNotABug, BugState.NotABug)
            .Permit(BugTrigger.MarkWontFix, BugState.WontFix)
            .Permit(BugTrigger.MarkDuplicate, BugState.Duplicate);

        _machine.Configure(BugState.Fixing)
            .Permit(BugTrigger.CannotReproduce, BugState.NotReproducible)
            .Permit(BugTrigger.SubmitFixResolved, BugState.InReview)
            .Permit(BugTrigger.SubmitFixNotResolved, BugState.Triage)
            .Permit(BugTrigger.DeferNoTime, BugState.Triage)
            .Permit(BugTrigger.DeferNeedInfo, BugState.Triage)
            .Permit(BugTrigger.DeferSeparateSolution, BugState.Triage)
            .Permit(BugTrigger.DeferOtherProduct, BugState.Triage);

        _machine.Configure(BugState.InReview)
            .Permit(BugTrigger.ApproveReview, BugState.Closing)
            .Permit(BugTrigger.RejectReview, BugState.Triage);

        _machine.Configure(BugState.NotReproducible)
            .Permit(BugTrigger.ConfirmNotReproOk, BugState.Closing)
            .Permit(BugTrigger.ConfirmNotReproReject, BugState.Triage);

        _machine.Configure(BugState.Closing)
            .Permit(BugTrigger.FinalClose, BugState.Closed);

        _machine.Configure(BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopening);

        _machine.Configure(BugState.Reopening)
            .Permit(BugTrigger.CompleteReopen, BugState.Triage);

        _machine.Configure(BugState.NotABug);
        _machine.Configure(BugState.WontFix);
        _machine.Configure(BugState.Duplicate);
    }

    public void Fire(BugTrigger trigger) => _machine.Fire(trigger);

    public bool CanFire(BugTrigger trigger) => _machine.CanFire(trigger);
}

static class Program
{
    static void Main()
    {
        var bug = new Bug();
        Console.WriteLine($"Начальное состояние: {bug.State}");

        bug.Fire(BugTrigger.Report);
        Console.WriteLine($"После регистрации: {bug.State}");

        bug.Fire(BugTrigger.AssignToDeveloper);
        Console.WriteLine($"Назначен разработчику: {bug.State}");

        bug.Fire(BugTrigger.SubmitFixResolved);
        Console.WriteLine($"Фикс на ревью: {bug.State}");

        bug.Fire(BugTrigger.ApproveReview);
        Console.WriteLine($"Одобрено, на закрытие: {bug.State}");

        bug.Fire(BugTrigger.FinalClose);
        Console.WriteLine($"Закрыт: {bug.State}");
    }
}
