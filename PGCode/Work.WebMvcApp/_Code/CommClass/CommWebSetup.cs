using System;
using ProcCore.WebCore;

namespace DotWeb.CommSetup
{
    public static class CommWebSetup
    {
        public static String WebCookiesId
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["WebCookiesId"];
            }
        }
        public static String ManageDefCTR
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ManageDefCTR"];
            }
        }
        public static DateTime Expire
        {
            get
            {
                return DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["Expire"]);
            }
        }
        public static String MasterGridID
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["MasterGridID"];
            }
        }
        public static int MasterGridDefHight
        {
            get
            {
                return int.Parse(System.Configuration.ConfigurationManager.AppSettings["GridHeight"]);
            }
        }
        public static int MasterGridDefPageSize
        {
            get
            {
                return int.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"]);
            }
        }
        public static String acNameMasterDataGridUrl
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["acNameMasterGridDataUrl"];
            }
        }
        public static String acNameMasterDataDelete
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["acNameMasterDataDelete"];
            }
        }
        public static String acNameEditMasterDataAddNew
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["acNameEditMasterDataAddNew"];
            }
        }
        public static String acNameEditMasterDataByID
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["acNameEditMasterDataByID"];
            }
        }
        public static String acNameMasterDataUpdateData
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["acNameMasterDataUpdateData"];
            }
        }
        public static String acNameSubDataUpdateData
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["acNameSubDataUpdateData"];
            }
        }
        public static String acNameSubDataDelete
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["acNameSubDataDelete"];
            }
        }
        public static String acNameMasterSubGridData
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["acNameMasterSubGridData"];
            }
        }
        public static String acNameEditFormReturnGridList
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["acNameEditFormReturnGridList"];
            }
        }
        public static String eleNameSearchForm
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["eleNameSearchForm"];
            }

        }
        public static String eleNameSearchButton
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["eleNameSearchButton"];
            }

        }
        public static String eleNameMasterDataForm
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["eleNameMasterDataForm"];
            }

        }
        public static String funNameMasterDataFormatterID
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["funNameMasterDataFormatterID"];
            }

        }
        public static String cssEditFormFieldLabel
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["EditFormFieldsNameCss"];
            }

        }
    }

    #region Image UpLoad Parma
    public static class ImageFileUpParm
    {
        public static ImageUpScope NewsBasic
        {
            get
            {
                ImageUpScope imUp = new ImageUpScope() { KeepOriginImage = true, LimitCount = 3, KindName = "DefaultKind", LimitSize = 1024 * 1024 * 2 };
                imUp.Parm = new ImageSizeParm[] {
                    new ImageSizeParm(){ SizeFolder=134, width=134},
                    new ImageSizeParm(){ SizeFolder=220, width=220},
                    new ImageSizeParm(){ SizeFolder=53, width=53}
                };
                return imUp;
            }
        }

        public static ImageUpScope Product
        {
            get
            {
                ImageUpScope imUp = new ImageUpScope() { KeepOriginImage = true, LimitCount = 1, KindName = "Def", LimitSize = 1024 * 1024 * 2 };
                imUp.Parm = new ImageSizeParm[] {
                    new ImageSizeParm(){ SizeFolder=134, width=134},
                    new ImageSizeParm(){ SizeFolder=220, width=220},
                    new ImageSizeParm(){ SizeFolder=53, width=53}
                };
                return imUp;
            }
        }

        public static ImageUpScope NewsBasicSingle
        {
            get
            {
                ImageUpScope imUp = new ImageUpScope() { KeepOriginImage = true, LimitCount = 5, LimitSize = 1024 * 1024 * 2 };
                imUp.Parm = new ImageSizeParm[] {
                    new ImageSizeParm(){ SizeFolder=700, width=700}
                };
                return imUp;
            }
        }

        public static ImageUpScope NewsBasicDouble
        {
            get
            {
                ImageUpScope imUp = new ImageUpScope() { KeepOriginImage = true, LimitCount = 10, LimitSize = 1024 * 1024 * 2 };
                imUp.Parm = new ImageSizeParm[] {
                    new ImageSizeParm(){ SizeFolder=350, width=350}
                };
                return imUp;
            }
        }

        public static ImageUpScope ProductShow
        {
            get
            {
                ImageUpScope imUp = new ImageUpScope() { KeepOriginImage = true, LimitCount = 10, LimitSize = 1024 * 1024 * 2 };
                imUp.Parm = new ImageSizeParm[] {
                    new ImageSizeParm(){ SizeFolder=232, width=232},
                    new ImageSizeParm(){ SizeFolder=800, width=800}
                };
                return imUp;
            }
        }

        public static ImageUpScope ProductList
        {
            get
            {
                ImageUpScope imUp = new ImageUpScope() { KeepOriginImage = true, LimitCount = 1, LimitSize = 1024 * 1024 * 2 };
                imUp.Parm = new ImageSizeParm[] {
                    new ImageSizeParm(){ SizeFolder=180, width=180},
                    new ImageSizeParm(){ SizeFolder=320, width=320}
                };
                return imUp;
            }
        }
    }
    public static class SysFileUpParm
    {
        public static FilesUpScope BaseLimit
        {
            get
            {
                FilesUpScope FiUp = new FilesUpScope() { LimitCount = 5, LimitSize = 1024 * 1024 * 256 };
                return FiUp;
            }
        }
    }
    #endregion
}