using Stateless;
using System;

namespace BugPro
{
    public class Bug
    {
        public enum State
        {
            New,                // НОВЫЙ ДЕФЕКТ
            Analysis,           // РАЗБОР ДЕФЕКТОВ
            Fixing,             // ИСПРАВЛЕНИЕ
            NotABug,            // НЕ ДЕФЕКТ
            WontFix,            // НЕ ИСПРАВЛЯТЬ
            Duplicate,          // ДУБЛИРОВАНИЕ
            NotReproducible,    // НЕ ВОСПРОИЗВОДИМО
            Closed,             // ЗАКРЫТИЕ
            Return,             // ВОЗВРАТ
            Reopened,           // ПЕРЕОТКРЫТИЕ
            NoTime,             // НЕТ ВРЕМЕНИ СЕЙЧАС
            SeparateSolution,   // НУЖНО ОТДЕЛЬНОЕ РЕШЕНИЕ
            OtherProduct,       // ПРОБЛЕМА ДРУГОГО ПРОДУКТА
            MoreInfo            // НУЖНО БОЛЬШЕ ИНФОРМАЦИИ
        }

        public enum Trigger
        {
            StartAnalysis,      // Начать разбор дефектов
            StartFixing,        // Начать исправление
            MarkNotABug,        // Пометить как "не дефект"
            MarkWontFix,        // Пометить как "не исправлять"
            MarkDuplicate,      // Пометить как дублирование
            MarkNotReproducible,// Пометить как "не воспроизводимо"
            ProblemSolved,      // Проблема решена (ДА)
            ProblemNotSolved,   // Проблема не решена (НЕТ)
            Close,              // Закрыть
            ReturnToAnalysis,   // Вернуть на разбор
            Reopen,             // Переоткрыть
            NoTimeNow,          // Нет времени сейчас
            NeedSeparateSolution,// Нужно отдельное решение
            ProblemOtherProduct,// Проблема другого продукта
            NeedMoreInfo,       // Нужно больше информации
            ConfirmOK,          // Подтвердить OK
            ConfirmNotOK        // Подтвердить не OK
        }

        private readonly StateMachine<State, Trigger> _machine;

        public Bug()
        {
            _machine = new StateMachine<State, Trigger>(State.New);

            _machine.Configure(State.New)
                .Permit(Trigger.StartAnalysis, State.Analysis);

            _machine.Configure(State.Analysis)
                .Permit(Trigger.StartFixing, State.Fixing)
                .Permit(Trigger.MarkNotABug, State.NotABug)
                .Permit(Trigger.MarkWontFix, State.WontFix)
                .Permit(Trigger.MarkDuplicate, State.Duplicate);

            _machine.Configure(State.Fixing)
                .Permit(Trigger.ProblemSolved, State.Closed)
                .Permit(Trigger.ProblemNotSolved, State.Return)
                .Permit(Trigger.MarkNotReproducible, State.NotReproducible)
                .Permit(Trigger.NoTimeNow, State.NoTime)
                .Permit(Trigger.NeedSeparateSolution, State.SeparateSolution)
                .Permit(Trigger.ProblemOtherProduct, State.OtherProduct)
                .Permit(Trigger.NeedMoreInfo, State.MoreInfo);

            _machine.Configure(State.NotReproducible)
                .Permit(Trigger.ConfirmOK, State.Closed)
                .Permit(Trigger.ConfirmNotOK, State.Return);

            _machine.Configure(State.NotABug)
                .Permit(Trigger.ReturnToAnalysis, State.Analysis);

            _machine.Configure(State.WontFix)
                .Permit(Trigger.ReturnToAnalysis, State.Analysis);

            _machine.Configure(State.Duplicate)
                .Permit(Trigger.ReturnToAnalysis, State.Analysis);

            _machine.Configure(State.Return)
                .Permit(Trigger.StartAnalysis, State.Analysis);

            _machine.Configure(State.NoTime)
                .Permit(Trigger.ReturnToAnalysis, State.Analysis);

            _machine.Configure(State.SeparateSolution)
                .Permit(Trigger.ReturnToAnalysis, State.Analysis);

            _machine.Configure(State.OtherProduct)
                .Permit(Trigger.ReturnToAnalysis, State.Analysis);

            _machine.Configure(State.MoreInfo)
                .Permit(Trigger.ReturnToAnalysis, State.Analysis);

            _machine.Configure(State.Closed)
                .Permit(Trigger.Reopen, State.Reopened);

            _machine.Configure(State.Reopened)
                .Permit(Trigger.StartAnalysis, State.Analysis);
        }

