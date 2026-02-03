using System.ComponentModel;
using System.Windows.Forms;

namespace YTPPlusDeluxe
{
    partial class YTPPlusDeluxe
    {
        private IContainer components = null;
        private CheckBox InsertTransitions;
        private CheckBox effect_RandomSound;
        private CheckBox effect_Reverse;
        private CheckBox effect_SpeedUp;
        private CheckBox effect_SlowDown;
        private CheckBox effect_Chorus;
        private CheckBox effect_Vibrato;
        private CheckBox effect_Stutter;
        private CheckBox effect_Earrape;
        private CheckBox effect_AutoTune;
        private CheckBox effect_Dance;
        private CheckBox effect_Squidward;
        private CheckBox effect_Invert;
        private CheckBox effect_Rainbow;
        private CheckBox effect_Mirror;
        private CheckBox effect_Sus;
        private CheckBox effect_Explosion;
        private CheckBox effect_FrameShuffle;
        private CheckBox effect_MemeInjection;
        private CheckBox effect_SentenceMixing;
        private CheckBox effect_RandomClipShuffle;
        private CheckBox effect_RandomCuts;
        private CheckBox pluginTest;
        private CheckBox InsertIntro;
        private CheckBox InsertOutro;
        private NumericUpDown Clips;
        private NumericUpDown WidthSet;
        private NumericUpDown HeightSet;
        private NumericUpDown MinStreamDur;
        private NumericUpDown MaxStreamDur;
        private TextBox Intro;
        private TextBox Outro;
        private TextBox TransitionDir;
        private TextBox Material;
        private ListBox VideoSources;
        private ListBox AudioSources;
        private ListBox ImageSources;
        private ListBox GifSources;
        private ListBox TransitionSources;
        private ListBox UrlSources;
        private Button AddVideo;
        private Button AddAudio;
        private Button AddImage;
        private Button AddGif;
        private Button AddTransition;
        private Button AddUrl;
        private Button Preview;
        private ComboBox ProjectType;
        private ComboBox OutputFormat;
        private Button Render;
        private Button SaveAs;
        private Button AddMaterial;
        private Button ClearMaterial;
        private Button PausePlay;
        private Button Start;
        private Button End;
        private ProgressBar progressBar1;
        private Label MaterialLabel;
        private Label RenderSettingsLabel;
        private Label VideoSourcesLabel;
        private Label AudioSourcesLabel;
        private Label ImageSourcesLabel;
        private Label GifSourcesLabel;
        private Label TransitionSourcesLabel;
        private Label UrlSourcesLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            InsertTransitions = new CheckBox();
            effect_RandomSound = new CheckBox();
            effect_Reverse = new CheckBox();
            effect_SpeedUp = new CheckBox();
            effect_SlowDown = new CheckBox();
            effect_Chorus = new CheckBox();
            effect_Vibrato = new CheckBox();
            effect_Stutter = new CheckBox();
            effect_Earrape = new CheckBox();
            effect_AutoTune = new CheckBox();
            effect_Dance = new CheckBox();
            effect_Squidward = new CheckBox();
            effect_Invert = new CheckBox();
            effect_Rainbow = new CheckBox();
            effect_Mirror = new CheckBox();
            effect_Sus = new CheckBox();
            effect_Explosion = new CheckBox();
            effect_FrameShuffle = new CheckBox();
            effect_MemeInjection = new CheckBox();
            effect_SentenceMixing = new CheckBox();
            effect_RandomClipShuffle = new CheckBox();
            effect_RandomCuts = new CheckBox();
            pluginTest = new CheckBox();
            InsertIntro = new CheckBox();
            InsertOutro = new CheckBox();
            Clips = new NumericUpDown();
            WidthSet = new NumericUpDown();
            HeightSet = new NumericUpDown();
            MinStreamDur = new NumericUpDown();
            MaxStreamDur = new NumericUpDown();
            Intro = new TextBox();
            Outro = new TextBox();
            TransitionDir = new TextBox();
            Material = new TextBox();
            VideoSources = new ListBox();
            AudioSources = new ListBox();
            ImageSources = new ListBox();
            GifSources = new ListBox();
            TransitionSources = new ListBox();
            UrlSources = new ListBox();
            AddVideo = new Button();
            AddAudio = new Button();
            AddImage = new Button();
            AddGif = new Button();
            AddTransition = new Button();
            AddUrl = new Button();
            Preview = new Button();
            ProjectType = new ComboBox();
            OutputFormat = new ComboBox();
            Render = new Button();
            SaveAs = new Button();
            AddMaterial = new Button();
            ClearMaterial = new Button();
            PausePlay = new Button();
            Start = new Button();
            End = new Button();
            progressBar1 = new ProgressBar();
            MaterialLabel = new Label();
            RenderSettingsLabel = new Label();
            VideoSourcesLabel = new Label();
            AudioSourcesLabel = new Label();
            ImageSourcesLabel = new Label();
            GifSourcesLabel = new Label();
            TransitionSourcesLabel = new Label();
            UrlSourcesLabel = new Label();

