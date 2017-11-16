namespace BitcoinService
{
    partial class Service1
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.BTCBGroundWorker = new System.ComponentModel.BackgroundWorker();
            // 
            // BTCBGroundWorker
            // 
            this.BTCBGroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BTCBGroundWorker_DoWork);
            // 
            // Service1
            // 
            this.ServiceName = "Service1";

        }

        #endregion

        private System.ComponentModel.BackgroundWorker BTCBGroundWorker;
    }
}
