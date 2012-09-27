using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Web.UI;

namespace NHibernate.Burrow.WebUtil.Impl
{
    internal class StatefulFieldPageModule
    {
        private readonly GlobalPlaceHolder gph;
        private readonly Page page;
        protected bool dataLoaded = false;
    	private IDictionary<string, StateBag> states = new Dictionary<string,StateBag>();
    	private const string Seperator = "_NHB_";
    	private const string Prefix = "NHibernate.Burrow.WebUtil.StatefulField";

        const string stopSettingKey = "NHibernate.Burrow.WebUtil.StopUsingStatefulFields";
		public IDictionary<string, StateBag> States
		{
			get { return states; }
    	}

    	public GlobalPlaceHolder GlobalPlaceHolder {
			get { return gph; }
    	}


        public StatefulFieldPageModule(Page page, GlobalPlaceHolder globalPlaceHolder)
        {
            this.page = page;
            gph = globalPlaceHolder;

            if ( ConfigurationManager.AppSettings[stopSettingKey] == "true" )
                return;
            if(!StatefulFieldsControlFilter.Instance.CanHaveStatefulFields(page))
                return;
            page.PreLoad += new EventHandler(LoadData);
            page.PreRenderComplete += new EventHandler(page_PreRenderComplete);
        }


		/// <summary>
		/// process has to happen after all controls are initiated otherwise will cause ASP.net control initiation problem - sub control even failed to register
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LoadData(object sender, EventArgs e)
		{
			if (!page.IsPostBack || dataLoaded)
			{
				return;
			}
			dataLoaded = true;
			LoadStates();
            LogFactory.Log.Debug("Injecting values for StatefulFields");
			new StatefulFieldLoader(page, this).Process();
		}


    	private void page_PreRenderComplete(object sender, EventArgs e)
        {
            LogFactory.Log.Debug("Retrieving values of StatefulFields");
            new StatefulFieldSaver(page, this).Process();
			SaveStates();
        }


		private void LoadStates()
		{
			NameValueCollection form = page.Request.Form;

			if (string.IsNullOrEmpty(form[Prefix]))
				return;
			
			string stateStr = File.ReadAllText(GetSpeicalBorrowStateFilePath(form[Prefix]));
			NameValueCollection stateCollection = (NameValueCollection)(Deserialize(stateStr));

			foreach (string key in stateCollection.AllKeys)
			{
				string[] keys = key.Split(new string[] { Seperator }, StringSplitOptions.RemoveEmptyEntries);
				if (keys.Length == 3 && keys[0] == Prefix)
				{
					GetControlState(keys[1]).Add(keys[2], Deserialize(stateCollection[key]));
					LogFactory.Log.Debug("StateBag:" + keys[1] + ",keys[2]:" + keys[2] + ",key.value:" + stateCollection[key]);
				}

			}



		}
		

		public StateBag GetControlState(string controlUID)
		{
			StateBag retVal;
			if(! states.TryGetValue(controlUID, out retVal))
				states[controlUID] = retVal = new StateBag();
			return retVal;
		}


		private void SaveStates()
		{

			NameValueCollection stateCollection = new NameValueCollection();
			foreach (KeyValuePair<string, StateBag> pair in states)
			{
				if (pair.Value != null)
				{
					string stateKeyPrefix = Prefix + Seperator + pair.Key + Seperator;
					foreach (DictionaryEntry state in pair.Value)
					{
						object value = state.Value != null ? ((StateItem)state.Value).Value : null;
						string valuestring = Serialize(value);
						stateCollection.Add(stateKeyPrefix + (string)state.Key, valuestring);
						LogFactory.Log.Debug("SaveStates:" + "state.Key:" + state.Key + ",value:" + valuestring);

					}
				}
			}

			string tempfile = Serialize(stateCollection);
			NameValueCollection form = page.Request.Form;
			string guid;


			if (!string.IsNullOrEmpty(form[Prefix]))

				guid = form[Prefix];
			else
				guid = Guid.NewGuid().ToString();
			DirectoryInfo tempFileFolder = new DirectoryInfo( page.Server.MapPath(WebUtilHTTPModule.BorrowStateFilePath));
			if (!tempFileFolder.Exists)
				tempFileFolder.Create();
			ThreadPool.QueueUserWorkItem(delegate { File.WriteAllText(GetSpeicalBorrowStateFilePath(guid), tempfile); });
			GlobalPlaceHolder.AddPostBackField(Prefix, guid);

		}
		private string GetSpeicalBorrowStateFilePath(string fileName) {
			return page.Server.MapPath(WebUtilHTTPModule.BorrowStateFilePath + fileName);
		}
    	private object Deserialize(string value)
		{
			LosFormatter lf = new LosFormatter();
			return lf.Deserialize(value);
		}

		private string Serialize(object val)
		{
			LosFormatter lf = new LosFormatter();
			TextWriter tw = new StringWriter();
			lf.Serialize(tw, val);
			return tw.ToString();
		}

	
    }
}