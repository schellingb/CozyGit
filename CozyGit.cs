using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using LibGit2Sharp;

[assembly: System.Reflection.AssemblyTitle("CozyGit")]
[assembly: System.Reflection.AssemblyProduct("CozyGit")]
[assembly: System.Reflection.AssemblyVersion("0.1.0.0")]
[assembly: System.Reflection.AssemblyFileVersion("0.1.0.0")]
[assembly: System.Reflection.AssemblyCopyright("(C) 2024 Bernhard Schelling")]
[assembly: System.Runtime.InteropServices.ComVisible(false)]

namespace CozyGit {

public static class Program
{
    static FormCommit f;
    static Repository repo;
    static GridDataList<Entry> el = new GridDataList<Entry>();
    static Dictionary<string, Icon> IconCache = new Dictionary<string,Icon>();
    static List<FormLog> LogWindows = new List<FormLog>();
    static FormSettings _SettingsFormCache;
    static bool MergeConflicts = false;
    static int ActiveCount = 0;

    static string ExceptionError(Exception e, string msg) { return msg + ":\n\n" + e.Message + (e.InnerException == null ? "" : "\n\n-------------------------------------------------\n\n" + e.ToString()); }
    static void ShowException(Exception e, string msg) { MessageBox.Show(f, ExceptionError(e, msg), "CozyGit - Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

    [STAThread] static void Main(string[] args)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        try
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(LibGit2Sharp.Core.NativeMethods).TypeHandle);
            #if DEBUG
            GlobalSettings.LogConfiguration = new LogConfiguration(LogLevel.Trace, (l, m) => { System.Console.WriteLine("[" + l.ToString() + "] " + m); });
            #endif
        }
        catch (Exception e)
        {
            if (e.InnerException is DllNotFoundException) MessageBox.Show(e.InnerException.Message, "CozyGit - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else ShowException(e, "Unable to load required DLL file");
            return;
        }

        f = new FormCommit();
        f.txtMessage.Font = new Font("Consolas", 9);
        if (f.txtMessage.Font.Name != "Consolas") f.txtMessage.Font = new Font(FontFamily.GenericMonospace, 9);

        f.gridMain.AddCol<DataGridViewCheckBoxCell>("", "ColActive", 0, null);
        f.gridMain.AddCol<DataGridViewFolderButton>("", "ColFolder", 0, null);
        f.gridMain.AddCol<DataGridViewImageCell>("", "ColIcon", 0, null);
        f.gridMain.AddCol<DataGridViewTextBoxCell>("Path", "ColPath", 6, "Path");
        f.gridMain.AddCol<DataGridViewTextBoxCell>("Status", "ColStatus", 1, "Status");
        f.gridMain.AddCol<DataGridViewTextBoxCell>("Modification Date", "ColModDate", 2, "ModDate");
        f.gridMain.AddCol<DataGridViewTextBoxCell>("Size", "ColSize", 1, "Size");
        f.gridMain.AddCol<DataGridViewTextBoxCell>("Extension", "ColExtension", 1, "Extension");
        (f.gridMain.Columns[1].DefaultCellStyle).Padding = new Padding(1);
        (f.gridMain.Columns[1]).Visible = false; // hide folder buttons
        (f.gridMain.Columns[2].CellTemplate as DataGridViewImageCell).ImageLayout = DataGridViewImageCellLayout.Zoom;
        f.gridMain.CellMouseEnter += (object o, DataGridViewCellEventArgs e) => { f.gridMain.ShowCellToolTips = e.ColumnIndex >= 3; };
        f.gridMain.DataSource = el;

        // Lock the first three columns in place
        f.gridMain.ColumnDisplayIndexChanged += (object sender, DataGridViewColumnEventArgs e) =>
        {
            System.Threading.SynchronizationContext.Current.Post(delegate(object o)
            {
                if (f.gridMain.Columns[2].DisplayIndex != 2) f.gridMain.Columns[2].DisplayIndex = 2;
                if (f.gridMain.Columns[1].DisplayIndex != 1) f.gridMain.Columns[1].DisplayIndex = 1;
                if (f.gridMain.Columns[0].DisplayIndex != 0) f.gridMain.Columns[0].DisplayIndex = 0;
            }, null);
        };

        // Search by key press, handle Enter and App (context menu) key
        string searchBuf = "";
        DateTimeOffset searchLast = DateTimeOffset.MinValue;
        f.gridMain.KeyDown += (object sender, KeyEventArgs e) =>
        {
            if (e.KeyCode == Keys.Enter && f.gridMain.CurrentRow != null)
            {
                e.Handled = true;
                Entry en = (Entry)f.gridMain.CurrentRow.DataBoundItem;
                ShowDiff(GetRepoBlob(en.Path, en.Status), en.Path);
            }
            else if (e.KeyCode == Keys.Space && f.gridMain.SelectedRows.Count > 0)
            {
                e.Handled = true;
                bool check = !((Entry)f.gridMain.SelectedRows[0].DataBoundItem).Active;
                foreach (DataGridViewRow r in f.gridMain.SelectedRows)
                {
                    ((Entry)r.DataBoundItem).ColActive = check;
                    el.BroadcastRowChanged(r.Index);
                }
            }
        };
        f.gridMain.KeyPress += (object sender, KeyPressEventArgs e) =>
        {
            if ((DateTimeOffset.Now - searchLast).TotalSeconds > 1) searchBuf = "";
            searchBuf += e.KeyChar;
            searchLast = DateTimeOffset.Now;
            for (int off = (f.gridMain.SelectedRows.Count > 0 ? f.gridMain.SelectedRows[0].Index : 0), i = 0; i != el.Count; i++)
                if (el[(i + off) % el.Count].Path.StartsWith(searchBuf, StringComparison.CurrentCultureIgnoreCase))
                    { f.gridMain.ClearSelection(); f.gridMain.CurrentCell = f.gridMain.Rows[(i + off) % el.Count].Cells[3]; f.gridMain.CurrentRow.Selected = true; return; }
        };
        f.gridMain.KeyUp += (object sender, KeyEventArgs e) =>
        {
            if (e.KeyCode != Keys.Apps || f.gridMain.CurrentRow == null) return;
            Point pos = f.gridMain.GetCellDisplayRectangle(3, f.gridMain.CurrentRow.Index, false).Location;
            pos.X += 25; pos.Y += 5;
            ShowEntryContextMenu(pos);
        };

        // Handle F5 to refresh
        f.KeyDown += (object sender, KeyEventArgs e) =>
        {
            if (e.KeyCode == Keys.F5) { f.btnRefresh.PerformClick(); e.Handled = true; }
        };

        f.gridMain.ColumnHeaderMouseClick       += el.GridOnColumnHeaderClick;
        f.gridMain.ColumnHeaderMouseDoubleClick += el.GridOnColumnHeaderClick;
        f.gridMain.CellContentClick             += gridMain_CellContentClick;
        f.gridMain.CellContentDoubleClick       += gridMain_CellContentClick;
        f.gridMain.CellMouseClick               += gridMain_CellMouseClick;
        f.gridMain.CellMouseDoubleClick         += gridMain_CellMouseClick;

        f.lnkCheckAll.Click         += (object _o, EventArgs _a) => DoMultiActive(false, FileStatus.Unreadable);
        f.lnkCheckNone.Click        += (object _o, EventArgs _a) => DoMultiActive(true, (FileStatus)0);
        f.lnkCheckUnversioned.Click += (object _o, EventArgs _a) => DoMultiActive(true, (FileStatus.NewInWorkdir | FileStatus.NewInIndex));
        f.lnkCheckVersioned.Click   += (object _o, EventArgs _a) => DoMultiActive(false, (FileStatus.NewInWorkdir | FileStatus.NewInIndex));
        f.lnkCheckDeleted.Click     += (object _o, EventArgs _a) => DoMultiActive(true, (FileStatus.DeletedFromWorkdir | FileStatus.DeletedFromIndex));
        f.lnkCheckModified.Click    += (object _o, EventArgs _a) => DoMultiActive(true, (FileStatus.ModifiedInWorkdir | FileStatus.ModifiedInIndex));
        f.lnkLocalRepository.Click  += (object _o, EventArgs _a) => System.Diagnostics.Process.Start("explorer", "\"" + f.lnkLocalRepository.Text + "\"");
        f.lnkRemoteRepository.Click += (object _o, EventArgs _a) => System.Diagnostics.Process.Start("explorer", "\"" + f.lnkRemoteRepository.Text + "\"");

        f.chkShowUnversioned.CheckedChanged += btnRefresh_Click;
        f.chkShowIgnored.CheckedChanged     += btnRefresh_Click;
        f.chkShowUnchanged.CheckedChanged   += btnRefresh_Click;
        f.btnRefresh.Click     += btnRefresh_Click;
        f.btnSettings.Click += (object _o, EventArgs _a) => GetSettings(GetSettingsMode.None, "");
        f.btnPullLatest.Click  += btnPullLatest_Click;
        f.btnShowLog.Click     += (object _o, EventArgs _a) => ShowLog();
        f.btnOK.Click          += btnOK_Click;
        f.Shown                += f_Shown;
        f.FormClosing += f_FormClosing;

        f.ShowDialog();
        while (LogWindows.Count > 0) LogWindows[0].ShowDialog();
        DeleteTemp();
    }

