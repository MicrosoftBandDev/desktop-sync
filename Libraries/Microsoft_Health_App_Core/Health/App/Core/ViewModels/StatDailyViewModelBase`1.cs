﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.StatDailyViewModelBase`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  [PanelInfo(PanelName = "StatDaily")]
  public abstract class StatDailyViewModelBase<T> : PageViewModelBase, IInsightModel
    where T : ILoadableParameters
  {
    private const int NumberOfDaysToLoad = 10;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\StatDailyViewModelBase.cs");
    private readonly IDateTimeService dateTimeService;
    private readonly IInsightsProvider insightsProvider;
    private readonly IMessageBoxService messageBoxService;
    private readonly ISmoothNavService navigationService;
    private readonly IUserProfileService profileManager;
    private readonly IServiceLocator serviceLocator;
    private readonly IUserDailySummaryProvider userDailySummaryProvider;
    private IList<StatHistoryEntry> days;
    private bool isLoadingMore;
    private DateTimeOffset lastLoadedHistoryDay;
    private HealthCommand loadMoreCommand;
    private bool moreDataAvailable = true;
    private HealthCommand<StatHistoryEntry> openDayCommand;
    private DateTimeOffset? profileCreatedDate;
    private int version;
    private string insightMessage;
    private string insightActionMessage;
    private bool showInsight;

    public StatDailyViewModelBase(
      ISmoothNavService navigationService,
      IMessageBoxService messageBoxService,
      INetworkService networkService,
      IServiceLocator serviceLocator,
      IUserProfileService profileManager,
      IDateTimeService dateTimeService,
      IUserDailySummaryProvider userDailySummaryProvider,
      IInsightsProvider insightsProvider)
      : base(networkService)
    {
      if (navigationService == null)
        throw new ArgumentNullException(nameof (navigationService));
      if (messageBoxService == null)
        throw new ArgumentNullException(nameof (messageBoxService));
      if (serviceLocator == null)
        throw new ArgumentNullException(nameof (serviceLocator));
      if (profileManager == null)
        throw new ArgumentNullException(nameof (profileManager));
      if (dateTimeService == null)
        throw new ArgumentNullException(nameof (dateTimeService));
      if (userDailySummaryProvider == null)
        throw new ArgumentNullException(nameof (userDailySummaryProvider));
      if (insightsProvider == null)
        throw new ArgumentNullException(nameof (insightsProvider));
      this.navigationService = navigationService;
      this.messageBoxService = messageBoxService;
      this.serviceLocator = serviceLocator;
      this.profileManager = profileManager;
      this.dateTimeService = dateTimeService;
      this.userDailySummaryProvider = userDailySummaryProvider;
      this.insightsProvider = insightsProvider;
      this.profileCreatedDate = profileManager.CreatedOn;
      this.DayViewModel = this.serviceLocator.GetInstance<T>();
    }

    public virtual string DeviceId { get; set; }

    public T DayViewModel { get; private set; }

    public IList<StatHistoryEntry> Days
    {
      get => this.days;
      set => this.SetProperty<IList<StatHistoryEntry>>(ref this.days, value, nameof (Days));
    }

    public bool IsLoadingMore
    {
      get => this.isLoadingMore;
      set => this.SetProperty<bool>(ref this.isLoadingMore, value, nameof (IsLoadingMore));
    }

    public string InsightMessage
    {
      get => this.insightMessage;
      set => this.SetProperty<string>(ref this.insightMessage, value, nameof (InsightMessage));
    }

    public string InsightActionMessage
    {
      get => this.insightActionMessage;
      set => this.SetProperty<string>(ref this.insightActionMessage, value, nameof (InsightActionMessage));
    }

    public bool ShowInsight
    {
      get => this.showInsight;
      set => this.SetProperty<bool>(ref this.showInsight, value, nameof (ShowInsight));
    }

    public IList<string> Disclaimers => (IList<string>) new List<string>();

    public ICommand LoadMoreCommand => (ICommand) this.loadMoreCommand ?? (ICommand) (this.loadMoreCommand = new HealthCommand(new Action(this.LoadNewPage)));

    public ICommand OpenDayCommand => (ICommand) this.openDayCommand ?? (ICommand) (this.openDayCommand = new HealthCommand<StatHistoryEntry>((Action<StatHistoryEntry>) (entry => this.navigationService.Navigate(typeof (DetailsViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
    {
      ["Header"] = this.GetFormattedStyledHeader(entry.Count).ToSerializedString(),
      ["Subheader"] = entry.DateText,
      ["TileIcon"] = this.TileIcon,
      ["TargetViewModelType"] = typeof (T).FullName,
      ["Date"] = entry.StartDateText
    }))));

    protected abstract string TileIcon { get; }

    protected abstract StyledSpan GetFormattedStyledHeader(int count);

    protected abstract InsightDataUsedPivot InsightDataUsed { get; }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      ++this.version;
      this.Days = (IList<StatHistoryEntry>) new ObservableCollection<StatHistoryEntry>();
      this.moreDataAvailable = true;
      this.lastLoadedHistoryDay = this.dateTimeService.GetToday();
      Task task1 = this.DayViewModel.LoadAsync((IDictionary<string, string>) new Dictionary<string, string>());
      Task task2 = this.LoadNewDaysAsync(this.version, true);
      Task<IList<RaisedInsight>> insightTask = this.GetRaisedInsightsAsync();
      await Task.WhenAll(task1, task2, (Task) insightTask);
      if (insightTask.Result == null)
        return;
      InsightUtilities.PopulateInsightModel((IInsightModel) this, (IEnumerable<RaisedInsight>) insightTask.Result);
    }

    private async Task<IList<RaisedInsight>> GetRaisedInsightsAsync()
    {
      try
      {
        return await this.insightsProvider.GetStatInsightsAsync(this.InsightDataUsed, InsightTimespanPivot.Day);
      }
      catch (Exception ex)
      {
        StatDailyViewModelBase<T>.Logger.Error(ex, "Error fetching raised insights.");
      }
      return (IList<RaisedInsight>) null;
    }

    private async Task LoadNewDaysAsync(int localVersion, bool throwErrors = false)
    {
      this.IsLoadingMore = true;
      Exception ex = (Exception) null;
      try
      {
        if (!this.profileCreatedDate.HasValue)
        {
          try
          {
            await this.profileManager.RefreshUserProfileAsync(CancellationToken.None);
            this.profileCreatedDate = this.profileManager.CreatedOn;
          }
          catch (Exception ex1)
          {
            StatDailyViewModelBase<T>.Logger.Error(ex1, "Could not refresh user profile. Showing all days since registration date could not be determined.");
          }
        }
        foreach (UserDailySummaryGroup userDailySummaryGroup in (IEnumerable<UserDailySummaryGroup>) (await this.userDailySummaryProvider.GetPreviousUserDailySummaryGroupsAsync(this.lastLoadedHistoryDay, 10, this.DeviceId)).OrderByDescending<UserDailySummaryGroup, DateTimeOffset>((Func<UserDailySummaryGroup, DateTimeOffset>) (g => g.RequestRange.Low)))
        {
          if (this.version == localVersion)
          {
            if (this.profileCreatedDate.HasValue && userDailySummaryGroup.RequestRange.High < this.profileCreatedDate.Value)
            {
              this.moreDataAvailable = false;
              break;
            }
            this.Days.Add(new StatHistoryEntry(userDailySummaryGroup.RequestRange.Low, this.GetStatHistoryEntryCount(userDailySummaryGroup), false));
            this.lastLoadedHistoryDay = userDailySummaryGroup.RequestRange.Low;
          }
        }
      }
      catch (Exception ex2)
      {
        if (throwErrors)
          throw;
        else
          ex = ex2;
      }
      finally
      {
        if (this.version == localVersion)
          this.IsLoadingMore = false;
      }
      if (ex == null || this.version != localVersion)
        return;
      StatDailyViewModelBase<T>.Logger.Error(ex, "Loading new days failed");
      int num = (int) await this.messageBoxService.ShowAsync(AppResources.NetworkErrorBody, AppResources.NetworkErrorTitle, PortableMessageBoxButton.OK);
    }

    protected abstract int GetStatHistoryEntryCount(UserDailySummaryGroup userDailySummaryGroup);

    private async void LoadNewPage()
    {
      if (this.IsLoadingMore || this.LoadState != LoadState.Loaded || !this.moreDataAvailable)
        return;
      await this.LoadNewDaysAsync(this.version);
    }
  }
}