        public State CurrentState => _machine.State;

        public void StartAnalysis()
        {
            _machine.Fire(Trigger.StartAnalysis);
            Console.WriteLine($"Начат разбор дефектов. Состояние: {_machine.State}");
        }

        public void StartFixing()
        {
            _machine.Fire(Trigger.StartFixing);
            Console.WriteLine($"Начато исправление. Состояние: {_machine.State}");
        }

        public void MarkNotABug()
        {
            _machine.Fire(Trigger.MarkNotABug);
            Console.WriteLine($"Помечено как 'не дефект'. Состояние: {_machine.State}");
        }

        public void MarkWontFix()
        {
            _machine.Fire(Trigger.MarkWontFix);
            Console.WriteLine($"Помечено как 'не исправлять'. Состояние: {_machine.State}");
        }

        public void MarkDuplicate()
        {
            _machine.Fire(Trigger.MarkDuplicate);
            Console.WriteLine($"Помечено как дублирование. Состояние: {_machine.State}");
        }

        public void MarkNotReproducible()
        {
            _machine.Fire(Trigger.MarkNotReproducible);
            Console.WriteLine($"Помечено как 'не воспроизводимо'. Состояние: {_machine.State}");
        }

        public void ProblemSolved()
        {
            _machine.Fire(Trigger.ProblemSolved);
            Console.WriteLine($"Проблема решена. Состояние: {_machine.State}");
        }

        public void ProblemNotSolved()
        {
            _machine.Fire(Trigger.ProblemNotSolved);
            Console.WriteLine($"Проблема не решена. Состояние: {_machine.State}");
        }

        public void Close()
        {
            _machine.Fire(Trigger.Close);
            Console.WriteLine($"Дефект закрыт. Состояние: {_machine.State}");
        }

        public void ReturnToAnalysis()
        {
            _machine.Fire(Trigger.ReturnToAnalysis);
            Console.WriteLine($"Возврат на разбор. Состояние: {_machine.State}");
        }

        public void Reopen()
        {
            _machine.Fire(Trigger.Reopen);
            Console.WriteLine($"Дефект переоткрыт. Состояние: {_machine.State}");
        }

        public void NoTimeNow()
        {
            _machine.Fire(Trigger.NoTimeNow);
            Console.WriteLine($"Нет времени сейчас. Состояние: {_machine.State}");
        }

        public void NeedSeparateSolution()
        {
            _machine.Fire(Trigger.NeedSeparateSolution);
            Console.WriteLine($"Нужно отдельное решение. Состояние: {_machine.State}");
        }

        public void ProblemOtherProduct()
        {
            _machine.Fire(Trigger.ProblemOtherProduct);
            Console.WriteLine($"Проблема другого продукта. Состояние: {_machine.State}");
        }

        public void NeedMoreInfo()
        {
            _machine.Fire(Trigger.NeedMoreInfo);
            Console.WriteLine($"Нужно больше информации. Состояние: {_machine.State}");
        }

        public void ConfirmOK()
        {
            _machine.Fire(Trigger.ConfirmOK);
            Console.WriteLine($"Подтверждено OK. Состояние: {_machine.State}");
        }

        public void ConfirmNotOK()
        {
            _machine.Fire(Trigger.ConfirmNotOK);
            Console.WriteLine($"Подтверждено не OK. Состояние: {_machine.State}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Демонстрация WorkFlow работы с багом ===\n");

            var bug = new Bug();
            Console.WriteLine($"Начальное состояние: {bug.CurrentState}\n");

            bug.StartAnalysis();
            bug.StartFixing();
            bug.ProblemSolved();
            Console.WriteLine($"\nФинальное состояние: {bug.CurrentState}\n");

            Console.WriteLine("--- Тест переоткрытия ---");
            var bug2 = new Bug();
            bug2.StartAnalysis();
            bug2.StartFixing();
            bug2.ProblemSolved();
            bug2.Reopen();
            bug2.StartAnalysis();
            Console.WriteLine($"Финальное состояние: {bug2.CurrentState}\n");

            Console.WriteLine("--- Тест невоспроизводимости ---");
            var bug3 = new Bug();
            bug3.StartAnalysis();
            bug3.StartFixing();
            bug3.MarkNotReproducible();
            bug3.ConfirmNotOK();
            bug3.StartAnalysis();
            Console.WriteLine($"Финальное состояние: {bug3.CurrentState}\n");

            Console.WriteLine("=== Демонстрация завершена ===");
        }
    }
}