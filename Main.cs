using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaApi.Server;
using TShockAPI;

namespace BeanAutoTeam
{
    [ApiVersion(2,1)]
    public class Main : TerrariaPlugin
    {
        public Main(Terraria.Main game) : base(game) { }
        public override string Name => "BeanAutoTeam";

        public override Version Version =>new Version(1,0);

        public override string Author => "Bean_Paste";
        public override string Description => "A plugin of joining specific team automatically";
        public static string ConfigPath = TShock.SavePath + "/BeanAutoTeam/config.json";
        public static string DicPath = TShock.SavePath + "/BeanAutoTeam";
        public static Config _config;
 
        public override void Initialize()
        {
            if (!Directory.Exists(DicPath))
            {
                Directory.CreateDirectory(DicPath);
                if (!File.Exists(ConfigPath))
                {
                    File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(new Config() {Team="none" }, Formatting.Indented));
                }
                else
                {
                    _config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigPath));
                }
            }
            else {
                _config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigPath));
            }
            GetDataHandlers.PlayerUpdate += PlayerUpdate;
            GetDataHandlers.PlayerTeam += OnTeamChange;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GetDataHandlers.PlayerTeam -= OnTeamChange;
                GetDataHandlers.PlayerUpdate -= PlayerUpdate;
            }
            base.Dispose(disposing);
        }

        private void PlayerUpdate(object sender ,GetDataHandlers.PlayerUpdateEventArgs args) {

            if (_config.Switch)
            {
                TSPlayer plr = new TSPlayer(args.PlayerId);
                plr.SetTeam(Convert.ToInt32(Enum.Parse(typeof(TeamType), _config.Team)));
            }
        }
        private void OnTeamChange (object sender,GetDataHandlers.PlayerTeamEventArgs args){
            if (_config.Switch)
            {
                TSPlayer plr = new TSPlayer(args.PlayerId);
                plr.SetTeam(Convert.ToInt32(Enum.Parse(typeof(TeamType), _config.Team)));
            }
        }
        public enum TeamType { 
         none=0,
         red=1,
         green=2,
         blue=3,
         yellow=4,
         pink=5
        }
    }
}
