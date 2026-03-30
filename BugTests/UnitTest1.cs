namespace BugTests;

[TestClass]
public class BugStateMachineTests
{
    [TestMethod]
    public void InitialState_IsNewDefect()
    {
        var bug = new Bug();
        Assert.AreEqual(BugState.NewDefect, bug.State);
    }

    [TestMethod]
    public void Report_FromNewDefect_GoesToTriage()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Triage_AssignToDeveloper_GoesToFixing()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        bug.Fire(BugTrigger.AssignToDeveloper);
        Assert.AreEqual(BugState.Fixing, bug.State);
    }

    [TestMethod]
    public void Triage_MarkNotABug_GoesToNotABug()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        bug.Fire(BugTrigger.MarkNotABug);
        Assert.AreEqual(BugState.NotABug, bug.State);
    }

    [TestMethod]
    public void Triage_MarkWontFix_GoesToWontFix()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        bug.Fire(BugTrigger.MarkWontFix);
        Assert.AreEqual(BugState.WontFix, bug.State);
    }

    [TestMethod]
    public void Triage_MarkDuplicate_GoesToDuplicate()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        bug.Fire(BugTrigger.MarkDuplicate);
        Assert.AreEqual(BugState.Duplicate, bug.State);
    }

    [TestMethod]
    public void Fixing_CannotReproduce_GoesToNotReproducible()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.CannotReproduce);
        Assert.AreEqual(BugState.NotReproducible, bug.State);
    }

    [TestMethod]
    public void Fixing_SubmitFixResolved_GoesToInReview()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.SubmitFixResolved);
        Assert.AreEqual(BugState.InReview, bug.State);
    }

    [TestMethod]
    public void InReview_ApproveReview_GoesToClosing()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.SubmitFixResolved);
        bug.Fire(BugTrigger.ApproveReview);
        Assert.AreEqual(BugState.Closing, bug.State);
    }

    [TestMethod]
    public void InReview_RejectReview_ReturnsToTriage()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.SubmitFixResolved);
        bug.Fire(BugTrigger.RejectReview);
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Closing_FinalClose_GoesToClosed()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.SubmitFixResolved);
        bug.Fire(BugTrigger.ApproveReview);
        bug.Fire(BugTrigger.FinalClose);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void Closed_Reopen_GoesToReopening()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.SubmitFixResolved);
        bug.Fire(BugTrigger.ApproveReview);
        bug.Fire(BugTrigger.FinalClose);
        bug.Fire(BugTrigger.Reopen);
        Assert.AreEqual(BugState.Reopening, bug.State);
    }

    [TestMethod]
    public void Reopening_CompleteReopen_GoesToTriage()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.SubmitFixResolved);
        bug.Fire(BugTrigger.ApproveReview);
        bug.Fire(BugTrigger.FinalClose);
        bug.Fire(BugTrigger.Reopen);
        bug.Fire(BugTrigger.CompleteReopen);
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void NotReproducible_ConfirmOk_GoesToClosing()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.CannotReproduce);
        bug.Fire(BugTrigger.ConfirmNotReproOk);
        Assert.AreEqual(BugState.Closing, bug.State);
    }

    [TestMethod]
    public void NotReproducible_ConfirmReject_ReturnsToTriage()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.CannotReproduce);
        bug.Fire(BugTrigger.ConfirmNotReproReject);
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Fixing_SubmitFixNotResolved_ReturnsToTriage()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.SubmitFixNotResolved);
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void Fixing_DeferNeedInfo_ReturnsToTriage()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.DeferNeedInfo);
        Assert.AreEqual(BugState.Triage, bug.State);
    }

    [TestMethod]
    public void FullPath_NewThroughReviewToClosed()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.SubmitFixResolved);
        bug.Fire(BugTrigger.ApproveReview);
        bug.Fire(BugTrigger.FinalClose);
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void Fire_FromNewDefect_AssignToDeveloper_ThrowsInvalidOperationException()
    {
        var bug = new Bug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.AssignToDeveloper));
    }

    [TestMethod]
    public void Fire_FromClosed_Report_ThrowsInvalidOperationException()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.SubmitFixResolved);
        bug.Fire(BugTrigger.ApproveReview);
        bug.Fire(BugTrigger.FinalClose);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.Report));
    }

    [TestMethod]
    public void Fire_FromTerminalNotABug_FireReport_ThrowsInvalidOperationException()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        bug.Fire(BugTrigger.MarkNotABug);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.Report));
    }

    [TestMethod]
    public void Fire_FromFixing_ApproveReview_ThrowsInvalidOperationException()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Report);
        bug.Fire(BugTrigger.AssignToDeveloper);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(BugTrigger.ApproveReview));
    }

    [TestMethod]
    public void CanFire_IsFalse_WhenTransitionNotAllowed()
    {
        var bug = new Bug();
        Assert.IsFalse(bug.CanFire(BugTrigger.FinalClose));
        bug.Fire(BugTrigger.Report);
        Assert.IsFalse(bug.CanFire(BugTrigger.FinalClose));
    }

    [TestMethod]
    public void CanFire_IsTrue_WhenTransitionAllowed()
    {
        var bug = new Bug();
        Assert.IsTrue(bug.CanFire(BugTrigger.Report));
        bug.Fire(BugTrigger.Report);
        Assert.IsTrue(bug.CanFire(BugTrigger.AssignToDeveloper));
    }
}
