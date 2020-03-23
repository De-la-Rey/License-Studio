using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using MernokAssets;
using MernokClients;
using MernokPasswords;

namespace Mernok_RFID_Licence_Studio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IDictionary<string, ViewModel> _viewModels;

        public static MainView Window_Master;
        public static DefaultDisplayManager MasterdisplayManager;
        public static ViewModelReturnData SystemVMReturnData;
        public DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public static MernokAssetFile mernokAssetFile = new MernokAssetFile();
        public static MernokClientFile mernokClientFile = new MernokClientFile();

        private bool ViewsLoaded = false;

        public App() : base()
        {

            #region MernokAssets
            mernokAssetFile = MernokAssetManager.ReadMernokAssetFile();
            mernokClientFile = MernokClientManager.ReadMernokClientFile();           


            foreach (MernokAsset item in mernokAssetFile.mernokAssetList)
            {
                TagTypesL.MernokAssetType.Add(item);
            }

            var  MernokAssetGroups = TagTypesL.MernokAssetType.Select(t => t.Group).Distinct().ToList();
            var MernokAssetGroupStr = TagTypesL.MernokAssetType.Select(t => t.GroupName).Distinct().ToList();
            int indx = 0;
            foreach (var item in MernokAssetGroupStr)
            {
                TagTypesL.MernokAssetGroups.Add(new AssetGroups { GroupNumber = MernokAssetGroups[indx], GroupName = item });
                indx++;
            }

            TagTypesL.MernokAssetGroups = TagTypesL.MernokAssetGroups.OrderBy(t => t.GroupNumber).ToList();
            #endregion

            //Console.WriteLine();
            ////  <-- Keep this information secure! -->
            //Console.WriteLine("MachineName: {0}", Environment.MachineName);

            //Start main 
            SystemVMReturnData = new ViewModelReturnData();

            RunMainSequence();
        }

        private void RunMainSequence()
        {
            Dispatcher.Invoke(() =>
            {
                Window_Master = new MainView();
                _viewModels = InitializeMainViewModels(Window_Master);
            });

            //Instanciate views
            MasterdisplayManager = new DefaultDisplayManager(_viewModels);

            while(!ViewsLoaded)
            {

            }

            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0,0,10);
            dispatcherTimer.Start();


        }

        private IDictionary<string, ViewModel> InitializeMainViewModels(MainView window)
        {
            var models = new Dictionary<string, ViewModel>
            {
                { "Main", new MainViewModel(window) },
                { window.NoRWDView.Name, new NoRWDViewModel(window.NoRWDView) },
                { window.StartUpView.Name, new StartupViewModel(window.StartUpView) },
                { window.RuleTemplateView.Name, new RuleTemplateViewModel(window.RuleTemplateView) },
                { window.LicenceView.Name, new LicenceViewModel(window.LicenceView) },
                { window.NewCardAccessView.Name, new NewCardAccessViewModel(window.NewCardAccessView) },
                { window.NewIssuerCardView.Name, new NewIssuerCardViewModel(window.NewIssuerCardView) },
                { window.NewCardDetails1View.Name, new NewCardDetailsViewModel(window.NewCardDetails1View) },
                { window.NewCardVIDView.Name, new NewCardVIDViewModel(window.NewCardVIDView) },
                { window.NewCardVehicleNamesView.Name, new NewCardVehicleNamesViewModel(window.NewCardVehicleNamesView) },
                { window.NewCardTypeView.Name, new NewCardTypeViewModel(window.NewCardTypeView) },
                { window.NewCardGroupView.Name, new NewCardGroupViewModel(window.NewCardGroupView) },
                { window.CardError.Name, new CardFormatErrorViewModel(window.CardError) },
                { window.CardProgramComplete.Name, new CardProgramDoneViewModel(window.CardProgramComplete) },
                { window.ExitPromptView.Name, new ExitPromptViewModel(window.ExitPromptView) },
                { window.MenuView.Name, new MenuViewModel(window.MenuView) },
                { window.CardProgramFail.Name, new CardProgramFailViewModel(window.CardProgramFail) },
                { window.IssuerCardPrompView.Name, new IssuerCardPrompViewModel(window.IssuerCardPrompView) },
                { window.AboutView.Name, new AboutViewModel(window.AboutView) },
                {window.EditCardWarningView.Name, new EditCardWarningViewModel(window.EditCardWarningView) },
                { window.HelpView.Name, new HelpViewModel(window.HelpView) },
                { window.NewCardWarningView.Name, new NewCardWarningViewModel(window.NewCardWarningView) },
                { window.ProgramPromptView.Name, new ProgramPromptViewModel(window.ProgramPromptView) },
                { window.NewIssuerPresentView.Name, new NewIssuerPresentViewModel(window.NewIssuerPresentView) },
                {window.AdvancedPasswordView.Name, new AdvancedPasswordViewModel(window.AdvancedPasswordView) },
                {window.AdvancedMenuView.Name, new AdvancedMenuViewModel(window.AdvancedMenuView) },
                {window.NewMernokCardView.Name, new NewMernokCardViewModel(window.NewMernokCardView) }
            };

            ViewsLoaded = true;


            return models;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            MasterdisplayManager.Display(SystemVMReturnData);
        }


    }

}
