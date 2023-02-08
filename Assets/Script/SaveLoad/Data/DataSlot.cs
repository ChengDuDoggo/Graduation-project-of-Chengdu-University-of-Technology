using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFarm.Transition;
namespace MFarm.Save
{
    public class DataSlot
    {
        /// <summary>
        /// 进度条,String是每一个类独一无二的GUID
        /// 每一个GUID对应每一个单独需要保存的类中的所有数据(GameSaveData)
        /// </summary>
        public Dictionary<string, GameSaveData> dataDict = new Dictionary<string, GameSaveData>();
        #region 用来显示UI进度
        public string DataTime
        {
            get
            {
                var key = TimeManager.Instance.GUID;
                if (dataDict.ContainsKey(key))
                {
                    var timeData = dataDict[key];
                    return timeData.timeDict["gameYear"] + "年/" + (Season)timeData.timeDict["gameSeason"] + "/" + timeData.timeDict["gameMonth"] + "月/" + timeData.timeDict["gameDay"] + "日/";
                }
                else return string.Empty;
            }
        }
        public string DataScene
        {
            get//拿到该属性时需要执行以下方法函数
            {
                var key = TransitionManager.Instance.GUID;
                if (dataDict.ContainsKey(key))
                {
                    var transitionData = dataDict[key];
                    return transitionData.dataSceneName switch
                    {
                        "00.Start" => "海边",
                        "01.Field" => "农场",
                        "02.Home" => "小木屋",
                        "03.SmellTown" => "市场",
                        _ => string.Empty
                    };
                }
                else return string.Empty;
            }
        }
        #endregion
    }
}