    static void f_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (LogWindows.Count == 0) return;
        for (int i = LogWindows.Count; i-- > 0;)
            { LogWindows[i].TopMost = true; LogWindows[i].TopMost = false; }
        if (e.CloseReason != CloseReason.ApplicationExitCall && e.CloseReason != CloseReason.WindowsShutDown && MessageBox.Show(LogWindows[0], "There " + (LogWindows.Count == 1 ? "is a log window" : "are " + LogWindows.Count.ToString() + " log windows") + " open, do you want to close " + (LogWindows.Count == 1 ? "it" : "them") + "?", "CozyGit - Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            { e.Cancel = true; return; }
        for (int i = LogWindows.Count; i-- > 0;)
            LogWindows[i].Close();
    }

    static void f_Shown(object _o, EventArgs _e)
    {
        f.splitMain.Enabled = false;
        string[] args = Environment.GetCommandLineArgs();
        string repoWorkDir = (args.Length > 1 ? args[1] : null);
        if (repoWorkDir == null)
        {
            f.splitMain.Enabled = false;
            MessageBox.Show(f, "Select a directory to use as the repository.\n\nIt can either be a directory that already containts a cloned git repository or an empty directory to clone a repository into it.", "CozyGit - Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            fbd.SelectedPath = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location).DirectoryName;
            if (fbd.ShowDialog(f) != DialogResult.OK) { fbd.Dispose(); f.DialogResult = DialogResult.Cancel; return; }
            repoWorkDir = fbd.SelectedPath;
            fbd.Dispose();
            f.splitMain.Enabled = true;
        }

        repoWorkDir = repoWorkDir.TrimEnd('\\', '/');
        f.lnkLocalRepository.Text = repoWorkDir;
        f.Text += " - " + Path.GetFileName(repoWorkDir);

        if (!Directory.Exists(repoWorkDir))
        {
            if (MessageBox.Show(f, "Directory '" + repoWorkDir + "' does not exist, do you want to create it?", "CozyGit - Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) { f.DialogResult = DialogResult.Cancel; return; }
            try { Directory.CreateDirectory(repoWorkDir); }
            catch { MessageBox.Show(f, "Could not create directory '" + repoWorkDir  + "', check the parent directory then try again.", "CozyGit - Error", MessageBoxButtons.OK, MessageBoxIcon.Error); f.DialogResult = DialogResult.Cancel; return; }
        }

        string err = OpenRepo(repoWorkDir);
        if (err != null)
        {
            if (err != "") MessageBox.Show(f, err, "CozyGit - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            f.DialogResult = DialogResult.Cancel;
            return;
        }

        btnRefresh_Click(f.btnRefresh);
    }

    static void SetProgress(int val, int max)
    {
        if (f.InvokeRequired) return; // TODO: Run git remote operations in a thread (can't call f.Invoke here as that just freezes the program)
        f.Progress.Maximum = Math.Max(max, 1);
        f.Progress.Value = Math.Min(Math.Max(val, 0), max);
        Application.DoEvents();
    }

    static bool OnPackProgress(LibGit2Sharp.Handlers.PackBuilderStage stage, int current, int total)
    {
        SetProgress(current, total);
        return true;
    }

    static bool OnPushProgress(int current, int total, long bytes)
    {
        SetProgress(current, total);
        return true;
    }

    static void OnCloneProgress(string path, int current, int total)
    {
        SetProgress(current, total);
    }

    static bool OnFetchProgress(TransferProgress progress)
    {
        SetProgress(progress.ReceivedObjects + progress.IndexedObjects, progress.TotalObjects * 2);
        return true;
    }

    static string OpenRepo(string path)
    {
        try
        {
            repo = new Repository(path);
        }
        catch (LibGit2Sharp.RepositoryNotFoundException)
        {
            if (Directory.EnumerateFileSystemEntries(path).GetEnumerator().MoveNext())
                return "Directory '" + path  + "' is not empty and is not an existing repository. Use an empty directory to clone a remote repository into it.";

            var cf = new FormClone();
            cf.radBranchDefault.Click += (object o, EventArgs a) => { cf.txtBranch.Enabled = false; };
            cf.radBranchCustom.Click += (object o, EventArgs a) => { cf.txtBranch.Enabled = true; };
            cf.radDepthEverything.Click += (object o, EventArgs a) => { cf.numDepth.Enabled = false; };
            cf.radDepthCustom.Click += (object o, EventArgs a) => { cf.numDepth.Enabled = true; };

            retry:
            if (cf.ShowDialog(f) != DialogResult.OK) return "";
            f.lnkRemoteRepository.Text = cf.txtURL.Text;

            try
            {
                PerformAuthedOperation(AuthedOperation.Clone, (LibGit2Sharp.Handlers.CredentialsHandler credentialsProvider) =>
                {
                    CloneOptions co = new CloneOptions { OnCheckoutProgress = OnCloneProgress };
                    if (cf.radBranchCustom.Checked) co.BranchName = cf.txtBranch.Text;
                    if (cf.radDepthCustom.Checked) co.Depth = (int)cf.numDepth.Value;
                    co.FetchOptions.CredentialsProvider = credentialsProvider;
                    co.FetchOptions.OnTransferProgress = OnFetchProgress;
                    Repository.Clone(cf.txtURL.Text, path, co);
                });
            }
            catch (System.Exception e)
            {
                MessageBox.Show(f, ExceptionError(e, "Error while trying to clone repository directory '" + path  + "'"), "CozyGit - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                goto retry;
            }
            try
            {
                repo = new Repository(path);
                SaveSettingsIfNeeded(repo.Config);
            }
            catch (System.Exception e) { return ExceptionError(e, "Unknown error while trying to open newly cloned repository directory '" + path  + "'"); }
        }
        catch (System.Exception e) { return ExceptionError(e, "Unknown error while trying to open repository directory '" + path  + "'"); }

        try { Remote repoRemote = repo.Network.Remotes["origin"]; if (repoRemote != null) f.lnkRemoteRepository.Text = repoRemote.PushUrl; } catch { }
        try { Branch head = repo.Head; if (head != null && head.Tip != null) f.lnkBranch.Text = head.FriendlyName; } catch { }
        return null;
    }

    static void gridMain_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.ColumnIndex != 0) return;
        f.gridMain.CommitEdit(DataGridViewDataErrorContexts.Commit); // trigger CellValueChanged and Active setter call
        el.BroadcastRowChanged(e.RowIndex); // refresh because Active setter might not have actually changed a read-only value
    }

    enum EntryContextMenuAction {  RestoreAfterCommit, RestoreNow, DiffRestore, CancelRestore, Diff, DiffConflict, Log, Revert, Open, OpenWith, ExploreTo, Delete };
    static void DoEntryAction(EntryContextMenuAction a)
    {
        foreach (DataGridViewRow r in f.gridMain.SelectedRows)
        {
            Entry en = (Entry)r.DataBoundItem;
            switch (a)
            {
                case EntryContextMenuAction.RestoreAfterCommit:
                    if (!File.Exists(en.AbsPath) || File.Exists(en.AbsRestorePath)) continue;
                    File.Copy(en.AbsPath, en.AbsRestorePath);
                    en.RestoreAfterCommit = true;
                    break;
                case EntryContextMenuAction.RestoreNow:
                    if (!File.Exists(en.AbsPath) || !File.Exists(en.AbsRestorePath)) continue; // TODO: support restoring after a file was deleted
                    if (!FileEqualContent(en.AbsPath, en.AbsRestorePath) && MessageBox.Show(f, "Are you sure you want to revert '" + en.Path + "' back to the restore point from " + File.GetLastWriteTime(en.AbsRestorePath).ToString() + "?", "CozyGit - Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) continue;
                    File.Delete(en.AbsPath);
                    File.Move(en.AbsRestorePath, en.AbsPath);
                    en.RestoreAfterCommit = false;
                    break;
                case EntryContextMenuAction.DiffRestore:
                    ShowDiff(en.Path + Entry.RestorePointExtension, en.Path);
                    break;
                case EntryContextMenuAction.CancelRestore:
                    if (!File.Exists(en.AbsRestorePath)) continue;
                    if (!FileEqualContent(en.AbsPath, en.AbsRestorePath) && MessageBox.Show(f, "Are you sure you want to delete the restore point of '" + en.Path + "' from " + File.GetLastWriteTime(en.AbsRestorePath).ToString() + "?", "CozyGit - Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) continue;
                    File.Delete(en.AbsRestorePath);
                    en.RestoreAfterCommit = false;
                    break;
                case EntryContextMenuAction.Diff:
                    ShowDiff(GetRepoBlob(en.Path, en.Status), en.Path);
                    break;
                case EntryContextMenuAction.DiffConflict:
                    ShowDiff(GetRepoBlob(en.Path, en.Status, true), en.Path);
                    break;
                case EntryContextMenuAction.Log:
                    ShowLog(en.Path);
                    break;
                case EntryContextMenuAction.Revert:
                    Blob src_blob = GetRepoBlob(en.Path, en.Status);
                    if (src_blob != null)
                    {
                        using (var fs = File.Create(en.AbsPath)) { src_blob.GetContentStream().CopyTo(fs); }
                        en.Size = src_blob.Size;
                        en.ModDate = DateTime.Now;
                        en.Icon = GetCachedIcon(new FileInfo(en.AbsPath));
                        if (en.Active) en.ColActive = false; // unstage and update ActiveCount
                        else en.Status = repo.RetrieveStatus(en.Path);
                    }
                    else if (File.Exists(en.AbsPath) && MessageBox.Show(f, "The file '" + en.Path + "' does not exist on the repository, do you want to delete the local file, too?", "CozyGit - Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        File.Delete(en.AbsPath);
                        goto removeRow;
                    }
                    break;
                case EntryContextMenuAction.Open:
                    System.Diagnostics.Process.Start("explorer", "\"" + en.AbsPath + "\"");
                    break;
                case EntryContextMenuAction.OpenWith:
                    System.Diagnostics.Process.Start("openwith", "\"" + en.AbsPath + "\"");
                    break;
                case EntryContextMenuAction.ExploreTo:
                    System.Diagnostics.Process.Start("explorer", (File.Exists(en.AbsPath) ? "/select, \"" + en.AbsPath + "\"" : "\"" + Path.GetDirectoryName(en.AbsPath) + "\""));
                    break;
                case EntryContextMenuAction.Delete:
                    if ((en.Status & (FileStatus.DeletedFromIndex | FileStatus.DeletedFromWorkdir)) != 0) continue;
                    if ((en.Status & (FileStatus.ModifiedInIndex | FileStatus.ModifiedInWorkdir)) != 0 && MessageBox.Show(f, "The file '" + en.Path + "' has local modifications, are you sure you want to delete the file without committing the changes first?", "CozyGit - Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) continue;
                    if ((en.Status & (FileStatus.NewInIndex | FileStatus.NewInWorkdir)) != 0 && MessageBox.Show(f, "The file '" + en.Path + "' does not exist in the repository, are you sure you want to delete the file without committing the file content first?", "CozyGit - Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) continue;
                    en.ColActive = false; // unstage and update ActiveCount
                    if (File.Exists(en.AbsPath)) File.Delete(en.AbsPath);
                    en.Status = FileStatus.DeletedFromWorkdir;
                    en.ColActive = true; // re-stage and update ActiveCount
                    en.Icon = GetCachedIcon(new FileInfo(en.AbsPath));
                    if (en.Status == FileStatus.Nonexistent) goto removeRow;
                    break;
                removeRow:
                    el.Remove(en);
                    el.BroadcastListChanged(f.gridMain);
                    return;
            }
            el.BroadcastRowChanged(r.Index);
        }
    }

    static void ShowEntryContextMenu(Point menuPos)
    {
        DataGridViewSelectedRowCollection rows = f.gridMain.SelectedRows;
        if (rows.Count == 0) return;
        bool singleFile = (rows.Count == 1), allFilesExist = true, allRestoresExist = true, noRestoresExist = true; FileStatus allStatus = FileStatus.Unaltered;
        foreach (DataGridViewRow r in rows)
        {
            Entry en = (Entry)r.DataBoundItem;
            if (!File.Exists(en.AbsPath)) allFilesExist = false;
            if (File.Exists(en.AbsRestorePath)) noRestoresExist = false; else allRestoresExist = false;
            allStatus |= en.Status;
        }
        ContextMenuStrip context = new ContextMenuStrip();
        if (allFilesExist && noRestoresExist) context.Items.Add("Restore after commit", Data.icon_restore).Click += (object _o, EventArgs _a) => DoEntryAction(EntryContextMenuAction.RestoreAfterCommit);
        if (allFilesExist && allRestoresExist) context.Items.Add("Restore now", Data.icon_restore).Click += (object _o, EventArgs _a) => DoEntryAction(EntryContextMenuAction.RestoreNow);
        if (singleFile && allFilesExist && allRestoresExist) context.Items.Add("Diff with restore point", Data.icon_restore).Click += (object _o, EventArgs _a) => DoEntryAction(EntryContextMenuAction.DiffRestore);
        if (allRestoresExist) context.Items.Add("Cancel restore (delete restore point)", Data.icon_restore).Click += (object _o, EventArgs _a) => DoEntryAction(EntryContextMenuAction.CancelRestore);
        context.Items.Add("Show diff", Data.icon_diff).Click += (object _o, EventArgs _a) => DoEntryAction(EntryContextMenuAction.Diff);
        if (MergeConflicts) context.Items.Add("Show diff against remote (due to merge conflicts)", Data.icon_diff).Click += (object _o, EventArgs _a) => DoEntryAction(EntryContextMenuAction.DiffConflict);
        if (singleFile) context.Items.Add("Show log", Data.icon_log).Click += (object _o, EventArgs _a) => DoEntryAction(EntryContextMenuAction.Log);
        context.Items.Add("Revert", Data.icon_revert).Click += (object _o, EventArgs _a) => DoEntryAction(EntryContextMenuAction.Revert);
        //context.Items.Add("Rename", Icons.icon_rename).Click += (object _o, EventArgs _a) => DoEntryAction(EntryContextMenuAction.Rename);
        //context.Items.Add("Blame", Icons.icon_blame).Click += (object _o, EventArgs _a) => DoEntryAction(EntryContextMenuAction.Blame);
        if (allFilesExist) context.Items.Add("Open", Data.icon_open).Click += (object _o, EventArgs _a) => DoEntryAction(EntryContextMenuAction.Open);
        if (singleFile && allFilesExist) context.Items.Add("Open with...", Data.icon_openwith).Click += (object _o, EventArgs _a) => DoEntryAction(EntryContextMenuAction.OpenWith);
        if (singleFile) context.Items.Add("Explore to", Data.icon_explore).Click += (object _o, EventArgs _a) => DoEntryAction(EntryContextMenuAction.ExploreTo);
        if ((allStatus & (FileStatus.DeletedFromIndex | FileStatus.DeletedFromWorkdir)) == 0)
            context.Items.Add("Delete", Data.icon_delete).Click += (object _o, EventArgs _a) => DoEntryAction(EntryContextMenuAction.Delete);
        context.Show(f.gridMain, menuPos);
    }

    static void gridMain_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex > el.Count) return;
        Entry en = el[e.RowIndex];
        if (e.ColumnIndex == 1) en.IsExpanded ^= true;
        else if (e.ColumnIndex > 1 && e.Button == MouseButtons.Right && e.Clicks == 1)
        {
            if (!f.gridMain.Rows[e.RowIndex].Selected)
            {
                f.gridMain.ClearSelection();
                f.gridMain.Rows[e.RowIndex].Selected = true;
            }
            ShowEntryContextMenu(f.gridMain.PointToClient(Cursor.Position));
        }
        else if (e.ColumnIndex > 1 && e.Clicks == 2 && e.Button == MouseButtons.Left)
        {
            ShowDiff(GetRepoBlob(en.Path, en.Status), en.Path);
        }
    }

    static Icon GetCachedIcon(FileInfo fi)
    {
        Icon res;
        if (!IconCache.TryGetValue((fi.Exists ? fi.Extension : "."), out res))
        {
            res = (fi.Exists ? Icon.ExtractAssociatedIcon(fi.FullName) : Icon.FromHandle(Data.icon_delete.GetHicon()));
            if (res == null && fi.Exists) res = Icon.FromHandle(new Bitmap(1,1).GetHicon());
            IconCache.Add((fi.Exists ? fi.Extension : "."), res);
        }
        return res;
    }

    static bool SynchronizeWithRemote() // return if push/pull/reset succeeded
    {
        Remote repoRemote = repo.Network.Remotes["origin"];
        Branch repoHeadBranch = repo.Head, repoTrackedHeadBranch = repoHeadBranch.TrackedBranch;
        if (repoRemote == null || repoTrackedHeadBranch == null)
            { MessageBox.Show(f, "Unknown error while trying to open repository directory '" + repo.Info.WorkingDirectory  + "':\n\n" + "Repository has no remote " + (repoRemote == null ? "origin" : "tracked branch"), "CozyGit - Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }
        if (repoHeadBranch.Tip == repoTrackedHeadBranch.Tip)
            return false;
        try
        {
            Commit mergeBase;
            try { mergeBase = repo.ObjectDatabase.FindMergeBase(repoTrackedHeadBranch.Tip, repoHeadBranch.Tip); } catch (LibGit2SharpException) { mergeBase = repoHeadBranch.Tip; /* log with truncated history depth */ } catch { mergeBase = null; }
            if (mergeBase != repoTrackedHeadBranch.Tip && mergeBase != repoHeadBranch.Tip)
            {
                if (MessageBox.Show(f, "There are commits that have been fetched but not merged from the remote repository and also commits that have not been pushed to the remote repository, do you want to merge them now? If you choose 'No' you can choose to reset your local commits", "CozyGit - Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    repo.MergeFetchedRefs(new Signature("-", "-", DateTimeOffset.Now), new MergeOptions { FastForwardStrategy = FastForwardStrategy.FastForwardOnly, OnCheckoutProgress = OnCloneProgress });
                else if (MessageBox.Show(f, "There are commits that have not been pushed to the remote repository, do you want to reset them? You will lose your locally created commit messages and time stamps but file contents will not be reset.?", "CozyGit - Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    repo.Reset(ResetMode.Soft, repoTrackedHeadBranch.Tip);
                else return false;
            }
            else if (mergeBase != repoTrackedHeadBranch.Tip)
            {
                MergeConflicts = true;
                if (MessageBox.Show(f, "There are commits that have been fetched but not merged from the remote repository, do you want to merge them now?", "CozyGit - Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    repo.MergeFetchedRefs(new Signature("-", "-", DateTimeOffset.Now), new MergeOptions { FastForwardStrategy = FastForwardStrategy.FastForwardOnly, OnCheckoutProgress = OnCloneProgress });
                else return false;
            }
            else if (MessageBox.Show(f, "There are commits that have not been pushed to the remote repository, do you want to push them now?", "CozyGit - Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                DoPush();
            else if (repoTrackedHeadBranch.Tip != null && MessageBox.Show(f, "There are commits that have not been pushed to the remote repository, do you want to reset them? You will lose your locally created commit messages and time stamps but file contents will not be reset.?", "CozyGit - Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                repo.Reset(ResetMode.Soft, repoTrackedHeadBranch.Tip);
            else return false;
            MergeConflicts = false;
            return true;
        }
        catch (Exception e) { ShowException(e, "Synchronizing local and remote repository failed"); return false; }
    }

    static void btnRefresh_Click(object _sender = null, EventArgs _e = null)
    {
        if (f.Enabled) f.splitMain.Enabled = false;
        bool skipNewInWorkdir = !f.chkShowUnversioned.Checked;
        bool skipIgnored = !f.chkShowIgnored.Checked;
        bool skipUnaltered = !f.chkShowUnchanged.Checked;
        bool haveUnversioned = false, haveVersioned = false, haveDeleted = false, haveModified = false;
        el.Clear();
        ActiveCount = 0;
        string repoWorkDir = repo.Info.WorkingDirectory;
        Dictionary<string, bool> restoreAfterCommits = new Dictionary<string,bool>();
        foreach (StatusEntry se in repo.RetrieveStatus(new StatusOptions { IncludeUnaltered = !skipUnaltered, IncludeIgnored = !skipIgnored }))
        {
            FileStatus s = se.State;
            string path = se.FilePath;
            if (path.EndsWith(Entry.RestorePointExtension))
            {
                if ((s & (FileStatus.NewInIndex | FileStatus.DeletedFromIndex | FileStatus.ModifiedInIndex | FileStatus.RenamedInIndex | FileStatus.TypeChangeInIndex)) != 0)
                    Commands.Unstage(repo, path); // force unstage restore point files
                path = path.Remove(path.Length - Entry.RestorePointExtension.Length);
                foreach (Entry e in el) { if (e.Path == path) { e.RestoreAfterCommit = true; goto found; } }
                restoreAfterCommits.Add(path, true);
                found: continue;
            }

            if (s == (FileStatus.NewInIndex | FileStatus.ModifiedInWorkdir) || s == (FileStatus.ModifiedInIndex | FileStatus.ModifiedInWorkdir))
            {
                // We force re-stage any file that has separate modifications in staged and local file
                Commands.Unstage(repo, path);
                Commands.Stage(repo, path, Entry.DefaultStageOptions);
                s = repo.RetrieveStatus(path);
            }

            if (skipNewInWorkdir && (s & (FileStatus.NewInWorkdir | FileStatus.NewInIndex)) != 0) continue;
            haveUnversioned |= (s & (FileStatus.NewInWorkdir | FileStatus.NewInIndex)) != 0;
            haveVersioned |= (s & (FileStatus.NewInWorkdir | FileStatus.NewInIndex)) == 0;
            haveDeleted |= (s & (FileStatus.DeletedFromWorkdir | FileStatus.DeletedFromIndex)) != 0;
            haveModified |= (s & (FileStatus.ModifiedInWorkdir | FileStatus.ModifiedInIndex)) != 0;

            bool active = (s & (FileStatus.NewInIndex | FileStatus.DeletedFromIndex | FileStatus.ModifiedInIndex | FileStatus.RenamedInIndex | FileStatus.TypeChangeInIndex)) != 0;

            FileInfo fi = new FileInfo(repoWorkDir + path);
            bool exists = fi.Exists;
            string extension = fi.Extension.TrimStart('.');
            Icon icon = GetCachedIcon(fi);
            el.Add(new Entry { Icon = icon, Active = active, Path = path, Status = s, ModDate = (exists ? (DateTimeOffset)fi.LastWriteTime : DateTimeOffset.MinValue), Size = (exists ? fi.Length : -1), Extension = extension });
            if (active) ActiveCount++;
            if (restoreAfterCommits.Count > 0 && restoreAfterCommits.ContainsKey(path)) { el[el.Count - 1].RestoreAfterCommit = true; }
        }

        //// TODO: Folder rows need to be added (or removed) depending on if sort function is path or something else, maybe add a checkbox "Show folders" to UI
        //string fLastDirectory = "";
        //int fLastDirectoryLen = 0;
        //for (int i = 0; i != el.Count; i++)
        //{
        //    string p = el[i].Path;
        //    int fDirectoryLen = Math.Max(p.LastIndexOf('/'), 0);
        //    if (fDirectoryLen != 0 && (fLastDirectoryLen != fDirectoryLen || string.CompareOrdinal(fLastDirectory, 0, p, 0, fDirectoryLen) != 0))
        //    {
        //        fLastDirectory = p.Substring(0, fDirectoryLen);
        //        fLastDirectoryLen = fDirectoryLen;
        //        el.Insert(i, new Entry { Path = fLastDirectory, IsFolder = true, IsExpanded = true });
        //        i++;
        //    }
        //}

        f.lnkCheckUnversioned.Enabled = haveUnversioned;
        f.lnkCheckVersioned.Enabled = haveVersioned;
        f.lnkCheckDeleted.Enabled = haveDeleted;
        f.lnkCheckModified.Enabled = haveModified;
        f.lblStatus.Text = ActiveCount.ToString() + " files selected, " + el.Count.ToString() + " files total";
        f.btnOK.Enabled = (ActiveCount > 0);
        el.BroadcastListChanged(f.gridMain, true);
        f.gridMain.ClearSelection();

        if (_sender == f.btnRefresh && SynchronizeWithRemote()) { btnRefresh_Click(); return; }

        f.splitMain.Enabled = true;
    }

    static void btnPullLatest_Click(object o, EventArgs a)
    {
        f.splitMain.Enabled = false;
        try
        {
            PerformAuthedOperation(AuthedOperation.Pull, (LibGit2Sharp.Handlers.CredentialsHandler credentialsProvider) =>
            {
                PullOptions po = new PullOptions();
                po.FetchOptions = new FetchOptions { CredentialsProvider = credentialsProvider, OnTransferProgress = OnFetchProgress };
                po.MergeOptions = new MergeOptions { FastForwardStrategy = FastForwardStrategy.FastForwardOnly, OnCheckoutProgress = OnCloneProgress };
                Commands.Pull(repo, new Signature("-", "-", DateTimeOffset.Now), po); // merger signature unused due to FastForwardOnly
            });
        }
        catch (MergeFetchHeadNotFoundException) { } // can happen when pulling nothing from a uninitialized empty remote repo
        catch (System.Exception e) { ShowException(e, "Error while trying to pull from the remote branch"); }
        finally { f.splitMain.Enabled = true; }
    }

    static void btnOK_Click(object o, EventArgs a)
    {
        f.splitMain.Enabled = false;

        Branch repoHeadBranch = repo.Head, repoTrackedHeadBranch = repoHeadBranch.TrackedBranch;
        if (repoHeadBranch.TrackedBranch == null || repoHeadBranch.Tip != repoTrackedHeadBranch.Tip)
        {
            MessageBox.Show(f, "Local and remote tip are on different points in the history, you must first synchronize them. Click OK to check.", "CozyGit - Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (!SynchronizeWithRemote()) { f.splitMain.Enabled = true; return; }
        }

        if (ActiveCount > 0)
        {
            if (f.txtMessage.Text == "" && MessageBox.Show(f, "Are you sure you want to commit with an empty commit message?", "CozyGit - Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                { f.splitMain.Enabled = true; return; }
            var sf = GetSettings(GetSettingsMode.AuthAndCommit, "Please enter login details for pushing to remote branch and author and committer details for committing");
            if (sf == null)
            {
                MessageBox.Show(f, "Missing user details for commit", "CozyGit - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                f.splitMain.Enabled = true;
                return;
            }
            Signature author = new Signature(sf.txtAuthorName.Text, sf.txtAuthorEmail.Text, DateTimeOffset.Now);
            Signature committer = new Signature(sf.txtCommitterName.Text, sf.txtCommitterEmail.Text, author.When);
            try { repo.Commit(f.txtMessage.Text, author, committer); }
            catch (EmptyCommitException) { }
            catch (Exception e) { ShowException(e, "Unknown error when committing"); }

            foreach (Entry en in el)
            {
                if (!en.Active) continue;
                en.Active = false; // set Active not ColActive because we just committed
                if (en.RestoreAfterCommit && File.Exists(en.AbsRestorePath))
                {
                    if (File.Exists(en.AbsPath)) File.Delete(en.AbsPath);
                    File.Move(en.AbsRestorePath, en.AbsPath);
                    en.RestoreAfterCommit = false;
                }
            }
            el.BroadcastListChanged(f.gridMain);
        }
        if (DoPush()) { f.btnOK.Enabled = false; btnRefresh_Click(); }
        f.splitMain.Enabled = true;
    }

    static void DoMultiActive(bool need, FileStatus s)
    {
        foreach (Entry e in el) e.ColActive = (need ? ((e.Status & s) != 0) : ((e.Status & s) == 0));
        el.BroadcastListChanged(f.gridMain);
    }

    static bool DoPush()
    {
        if (repo.Head.TrackedBranch.Tip == null)
        {
            MessageBox.Show(f, "The remote repository has no branch yet, please specifiy the default branch you want to push to.", "CozyGit - Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            FormInput fi = new FormInput();
            fi.Text = "Set Default Branch Name";
            fi.group.Text = "Default branch name:";
            fi.txt.Text = "main";
            retry:
            if (fi.ShowDialog() != DialogResult.OK) { f.splitMain.Enabled = true; return false; }
            try
            {
                Branch oldBranch = repo.Head, newBranch = (fi.txt.Text == oldBranch.FriendlyName ? oldBranch : repo.CreateBranch(fi.txt.Text));
                if (oldBranch != newBranch)
                {
                    repo.Refs.UpdateHeadTarget(newBranch.Reference, "");
                    repo.Branches.Remove(oldBranch);
                }
                new BranchUpdater(repo, newBranch).TrackedBranch = "refs/remotes/origin/" + fi.txt.Text;
            }
            catch (Exception e) { ShowException(e, "Error while trying to set new remote branch"); goto retry; }
        }
        try
        {
            PerformAuthedOperation(AuthedOperation.Push, (LibGit2Sharp.Handlers.CredentialsHandler credentialsProvider) =>
            {
                PushOptions po = new PushOptions { CredentialsProvider = credentialsProvider, OnPushTransferProgress = OnPushProgress, OnPackBuilderProgress = OnPackProgress };
                repo.Network.Push(repo.Head, po);
            });
            return true;
        }
        catch (Exception e) { ShowException(e, "Error while trying to pushing to remote branch"); }
        return false;
    }

    enum GetSettingsMode { None, Auth, Commit, AuthAndCommit, DiffTool };
    static FormSettings GetSettings(GetSettingsMode mode, string message)
    {
        bool firstTime = (_SettingsFormCache == null), useCurrent = true;
        FormSettings sf = (firstTime ? (_SettingsFormCache = new FormSettings()) : _SettingsFormCache);

        if (firstTime)
        {
            Configuration config = (repo == null ? new Configuration(null, null, null, null, null) : repo.Config);
            ConfigurationEntry<string> cfgRemoteUser     = config.Get<string>("cozygit.remoteuser");
            ConfigurationEntry<string> cfgRemotePassword = config.Get<string>("cozygit.remotepassword");
            ConfigurationEntry<string> cfgAuthorName     = config.Get<string>("cozygit.authorname");
            ConfigurationEntry<string> cfgAuthorEmail    = config.Get<string>("cozygit.authoremail");
            ConfigurationEntry<string> cfgCommitterName  = config.Get<string>("cozygit.committername");
            ConfigurationEntry<string> cfgCommitterEmail = config.Get<string>("cozygit.committeremail");
            ConfigurationEntry<string> cfgDiffTool       = config.Get<string>("cozygit.difftool");

            sf.txtRemoteUser.Text     = (cfgRemoteUser     != null ? cfgRemoteUser.Value     : "");
            sf.txtRemotePassword.Text = (cfgRemotePassword != null ? cfgRemotePassword.Value : "");
            sf.txtAuthorName.Text     = (cfgAuthorName     != null ? cfgAuthorName.Value     : config.GetValueOrDefault<string>("user.name", sf.txtRemoteUser.Text));
            sf.txtAuthorEmail.Text    = (cfgAuthorEmail    != null ? cfgAuthorEmail.Value    : config.GetValueOrDefault<string>("user.email", ""));
            sf.txtCommitterName.Text  = (cfgCommitterName  != null ? cfgCommitterName.Value  : sf.txtAuthorName.Text);
            sf.txtCommitterEmail.Text = (cfgCommitterEmail != null ? cfgCommitterEmail.Value : sf.txtAuthorEmail.Text);
            sf.txtDiffTool.Text       = (cfgDiffTool       != null ? cfgDiffTool.Value       : "");

            sf.chkRemoteUser.Checked     = (cfgRemoteUser     != null);
            sf.chkRemotePassword.Checked = (cfgRemotePassword != null);
            sf.chkAuthorName.Checked     = (cfgAuthorName     != null);
            sf.chkAuthorEmail.Checked    = (cfgAuthorEmail    != null);
            sf.chkCommitterName.Checked  = (cfgCommitterName  != null);
            sf.chkCommitterEmail.Checked = (cfgCommitterEmail != null);

            ConfigurationLevel minLevel = (ConfigurationLevel)999, maxLevel = (ConfigurationLevel)0;
            if (cfgRemoteUser     != null) { minLevel = (ConfigurationLevel)Math.Min((int)cfgRemoteUser.Level,     (int)minLevel); maxLevel = (ConfigurationLevel)Math.Max((int)cfgRemoteUser.Level,     (int)maxLevel); }
            if (cfgRemotePassword != null) { minLevel = (ConfigurationLevel)Math.Min((int)cfgRemotePassword.Level, (int)minLevel); maxLevel = (ConfigurationLevel)Math.Max((int)cfgRemotePassword.Level, (int)maxLevel); }
            if (cfgAuthorName     != null) { minLevel = (ConfigurationLevel)Math.Min((int)cfgAuthorName.Level,     (int)minLevel); maxLevel = (ConfigurationLevel)Math.Max((int)cfgAuthorName.Level,      (int)maxLevel); }
            if (cfgAuthorEmail    != null) { minLevel = (ConfigurationLevel)Math.Min((int)cfgAuthorEmail.Level,    (int)minLevel); maxLevel = (ConfigurationLevel)Math.Max((int)cfgAuthorEmail.Level,     (int)maxLevel); }
            if (cfgCommitterName  != null) { minLevel = (ConfigurationLevel)Math.Min((int)cfgCommitterName.Level,  (int)minLevel); maxLevel = (ConfigurationLevel)Math.Max((int)cfgCommitterName.Level,   (int)maxLevel); }
            if (cfgCommitterEmail != null) { minLevel = (ConfigurationLevel)Math.Min((int)cfgCommitterEmail.Level, (int)minLevel); maxLevel = (ConfigurationLevel)Math.Max((int)cfgCommitterEmail.Level,  (int)maxLevel); }

            if (minLevel == maxLevel) sf.chkUseGlobalConfig.CheckState = (minLevel == ConfigurationLevel.Global ? CheckState.Checked : CheckState.Unchecked);
            if (minLevel < maxLevel) sf.chkUseGlobalConfig.CheckState = CheckState.Indeterminate;

            useCurrent = config.GetValueOrDefault<bool>("cozygit.savedsettings", ConfigurationLevel.Local, false);

            sf.btnOK.Click += (object sender, EventArgs e) =>
            {
                if (sf.Check()) { sf.DialogResult = DialogResult.OK; }
                else { MessageBox.Show(sf, "Missing input.", "CozyGit - Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            sf.btnDiffTool.Click += (object sender, EventArgs e) =>
            {
                OpenFileDialog fd = new OpenFileDialog();
                fd.Filter = "Executables|*.exe;*.com;*.bat|All files (*.*)|*.*";
                fd.DefaultExt = "exe";
                fd.CheckFileExists = true;
                if (fd.ShowDialog(sf) == DialogResult.OK) sf.txtDiffTool.Text = fd.FileName;
                fd.Dispose();
            };

            sf.btnDonate.Click += (object _s, EventArgs _e) => System.Diagnostics.Process.Start("https://paypal.com/donate?hosted_button_id=3WX4KRRTHN6EL");
            sf.btnGitHub.Click += (object _s, EventArgs _e) => System.Diagnostics.Process.Start("https://github.com/schellingb/CozyGit");
            sf.btnLicenses.Click += (object sender, EventArgs e) =>
            {
                byte[] buf = new byte[50980];
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(Data.License, false))
                    using (System.IO.Compression.DeflateStream ds = new System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Decompress, false))
                        ds.Read(buf, 0, buf.Length);
                Form pu = new Form{Icon = f.Icon, StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.SizableToolWindow, Text = sf.btnLicenses.Text, Width = 650, Height = sf.Height - 100, ShowInTaskbar = false };
                pu.Controls.Add(new TextBox{ Multiline = true, ReadOnly = true, Text = Encoding.UTF8.GetString(buf).Replace("\n", Environment.NewLine), Font = f.txtMessage.Font, Dock = DockStyle.Fill, ScrollBars = ScrollBars.Vertical });
                (pu.Controls[0] as TextBox).Select(0,0);
                (pu.Controls[0] as TextBox).KeyDown += (object _o, KeyEventArgs _a) => { if (_a.KeyCode != Keys.Escape && _a.KeyCode != Keys.Enter) return; pu.DialogResult = DialogResult.OK; _a.Handled = true; };
                pu.ShowDialog(sf);
            };
        }

        if      (mode == GetSettingsMode.Auth         ) sf.Check = () => (sf.txtRemoteUser.Text != "" && sf.txtRemotePassword.Text != "");
        else if (mode == GetSettingsMode.Commit       ) sf.Check = () => (sf.txtAuthorName.Text != "" && sf.txtAuthorEmail.Text != "" && sf.txtCommitterName.Text != "" && sf.txtCommitterEmail.Text != "");
        else if (mode == GetSettingsMode.AuthAndCommit) sf.Check = () => (sf.txtRemoteUser.Text != "" && sf.txtRemotePassword.Text != "" && sf.txtAuthorName.Text != "" && sf.txtAuthorEmail.Text != "" && sf.txtCommitterName.Text != "" && sf.txtCommitterEmail.Text != "");
        else if (mode == GetSettingsMode.DiffTool   ) { sf.Check = () => (sf.txtDiffTool.Text != "" && File.Exists(sf.txtDiffTool.Text)); useCurrent = true; }
        else                                            sf.Check = () => true;

        if (useCurrent && mode != GetSettingsMode.None && sf.Check()) return sf;
        sf.lblMessage.Text = message;
        if (sf.ShowDialog() != DialogResult.OK) { if (firstTime) _SettingsFormCache = null; return null; }
        sf.NeedSave = true;
        if (repo != null) SaveSettingsIfNeeded(repo.Config);

        return sf;
    }

    static void SaveSettingsIfNeeded(Configuration cfg)
    {
        var cf = _SettingsFormCache;
        if (cf != null && cf.SetBoolOption != null) { cfg.Set(cf.SetBoolOption, true, ConfigurationLevel.Local); cf.SetBoolOption = null; }
        if (cf == null || !cf.NeedSave) return;
        ConfigurationLevel storeLevel = (cf.chkUseGlobalConfig.CheckState == CheckState.Checked ? ConfigurationLevel.Global : ConfigurationLevel.Local);
        if (cf.chkRemoteUser.Checked     && cf.txtRemoteUser.Text      != "") cfg.Set("cozygit.remoteuser",     cf.txtRemoteUser.Text,     storeLevel); else cfg.UnsetAll("cozygit.remoteuser",     storeLevel);
        if (cf.chkRemotePassword.Checked && cf.txtRemotePassword.Text  != "") cfg.Set("cozygit.remotepassword", cf.txtRemotePassword.Text, storeLevel); else cfg.UnsetAll("cozygit.remotepassword", storeLevel);
        if (cf.chkAuthorName.Checked     && cf.txtAuthorName.Text      != "") cfg.Set("cozygit.authorname",     cf.txtAuthorName.Text,     storeLevel); else cfg.UnsetAll("cozygit.authorname",     storeLevel);
        if (cf.chkAuthorEmail.Checked    && cf.txtAuthorEmail.Text     != "") cfg.Set("cozygit.authoremail",    cf.txtAuthorEmail.Text,    storeLevel); else cfg.UnsetAll("cozygit.authoremail",    storeLevel);
        if (cf.chkCommitterName.Checked  && cf.txtCommitterName.Text   != "") cfg.Set("cozygit.committername",  cf.txtCommitterName.Text,  storeLevel); else cfg.UnsetAll("cozygit.committername",  storeLevel);
        if (cf.chkCommitterEmail.Checked && cf.txtCommitterEmail.Text  != "") cfg.Set("cozygit.committeremail", cf.txtCommitterEmail.Text, storeLevel); else cfg.UnsetAll("cozygit.committeremail", storeLevel);
        if (cf.txtDiffTool.Text != "") cfg.Set("cozygit.difftool", cf.txtDiffTool.Text, ConfigurationLevel.Global); else cfg.UnsetAll("cozygit.difftool", ConfigurationLevel.Global);
        cfg.Set("cozygit.savedsettings", true, ConfigurationLevel.Local);
        cf.NeedSave = false;
    }

    enum AuthedOperation { Clone, Pull, Push };
    static void PerformAuthedOperation(AuthedOperation op, Action<LibGit2Sharp.Handlers.CredentialsHandler> action)
    {
        f.Progress.Value = 0;
        f.Progress.Visible = true;
        SetProgress(1,100);
        bool uselogin = false;
        try
        {
            uselogin = (repo != null && repo.Config.GetValueOrDefault<bool>("cozygit.uselogin", ConfigurationLevel.Local));
            if (!uselogin && op == AuthedOperation.Push && repo != null) uselogin = repo.Config.GetValueOrDefault<bool>("cozygit.pushuselogin", ConfigurationLevel.Local);
            if (uselogin) throw new LibGit2Sharp.LibGit2SharpException("401");
            action(null);
            return;
        }
        catch (LibGit2SharpException e)
        {
            if (!e.Message.Contains("401")) throw; // unauthorized
            SetProgress(2,100);
            try
            {
                var sf = GetSettings(GetSettingsMode.Auth, "Please enter login details for " + (op == AuthedOperation.Clone ? "cloning" : op == AuthedOperation.Pull ? "pulling from" : "pushing to") + " remote branch");
                if (sf == null) throw new System.Exception("Credentials not entered");
                action((_url, _user, _cred) => new UsernamePasswordCredentials { Username = sf.txtRemoteUser.Text, Password = sf.txtRemotePassword.Text });
                if (!uselogin) sf.SetBoolOption = (op == AuthedOperation.Push ? "cozygit.pushuselogin" : "cozygit.uselogin");
                if (!uselogin && repo != null) SaveSettingsIfNeeded(repo.Config);
            }
            catch (Exception) { throw; }
        }
        catch (Exception) { throw; }
        finally { f.Progress.Visible = false; }
    }

    static Blob GetRepoBlob(string fileRelPath, FileStatus fileStatus, bool getRemote = false)
    {
        if ((fileStatus & FileStatus.NewInWorkdir) != 0) return null;
        Commit tip = (getRemote ? repo.Head.TrackedBranch : repo.Head).Tip;
        TreeEntry treeEntry = (tip == null ? null : tip[fileRelPath]);
        return (treeEntry != null && treeEntry.TargetType == TreeEntryTargetType.Blob ? (Blob)treeEntry.Target : null);
    }

    static void RunDiff(string src_path, string dst_path)
    {
        var sf = GetSettings(GetSettingsMode.DiffTool, "Please configure the path to your diff viewer tool");
        if (sf == null)
            MessageBox.Show(f, "Missing path to diff tool", "CozyGit - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        else
            System.Diagnostics.Process.Start(sf.txtDiffTool.Text, "\"" + src_path + "\" \"" + dst_path + "\"");
    }

    static void ShowDiff(string src_relpath, string dst_relpath)
    {
        string src_path = repo.Info.WorkingDirectory + src_relpath, dst_path = repo.Info.WorkingDirectory + dst_relpath;
        if (!File.Exists(src_path)) using (var fs = MakeTemp(out src_path, "diff-src-" + src_relpath.GetHashCode().ToString("x"), dst_relpath)) { }
        if (!File.Exists(dst_path)) using (var fs = MakeTemp(out dst_path, "diff-dst-" + dst_relpath.GetHashCode().ToString("x"), dst_relpath)) { }
        RunDiff(src_path, dst_path);
    }

    static void ShowDiff(Blob src_blob, string dst_relpath)
    {
        string src_path, dst_path = repo.Info.WorkingDirectory + dst_relpath;
        if (!File.Exists(dst_path)) { ShowDiff(src_blob, (Blob)null, dst_relpath); return; }
        using (var fs = MakeTemp(out src_path, "diff-src-" + dst_relpath.GetHashCode().ToString("x"), dst_relpath)) { if (src_blob != null) src_blob.GetContentStream().CopyTo(fs); }
        RunDiff(src_path, dst_path);
    }

    static void ShowDiff(Blob src_blob, Blob dst_blob, string relpath)
    {
        string src_path, dst_path;
        using (var fs = MakeTemp(out src_path, "diff-src-" + (src_blob == null ? "empty" : src_blob.Id.ToString()), relpath)) { if (src_blob != null) src_blob.GetContentStream().CopyTo(fs); }
        using (var fs = MakeTemp(out dst_path, "diff-dst-" + (dst_blob == null ? "empty" : dst_blob.Id.ToString()), relpath)) { if (dst_blob != null) dst_blob.GetContentStream().CopyTo(fs); }
        RunDiff(src_path, dst_path);
    }

    static void ShowLog(string filterPath = null)
    {
        FormLog lf = new FormLog();
        lf.txtMessage.Font = f.txtMessage.Font;

        GridDataList<LogItem> loglist = new GridDataList<LogItem>(), filterloglist = null;
        lf.gridHistory.AddCol<DataGridViewTextBoxCell>("Commit SHA", "ColSHA", 1, "SHA");
        lf.gridHistory.AddCol<DataGridViewTextBoxCell>("Author", "ColAuthor", 3, "Author");
        lf.gridHistory.AddCol<DataGridViewTextBoxCell>("Committer", "ColCommitter", 3, "Committer");
        lf.gridHistory.AddCol<DataGridViewTextBoxCell>("Date", "ColDate", 2, "Date");
        lf.gridHistory.AddCol<DataGridViewTextBoxCell>("Parent SHA", "ColParentSHA", 1, "ParentSHA");
        lf.gridHistory.ColumnHeaderMouseClick       += loglist.GridOnColumnHeaderClick;
        lf.gridHistory.ColumnHeaderMouseDoubleClick += loglist.GridOnColumnHeaderClick;

        GridDataList<LogFileItem> filelist = new GridDataList<LogFileItem>();
        lf.gridFiles.AddCol<DataGridViewImageCell>("", "ColIcon", 0, null);
        lf.gridFiles.AddCol<DataGridViewTextBoxCell>("Path", "ColPath", 6, "Path");
        lf.gridFiles.AddCol<DataGridViewTextBoxCell>("Action", "ColStatus", 1, "Status");
        lf.gridFiles.AddCol<DataGridViewTextBoxCell>("Renamed/Copied From", "ColOldPath", 2, "OldPath");
        (lf.gridFiles.Columns[0].CellTemplate as DataGridViewImageCell).ImageLayout = DataGridViewImageCellLayout.Zoom;
        lf.gridFiles.ColumnHeaderMouseClick       += filelist.GridOnColumnHeaderClick;
        lf.gridFiles.ColumnHeaderMouseDoubleClick += filelist.GridOnColumnHeaderClick;
        lf.gridFiles.CellMouseDoubleClick += (object sender, DataGridViewCellMouseEventArgs e) =>
        {
            if (e.RowIndex >= 0 && e.RowIndex < filelist.Count && e.Clicks == 2 && e.Button == MouseButtons.Left)
                filelist[e.RowIndex].Diff();
        };
        Action<Point, int> gridFileContextMenu = (Point menuPos, int row) =>
        {
            ContextMenuStrip context = new ContextMenuStrip();
            context.Items.Add("Diff", Data.icon_diff).Click += (object _o, EventArgs _a) => filelist[row].Diff();
            context.Items.Add("Show log", Data.icon_log).Click += (object _o, EventArgs _a) => ShowLog(filelist[row].Path);
            context.Show(lf.gridFiles, menuPos);
        };
        lf.gridFiles.CellMouseClick += (object sender, DataGridViewCellMouseEventArgs e) =>
        {
            if (e.Button != MouseButtons.Right || e.RowIndex < 0 || e.RowIndex > lf.gridFiles.RowCount) return;
            if (!lf.gridFiles.Rows[e.RowIndex].Selected)
            {
                lf.gridFiles.ClearSelection();
                lf.gridFiles.Rows[e.RowIndex].Selected = true;
            }
            gridFileContextMenu(lf.gridFiles.PointToClient(Cursor.Position), e.RowIndex);
        };
        lf.gridFiles.KeyUp += (object sender, KeyEventArgs e) =>
        {
            if (e.KeyCode != Keys.Apps || lf.gridFiles.CurrentRow == null) return;
            Point pos = lf.gridFiles.GetCellDisplayRectangle(1, lf.gridFiles.CurrentRow.Index, false).Location;
            pos.X += 25; pos.Y += 5;
            gridFileContextMenu(pos, lf.gridFiles.CurrentRow.Index);
        };
        lf.gridFiles.KeyDown += (object sender, KeyEventArgs e) =>
        {
            if (e.KeyCode != Keys.Enter || lf.gridFiles.CurrentRow == null) return;
            ((LogFileItem)lf.gridFiles.CurrentRow.DataBoundItem).Diff();
            e.Handled = true;
        };

        int selectedCount = 0;
        Action updateStats = () =>
        {
            lf.lblFooter.Text = "Listing " + loglist.Count + " revision(s) - "
                + ((filterloglist != null && filterloglist.Count > 0) ? "Filtering " + filterloglist.Count.ToString() + " revision(s) - " : "")
                + selectedCount.ToString() + " revision(s) selected, showing " + filelist.Count.ToString() + " changed file(s)";
        };

        lf.gridHistory.SelectionChanged += (object sender, EventArgs e) =>
        {
            StringBuilder sb = new StringBuilder();
            string split = Environment.NewLine + "- - - - - - - - - - - - - - - - - - - -" + Environment.NewLine;
            filelist.Clear();
            Dictionary<string, int> listedFiles = new Dictionary<string, int>();
            selectedCount = 0;
            foreach (DataGridViewRow row in lf.gridHistory.SelectedRows)
            {
                if (!row.Visible) continue;
                selectedCount++;
                Commit c = ((LogItem)row.DataBoundItem).Commit;
                sb.Append((sb.Length > 0 ? split : "") + c.Message);

                Commit parent = c.Parents.GetFirstElement();
                foreach (TreeEntryChanges it in repo.Diff.Compare<TreeChanges>((parent == null ? null : parent.Tree), c.Tree))
                {
                    int existingIndex;
                    if (listedFiles.TryGetValue(it.Path, out existingIndex))
                    {
                        LogFileItem jt = filelist[existingIndex];
                        if (c.Committer.When < jt.MinDate) { jt.MinDate = c.Committer.When; jt.MinCommit = c; }
                        if (c.Committer.When > jt.MaxDate) { jt.MaxDate = c.Committer.When; jt.MaxCommit = c; }
                        continue;
                    }
                    Icon icon = GetCachedIcon(new FileInfo(repo.Info.WorkingDirectory + it.Path));
                    listedFiles.Add(it.Path, filelist.Count);
                    filelist.Add(new LogFileItem { Icon = icon, Path = it.Path, Status = it.Status, OldPath = (it.Status == ChangeKind.Renamed ? it.OldPath : null), MinCommit = c, MaxCommit = c, MinDate = c.Committer.When, MaxDate = c.Committer.When });
                }
            }
            lf.txtMessage.Text = sb.ToString();
            filelist.BroadcastListChanged(lf.gridFiles, true);
            updateStats();
        };

        Action updateFilter = () =>
        {
            string flt = lf.txtFilter.Text.Trim();
            if (flt.Length > 0)
            {
                if (filterloglist == null) filterloglist = new GridDataList<LogItem>(); else filterloglist.Clear();
                for (int i = 0; i != loglist.Count; i++)
                {
                    LogItem li = loglist[i];
                    if (li.Commit.Message.IndexOf(flt, StringComparison.CurrentCultureIgnoreCase) >= 0 || li.Author.IndexOf(flt, StringComparison.CurrentCultureIgnoreCase) >= 0 || li.Committer.IndexOf(flt, StringComparison.CurrentCultureIgnoreCase) >= 0 || li.SHA.IndexOf(flt, StringComparison.OrdinalIgnoreCase) >= 0 || li.ParentSHA.IndexOf(flt, StringComparison.OrdinalIgnoreCase) >= 0)
                        filterloglist.Add(li);
                }
            }
            lf.gridHistory.DataSource = (flt.Length > 0 ? filterloglist : loglist);
            (flt.Length > 0 ? filterloglist : loglist).BroadcastListChanged(lf.gridHistory);
            updateStats();
        };

        lf.txtFilter.TextChanged += (object sender, EventArgs e) => updateFilter();
        lf.btnClearFilter.Click += (object sender, EventArgs e) => lf.txtFilter.Text = "";

        IEnumerator<LogEntry> LogEntryEnumerator = null;
        IEnumerator<Commit> CommitEnumerator = null;
        Action<int> GetMoreEntries = (int n) =>
        {
            for (Commit c; n > 0; n--)
            {
                if (CommitEnumerator != null) { try { if (!CommitEnumerator.MoveNext()) break; } catch (NotFoundException) { break; /* log with truncated history depth */ } c = CommitEnumerator.Current; }
                else { if (!LogEntryEnumerator.MoveNext()) break; c = LogEntryEnumerator.Current.Commit; }
                Commit parent = c.Parents.GetFirstElement();
                loglist.Add(new LogItem { SHA = c.Sha, Author = c.Author.ToString(), Committer = (c.Author == c.Committer ? "" : c.Committer.ToString()), Date = c.Committer.When, ParentSHA = (parent == null ? "" : parent.Sha), Commit = c });
            }
            if (n > 0) lf.btnShowAll.Enabled = lf.btnNext100.Enabled = false;
            updateFilter();
        };

        lf.btnNext100.Click += (object sender, EventArgs e) => GetMoreEntries(100);
        lf.btnShowAll.Click += (object sender, EventArgs e) => GetMoreEntries(int.MaxValue);

        lf.Shown += (object sender, EventArgs e) =>
        {
            lf.Enabled = false;

            lf.gridFiles.DataSource = filelist;
            lf.gridHistory.DataSource = loglist;
            if (filterPath == null) CommitEnumerator = repo.Commits.QueryBy(new CommitFilter { SortBy = CommitSortStrategies.None }).GetEnumerator();
            else LogEntryEnumerator = repo.Commits.QueryBy(filterPath).GetEnumerator();
            GetMoreEntries(100);

            lf.Enabled = true;
        };

        LogWindows.Add(lf);
        lf.FormClosed += (object sender, FormClosedEventArgs e) => LogWindows.Remove(lf);
        lf.btnOK.Click += (object sender, EventArgs e) => lf.Close();

        if (filterPath != null) lf.Text += " - " + filterPath;

        lf.Show();
    }

    class LogItem
    {
        public string         SHA;       public string ColSHA       { get { return SHA;       } }
        public string         Author;    public string ColAuthor    { get { return Author;    } }
        public string         Committer; public string ColCommitter { get { return Committer; } }
        public DateTimeOffset Date;      public string ColDate      { get { return Date.LocalDateTime.ToString(); } }
        public string         ParentSHA; public string ColParentSHA { get { return ParentSHA; } }
        public Commit Commit;
    };

    class LogFileItem
    {
        public string     Path;    public string     ColPath    { get { return Path;    } }
        public Icon       Icon;    public Icon       ColIcon    { get { return Icon;    } }
        public ChangeKind Status;  public ChangeKind ColStatus  { get { return Status;  } }
        public string     OldPath; public string     ColOldPath { get { return OldPath; } }
        public DateTimeOffset MinDate, MaxDate;
        public Commit MinCommit, MaxCommit;

        public void Diff()
        {
            Commit cFrom = MinCommit.Parents.GetFirstElement(), cTo = MaxCommit;
            TreeEntry teFrom = (cFrom == null ? null : cFrom[Path]), teTo = (cTo == null ? null : cTo[Path]);
            Blob blobFrom = (teFrom != null && teFrom.TargetType == TreeEntryTargetType.Blob ? (Blob)teFrom.Target : null), blobTo = (teTo != null && teTo.TargetType == TreeEntryTargetType.Blob ? (Blob)teTo.Target : null);
            ShowDiff(blobFrom, blobTo, Path);
        }
    };

    class Entry
    {
        internal static string RestorePointExtension = ".cozygitrestore";
        internal static StageOptions DefaultStageOptions = new StageOptions { IncludeIgnored = true };
        static Icon RestoreIcon = Icon.FromHandle(Data.icon_restore.GetHicon());

        public bool           Active;                     public bool   ColActive    { get { return Active; } set { if (Active != value && Status != FileStatus.Unaltered) SetActive(value); } }
        public bool IsFolder = false, IsExpanded = false; public bool?  ColFolder    { get { if (IsFolder) return IsExpanded; return null;     } }
        public Icon           Icon;                       public Icon   ColIcon      { get { return (RestoreAfterCommit ? RestoreIcon : Icon); } }
        public string         Path;                       public string ColPath      { get { return Path;                                      } }
        public DateTimeOffset ModDate;                    public string ColModDate   { get { return ModDate.LocalDateTime.ToString();          } }
        public long           Size;                       public string ColSize      { get { return (Size >= 0 ? Size.ToString("N0") : string.Empty); } }
        public string         Extension;                  public string ColExtension { get { return Extension;                                 } }
        public FileStatus     Status;                     public string ColStatus    { get
        {
            switch (Status)
            {
                case FileStatus.Unaltered: return "unchanged";
                case FileStatus.NewInWorkdir:case FileStatus.NewInIndex: return "unversioned";
                case FileStatus.ModifiedInWorkdir:case FileStatus.ModifiedInIndex: return "modified";
                case FileStatus.DeletedFromWorkdir:case FileStatus.DeletedFromIndex: return "deleted";
                case FileStatus.DeletedFromIndex | FileStatus.NewInWorkdir: return "deleted (local file exists)";
                case FileStatus.RenamedInWorkdir: case FileStatus.RenamedInIndex: return "renamed";
                case FileStatus.TypeChangeInWorkdir: case FileStatus.TypeChangeInIndex: return "type change";
                case FileStatus.Ignored: return "ignored";
                case FileStatus.Conflicted: return "conflicted";
            }
            return "Unknown-" + Status.ToString() + "-0x" + ((int)Status).ToString("x");;
        }}
        public bool RestoreAfterCommit;
        public string AbsPath { get { return repo.Info.WorkingDirectory + Path.Replace('/', System.IO.Path.DirectorySeparatorChar); } }
        public string AbsRestorePath { get { return AbsPath + RestorePointExtension; } }

        void SetActive(bool v)
        {
            Active = v;
            if (v) { Commands.Stage(repo, Path, DefaultStageOptions); ActiveCount++; }
            else { Commands.Unstage(repo, Path); ActiveCount--; }
            Status = repo.RetrieveStatus(Path);
            f.lblStatus.Text = ActiveCount.ToString() + " files selected, " + el.Count.ToString() + " files total";
            f.btnOK.Enabled = (ActiveCount > 0);
        }
    }

    static FileStream MakeTemp(out string path, string basename, string getExtensionFrom)
    {
        Directory.CreateDirectory(repo.Info.Path + "CozyGitTemp");
        return File.Create(path = repo.Info.Path + "CozyGitTemp" + Path.DirectorySeparatorChar + basename + Path.GetExtension(getExtensionFrom));
    }

    static void DeleteTemp()
    {
        if (repo != null) SuperDelete(repo.Info.Path + "CozyGitTemp");
    }

    static void SuperDelete(string dirname)
    {
        DirectoryInfo di = new DirectoryInfo(dirname);
        for (int retry = 0; di.Exists && retry < 5; retry++)
        {
            try { di.Delete(true); } catch { }
            di.Refresh();
            if (!di.Exists) return;
            try { foreach (var f in di.EnumerateFileSystemInfos("*.*", SearchOption.AllDirectories)) f.Attributes &= ~FileAttributes.ReadOnly; } catch { }
        }
    }

    static bool FileEqualContent(string a_path, string b_path)
    {
        FileInfo a = new FileInfo(a_path), b = new FileInfo(b_path);
        if (a.Length != b.Length) return false;
        byte[] bufa = new byte[8], bufb = new byte[8];
        using (FileStream fsa = a.OpenRead())
            using (FileStream fsb = b.OpenRead())
                for (long i = 0, end = ((a.Length + 7) / 8); i != end; i++) if (fsa.Read(bufa, 0, 8) == 0 || fsb.Read(bufb, 0, 8) == 0 || BitConverter.ToInt64(bufa, 0) != BitConverter.ToInt64(bufb, 0)) return false;
        return true;
    }
}

class DataGridViewFolderButton : DataGridViewButtonCell
{
    protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
    {
        if (value == null) paintParts &= ~(DataGridViewPaintParts.ContentBackground | DataGridViewPaintParts.ContentForeground);
        advancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
        base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, null, null, errorText, cellStyle, advancedBorderStyle, paintParts);
        if (value != null)
            TextRenderer.DrawText(graphics, ((bool)value ? "--" : "+"), cellStyle.Font, new Point(cellBounds.Left + 4, cellBounds.Top + 3), Color.DarkSlateGray);
    }
}

class GridDataList<T> : List<T>, System.ComponentModel.IBindingList
{
    private Comparison<T> SortFn;
    private SortPropDesc SortProp;
    private bool SortDesc;

    public class SortPropDesc : System.ComponentModel.PropertyDescriptor
    {
        public SortPropDesc(string name) : base(name, null) { }
        public override bool CanResetValue(object c) { return false; }
        public override Type ComponentType { get { return null; } }
        public override object GetValue(object c) { return null; }
        public override bool IsReadOnly { get { return false; } }
        public override Type PropertyType { get { return null; } }
        public override void ResetValue(object c) { }
        public override void SetValue(object c, object v) { }
        public override bool ShouldSerializeValue(object c) { return false; }
    }

    public void SetSortFn(string sortProp, string sortField, bool desc = false)
    {
        if (sortProp == null)
        {
            SortDesc = false;
            SortProp = null;
            SortFn = null;
            return;
        }

        System.Reflection.FieldInfo fi = GetType().GetGenericArguments()[0].GetField(sortField);
        if      (fi.FieldType == typeof(string)         && !desc) SortFn = (T a, T b) =>  string.Compare((string)fi.GetValue(a), (string)fi.GetValue(b), StringComparison.OrdinalIgnoreCase);
        else if (fi.FieldType == typeof(string)         &&  desc) SortFn = (T a, T b) => -string.Compare((string)fi.GetValue(a), (string)fi.GetValue(b), StringComparison.OrdinalIgnoreCase);
        else if (fi.FieldType == typeof(DateTimeOffset) && !desc) SortFn = (T a, T b) =>  DateTimeOffset.Compare((DateTimeOffset)fi.GetValue(a), (DateTimeOffset)fi.GetValue(b));
        else if (fi.FieldType == typeof(DateTimeOffset) &&  desc) SortFn = (T a, T b) => -DateTimeOffset.Compare((DateTimeOffset)fi.GetValue(a), (DateTimeOffset)fi.GetValue(b));
        else if (fi.FieldType == typeof(int)           && !desc) SortFn = (T a, T b) => (int)fi.GetValue(a)-(int)fi.GetValue(b);
        else if (fi.FieldType == typeof(int)           &&  desc) SortFn = (T a, T b) => (int)fi.GetValue(b)-(int)fi.GetValue(a);
        else if (fi.FieldType == typeof(uint)          && !desc) SortFn = (T a, T b) => (uint)fi.GetValue(a)>(uint)fi.GetValue(b)?1:(uint)fi.GetValue(a)<(uint)fi.GetValue(b)?-1:0;
        else if (fi.FieldType == typeof(uint)          &&  desc) SortFn = (T a, T b) => (uint)fi.GetValue(b)>(uint)fi.GetValue(a)?1:(uint)fi.GetValue(b)<(uint)fi.GetValue(a)?-1:0;
        else if (fi.FieldType == typeof(Int64)         && !desc) SortFn = (T a, T b) => (Int64)fi.GetValue(a)>(Int64)fi.GetValue(b)?1:(Int64)fi.GetValue(a)<(Int64)fi.GetValue(b)?-1:0;
        else if (fi.FieldType == typeof(Int64)         &&  desc) SortFn = (T a, T b) => (Int64)fi.GetValue(b)>(Int64)fi.GetValue(a)?1:(Int64)fi.GetValue(b)<(Int64)fi.GetValue(a)?-1:0;
        else if (fi.FieldType == typeof(bool)          && !desc) SortFn = (T a, T b) => ((bool)fi.GetValue(a) ? 1 : 0) - ((bool)fi.GetValue(b) ? 1 : 0);
        else if (fi.FieldType == typeof(bool)          &&  desc) SortFn = (T a, T b) => ((bool)fi.GetValue(b) ? 1 : 0) - ((bool)fi.GetValue(a) ? 1 : 0);
        else if (fi.FieldType.IsEnum                   && !desc) SortFn = (T a, T b) => (int)fi.GetValue(a)-(int)fi.GetValue(b);
        else if (fi.FieldType.IsEnum                   &&  desc) SortFn = (T a, T b) => (int)fi.GetValue(b)-(int)fi.GetValue(a);
        else throw new NotImplementedException();
        SortDesc = desc;
        SortProp = new SortPropDesc(sortProp);
        Sort(SortFn);
    }

    public bool AllowEdit                  { get { return true; } }
    public bool IsSorted                   { get { return (SortProp != null); } }
    public bool SupportsChangeNotification { get { return true; } }
    public bool SupportsSorting            { get { return true; } }
    public bool SupportsSearching          { get { return false; } }
    public bool AllowNew                   { get { return false; } }
    public bool AllowRemove                { get { return false; } }
    public System.ComponentModel.ListSortDirection SortDirection { get { return (SortDesc ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending); } }
    public System.ComponentModel.PropertyDescriptor SortProperty { get { return SortProp; } }
    public void AddIndex(System.ComponentModel.PropertyDescriptor property) { }
    public object AddNew() { return null; }
    public void ApplySort(System.ComponentModel.PropertyDescriptor property, System.ComponentModel.ListSortDirection direction) { }
    public int Find(System.ComponentModel.PropertyDescriptor property, object key) { return 0; }
    public void RemoveIndex(System.ComponentModel.PropertyDescriptor property) { }
    public void RemoveSort() { }
    public event System.ComponentModel.ListChangedEventHandler ListChanged;
    public void BroadcastRowChanged(int row) { if (ListChanged != null) ListChanged(this, new System.ComponentModel.ListChangedEventArgs(System.ComponentModel.ListChangedType.ItemChanged, row)); }
    public void BroadcastListChanged(DataGridView g, bool reApplySort = false)
    {
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // without this column sizes jitter weirdly
        if (ListChanged != null) ListChanged(this, new System.ComponentModel.ListChangedEventArgs(System.ComponentModel.ListChangedType.Reset, 0));
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        if (reApplySort && SortFn != null) Sort(SortFn);
    }

    public void GridOnColumnHeaderClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        DataGridView g = (DataGridView)sender;
        DataGridViewColumn col = g.Columns[e.ColumnIndex];
        if (col.HeaderText.Length == 0) return;
        if (e.Button == MouseButtons.Right)
        {
            Point menuPos = g.PointToClient(Cursor.Position);
            ContextMenu context = new ContextMenu();
            foreach (DataGridViewColumn it in g.Columns)
            {
                if (it.HeaderText.Length == 0) continue;
                MenuItem i = context.MenuItems.Add(it.HeaderText);
                i.Checked = it.Visible;
                i.Tag = it;
                i.Click += (object _o, EventArgs ee) =>
                {
                    (_o as MenuItem).Checked ^= true;
                    ((DataGridViewColumn)(_o as MenuItem).Tag).Visible ^= true;
                    (_o as MenuItem).GetContextMenu().Show(g, menuPos);
                };
            }
            context.Show(g, menuPos);
            return;
        }
        if (col.Tag == null) return; // no sort field
        SortOrder order = g.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection;
        order = (order == SortOrder.Ascending ? SortOrder.Descending : (order == SortOrder.Descending ? SortOrder.None : SortOrder.Ascending));
        foreach (DataGridViewColumn c in g.Columns) c.HeaderCell.SortGlyphDirection = SortOrder.None;
        g.CancelEdit();
        g.CurrentCell = null;

        if (order == SortOrder.None)
            SetSortFn(null, null);
        else
            SetSortFn(g.Columns[e.ColumnIndex].DataPropertyName, (string)col.Tag, (order == SortOrder.Descending));

        BroadcastListChanged(g);
    }
}

static class ExtensionMethods
{
    //Enable double buffering on the data grid for fast rendering and scrolling (disable while resizing columns)
    internal static void SetDoubleBuffering(this Control c)
    {
        c.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(c, true, null);
        if (!(c is DataGridView)) return;
        c.MouseDown += (object s, MouseEventArgs e) => { if (e.Y <= (s as DataGridView).ColumnHeadersHeight) s.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(s, false, null); };
        c.MouseUp   += (object s, MouseEventArgs e) => {                                                     s.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(s,  true, null); };
    }

    internal static void AddCol<T>(this DataGridView g, string HeaderText, string DataPropertyName, float FillWeight, string SortField) where T : DataGridViewCell, new()
    {
        DataGridViewColumn res = new DataGridViewColumn(new T());
        res.HeaderText = HeaderText;
        res.DataPropertyName = DataPropertyName;
        res.HeaderCell.Style.WrapMode = DataGridViewTriState.False;
        res.SortMode = (SortField != null ? DataGridViewColumnSortMode.Programmatic : DataGridViewColumnSortMode.NotSortable);
        res.Tag = SortField;
        if (FillWeight > 0) res.FillWeight = FillWeight;
        else { res.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; res.Resizable = DataGridViewTriState.False; res.Width = 20; }
        g.Columns.Add(res);
    }

    internal static T GetFirstElement<T>(this IEnumerable<T> e)
    {
        if (e == null) return default(T);
        IEnumerator<T> ee = e.GetEnumerator();
        return (ee.MoveNext() ? (T)ee.Current : default(T));
    }
}
}
