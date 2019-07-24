using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckOut
{
    public partial class Form1 : Form
    {
        List<UserPrincipal> listAdUser = new List<UserPrincipal>();

        private string ausgeschiedenOU = "OU=Ausgeschieden,OU=Arges_Intern,DC=arges,DC=local";
        private string ausgeschiedenGruppe = "Ausgeschieden";

        public Form1()
        {
            InitializeComponent();

            listAdUser = GetAllADUsers();
            fillListWithUser();
        }

        // HAUPTFUNKTION
        private void btn_checkout_Click(object sender, EventArgs e)
        {
            // Prüfung ob Pflichtfelder ausgefüllt sind
            if (txt_kuerzel.Text == "" || txt_austrittsdatum.Text == "")
            {
                label1.ForeColor = Color.Red;
                label2.ForeColor = Color.Red;
                MessageBox.Show("Bitte die Plichtfelder ausfüllen.",
                                "Pflichtfeld",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation,
                                MessageBoxDefaultButton.Button1);
                return;
            }

            // Zeige Sanduhr
            Cursor.Current = Cursors.WaitCursor;

            string userName = listBox1.SelectedItem.ToString();

            // Suche User Pricipal
            PrincipalContext principalContext = new PrincipalContext(ContextType.Domain);
            UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(principalContext, userName);

            List<GroupPrincipal> lstGrp = findGroupToUser(userPrincipal);

            DeleteManager(userPrincipal);
            DeleteTelNumber(userPrincipal);

            AddUserToGroup(userPrincipal, ausgeschiedenGruppe);
            //System.Threading.Thread.Sleep(10000);
            SetPrimaryGroup(userPrincipal, ausgeschiedenGruppe);

            // Baue Text für das Notiz Feld zusammen
            string kuerzel = txt_kuerzel.Text;
            string austritt = txt_austrittsdatum.Text;
            string note = $"Ausgeschieden zum {austritt},\r\nMitgliedschaften entfern von ";
            foreach (var groupPrincipal in lstGrp)
            {
                if (groupPrincipal.Name == ausgeschiedenGruppe) continue;

                RemoveUserFromGroup(userPrincipal, groupPrincipal);
                note += groupPrincipal.Name + "; ";
            }
            note += "\r\nKonto deaktiviert " + DateTime.Now.ToShortDateString() + " " + kuerzel;

            WriteNoteToUser(userPrincipal, note);

            DisableADUser(userPrincipal);
            MoveUserToAusgeschiedenOU(userPrincipal);

            // Zeige nomalen Mauszeiger
            Cursor.Current = Cursors.Default;
        }

        void fillListWithUser()
        {
            foreach (var user in listAdUser)
            {
                if (user.UserPrincipalName != null)
                {
                    listBox1.Items.Add(user.Name);
                }
            }
            listBox1.Sorted = true;
        }

        List<UserPrincipal> GetAllADUsers()
        {
            // create your domain context
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
            // define a "query-by-example" principal - here, we search for a UserPrincipal 
            UserPrincipal qbeUser = new UserPrincipal(ctx);
            // create your principal searcher passing in the QBE principal    
            PrincipalSearcher srch = new PrincipalSearcher(qbeUser);

            List<UserPrincipal> lst = new List<UserPrincipal>();
            // find all matches
            foreach (var found in srch.FindAll())
            {
                UserPrincipal user = found as UserPrincipal;

                if (user != null)
                {
                    var usersSid = (user.Sid.Value == null) ? "" : user.Sid.Value;
                    var username = (user.DisplayName == null) ? "" : user.DisplayName;
                    var userSamAccountName = (user.SamAccountName == null) ? "" : user.SamAccountName;
                    var userDis = (user.DistinguishedName == null) ? "" : user.DistinguishedName;

                    if (user.Enabled.Value)
                    {
                        lst.Add(user);
                    }
                }
            }
            return lst;
        }

        #region TEXTBOX USER SEARCH
        void txt_userSearch_Init()
        {
            txt_userSearch.Text = "search user...";
        }

        private void txt_userSearch_TextChanged(object sender, EventArgs e)
        {
            if (txt_userSearch.Text == "search user...")
                return;

            listBox1.Items.Clear();
            foreach (var user in listAdUser)
            {
                if (user.SamAccountName.StartsWith(txt_userSearch.Text, StringComparison.CurrentCultureIgnoreCase))
                {
                    listBox1.Items.Add(user.Name);
                }
            }
        }

        private void txt_userSearch_Enter(object sender, EventArgs e)
        {
            if (txt_userSearch.Text == "search user...")
            {
                txt_userSearch.Text = "";
            }
        }

        private void txt_userSearch_Leave(object sender, EventArgs e)
        {
            if (txt_userSearch.Text == "")
            {
                txt_userSearch.Text = "search user...";
            }
        }
        #endregion


        void MoveUserToAusgeschiedenOU(UserPrincipal userPrincipal)
        {
            string locationLDAP = GetObjectLocation(userPrincipal);

            DirectoryEntry eLocation = new DirectoryEntry("LDAP://" + locationLDAP);
            DirectoryEntry nLocation = new DirectoryEntry("LDAP://" + ausgeschiedenOU);
            string newName = eLocation.Name;
            eLocation.MoveTo(nLocation, newName);
            nLocation.Close();
            eLocation.Close();
        }

        List<GroupPrincipal> findGroupToUser(UserPrincipal userPrincipal)
        {
            List<GroupPrincipal> retList = new List<GroupPrincipal>();

            var grp = userPrincipal.GetGroups();

            foreach (var g in grp)
            {
                if (g.Sid == null)
                    continue;

                retList.Add(g as GroupPrincipal);
            }

            return retList;
        }

        void AddUserToGroup(UserPrincipal userPrincipal, string groupName)
        {
            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain))
                {
                    GroupPrincipal group = GroupPrincipal.FindByIdentity(pc, groupName);
                    group.Members.Add(pc, IdentityType.Sid, userPrincipal.Sid.ToString());
                    group.Save();
                }
            }
            catch (Exception)
            {
                //doSomething with E.Message.ToString(); 
                //MessageBox.Show("AddUserToGroup");
            }
        }

        void RemoveUserFromGroup(UserPrincipal userPrincipal, GroupPrincipal groupPrincipal)
        {
            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain))
                {
                    groupPrincipal.Members.Remove(pc, IdentityType.Sid, userPrincipal.Sid.Value);
                    groupPrincipal.Save();
                }
            }
            catch (Exception)
            {
                //doSomething with E.Message.ToString(); 
                //MessageBox.Show("RemoveUserFromGroup");
                System.Threading.Thread.Sleep(3000);
                RemoveUserFromGroup(userPrincipal, groupPrincipal);
            }
        }

        void WriteNoteToUser(UserPrincipal userPrincipal, string note)
        {
            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "arges"))
                {
                    DirectoryEntry udo = userPrincipal.GetUnderlyingObject() as DirectoryEntry;

                    string _note = (udo.Properties["info"].Value == null) ? "" : udo.Properties["info"].Value.ToString() + "\r\n\r\n";

                    udo.Properties["info"].Value = _note + note;
                    userPrincipal.Save();
                }
            }
            catch (Exception)
            {
                //doSomething with E.Message.ToString(); 
                //MessageBox.Show("WriteNoteToUser");
            }
        }

        void SetPrimaryGroup(UserPrincipal userPrincipal, string groupname)
        {
            var ctx = new PrincipalContext(ContextType.Domain);
            var group = GroupPrincipal.FindByIdentity(ctx, groupname);

            string sid = group.Sid.Value;
            int newPrimaryGroupId = Convert.ToInt32(sid.Substring(sid.LastIndexOf('-') + 1));
            var userEntry = userPrincipal.GetUnderlyingObject() as DirectoryEntry;
            userEntry.Properties["primaryGroupID"].Value = newPrimaryGroupId;

            for (int i = 0; i < 50; i++)
            {
                try
                {
                    userEntry.CommitChanges();
                    return;
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(3000);
                }
            }

            
        }

        void DisableADUser(UserPrincipal userPrincipal)
        {
            try
            {
                userPrincipal.Enabled = false;
                userPrincipal.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        string GetObjectLocation(UserPrincipal userPrincipal)
        {
            foreach (var user in listAdUser)
            {
                if (user.Sid == userPrincipal.Sid)
                {
                    return user.DistinguishedName;
                }
            }
            return null;
        }

        void DeleteManager(UserPrincipal userPrincipal)
        {
            var userEntry = userPrincipal.GetUnderlyingObject() as DirectoryEntry;
            userEntry.Properties["manager"].Clear();
            userEntry.CommitChanges();
        }

        void DeleteTelNumber(UserPrincipal userPrincipal)
        {
            var userEntry = userPrincipal.GetUnderlyingObject() as DirectoryEntry;
            userEntry.Properties["telephoneNumber"].Clear();
            userEntry.CommitChanges();
        }

    }
}