            ((ISupportInitialize)Clips).BeginInit();
            ((ISupportInitialize)WidthSet).BeginInit();
            ((ISupportInitialize)HeightSet).BeginInit();
            ((ISupportInitialize)MinStreamDur).BeginInit();
            ((ISupportInitialize)MaxStreamDur).BeginInit();

            SuspendLayout();

            InsertTransitions.Text = "Insert Transitions";
            InsertTransitions.Location = new System.Drawing.Point(12, 12);
            InsertTransitions.Checked = true;

            pluginTest.Text = "Plugin Test";
            pluginTest.Location = new System.Drawing.Point(12, 40);

            InsertIntro.Text = "Insert Intro";
            InsertIntro.Location = new System.Drawing.Point(12, 68);

            InsertOutro.Text = "Insert Outro";
            InsertOutro.Location = new System.Drawing.Point(12, 96);

            effect_Reverse.Text = "Reverse";
            effect_Reverse.Location = new System.Drawing.Point(12, 150);
            effect_Reverse.Checked = true;

            effect_Stutter.Text = "Stutter";
            effect_Stutter.Location = new System.Drawing.Point(12, 175);
            effect_Stutter.Checked = true;

            effect_Earrape.Text = "Earrape";
            effect_Earrape.Location = new System.Drawing.Point(12, 200);
            effect_Earrape.Checked = true;

            effect_Rainbow.Text = "Rainbow";
            effect_Rainbow.Location = new System.Drawing.Point(12, 225);
            effect_Rainbow.Checked = true;

            effect_Mirror.Text = "Mirror";
            effect_Mirror.Location = new System.Drawing.Point(12, 250);
            effect_Mirror.Checked = true;

            effect_Sus.Text = "Sus";
            effect_Sus.Location = new System.Drawing.Point(12, 275);
            effect_Sus.Checked = true;

            Render.Text = "Render";
            Render.Location = new System.Drawing.Point(12, 400);
            Render.Click += Render_Click;

            SaveAs.Text = "Save As";
            SaveAs.Location = new System.Drawing.Point(100, 400);

            Preview.Text = "Preview";
            Preview.Location = new System.Drawing.Point(188, 400);
            Preview.Click += Preview_Click;

            AddMaterial.Text = "Add Material";
            AddMaterial.Location = new System.Drawing.Point(12, 320);
            AddMaterial.Click += AddMaterial_Click;

            ClearMaterial.Text = "Clear Material";
            ClearMaterial.Location = new System.Drawing.Point(120, 320);
            ClearMaterial.Click += ClearMaterial_Click;

            PausePlay.Text = "▶️";
            PausePlay.Location = new System.Drawing.Point(12, 360);
            PausePlay.Click += PausePlay_Click;

            Start.Text = "|<";
            Start.Location = new System.Drawing.Point(70, 360);
            Start.Click += Start_Click;

            End.Text = ">|";
            End.Location = new System.Drawing.Point(128, 360);
            End.Click += End_Click;

            progressBar1.Location = new System.Drawing.Point(12, 440);
            progressBar1.Size = new System.Drawing.Size(320, 18);

            Material.Multiline = true;
            Material.ScrollBars = ScrollBars.Vertical;
            Material.Location = new System.Drawing.Point(12, 210);
            Material.Size = new System.Drawing.Size(320, 100);

            MaterialLabel.Text = "Materials";
            MaterialLabel.Location = new System.Drawing.Point(12, 190);

            RenderSettingsLabel.Text = "Render Settings";
            RenderSettingsLabel.Location = new System.Drawing.Point(12, 130);

            VideoSourcesLabel.Text = "Video Sources";
            VideoSourcesLabel.Location = new System.Drawing.Point(360, 12);

            AudioSourcesLabel.Text = "Audio Sources";
            AudioSourcesLabel.Location = new System.Drawing.Point(360, 100);

            ImageSourcesLabel.Text = "Image Sources";
            ImageSourcesLabel.Location = new System.Drawing.Point(360, 190);

            GifSourcesLabel.Text = "GIF Sources";
            GifSourcesLabel.Location = new System.Drawing.Point(360, 280);

