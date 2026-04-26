using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;
using System;

namespace BugTests
{
    [TestClass]
    public class BugStateMachineTests
    {
        private Bug _bug = null!;

        [TestInitialize]
        public void Setup()
        {
            _bug = new Bug();
        }

        [TestMethod]
        public void InitialState_ShouldBe_New()
        {
            Assert.AreEqual(Bug.State.New, _bug.CurrentState);
        }

        [TestMethod]
        public void StartAnalysis_FromNew_ShouldMoveToAnalysis()
        {
            _bug.StartAnalysis();
            Assert.AreEqual(Bug.State.Analysis, _bug.CurrentState);
        }

        [TestMethod]
        public void StartFixing_FromAnalysis_ShouldMoveToFixing()
        {
            _bug.StartAnalysis();
            _bug.StartFixing();
            Assert.AreEqual(Bug.State.Fixing, _bug.CurrentState);
        }

        [TestMethod]
        public void MarkNotABug_FromAnalysis_ShouldMoveToNotABug()
        {
            _bug.StartAnalysis();
            _bug.MarkNotABug();
            Assert.AreEqual(Bug.State.NotABug, _bug.CurrentState);
        }

        [TestMethod]
        public void MarkWontFix_FromAnalysis_ShouldMoveToWontFix()
        {
            _bug.StartAnalysis();
            _bug.MarkWontFix();
            Assert.AreEqual(Bug.State.WontFix, _bug.CurrentState);
        }

        [TestMethod]
        public void MarkDuplicate_FromAnalysis_ShouldMoveToDuplicate()
        {
            _bug.StartAnalysis();
            _bug.MarkDuplicate();
            Assert.AreEqual(Bug.State.Duplicate, _bug.CurrentState);
        }

        [TestMethod]
        public void ProblemSolved_FromFixing_ShouldMoveToClosed()
        {
            _bug.StartAnalysis();
            _bug.StartFixing();
            _bug.ProblemSolved();
            Assert.AreEqual(Bug.State.Closed, _bug.CurrentState);
        }

        [TestMethod]
        public void ProblemNotSolved_FromFixing_ShouldMoveToReturn()
        {
            _bug.StartAnalysis();
            _bug.StartFixing();
            _bug.ProblemNotSolved();
            Assert.AreEqual(Bug.State.Return, _bug.CurrentState);
        }

        [TestMethod]
        public void MarkNotReproducible_FromFixing_ShouldMoveToNotReproducible()
        {
            _bug.StartAnalysis();
            _bug.StartFixing();
            _bug.MarkNotReproducible();
            Assert.AreEqual(Bug.State.NotReproducible, _bug.CurrentState);
        }

        [TestMethod]
        public void ConfirmOK_FromNotReproducible_ShouldMoveToClosed()
        {
            _bug.StartAnalysis();
            _bug.StartFixing();
            _bug.MarkNotReproducible();
            _bug.ConfirmOK();
            Assert.AreEqual(Bug.State.Closed, _bug.CurrentState);
        }

        [TestMethod]
        public void ConfirmNotOK_FromNotReproducible_ShouldMoveToReturn()
        {
            _bug.StartAnalysis();
            _bug.StartFixing();
            _bug.MarkNotReproducible();
            _bug.ConfirmNotOK();
            Assert.AreEqual(Bug.State.Return, _bug.CurrentState);
        }

        [TestMethod]
        public void NoTimeNow_FromFixing_ShouldMoveToNoTime()
        {
            _bug.StartAnalysis();
            _bug.StartFixing();
            _bug.NoTimeNow();
            Assert.AreEqual(Bug.State.NoTime, _bug.CurrentState);
        }

        [TestMethod]
        public void NeedSeparateSolution_FromFixing_ShouldMoveToSeparateSolution()
        {
            _bug.StartAnalysis();
            _bug.StartFixing();
            _bug.NeedSeparateSolution();
            Assert.AreEqual(Bug.State.SeparateSolution, _bug.CurrentState);
        }

        [TestMethod]
        public void ProblemOtherProduct_FromFixing_ShouldMoveToOtherProduct()
        {
            _bug.StartAnalysis();
            _bug.StartFixing();
            _bug.ProblemOtherProduct();
            Assert.AreEqual(Bug.State.OtherProduct, _bug.CurrentState);
        }

        [TestMethod]
        public void NeedMoreInfo_FromFixing_ShouldMoveToMoreInfo()
        {
            _bug.StartAnalysis();
            _bug.StartFixing();
            _bug.NeedMoreInfo();
            Assert.AreEqual(Bug.State.MoreInfo, _bug.CurrentState);
        }

        [TestMethod]
        public void Reopen_FromClosed_ShouldMoveToReopened()
        {
            _bug.StartAnalysis();
            _bug.StartFixing();
            _bug.ProblemSolved();
            _bug.Reopen();
            Assert.AreEqual(Bug.State.Reopened, _bug.CurrentState);
        }

        [TestMethod]
        public void StartAnalysis_FromReopened_ShouldMoveToAnalysis()
        {
            _bug.StartAnalysis();
            _bug.StartFixing();
            _bug.ProblemSolved();
            _bug.Reopen();
            _bug.StartAnalysis();
            Assert.AreEqual(Bug.State.Analysis, _bug.CurrentState);
        }

        [TestMethod]
        public void ReturnToAnalysis_FromNotABug_ShouldMoveToAnalysis()
        {
            _bug.StartAnalysis();
            _bug.MarkNotABug();
            _bug.ReturnToAnalysis();
            Assert.AreEqual(Bug.State.Analysis, _bug.CurrentState);
        }

        [TestMethod]
        public void ReturnToAnalysis_FromNoTime_ShouldMoveToAnalysis()
        {
            _bug.StartAnalysis();
            _bug.StartFixing();
            _bug.NoTimeNow();
            _bug.ReturnToAnalysis();
            Assert.AreEqual(Bug.State.Analysis, _bug.CurrentState);
        }

        [TestMethod]
        public void StartAnalysis_FromReturn_ShouldMoveToAnalysis()
        {
            _bug.StartAnalysis();
            _bug.StartFixing();
            _bug.ProblemNotSolved();
            _bug.StartAnalysis();
            Assert.AreEqual(Bug.State.Analysis, _bug.CurrentState);
        }

        [TestMethod]
        public void StartFixing_FromNew_ShouldThrowException()
        {
            Assert.Throws<InvalidOperationException>(() => _bug.StartFixing());
        }

        [TestMethod]
        public void Reopen_FromNew_ShouldThrowException()
        {
            Assert.Throws<InvalidOperationException>(() => _bug.Reopen());
        }

        [TestMethod]
        public void ProblemSolved_FromAnalysis_ShouldThrowException()
        {
            _bug.StartAnalysis();
            Assert.Throws<InvalidOperationException>(() => _bug.ProblemSolved());
        }

        [TestMethod]
        public void ConfirmOK_FromNew_ShouldThrowException()
        {
            Assert.Throws<InvalidOperationException>(() => _bug.ConfirmOK());
        }

        [TestMethod]
        public void FullCycle_WithReopen_ShouldWork()
        {
            _bug.StartAnalysis();
            _bug.StartFixing();
            _bug.ProblemSolved();
            _bug.Reopen();
            _bug.StartAnalysis();
            _bug.StartFixing();
            _bug.ProblemSolved();
            Assert.AreEqual(Bug.State.Closed, _bug.CurrentState);
        }
    }
}