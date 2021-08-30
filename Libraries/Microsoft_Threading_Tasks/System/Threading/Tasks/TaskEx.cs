﻿// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.TaskEx
// Assembly: Microsoft.Threading.Tasks, Version=1.0.12.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 1C7D529D-87EC-4BDC-BDE0-2E9494F3B5AE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Threading_Tasks.dll

using Microsoft.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace System.Threading.Tasks
{
  public static class TaskEx
  {
    private const string ArgumentOutOfRange_TimeoutNonNegativeOrMinusOne = "The timeout must be non-negative or -1, and it must be less than or equal to Int32.MaxValue.";
    private static Task s_preCompletedTask = (Task) TaskEx.FromResult<bool>(false);

    public static Task Run(Action action) => TaskEx.Run(action, CancellationToken.None);

    public static Task Run(Action action, CancellationToken cancellationToken) => Task.Factory.StartNew(action, cancellationToken, TaskCreationOptions.None, TaskScheduler.Default);

    public static Task<TResult> Run<TResult>(Func<TResult> function) => TaskEx.Run<TResult>(function, CancellationToken.None);

    public static Task<TResult> Run<TResult>(
      Func<TResult> function,
      CancellationToken cancellationToken)
    {
      return Task.Factory.StartNew<TResult>(function, cancellationToken, TaskCreationOptions.None, TaskScheduler.Default);
    }

    public static Task Run(Func<Task> function) => TaskEx.Run(function, CancellationToken.None);

    public static Task Run(Func<Task> function, CancellationToken cancellationToken) => TaskEx.Run<Task>(function, cancellationToken).Unwrap();

    public static Task<TResult> Run<TResult>(Func<Task<TResult>> function) => TaskEx.Run<TResult>(function, CancellationToken.None);

    public static Task<TResult> Run<TResult>(
      Func<Task<TResult>> function,
      CancellationToken cancellationToken)
    {
      return TaskEx.Run<Task<TResult>>(function, cancellationToken).Unwrap<TResult>();
    }

    public static Task Delay(int dueTime) => TaskEx.Delay(dueTime, CancellationToken.None);

    public static Task Delay(TimeSpan dueTime) => TaskEx.Delay(dueTime, CancellationToken.None);

    public static Task Delay(TimeSpan dueTime, CancellationToken cancellationToken)
    {
      long totalMilliseconds = (long) dueTime.TotalMilliseconds;
      if (totalMilliseconds < -1L || totalMilliseconds > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (dueTime), "The timeout must be non-negative or -1, and it must be less than or equal to Int32.MaxValue.");
      Contract.EndContractBlock();
      return TaskEx.Delay((int) totalMilliseconds, cancellationToken);
    }

    public static Task Delay(int dueTime, CancellationToken cancellationToken)
    {
      if (dueTime < -1)
        throw new ArgumentOutOfRangeException(nameof (dueTime), "The timeout must be non-negative or -1, and it must be less than or equal to Int32.MaxValue.");
      Contract.EndContractBlock();
      if (cancellationToken.IsCancellationRequested)
        return new Task((Action) (() => { }), cancellationToken);
      if (dueTime == 0)
        return TaskEx.s_preCompletedTask;
      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      CancellationTokenRegistration ctr = new CancellationTokenRegistration();
      Timer timer = (Timer) null;
      timer = new Timer((TimerCallback) (state =>
      {
        ctr.Dispose();
        timer.Dispose();
        tcs.TrySetResult(true);
        TimerManager.Remove(timer);
      }), (object) null, -1, -1);
      TimerManager.Add(timer);
      if (cancellationToken.CanBeCanceled)
        ctr = cancellationToken.Register((Action) (() =>
        {
          timer.Dispose();
          tcs.TrySetCanceled();
          TimerManager.Remove(timer);
        }));
      timer.Change(dueTime, -1);
      return (Task) tcs.Task;
    }

    public static Task WhenAll(params Task[] tasks) => TaskEx.WhenAll((IEnumerable<Task>) tasks);

    public static Task<TResult[]> WhenAll<TResult>(params Task<TResult>[] tasks) => TaskEx.WhenAll<TResult>((IEnumerable<Task<TResult>>) tasks);

    public static Task WhenAll(IEnumerable<Task> tasks) => (Task) TaskEx.WhenAllCore<object>(tasks, (Action<Task[], TaskCompletionSource<object>>) ((completedTasks, tcs) => tcs.TrySetResult((object) null)));

    public static Task<TResult[]> WhenAll<TResult>(IEnumerable<Task<TResult>> tasks) => TaskEx.WhenAllCore<TResult[]>(tasks.Cast<Task>(), (Action<Task[], TaskCompletionSource<TResult[]>>) ((completedTasks, tcs) => tcs.TrySetResult(((IEnumerable<Task>) completedTasks).Select<Task, TResult>((Func<Task, TResult>) (t => ((Task<TResult>) t).Result)).ToArray<TResult>())));

    private static Task<TResult> WhenAllCore<TResult>(
      IEnumerable<Task> tasks,
      Action<Task[], TaskCompletionSource<TResult>> setResultAction)
    {
      if (tasks == null)
        throw new ArgumentNullException(nameof (tasks));
      Contract.EndContractBlock();
      Contract.Assert(setResultAction != null);
      TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
      if (!(tasks is Task[] taskArray))
        taskArray = tasks.ToArray<Task>();
      Task[] tasks1 = taskArray;
      if (tasks1.Length == 0)
        setResultAction(tasks1, tcs);
      else
        Task.Factory.ContinueWhenAll(tasks1, (Action<Task[]>) (completedTasks =>
        {
          List<Exception> targetList = (List<Exception>) null;
          bool flag = false;
          foreach (Task completedTask in completedTasks)
          {
            if (completedTask.IsFaulted)
              TaskEx.AddPotentiallyUnwrappedExceptions(ref targetList, (Exception) completedTask.Exception);
            else if (completedTask.IsCanceled)
              flag = true;
          }
          if (targetList != null && targetList.Count > 0)
            tcs.TrySetException((IEnumerable<Exception>) targetList);
          else if (flag)
            tcs.TrySetCanceled();
          else
            setResultAction(completedTasks, tcs);
        }), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
      return tcs.Task;
    }

    public static Task<Task> WhenAny(params Task[] tasks) => TaskEx.WhenAny((IEnumerable<Task>) tasks);

    public static Task<Task> WhenAny(IEnumerable<Task> tasks)
    {
      if (tasks == null)
        throw new ArgumentNullException(nameof (tasks));
      Contract.EndContractBlock();
      TaskCompletionSource<Task> tcs = new TaskCompletionSource<Task>();
      TaskFactory factory = Task.Factory;
      if (!(tasks is Task[] tasks1))
        tasks1 = tasks.ToArray<Task>();
      Func<Task, bool> continuationFunction = (Func<Task, bool>) (completed => tcs.TrySetResult(completed));
      CancellationToken none = CancellationToken.None;
      TaskScheduler scheduler = TaskScheduler.Default;
      factory.ContinueWhenAny<bool>(tasks1, continuationFunction, none, TaskContinuationOptions.ExecuteSynchronously, scheduler);
      return tcs.Task;
    }

    public static Task<Task<TResult>> WhenAny<TResult>(params Task<TResult>[] tasks) => TaskEx.WhenAny<TResult>((IEnumerable<Task<TResult>>) tasks);

    public static Task<Task<TResult>> WhenAny<TResult>(IEnumerable<Task<TResult>> tasks)
    {
      if (tasks == null)
        throw new ArgumentNullException(nameof (tasks));
      Contract.EndContractBlock();
      TaskCompletionSource<Task<TResult>> tcs = new TaskCompletionSource<Task<TResult>>();
      TaskFactory factory = Task.Factory;
      if (!(tasks is Task<TResult>[] tasks1))
        tasks1 = tasks.ToArray<Task<TResult>>();
      Func<Task<TResult>, bool> continuationFunction = (Func<Task<TResult>, bool>) (completed => tcs.TrySetResult(completed));
      CancellationToken none = CancellationToken.None;
      TaskScheduler scheduler = TaskScheduler.Default;
      factory.ContinueWhenAny<TResult, bool>(tasks1, continuationFunction, none, TaskContinuationOptions.ExecuteSynchronously, scheduler);
      return tcs.Task;
    }

    public static Task<TResult> FromResult<TResult>(TResult result)
    {
      TaskCompletionSource<TResult> completionSource = new TaskCompletionSource<TResult>((object) result);
      completionSource.TrySetResult(result);
      return completionSource.Task;
    }

    public static YieldAwaitable Yield() => new YieldAwaitable();

    private static void AddPotentiallyUnwrappedExceptions(
      ref List<Exception> targetList,
      Exception exception)
    {
      AggregateException aggregateException = exception as AggregateException;
      Contract.Assert(exception != null);
      Contract.Assert(aggregateException == null || aggregateException.InnerExceptions.Count > 0);
      if (targetList == null)
        targetList = new List<Exception>();
      if (aggregateException != null)
        targetList.Add(aggregateException.InnerExceptions.Count == 1 ? exception.InnerException : exception);
      else
        targetList.Add(exception);
    }
  }
}