            TransitionSourcesLabel.Text = "Transitions";
            TransitionSourcesLabel.Location = new System.Drawing.Point(360, 370);

            UrlSourcesLabel.Text = "URL Sources";
            UrlSourcesLabel.Location = new System.Drawing.Point(360, 460);

            ProjectType.DropDownStyle = ComboBoxStyle.DropDownList;
            ProjectType.Items.AddRange(new object[] { "Generic", "YTP Tennis", "Collab Entry", "YTPMV" });
            ProjectType.Location = new System.Drawing.Point(140, 12);

            OutputFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            OutputFormat.Items.AddRange(new object[] { "mp4", "wmv", "avi", "mkv" });
            OutputFormat.Location = new System.Drawing.Point(140, 40);

            VideoSources.Location = new System.Drawing.Point(360, 30);
            VideoSources.Size = new System.Drawing.Size(260, 70);
            AudioSources.Location = new System.Drawing.Point(360, 120);
            AudioSources.Size = new System.Drawing.Size(260, 70);
            ImageSources.Location = new System.Drawing.Point(360, 210);
            ImageSources.Size = new System.Drawing.Size(260, 70);
            GifSources.Location = new System.Drawing.Point(360, 300);
            GifSources.Size = new System.Drawing.Size(260, 70);
            TransitionSources.Location = new System.Drawing.Point(360, 390);
            TransitionSources.Size = new System.Drawing.Size(260, 70);
            UrlSources.Location = new System.Drawing.Point(360, 480);
            UrlSources.Size = new System.Drawing.Size(260, 70);

            AddVideo.Text = "Add Video";
            AddVideo.Location = new System.Drawing.Point(630, 30);
            AddVideo.Click += AddVideo_Click;

            AddAudio.Text = "Add Audio";
            AddAudio.Location = new System.Drawing.Point(630, 120);
            AddAudio.Click += AddAudio_Click;

            AddImage.Text = "Add Image";
            AddImage.Location = new System.Drawing.Point(630, 210);
            AddImage.Click += AddImage_Click;

            AddGif.Text = "Add GIF";
            AddGif.Location = new System.Drawing.Point(630, 300);
            AddGif.Click += AddGif_Click;

            AddTransition.Text = "Add Transition";
            AddTransition.Location = new System.Drawing.Point(630, 390);
            AddTransition.Click += AddTransition_Click;

            AddUrl.Text = "Add URL";
            AddUrl.Location = new System.Drawing.Point(630, 480);
            AddUrl.Click += AddUrl_Click;

            Controls.Add(InsertTransitions);
            Controls.Add(pluginTest);
            Controls.Add(InsertIntro);
            Controls.Add(InsertOutro);
            Controls.Add(effect_Reverse);
            Controls.Add(effect_Stutter);
            Controls.Add(effect_Earrape);
            Controls.Add(effect_Rainbow);
            Controls.Add(effect_Mirror);
            Controls.Add(effect_Sus);
            Controls.Add(Render);
            Controls.Add(SaveAs);
            Controls.Add(Preview);
            Controls.Add(AddMaterial);
            Controls.Add(ClearMaterial);
            Controls.Add(PausePlay);
            Controls.Add(Start);
            Controls.Add(End);
            Controls.Add(progressBar1);
            Controls.Add(Material);
            Controls.Add(MaterialLabel);
            Controls.Add(RenderSettingsLabel);
            Controls.Add(VideoSourcesLabel);
            Controls.Add(AudioSourcesLabel);
            Controls.Add(ImageSourcesLabel);
            Controls.Add(GifSourcesLabel);
            Controls.Add(TransitionSourcesLabel);
            Controls.Add(UrlSourcesLabel);
            Controls.Add(ProjectType);
            Controls.Add(OutputFormat);
            Controls.Add(VideoSources);
            Controls.Add(AudioSources);
            Controls.Add(ImageSources);
            Controls.Add(GifSources);
            Controls.Add(TransitionSources);
            Controls.Add(UrlSources);
            Controls.Add(AddVideo);
            Controls.Add(AddAudio);
            Controls.Add(AddImage);
            Controls.Add(AddGif);
            Controls.Add(AddTransition);
            Controls.Add(AddUrl);

            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(760, 560);
            Text = "YTP+ Deluxe Edition";

            ((ISupportInitialize)Clips).EndInit();
            ((ISupportInitialize)WidthSet).EndInit();
            ((ISupportInitialize)HeightSet).EndInit();
            ((ISupportInitialize)MinStreamDur).EndInit();
            ((ISupportInitialize)MaxStreamDur).EndInit();

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
